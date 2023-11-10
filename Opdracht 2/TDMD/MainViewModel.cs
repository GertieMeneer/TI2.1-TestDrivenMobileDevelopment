using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace TDMD
{
    public class MainViewModel : BindableObject
    {
        private ObservableCollection<Lamp> _lamps;

        public ObservableCollection<Lamp> Lamps
        {
            get => _lamps;
            set
            {
                _lamps = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            // Initialize the ObservableCollection
            Lamps = new ObservableCollection<Lamp>(new List<Lamp>());
        }
    }
}
