using HomeIoTHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeIoTHub.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DogWaterPage : ContentPage
    {
        public DogWaterPage()
        {
            InitializeComponent();
            BindingContext = new DogWaterViewModel();

        }
    }
}