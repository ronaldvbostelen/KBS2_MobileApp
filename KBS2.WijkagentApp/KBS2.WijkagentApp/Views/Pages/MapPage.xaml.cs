using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KBS2.WijkagentApp.Datamodels.Enums;

namespace KBS2.WijkagentApp.Views.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
		public MapPage ()
		{
            InitializeComponent();   
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