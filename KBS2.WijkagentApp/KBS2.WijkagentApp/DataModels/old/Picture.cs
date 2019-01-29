﻿using System;

namespace KBS2.WijkagentApp.DataModels.old
{
    class Picture : BaseDataModel
    {
        private string officialReportId;
        private Uri url;

        public string OfficialReportId
        {
            get { return officialReportId; }
            set
            {
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
            set
            {
                if (value != url)
                {
                    url = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}