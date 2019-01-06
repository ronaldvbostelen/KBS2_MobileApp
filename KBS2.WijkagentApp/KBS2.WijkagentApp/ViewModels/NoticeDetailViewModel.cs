using System;
using System.Collections.Generic;
using System.Text;

namespace KBS2.WijkagentApp.ViewModels
{
    public class NoticeDetailViewModel : BaseViewModel
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public string Suspect { get; set; }
        public string Victim { get; set; }

        public NoticeDetailViewModel()
        {
            //This is an example and needs to be filled with real data from the database later
            Type = "Melding: mishandeling";
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
            Suspect = "Karen Bosch";
            Victim = "Ronald van Bostelen";
        }
    }
}
