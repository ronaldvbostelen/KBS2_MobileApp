using Android.App;
using KBS2.WijkagentApp.Services.Dependecies;
using Plugin.CurrentActivity;

namespace KBS2.WijkagentApp.Droid
{
    class SendDialog : IDisplayAlert
    {
        private AlertDialog dialog;

        public void DisplayAlert()
        {
            AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity);
            dialogBuilder.SetTitle("Moment geduld a.u.b.");
            dialogBuilder.SetMessage("Bericht wordt verstuurd..");
            dialogBuilder.SetCancelable(false);
            dialog = dialogBuilder.Create();
            dialog.Show();
        }

        public void CloseAlert()
        {
            dialog.Dismiss();
        }
    }
}