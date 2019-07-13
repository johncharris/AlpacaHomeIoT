using HomeIoTHub.Views;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HomeIoTHub
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("dogwater", typeof(DogWaterPage));
            Routing.RegisterRoute("pressure", typeof(PressurePage));

        }
    }
}
