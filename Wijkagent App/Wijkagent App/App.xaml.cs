using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Wijkagent_App.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Wijkagent_App
{
    public partial class App : Application
    {

        public App()
        {
            var startPage = new TabbedPage();
            
            startPage.Children.Add(new MapPage { Title = "Map/Zoom", Icon = "target_with_circle.png" });
            
            startPage.Children.Add(new PageOne { Title = "Pins", Icon = "big_map_placeholder.png" });
            
            startPage.Children.Add(new PageTwo { Title = "5 Stars", Icon = "star_point.png" });
            
            startPage.Children.Add(new PageThree { Title = "Messages", Icon = "error_message.png" });

          
            MainPage = startPage;
        }
    }
}
