using System;

namespace KBS2.WijkagentApp.DataModels.old
{
    class Officer : BaseDataModel
    {
        private Guid officerId;
        private string userName;
        private string password;

        public Guid OfficerId
        {
            get { return officerId; }
            set
            {
                if (value != officerId)
                {
                    officerId = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string UserName
        {
            get { return userName; }
            set
            {
                if (value != userName)
                {
                    userName = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string Password
        {
            get { return password; }
            set
            {
                if (value != password)
                {
                    password = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}