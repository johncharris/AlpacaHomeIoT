using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace HomeIoTHub.ViewModels
{
    class DogWaterViewModel : INotifyPropertyChanged
    {
        public string PercentOfWater { get; set; }

        public DogWaterViewModel()
        {
            MessagingCenter.Instance.Subscribe<App, string>(this, Constants.DogWaterTopic, OnDogwaterChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnDogwaterChanged(App app, string waterLevel)
        {
            PercentOfWater = waterLevel;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PercentOfWater)));
        }
    }
}
