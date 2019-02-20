using KBS2.WijkagentApp.DataModels;
using KBS2.WijkagentApp.DataModels.Collections;

namespace KBS2.WijkagentApp.ViewModels
{
    class MainPageViewModel
    {
        public MainPageViewModel()
        {
            // get and set global reports
            App.ReportsCollection = new ReportsCollection();
            App.ReportsCollection.AddReportsToCollection();
        }
    }
}
