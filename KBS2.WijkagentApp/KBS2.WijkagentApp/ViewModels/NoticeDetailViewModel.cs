using KBS2.WijkagentApp.DataModels;

namespace KBS2.WijkagentApp.ViewModels
{
    public class NoticeDetailViewModel : BaseViewModel
    {
        public Report Report { get; }
        public Person Suspect { get; }
        public Person Victim { get; }

        public NoticeDetailViewModel(Report report, Person suspect, Person victim)
        {
            //This is an example and needs to be filled with real data from the database later
            Report = report;
            Suspect = suspect;
            Victim = victim;
        }
    }
}
