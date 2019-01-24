using System.Collections.ObjectModel;
using TK.CustomMap;

namespace KBS2.WijkagentApp.DataModels
{
    /*
     * Class containing the notices displayed on the map
     * first draw. probably better to make a custum pinsclass eg for creating a custum panel / menu
     * derived from BaseDataModel which implements the inotifychanged interface
     * PinTypeChooser to set the corresponding colors of the pins (NOT WORKING AT THE MOMENT)
     */
    public class Notice : BaseDataModel
    {
        public old.Report Report { get; set; }
        public TKCustomMapPin Pin { get; set; }
        public ObservableCollection<old.ReportDetails> ReportDetails { get; set; }
        public ObservableCollection<old.Person> Persons { get; set; }

        public Notice(old.Report report)
        {
            Report = report;
        }
    }
}