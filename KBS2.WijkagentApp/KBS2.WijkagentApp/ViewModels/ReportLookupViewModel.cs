using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.Models.DataControllers;
using KBS2.WijkagentApp.ViewModels.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace KBS2.WijkagentApp.ViewModels
{
    class ReportLookupViewModel : BaseViewModel
    {
        private string[] priorityArray = {"Laag", "Gemiddeld", "Hoog"};
        private List<Report> queryResults;
        private TwitterDataController twitter;
        private bool onlyActiveReports;

        private Report selectedReport;
        private Tweet selectedTweet;
        private Report detailReport;
        
        private string processingOfficerFullname;
        private string reporterOfficerFullname;
        private string priority;
        
        private ICommand searchCommand;
        private ICommand helpCommand;
        private ICommand searchTextCommand;
        private ICommand tweetTappedCommand;

        public ICommand SearchCommand => searchCommand ?? (searchCommand = new ActionCommand(searchParameter => SearchAsync((string) searchParameter)));
        public ICommand HelpCommand => helpCommand ?? (helpCommand = new ActionCommand(x => HelpDialog()));
        public ICommand SearchTextCommand => searchTextCommand ?? (searchTextCommand = new ActionCommand(x => SearchText((TextChangedEventArgs) x)));
        public ICommand TweetTappedCommand => tweetTappedCommand ?? (tweetTappedCommand = new ActionCommand(x => TweetTappedAsync((ItemTappedEventArgs)x)));

        public ObservableCollection<Report> FoundReports { get; set; }
        public ObservableCollection<Tweet> Tweets { get; set; }

        public string ProcessingOfficerFullname { get { return processingOfficerFullname;} set{ if (value != processingOfficerFullname) { processingOfficerFullname = value; NotifyPropertyChanged(); } } }
        public string ReporterOfficerFullname { get { return reporterOfficerFullname;} set{ if (value != reporterOfficerFullname) { reporterOfficerFullname = value; NotifyPropertyChanged(); } } }
        public string Priority { get { return priority;} set{ if (value != priority) { priority = value; NotifyPropertyChanged(); } } }

        public bool OnlyActiveReports //this thingy is for the switch and filters the reports on only active (a/p) or all
        {
            get { return onlyActiveReports; }
            set { if (value != onlyActiveReports) { onlyActiveReports = value; NotifyPropertyChanged(); UpdateFoundReports(value); } }
        }

        public Report DetailReport { get { return detailReport; } set { if (value != detailReport) { detailReport = value; NotifyPropertyChanged(); } } }
        public Tweet SelectedTweet { get { return selectedTweet; } set { if (value != selectedTweet) { selectedTweet = value; NotifyPropertyChanged(); } } }
        public Report SelectedReport
        {
            get { return selectedReport; }
            set { if (value != selectedReport) { selectedReport = value; NotifyPropertyChanged(); SetDetailRecordAsync((Report)value); SetTwitterTweetsAsync((Report)value); } }
        }

        public ReportLookupViewModel()
        {
            Initialize();
        }

        // we create new objects when we start the app/this viewmodel
        private void Initialize()
        {
            FoundReports = new ObservableCollection<Report>();

            queryResults = new List<Report>();

            twitter = new TwitterDataController();

            Tweets = new ObservableCollection<Tweet>();
        }

        //if you press the "Zoek melding" words this will popup, its quite hidden because i couldnt get it nice in the UI / formatting / placing / #IwantToGoBackToWPFFuckXamarin
        private async Task HelpDialog()
        {
            await Application.Current.MainPage.DisplayAlert("Zoeken",
                "U kunt zoeken op aanleiding, locatie en omschrijving. Voer trefwoord(en) in en druk op enter. " +
                "De gevonden meldingen worden direct getoond. De meldingen worden verrijkt met twitterberichten " +
                "die in de nabije omgeving zijn verstuurd.", "Ok");
        }

        // so if you press search this task will be triggered with the enterd text (parameter)
        private async Task SearchAsync(string searchParameter)
        {
            try
            {
                queryResults.Clear();

                var queryTask = App.DataController.QueryReports(searchParameter);
                var result = await queryTask;

                result.ForEach(x => queryResults.Add(x));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            UpdateFoundReports(onlyActiveReports);
        }

        // active is A (not beinig processed) or P being processed
        private void UpdateFoundReports(bool onlyActive)
        {
            FoundReports.Clear();

            queryResults.Where(x => x.Status == "A" || x.Status == "P" || !onlyActive).ForEach(x => FoundReports.Add(x));
        }

        // so this method set the displaying data of the record. ALL information is displayed. There are some conversions of data eg priority. Yes alot of NULL, yes it is necessary
        private async Task SetDetailRecordAsync(Report report)
        {
            try
            {
            Task<Officer> lookupReporterTask = null;
            Task<Officer> lookupProcessedByTask = null;

            if (!report.ReportId.Equals(Guid.Empty)) lookupReporterTask = App.DataController.OfficerTable.LookupAsync(report.ReporterId);
            if (report.ProcessedBy != null) lookupProcessedByTask = App.DataController.OfficerTable.LookupAsync(report.ProcessedBy);

            // preventing nullable values
            report.Time = report.Time ?? new DateTime();
            report.Longitude = report.Longitude ?? 0;
            report.Latitude = report.Latitude ?? 0;

            Officer reporterOfficer = null;
            Officer proccesedByOfficer = null;

            if (lookupReporterTask != null) reporterOfficer = await lookupReporterTask;
            if (lookupProcessedByTask != null) proccesedByOfficer = await lookupProcessedByTask;

            Task<Person> lookupReporterPersonTask = null;
            Task<Person> lookupProcessedByPersonTask = null;

            if (reporterOfficer != null) lookupReporterPersonTask = App.DataController.PersonTable.LookupAsync(reporterOfficer.PersonId);
            if (proccesedByOfficer != null) lookupProcessedByPersonTask = App.DataController.PersonTable.LookupAsync(proccesedByOfficer.PersonId);

            Person reporterPerson = null;
            Person processedByPerson = null;

            if (lookupReporterPersonTask != null) reporterPerson = await lookupReporterPersonTask;
            if (lookupProcessedByPersonTask != null) processedByPerson = await lookupProcessedByPersonTask;

            ReporterOfficerFullname = reporterPerson == null ? "[Onbekend]" : reporterPerson.FullName;
            ProcessingOfficerFullname = processedByPerson == null ? "[Niet in behandeling]" : processedByPerson.FullName;

            Priority = priorityArray[(report.Priority ?? 1) - 1];

            DetailReport = report;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Ophalen rapport mislukt. Probeer later nogmaals.", "Ok");
            }
        }

        // so this thingy calls the twittercontroller and fetches the tweets. every tweet is added to the tweetslist after its cleared
        private async Task SetTwitterTweetsAsync(Report report)
        {
            try
            {
                Tweets.Clear();

                var twitterQueryTask = twitter.GetTweetsInRadius(report.Latitude ?? 0, report.Longitude ?? 0, 5, report.Comment, report.Location, report.Type);
                var twitterresults = await twitterQueryTask;

                foreach (var twitterresult in twitterresults)
                {
                    Tweets.Add(new Tweet(twitterresult.User.Name,twitterresult.CreatedAt,twitterresult.FullText,twitterresult.Source,twitterresult.StatusID.ToString()));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                await Application.Current.MainPage.DisplayAlert("Er ging iets mis..", "Ophalen Twitter-tweets mislukt.", "Ok");
            }
        }

        // this displays a tweet in detail with a dialog. just a simple solution.
        private async Task TweetTappedAsync(ItemTappedEventArgs args)
        {
            var tweet = (Tweet) args.Item;
            await Application.Current.MainPage.DisplayAlert("Tweet", $"ID: {tweet.Id}\r\nGeplaatst op: {tweet.CreatedAt}\r\nAuteur: {tweet.UserName}\r\n{tweet.Text}", "Ok");
        } 
        
        //so when the searchbar is cleared with the X pressed the panels are cleared, otherwise they aint. just some gimmick i liked.
        private void SearchText(TextChangedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.NewTextValue) && args.OldTextValue.Length > 1)
            {
                DetailReport = null;
                Tweets.Clear();
                FoundReports.Clear();
            }
        }
    }
}