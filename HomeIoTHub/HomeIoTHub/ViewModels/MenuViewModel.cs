using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace HomeIoTHub.ViewModels
{
    class MenuViewModel : INotifyPropertyChanged
    {
        public Page SelectedItem { get; set; }

        public List<Page> Pages { get; set; } = new List<Page>
        {
            new Page("Dog Water", "DogWaterPage", "resource://HomeIoTHub.Resources.bowl.webp"),
            new Page("Sprinklers", "SprinklersPage", "resource://HomeIoTHub.Resources.rainbird.jpg"),
            new Page("Pressure", "PressurePage", null)
        };

        public Command<Page> OnItemSelected => new Command<Page>(async page =>
        {
            if (SelectedItem != null)
            {
                await (App.Current.MainPage as Shell).GoToAsync(SelectedItem.UriPath);
                SelectedItem = null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        });

        public event PropertyChangedEventHandler PropertyChanged;

        public class Page
        {
            public Page(string title, string uriPath, string imageUri)
            {
                Title = title;
                UriPath = uriPath;
                ImageUri = imageUri;
            }

            public string Title { get; set; }
            public string UriPath { get; set; }
            public string ImageUri { get; set; }
        }
    }

}
