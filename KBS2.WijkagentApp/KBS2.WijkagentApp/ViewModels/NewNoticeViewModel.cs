using System;
using System.Collections.Generic;
using System.Text;
using TK.CustomMap;

namespace KBS2.WijkagentApp.ViewModels
{
    public class NewNoticeViewModel : BaseViewModel
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public NewNoticeViewModel(Position position)
        {
            //testpurpose
            Longitude = position.Longitude.ToString();
            Latitude = position.Latitude.ToString();
        }
    }
}
