using SpeckleCore;
using System.Collections.ObjectModel;

namespace SpeckleNavisworks.ViewModels
{
    public class Overview : Base
    {
        public ObservableCollection<StreamDetails> StreamDetails { get; set; } = new ObservableCollection<StreamDetails>();

        public Overview()
        {
            foreach (var speckleStream in Models.StreamController.SpeckleStreams)
            {
                var streamDetail = new StreamDetails() { SpeckleStream = speckleStream };
                StreamDetails.Add(streamDetail);
            }
        }
    }
}