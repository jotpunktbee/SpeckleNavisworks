using SpeckleCore;
using System.Collections.ObjectModel;

namespace SpeckleNavisworks.ViewModels
{
    public class Overview : Base
    {
        public ObservableCollection<StreamDetails> StreamDetails { get; set; } = new ObservableCollection<StreamDetails>();

        public Overview()
        {
            foreach (var speckleStreamWrapper in Models.StreamController.SpeckleStreamsWrappers)
            {
                var streamDetail = new StreamDetails() { SpeckleStreamWrapper = speckleStreamWrapper };
                StreamDetails.Add(streamDetail);
            }
        }
    }
}