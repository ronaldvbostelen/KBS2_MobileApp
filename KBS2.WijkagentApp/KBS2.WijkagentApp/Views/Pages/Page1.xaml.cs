using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.CustomMap;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KBS2.WijkagentApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        private TKCustomMap _map;

        public Page1()
        {
            _map = new TKCustomMap();
            _map.MapClicked += OnMapClicked;

            Content = _map;
        }

        private void OnMapClicked(object sender, TKGenericEventArgs<Position> e)
        {
            //_map.Pins.Add(new TKCustomMapPin
            //{
            //    Title = "New Pin",
            //    Position = e.Value
            //});
            var initPin = new TKCustomMapPin()
            {
                IsDraggable = true,
                Position = new Position(24.774265, 46.738586),
                IsVisible = true,
                Title = "Concern Location",
                Image = Device.OnPlatform("free.png", "pinicon.png", "Assets/pinicon.png")
            };
            Console.WriteLine("ba");
        }
    }
}