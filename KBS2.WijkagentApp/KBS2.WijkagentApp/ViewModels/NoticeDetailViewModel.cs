using System;
using System.Collections.Generic;
using System.Text;
using KBS2.WijkagentApp.Datamodels;

namespace KBS2.WijkagentApp.ViewModels
{
    public class NoticeDetailViewModel : BaseViewModel
    {
        public Notice Notice { get; set; }

        public NoticeDetailViewModel(Notice notice)
        {
            //This is an example and needs to be filled with real data from the database later
            Notice = notice;
        }
    }
}
