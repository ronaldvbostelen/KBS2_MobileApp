
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KBS2.WijkagentApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportLookupPage : ContentPage
    {
        private double direction;

		public ReportLookupPage ()
		{
			InitializeComponent ();
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

	    private void SlidePanel(bool up)
	    {
            // because the panel is transparent we hide the scrolled-over panel.. (its stupid but a simple trick that works)
	        ReportDetailsView.IsVisible = !up;
	        AbsoluteLayout.SetLayoutBounds(TwitterDetailsView, new Rectangle(0, up ? 0 : 1, 1, up ? 1 : 0.06));   
	    }
    }
}