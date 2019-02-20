using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using KBS2.WijkagentApp.Extensions;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.DataModels.Collections
{
    public class ReportsCollection
    {
        public ObservableCollection<Report> Reports { get; set; }

        public ReportsCollection()
        {
            Reports = new ObservableCollection<Report>();
        }

        private async Task<List<Report>> FetchReports()
        {
            try
            {
                var reportList = await App.DataController.ReportTable.ToListAsync();

                // setting ID (!important!) & adding to ReportsCollection
                reportList.ForEach(x => x.id = x.ReportId);

                return reportList;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Mislukt", "Er ging iets fout tijdens het ophalen van de reportgegevens. Probeer opnieuw.", "OK");
                return new List<Report>();
            }
        }

        public async void AddReportsToCollection()
        {
            var reportListTask = await FetchReports();
            reportListTask.ForEach(x => Reports.Add(x));
        } 
        
    }
}
