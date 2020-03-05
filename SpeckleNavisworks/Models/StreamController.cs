using System;
using System.Collections.Generic;
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
    }
}
