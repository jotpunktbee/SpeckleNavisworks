using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using SpeckleCore;

namespace SpeckleNavisworks.ViewModels
{
    public class CreateNewStream : Base
    {
        private List<SpeckleStream> _streams;
        private SpeckleStream _activeStream;
        private string _streamName;
        private string _streamDescription;

        public Account Account { get; set; }
        public SpeckleApiClient Client { get; set; }
        public List<SpeckleStream> Streams
        {
            get
            {
                return _streams;
            }
            set
            {
                _streams = value;
                OnPropertyChanged();
            }
        }

        public SpeckleStream ActiveStream
        {
            get
            {
                return _activeStream;
            }
            set
            {
                _activeStream = value;
                OnPropertyChanged();
            }
        }

        public string StreamName
        {
            get
            {
                return _streamName;
            }
            set
            {
                _streamName = value;
                OnPropertyChanged();
            }
        }

        public string StreamDescription
        {
            get
            {
                return _streamDescription;
            }
            set
            {
                _streamDescription = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<StreamDetails> CreatedStreams { get; set; }

        private ICommand _createNewStreamCommand;
        public ICommand CreateNewStreamCommand
        {
            get
            {
                if (_createNewStreamCommand == null)
                {
                    _createNewStreamCommand = new RelayCommand(
                        p => true,
                        p => CreateNewSender());
                }

                return _createNewStreamCommand;
            }
        }

        public CreateNewStream(SpeckleApiClient speckleApiClient)
        {
            Client = speckleApiClient;
            CreatedStreams = new ObservableCollection<StreamDetails>();
        }

        public async void GetAllStreams()
        {
            try
            {
                var responseStream = await Client.StreamsGetAllAsync("");
                Streams = responseStream.Resources;
                Streams = Streams.Where(s => s.Deleted != true).ToList();

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(Streams.Count.ToString());

                System.Windows.MessageBox.Show(stringBuilder.ToString());
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public async void CreateNewSender()
        {
            try
            {
                var newSender = await Client.IntializeSender(Client.AuthToken, "Navisworks", "Navisworks", new Random().Next().ToString());
                ActiveStream = Client.Stream;
                ActiveStream.Name = StreamName;
                ActiveStream.Description = StreamDescription;

                await Client.StreamUpdateAsync(Client.StreamId, ActiveStream);

                CreatedStreams.Add(new StreamDetails() { SpeckleStream = ActiveStream });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
