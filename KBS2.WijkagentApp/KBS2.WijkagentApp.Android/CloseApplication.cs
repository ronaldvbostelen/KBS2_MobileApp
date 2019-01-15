using Java.Lang;
using KBS2.WijkagentApp.Services.Dependecies;


namespace KBS2.WijkagentApp.Droid
{
    class CloseApplication : ICloseApplication
    {
        public void CloseApp()
        {
            JavaSystem.Exit(0);
        }
    }
}