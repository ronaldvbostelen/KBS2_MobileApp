using KBS2.WijkagentApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KBS2.WijkagentApp.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OfficalReportPage : ContentPage
	{
		public OfficalReportPage (BaseViewModel viewModel)
		{
		    InitializeComponent();
            BindingContext = viewModel;
		}
	}
}