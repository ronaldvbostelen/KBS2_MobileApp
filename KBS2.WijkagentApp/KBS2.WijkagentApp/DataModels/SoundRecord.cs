using System;
using System.Collections.Generic;
using System.Text;

namespace KBS2.WijkagentApp.DataModels
{
    class SoundRecord : BaseDataModel
    {
        private string officialReportId;
        private Uri url;

        public string OfficialReportId
        {
            get { return officialReportId; }
            set {
                if (value != officialReportId)
                {
                    officialReportId = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public Uri Url
        {
            get { return url; }
            set {
                if (value != url)
                {
                    url = value;
                    NotifyPropertyChanged();
                }
            }
        }


    }
}
