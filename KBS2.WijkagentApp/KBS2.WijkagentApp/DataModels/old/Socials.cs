using System;

namespace KBS2.WijkagentApp.DataModels.old
{
    class Socials : BaseDataModel
    {
        private string socialsId;
        private string personId;
        private Uri profile;

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

        public string PersonId
        {
            get { return personId; }
            set
            {
                if (value != personId)
                {
                    personId = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public Uri Profile
        {
            get { return profile; }
            set
            {
                if (value != profile)
                {
                    profile = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}