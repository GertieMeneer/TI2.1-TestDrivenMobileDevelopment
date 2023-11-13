using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace TDMD
{
    public class MainViewModel : BindableObject
    {
        private ObservableCollection<Lamp> _lamps;
        private bool _isRefreshing = false;
        private string _status;
        private string _userID;

        public MainViewModel()
        {
            Lamps = new ObservableCollection<Lamp>(new List<Lamp>());
        }

        public string UserIDText
        {
            get { return _userID; }
            set
            { 
                _userID = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Lamp> Lamps
        {
            get => _lamps;
            set
            {
                _lamps = value;
                OnPropertyChanged();
            }
        }

        public string ConnectionStatus
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    // Call your method to refresh data here
                    await RefreshData();

                    IsRefreshing = false;
                });
            }
        }

        private async Task RefreshData()
        {
            if(Communicator.userid == null)
            {
                await Communicator.GetUserIdAsync();
                if(Communicator.userid != null)
                {
                    UserIDText = $"UserID: {Communicator.userid}";
                }
            }
            LoadLamps();
        }

        public async Task LoadLamps()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://10.0.2.2:8000/api/newdeveloper");

                    if (response.IsSuccessStatusCode)
                    {
                        ConnectionStatus = "Status: Connected!";
                        string jsonString = await response.Content.ReadAsStringAsync();

                        Lamps = LampParser.ParseLights(jsonString);
                    }
                    else
                    {
                        //Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    //Debug.WriteLine(ex);
                }
            }
        }
    }
}
