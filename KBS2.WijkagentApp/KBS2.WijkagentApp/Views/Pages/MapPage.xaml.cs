using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TK.CustomMap;

namespace KBS2.WijkagentApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private double direction;

        public MapPage()
        {
            InitializeComponent();
            CreateView();
        }

        private void CreateView()
        {
            var map = new TKCustomMap();

            map.SetBinding(TKCustomMap.MapTypeProperty, "MapType");
            map.SetBinding(TKCustomMap.IsShowingUserProperty, "ShowingUser");
            map.SetBinding(TKCustomMap.IsRegionChangeAnimatedProperty, "RegionChangeAnimated");
            map.SetBinding(TKCustomMap.PinsProperty, "Pins");
            map.SetBinding(TKCustomMap.MapRegionProperty, "MapRegion");
            map.SetBinding(TKCustomMap.SelectedPinProperty, "SelectedPin");
            map.SetBinding(TKCustomMap.PinSelectedCommandProperty, "SelectedPinCommand");
            map.SetBinding(TKCustomMap.MapLongPressCommandProperty, "MapLongPressCommand");
            map.SetBinding(TKCustomMap.CalloutClickedCommandProperty, "CalloutClickedCommand");
            MapLayout.Content = map;
        }

        /*
         * Pangestures dont have bindable commands, therefor the ui logic in the code behind
         */
        private void PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    direction = e.TotalY;
                    break;
                case GestureStatus.Completed:
                    SlidePanel(direction < 0);
                    break;
            }
        }

        private void OnClicked(object sender, EventArgs e) => SlidePanel(false);

        private void SlidePanel(bool up)
        {
            AbsoluteLayout.SetLayoutBounds(MapLayout, new Rectangle(0, 0, 1, up ? 0.5 : 0.96));
            AbsoluteLayout.SetLayoutBounds(EmergencyPanel, new Rectangle(1, 1, 1, up ? 0.5 : 0.04));
        }   
    }
}
