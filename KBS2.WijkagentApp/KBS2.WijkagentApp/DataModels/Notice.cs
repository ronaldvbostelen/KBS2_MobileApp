using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KBS2.WijkagentApp.Datamodels.Enums;
using KBS2.WijkagentApp.Views.Pages;
using Xamarin.Forms;
using KBS2.WijkagentApp.DataModels;
using TK.CustomMap;

namespace KBS2.WijkagentApp.Datamodels
{
    /*
     * Class containing the notices displayed on the map
     * first draw. probably better to make a custum pinsclass eg for creating a custum panel / menu
     * derived from BaseDataModel which implements the inotifychanged interface
     * PinTypeChooser to set the corresponding colors of the pins (NOT WORKING AT THE MOMENT)
     */
    public class Notice : BaseDataModel
    {
        public Report Report { get; set; }
        public TKCustomMapPin Pin { get; set; }
        public ObservableCollection<ReportDetails> ReportDetails { get; set; }
        public ObservableCollection<Person> Persons { get; set; }

        public Notice(Report report)
        {
            Report = report;
        }
    }
}