﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
    }
}