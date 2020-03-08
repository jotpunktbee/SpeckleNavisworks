using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using SpeckleCore;

namespace SpeckleNavisworks.Models
{
    public static class StreamController
    {
        /// <summary>
        /// Collection of SpeckleStreams in this session
        /// </summary>
        public static List<SpeckleStreamWrapper> SpeckleStreamsWrappers { get; private set; } = new List<SpeckleStreamWrapper>();
        public static SpeckleApiClient Client { get; set; }

        /// <summary>
        /// Creates a new SpeckleStream
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public static async Task<bool> NewStream(string name, string description)
        {
            if (Client != null)
            {
                try
                {
                    var newSender = await Client.IntializeSender(
                        Client.AuthToken,
                        Path.GetFileName(NavisworksWrapper.Document.FileName),
                        "Navisworks Manage",
                        NavisworksWrapper.DocumentGUID);
                    var newStream = Client.Stream;
                    newStream.Name = String.IsNullOrEmpty(name) ? "Anonymous Naviworks Stream" : name;
                    newStream.Description = String.IsNullOrEmpty(description) ? "There is no description" : description;

                    await Client.StreamUpdateAsync(Client.StreamId, newStream);

                    AddStream(new SpeckleStreamWrapper(newStream));

                    return true;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"{ex.Message} | {ex.InnerException.Message}");
                }
            }
            else
            {
                throw new ArgumentNullException("Client", "SpeckleApiClient is not defined");
            }

            return false;
        }

        /// <summary>
        /// Adds a SpeckleStream to the collection of streams
        /// </summary>
        /// <param name="speckleStream"></param>
        private static void AddStream(SpeckleStreamWrapper speckleStream)
        {
            SpeckleStreamsWrappers.Add(speckleStream);

            var speckleStreams = JsonConvert.SerializeObject(SpeckleStreamsWrappers);
            System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\SpeckleNavisworks");
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\SpeckleNavisworks\\sessions.json";

            using (System.IO.StreamWriter streamWriter = new StreamWriter(
                path,
                false))
            {
                streamWriter.Write(speckleStreams);
                streamWriter.Close();
            }
        }

        /// <summary>
        /// Update a SpeckleStream and show progress
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="viewModelProgress"></param>
        public static async Task<Task> UpdateStream(List<SpeckleObject> objects)
        {
            return Task.Run(() =>
            {
                var convertedSpeckleObjects = objects;
                // LocalContext.PruneExistingObjects(convertedSpeckleObjects, Client.BaseUrl);

                var cloneResult = Client.StreamCloneAsync(Client.Stream.StreamId).Result;
                Client.Stream.Children.Add(cloneResult.Clone.StreamId);

                List<SpeckleObject> persistedSpeckleObjects = new List<SpeckleObject>();
                if (convertedSpeckleObjects.Count(obj => obj.Type == "Placeholder") != convertedSpeckleObjects.Count)
                {
                    int count = 0;
                    var objectUpdatePayloads = new List<List<SpeckleObject>>();
                    long totalBucketSize = 0;
                    long currentBucketSize = 0;
                    var currentBucketObjects = new List<SpeckleObject>();
                    var allObjects = new List<SpeckleObject>();

                    foreach (SpeckleObject convertedSpeckleObject in convertedSpeckleObjects)
                    {
                        long size = Converter.getBytes(convertedSpeckleObject).Length;
                        currentBucketSize += size;
                        totalBucketSize += size;
                        currentBucketObjects.Add(convertedSpeckleObject);

                        if (size > 2e6)
                        {
                            System.Windows.MessageBox.Show("Object is too big to upload!");
                            currentBucketObjects.Remove(convertedSpeckleObject);
                        }

                        if (currentBucketSize > 5e5)
                        {
                            Debug.WriteLine("Reached payload limit. Making a new one, current #: )" + objectUpdatePayloads.Count);
                            objectUpdatePayloads.Add(currentBucketObjects);
                            currentBucketObjects = new List<SpeckleObject>();
                            currentBucketSize = 0;
                        }
                    }

                    // Add object in the last bucket
                    if (currentBucketObjects.Count > 0)
                    {
                        objectUpdatePayloads.Add(currentBucketObjects);
                    }

                    Debug.WriteLine("Finished, payload object update count is: " + objectUpdatePayloads.Count + " total bucket size is (kb) " + totalBucketSize / 1000);

                    int k = 0;
                    List<ResponseObject> responses = new List<ResponseObject>();
                    foreach (var payload in objectUpdatePayloads)
                    {
                        Debug.WriteLine(String.Format("Sending payload {0} out of {1}", k++, objectUpdatePayloads.Count));

                        try
                        {
                            var objectResponse = Client.ObjectCreateAsync(payload).Result;
                            responses.Add(objectResponse);
                            persistedSpeckleObjects.AddRange(objectResponse.Resources);

                            int m = 0;
                            foreach (var oL in payload)
                            {
                                oL._id = objectResponse.Resources[m++]._id;

                                if (oL.Type != "Placeholder")
                                {
                                    LocalContext.AddSentObject(oL, Client.BaseUrl);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                    }
                }
                else
                {
                    persistedSpeckleObjects = convertedSpeckleObjects;
                }

                Debug.WriteLine("Updating stream...");

                List<SpeckleObject> placeholders = new List<SpeckleObject>();

                int progressVal = 0;
                foreach (var obj in persistedSpeckleObjects)
                {
                    placeholders.Add(new SpecklePlaceholder() { _id = obj._id });
                }

                SpeckleStream updateStream = new SpeckleStream()
                {
                    Objects = placeholders
                };

                try
                {
                    var response = Client.StreamUpdateAsync(Client.Stream.StreamId, updateStream).Result;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

                Client.BroadcastMessage("stream", Client.StreamId, new { eventType = "update-global" });

                Debug.WriteLine("Data sent!");
            });
        }
    }
}
