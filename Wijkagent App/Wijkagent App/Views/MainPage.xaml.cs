using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wijkagent_App.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wijkagent_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage ()
        {
            InitializeComponent();
        }
        
    }
}