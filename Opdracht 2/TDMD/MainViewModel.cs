using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;

namespace TDMD
{
    public class MainViewModel : BindableObject
    {
        private ObservableCollection<Lamp> _lamps;
        private bool _isRefreshing = false;
        private string _status;

        public ObservableCollection<Lamp> Lamps
        {
            get => _lamps;
            set
            {
                _lamps = value;
                OnPropertyChanged();
            }
        }

        public string Status
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

        public MainViewModel()
        {
            // Initialize the ObservableCollection
            Lamps = new ObservableCollection<Lamp>(new List<Lamp>());
        }

        private async Task RefreshData()
        {
            if(Communicator.userid == null)
            {
                await Communicator.GetUserIdAsync();
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
                        Status = "Status: Connected!";
                        string jsonString = await response.Content.ReadAsStringAsync();

                        Lamps = LightParser.ParseLights(jsonString);
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
