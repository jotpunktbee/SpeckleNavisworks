using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpeckleCore;

namespace SpeckleNavisworks.Models
{
    public static class StreamController
    {
        /// <summary>
        /// Collection of SpeckleStreams in this session
        /// </summary>
        public static List<SpeckleStream> SpeckleStreams { get; private set; } = new List<SpeckleStream>();
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
                    var newSender = await Client.IntializeSender(Client.AuthToken, "Navisworks Document", "Navisworks Manage", Guid.NewGuid().ToString());
                    var newStream = Client.Stream;
                    newStream.Name = String.IsNullOrEmpty(name) ? "Anonymous Naviworks Stream" : name;
                    newStream.Description = String.IsNullOrEmpty(description) ? "There is no description" : description;

                    await Client.StreamUpdateAsync(Client.StreamId, newStream);

                    AddStream(newStream);

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
        private static void AddStream(SpeckleStream speckleStream)
        {
            SpeckleStreams.Add(speckleStream);
        }
    }
}
