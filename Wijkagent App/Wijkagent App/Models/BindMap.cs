using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace Wijkagent_App.Models
{
    public class BindMap : Map
    {

        public BindMap()
        {
            SetCurrPosition();
            IsShowingUser = true;
            MapType = MapType.Hybrid;
        }

        async void SetCurrPosition()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude,position.Longitude),Distance.FromMeters(50)));

        }
    }
}