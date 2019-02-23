using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Services.Dependecies;
using KBS2.WijkagentApp.ViewModels.Commands;
using Plugin.Geolocator;
using Xamarin.Forms;
using Application = Xamarin.Forms.Application;
using Position = Plugin.Geolocator.Abstractions.Position;

namespace KBS2.WijkagentApp.ViewModels
{
    class EmergencyViewModel : BaseViewModel
    {
        public string FullName { get; set; }

        private Emergency emergency;
        public Emergency Emergency { get { return emergency; } set { if (value != emergency) { emergency = value; NotifyPropertyChanged(); } } }

        private ICommand emergencyTriggerCommand;
        public ICommand EmergencyTriggerCommand => emergencyTriggerCommand ?? (emergencyTriggerCommand = new ActionCommand(x => EmergencyTriggerAsync()));

        public EmergencyViewModel()
        {
            FullName = User.Person.FullName;
            Emergency = SetEmergency();
        }

        private Emergency SetEmergency() => new Emergency { Status = "A", OfficerId = User.Id, Time = DateTime.Now };

        private async void EmergencyTriggerAsync()
        {
            var currentPositionTask = GetCurrentPositionAsync();

            var alert = DependencyService.Get<IDisplayAlert>();
            alert?.DisplayAlert();

            Emergency.Time = DateTime.Now;

            var currentPosition = await currentPositionTask;

            Emergency.Latitude = currentPosition.Latitude;
            Emergency.Longitude = currentPosition.Longitude;
            
            try
            {
                await App.DataController.EmergencyTable.InsertAsync(Emergency);
                alert?.CloseAlert();
                await Application.Current.MainPage.DisplayAlert("Geslaagd", "Noodbericht verstuurd", "OK");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                alert?.CloseAlert();
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Probeer opnieuw", "OK");
            }

            Emergency = SetEmergency();
        }

        private async Task<Position> GetCurrentPositionAsync() => await CrossGeolocator.Current.GetPositionAsync();
    }
}