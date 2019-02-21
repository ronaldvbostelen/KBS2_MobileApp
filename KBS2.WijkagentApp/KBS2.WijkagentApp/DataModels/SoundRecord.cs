using System;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class SoundRecord : BaseDataModel
    {
        private Guid soundRecordId;
        private Guid officialReportId;
        private string url;

        [JsonProperty(PropertyName = "soundRecordId")]
        public Guid SoundRecordId
        {
            get { return soundRecordId; }
            set
            {
                if (value != soundRecordId)
                {
                    soundRecordId = value;
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