using System;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class Picture : BaseDataModel
    {
        private Guid pictureId;
        private Guid officialReportId;
        private string url;

        [JsonProperty(PropertyName = "pictureId")]
        public Guid PictureId
        {
            get { return pictureId; }
            set
            {
                if (value != pictureId)
                {
                    pictureId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "officialReportId")]
        public Guid OfficialReportId
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

        [JsonProperty(PropertyName = "URL")]
        public string Url
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