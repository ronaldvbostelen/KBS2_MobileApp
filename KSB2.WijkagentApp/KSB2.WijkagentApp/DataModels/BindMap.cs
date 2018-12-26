using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace KSB2.WijkagentApp.Datamodels
{
    public class BindMap : Map
    {

        public BindMap()
        {
            IsShowingUser = true;
            MapType = MapType.Hybrid;
            SetCurrPosition(); //this may cause a exception because its called before given permissions for the location
        }

        async void SetCurrPosition()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude,position.Longitude),Distance.FromMeters(50)));
        }
    }
}