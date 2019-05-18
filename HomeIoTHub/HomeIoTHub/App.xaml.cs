using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HomeIoTHub.Services;
using HomeIoTHub.Views;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Client.Options;
using MQTTnet;
using System.Threading.Tasks;
using System.Text;

namespace HomeIoTHub
{
    public partial class App : Application
    {
        IManagedMqttClient mqttClient;

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            StartMqtt();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            StopMqtt();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            StartMqtt();
        }

        void StartMqtt()
        {
            // Setup and start a managed MQTT client.
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId("XamarinUser")
                    .WithTcpServer("192.168.39.182")
                    //.WithTls()
                    .Build())
                .Build();

            mqttClient = new MqttFactory().CreateManagedMqttClient();
            Task.Run(async () =>
            {
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(Constants.DogWaterTopic).Build());
                await mqttClient.StartAsync(options);
            });

            mqttClient.UseApplicationMessageReceivedHandler(m =>
            {
                var payload = Encoding.UTF8.GetString(m.ApplicationMessage.Payload);

                if (m.ApplicationMessage.Topic == Constants.DogWaterTopic)
                {
                    MessagingCenter.Instance.Send(this, Constants.DogWaterTopic, payload);
                }

            });
        }

        void StopMqtt()
        {
            Task.Run(async () => await mqttClient?.StopAsync());
        }

    }
}
