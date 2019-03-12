using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace KBS2.WijkagentApp.Managers
{
    class CameraManager
    {
        private StoreCameraMediaOptions mediaOptions;

        public CameraManager(Guid reportId)
        {
            mediaOptions = new StoreCameraMediaOptions
            {
                Name = $"{reportId}_{DateTime.Now}",
                Directory = "proces_verbaal_images",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Front
            };
        }

        public Task<MediaFile> TakePhoto() => CrossMedia.Current.TakePhotoAsync(mediaOptions);
    }
}

