using Android.OS;
using KBS2.WijkagentApp.Services.Dependecies;

namespace KBS2.WijkagentApp.Droid
{
    class PublicPathManager : IPathInformation
    {
        public string GetPublicPath() => Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath;
        
    }
}