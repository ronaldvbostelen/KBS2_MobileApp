using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.ViewModels.Commands;
using KBS2.WijkagentApp.Views.Pages;
using TK.CustomMap;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.ViewModels
{
    public class PinsViewModel : BaseViewModel
    {
        private Report currentTappedReport;
        private ICommand showPinOnMapCommand;
        private ICommand itemTappedCommand;

        //these objects will be filled with DB-entrys for not its the static data, with a select
        public ObservableCollection<Report> HighReports { get; }
        public ObservableCollection<Report> MidReports { get; }
        public ObservableCollection<Report> LowReports { get; }

        public ICommand ShowPinOnMapCommand => showPinOnMapCommand ?? (showPinOnMapCommand = new ActionCommand(report => ShowPinOnMap((Report)report)));
        public ICommand ItemTappedCommand => itemTappedCommand ?? (itemTappedCommand = new ActionCommand(eventArgs => ShowDetailPageOfReport((ItemTappedEventArgs)eventArgs)));

        public PinsViewModel()
        {
            HighReports = new ObservableCollection<Report>();
            MidReports = new ObservableCollection<Report>();
            LowReports = new ObservableCollection<Report>();

            FillReportLists();
        }

        private async void FillReportLists()
        {
            try
            {
                var reports = await App.DataController.ReportTable.ToEnumerableAsync();
                foreach (var report in reports)
                {
                    report.id = report.ReportId;

                    switch (report.Priority)
                    {
                        case 1:
                            HighReports.Add(report);
                            break;
                        case 2:
                            MidReports.Add(report);
                            break;
                        default:
                            LowReports.Add(report);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Ophalen meldingen mislukt", "Probeer later opnieuw " + e.Message, "OK");
            }
        }

        /*
         * this method makes it possible that a double tap on a listitem a new modal with details will popup
         */
        private void ShowDetailPageOfReport(ItemTappedEventArgs eventArgs)
        {
            if (!ReferenceEquals(eventArgs.Item, currentTappedReport))
            {
                currentTappedReport = (Report) eventArgs.Item;
            }
            else
            {
                Application.Current.MainPage.Navigation.PushModalAsync(new NoticeDetailPage(new NoticeDetailViewModel((Report)eventArgs.Item)));
                currentTappedReport = null;
            }
        }

        /*
         * !![-DISCLAIMER-]!!
         * this is some hella ugly code. prolly not up to standards. searched alot and tried alot. pretty hard to navigate + set some parameters
         * so i came up with this. its prolly temporary, if you got a better way: please inform me / make it better ~RvB
         */
        private void ShowPinOnMap(Report report)
        {
            TabbedPage tabbed = (TabbedPage) Application.Current.MainPage;
            NavigationPage mappage = (NavigationPage) tabbed.Children[0];
            var mapPage = mappage.CurrentPage;
            MapViewModel mapPageMapViewModel = (MapViewModel) mapPage.BindingContext;
            mapPageMapViewModel.SelectedPin = mapPageMapViewModel.Pins.First(x => x.Position.Equals(new Position((report.Latitude ?? 0),(report.Longitude ?? 0))));
            mapPageMapViewModel.MapRegion = MapSpan.FromCenterAndRadius(mapPageMapViewModel.SelectedPin.Position, Distance.FromMeters(35));
            tabbed.CurrentPage = tabbed.Children[0];
        }
    }
}