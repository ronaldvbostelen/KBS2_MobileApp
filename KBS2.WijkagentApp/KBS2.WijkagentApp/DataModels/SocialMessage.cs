using System;
using System.Collections.Generic;
using System.Text;

namespace KBS2.WijkagentApp.DataModels
{
    class SocialMessage : BaseDataModel
    {
        private string socialsId;
        private string content;

        public string SocialsId
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