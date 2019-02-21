using System;
using Newtonsoft.Json;

namespace KBS2.WijkagentApp.DataModels
{
    public partial class SocialMessage : BaseDataModel
    {
        private Guid socialMessageId;
        private Guid socialsId;
        private string content;

        [JsonProperty(PropertyName = "socialMessageId")]
        public Guid SocialMessageId
        {
            get { return socialMessageId; }
            set
            {
                if (value != socialMessageId)
                {
                    socialMessageId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "socialsId")]
        public Guid SocialsId
        {
            get { return socialsId; }
            set
            {
                if (value != socialsId)
                {
                    socialsId = value;
                    NotifyPropertyChanged();
                }
            }
        }

        [JsonProperty(PropertyName = "content")]
        public string Content
        {
            get { return content; }
            set
            {
                if (value != content)
                {
                    content = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}