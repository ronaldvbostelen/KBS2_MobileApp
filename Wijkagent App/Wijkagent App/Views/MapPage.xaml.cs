using System;
using System.Collections.Generic;
using Wijkagent_App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Wijkagent_App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
	    private List<Message> messages;

		public MapPage ()
		{
            messages = new List<Message>
            {
                new Message("<<LAAG>>","Zwolle CS",Priority.Low,new Position(52.505969, 6.090399)),
                new Message("<<MIDDEL>>","GGD",Priority.Medium,new Position(52.508171, 6.093015)),
                new Message("<<HOOG>>","Wezenlanden park",Priority.High,new Position(52.507746, 6.105814)),
            };

            InitializeComponent();
		    SetInitialPosition();
		    SetInitialPins();

            BtnOne.Clicked += OnClicked;
            BtnTwo.Clicked += OnClicked;
            BtnThree.Clicked += OnClicked;
        }

	    private void SetInitialPosition()
	    {
	        MainMap.MapType = MapType.Hybrid;
	        MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(52.499312, 6.079549), Distance.FromKilometers(1)));
        }

	    private void SetInitialPins()
	    {
            messages.ForEach(x => MainMap.Pins.Add(x.Pin));
	    }

	    private void OnClicked(object sender, EventArgs e)
	    {
	        Button pressedButton = (Button) sender;

	        if (pressedButton.Equals(BtnOne))
	        {
	            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(messages.Find(x => x.Priority == Priority.High).Pin.Position, Distance.FromMeters(35)));
	        }
	        else if (pressedButton.Equals(BtnTwo))
	        {
	            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(messages.Find(x => x.Priority == Priority.Medium).Pin.Position, Distance.FromMeters(35)));
            }
	        else
	        {
	            MainMap.MoveToRegion(MapSpan.FromCenterAndRadius(messages.Find(x => x.Priority == Priority.Low).Pin.Position, Distance.FromMeters(35)));
            }
	    }
	}
}