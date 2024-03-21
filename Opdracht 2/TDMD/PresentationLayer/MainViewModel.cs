using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TDMD.DomainLayer;
using TDMD.InfrastructureLayer;

namespace TDMD.ApplicationLayer
{
    public partial class MainViewModel : ObservableObject, IMainViewModel
    {
        [ObservableProperty]
        private List<Lamp> _lampsList;

        private bool _isRefreshing = false;
        private string _status;
        private string _userID;

        private string userId;

        private IServiceProvider services;
        private ApiService _apiService;

        public MainViewModel(IServiceProvider services, ApiService apiService)
        {
            _apiService = apiService;

            this.services = services;

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

        [RelayCommand]
        public async Task GoToLampInfoPage(Lamp lamp)
        {
            if (lamp == null)
                return;

            await Shell.Current.GoToAsync(nameof(LampInfoPage), true, new Dictionary<string, object>
            {
                {"Lamp", lamp }
            });
        }

        public async Task RefreshData()
        {
            if (userId != null)
            {
                Lamps = _apiService.Loadlamps().Result;
            }
            else
            {
                await GetUserIDAsync();
                Lamps = _apiService.Loadlamps().Result;
            }
        }

        public async void InitializeAsync()
        {
            await GetUserIDAsync();

            if (userId != null)
            {
                Lamps = await _apiService.Loadlamps();
            }
            else
            {
                UserIDText = "error";
            }
        }

        public async Task GetUserIDAsync()
        {
            // before running the app click on the link button in the HUE emulator!!!

            string result = await _apiService.getUserId();

            if (result.Equals(string.Empty))
            {
                ConnectionStatus = $"Failed to connect. Did you press the link button?";
                UserIDText = "No UserID. Link button > refresh app";
            }
            else
            {
                userId = result;
                ConnectionStatus = $"UserID: {userId}";
                UserIDText = $"UserID: {userId}";
            }
        }
    }
}
