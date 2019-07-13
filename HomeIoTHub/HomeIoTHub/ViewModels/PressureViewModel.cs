using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace HomeIoTHub.ViewModels
{
    class PressureViewModel : INotifyPropertyChanged
    {
        public string PercentOfWater { get; set; }

        public PressureViewModel()
        {
            MessagingCenter.Instance.Subscribe<App, string>(this, Constants.PressureTopic, OnPressureChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPressureChanged(App app, string pressure)
        {
            PercentOfWater = pressure; 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PercentOfWater)));
        }
    }
}
