using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using TDMD.Classes;

namespace TDMD.ViewModels
{
    public partial class MainViewModel : ObservableObject 
    {
        [ObservableProperty]
        private List<Lamp> _lampsList;

        private bool _isRefreshing = false;
        private string _status;
        private string _userID;

        public MainViewModel()
        {
            Lamps = new List<Lamp>(new List<Lamp>());

            InitializeAsync();
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

        public List<Lamp> Lamps
        {
            get => _lampsList;
            set
            {
                _lampsList = value;
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

        [RelayCommand]
        public async Task Refresh()
        {
            IsRefreshing = true;

            // Call your method to refresh data here
            await RefreshData();

            IsRefreshing = false;
        }

        private async Task RefreshData()
        {
            if (Communicator.userid == null)
            {
                await Communicator.GetUserIdAsync();
                if (Communicator.userid != null)
                {
                    UserIDText = $"UserID: {Communicator.userid}";
                }
            }
            LoadLamps();
        }

        private async void InitializeAsync()
        {
            await GetUserIDAsync();

            if (Communicator.userid != null)
            {
                await LoadLamps();
            }
            else
            {
                UserIDText = "error";
            }
        }

        private async Task GetUserIDAsync()
        {
            // before running the app click on the link button in the HUE emulator!!!
            if (await Communicator.GetUserIdAsync() == false)
            {
                UserIDText = "No UserID. Link button > refresh app";
            }
            else
            {
                UserIDText = $"UserID: {Communicator.userid}";
            }

        }

        public async Task LoadLamps()
        {
            string url = $"http://192.168.12.24/api/" + Communicator.userid;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //when android phone: http://10.0.2.2:8000/api/newdeveloper
                    //when windows: http://192.168.1.179/api/newdeveloper

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        ConnectionStatus = "Status: Connected!";
                        string jsonString = await response.Content.ReadAsStringAsync();

                        Debug.WriteLine(jsonString);

                        Lamps = LampParser.ParseLights(jsonString);
                    }
                    else
                    {
                        Debug.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
