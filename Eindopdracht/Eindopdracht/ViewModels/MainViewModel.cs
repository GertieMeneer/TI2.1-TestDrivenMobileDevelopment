using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Eindopdracht.NSData;
using Plugin.LocalNotification;

namespace Eindopdracht.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isLoading;

        //binding with ui
        private List<NSStation> _visibleStations;

        private List<NSStation> _allStations;
        private List<NSStation> _nearestStations;
        private List<NSStation> _favouriteStations;

        private string _searchQuery;
        private Location _location;

        public MainViewModel()
        {
            SearchCommand = new Command(SearchStations);
        }

        public Location Location
        {
            get => _location;
            set => _location = value;
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SetStations(int option)
        {
            switch (option)
            {
                case 0:
                    VisibleStations = AllStations;
                    break;
                case 1:
                    VisibleStations = NearestStations;
                    break;
                case 2:
                    VisibleStations = FavouriteStations;
                    break;
                default:
                    throw new Exception("SetStation received wrong option");    //if this gets called: serious skill issue
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                CheckForNoSearch();
            }
        }

        public ICommand SearchCommand { get; private set; }

        //binding with ui
        public List<NSStation> VisibleStations
        {
            get => _visibleStations;
            set
            {
                _visibleStations = value;
                OnPropertyChanged();
            }
        }

        public List<NSStation> NearestStations
        {
            get => _nearestStations;
            set
            {
                _nearestStations = value;
                OnPropertyChanged();
            }
        }

        public List<NSStation> AllStations
        {
            get => _allStations;
            set
            {
                _allStations = value;
                OnPropertyChanged();
            }
        }

        public List<NSStation> FavouriteStations
        {
            get => _favouriteStations;
            set
            {
                _favouriteStations = value;
            }
        }

        private void CheckForNoSearch()
        {
            if (!IsLoading)
            {
                if (SearchQuery.Equals("") || SearchQuery == null)
                {
                    VisibleStations = NearestStations;
                }
            }
            else
            {
                showNotification(0, "ERROR", "Pls no searching while loading ty :DD");
            }
        }

        private void SearchStations()
        {
            if (!IsLoading)
            {
                VisibleStations = AllStations.FindAll(station => station.Naam.ToLower().Contains(SearchQuery.ToLower()));
            }
            else
            {
                showNotification(0, "ERROR", "Pls no searching while loading ty :DD");
            }
        }

        private async Task showNotification(int whenSeconds, string title, string description)
        {
            var request = new NotificationRequest
            {
                Title = title,
                Description = description,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(whenSeconds)
                    //NotifyRepeatInterval = TimeSpan.FromSeconds(10),
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}