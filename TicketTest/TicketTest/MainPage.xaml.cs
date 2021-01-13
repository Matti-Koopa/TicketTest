using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TicketTest
{
    public partial class MainPage : ContentPage
    {
        private IShakeServiceStarter _shakeServiceStarter;

        public MainPage()
        {
            InitializeComponent();
            _shakeServiceStarter = DependencyService.Get<IShakeServiceStarter>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SetToggleButtonText();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (_shakeServiceStarter.IsRunning)
                    _shakeServiceStarter.Stop();
                else
                    _shakeServiceStarter.Start();
                SetToggleButtonText();
            }
            catch(Exception ex)
            {
                DisplayAlert("Fehler", ex.Message, "OK");
            }
        }

        private void SetToggleButtonText()
        {
            string nextServiceStatus = _shakeServiceStarter.IsRunning ? "Stoppe" : "Starte";
            ButtonToggle.Text = $"{nextServiceStatus} Hintergrundservice";
        }
    }
}
