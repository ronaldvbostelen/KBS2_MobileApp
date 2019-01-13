using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KBS2.WijkagentApp.Datamodels.Enums;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using TK.CustomMap;

namespace KBS2.WijkagentApp.Views.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
		public MapPage ()
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

	    private void OnButtonClicked(object sender, EventArgs e)
	    {
	        Button clickedButton = (Button)sender;

	        BtnOne.TextColor = BtnOne.Equals(clickedButton) ? new Color(255, 255, 255) : new Color(0, 0, 0);
	        BtnOne.Image = BtnOne.Equals(clickedButton) ? "map_locator_med_wt.png" : "map_locator_med.png";
            BtnTwo.TextColor = BtnTwo.Equals(clickedButton) ? new Color(255, 255, 255) : new Color(0, 0, 0);
	        BtnTwo.Image = BtnTwo.Equals(clickedButton) ? "map_locator_med_wt.png" : "map_locator_med.png";
            BtnThree.TextColor = BtnThree.Equals(clickedButton) ? new Color(255, 255, 255) : new Color(0, 0, 0);
	        BtnThree.Image = BtnThree.Equals(clickedButton) ? "map_locator_med_wt.png" : "map_locator_med.png";
        }

    }
}