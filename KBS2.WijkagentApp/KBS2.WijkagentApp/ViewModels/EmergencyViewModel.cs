using System;
using System.Diagnostics;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Services.Dependecies;
using KBS2.WijkagentApp.ViewModels.Commands;
using Plugin.Geolocator;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;

namespace KBS2.WijkagentApp.ViewModels
{
    class EmergencyViewModel : BaseViewModel
    {
        public string FullName { get; set; }

        private Emergency emergency;
        public Emergency Emergency { get { return emergency; } set { if (value != emergency) { emergency = value; NotifyPropertyChanged(); } } }

        private ICommand emergencyTriggerCommand;
        public ICommand EmergencyTriggerCommand => emergencyTriggerCommand ?? (emergencyTriggerCommand = new ActionCommand(x => EmergencyTrigger()));

        public EmergencyViewModel()
        {
            FullName = User.Person.FullName;
            Emergency = SetEmergency();
        }

        private Emergency SetEmergency() => new Emergency { Status = "A", OfficerId = User.Id, Time = DateTime.Now };

        private async void EmergencyTrigger()
        {
            var alert = DependencyService.Get<IDisplayAlert>();
            alert?.DisplayAlert();

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            Emergency.Latitude = position.Latitude;
            Emergency.Longitude = position.Longitude;
            Emergency.Time = DateTime.Now;
            try
            {
                await App.DataController.EmergencyTable.InsertAsync(Emergency);
                alert?.CloseAlert();
                await Application.Current.MainPage.DisplayAlert("Geslaagd", "Noodbericht verstuurd", "OK");
                Emergency = SetEmergency();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                alert?.CloseAlert();
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Probeer opnieuw", "OK");
            }
        }
    }
}