using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using KBS2.WijkagentApp.Extensions;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.DataModels.Collections
{
    public class ReportsCollection
    {
        public ObservableCollection<Report> Reports { get; set; }

        public ReportsCollection()
        {
            FetchReports();
        }

        private async void FetchReports()
        {
            try
            {
                var reportList = await App.DataController.ReportTable.ToListAsync();

                // setting ID (!important!)
                reportList.ForEach(x => x.id = x.ReportId);

                Reports = reportList.ListToObservableCollection();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Reports = new ObservableCollection<Report>();
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets fout tijdens het ophalen van de reportgegevens. Probeer opnieuw.", "OK");
            }
            
        }
    }
}
