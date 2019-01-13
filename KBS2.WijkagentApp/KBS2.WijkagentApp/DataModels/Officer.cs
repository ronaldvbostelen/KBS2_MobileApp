using System;
using System.Collections.Generic;
using System.Text;

namespace KBS2.WijkagentApp.DataModels
{
    class Officer : BaseDataModel
    {
        private string officerId;
        private string userName;
        private string passWord;


        public string OfficerId
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


        public string PassWord
        {
            get { return passWord; }
            set
            {
                if (value != passWord)
                {
                    passWord = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}