namespace KBS2.WijkagentApp.DataModels.old
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