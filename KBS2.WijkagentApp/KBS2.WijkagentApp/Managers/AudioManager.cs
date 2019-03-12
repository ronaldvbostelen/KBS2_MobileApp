using System;
using System.IO;
using System.Threading.Tasks;
using KBS2.WijkagentApp.Services.Dependecies;
using Plugin.AudioRecorder;
using Xamarin.Forms;

namespace KBS2.WijkagentApp.Managers
{
    class AudioManager : AudioRecorderService
    {
        
        public AudioManager()
        {
            StopRecordingOnSilence = false;
            StopRecordingAfterTimeout = true;
            TotalAudioTimeout = TimeSpan.FromSeconds(60);

            //default name
            FilePath = Path.Combine(GetDroidSavePath(), $"Proces_verbaal_opname_{DateTime.Now}.wav");
        }

        public void SetFileName(string name)
        {
            FilePath = Path.Combine(GetDroidSavePath(), $"{name}_{DateTime.Now}.wav");
        }

        public async Task<string> StartAsync()
        {
            if (!IsRecording)
            {
                var recordTask = await StartRecording();
                return await recordTask;
            }

            return string.Empty;
        }

        public async Task StopAsync()
        {
            if (IsRecording)
            {
                await StopRecording();
            }
        }

        private string GetDroidSavePath()
        {       
            var pathInformation = DependencyService.Get<IPathInformation>();
            return pathInformation?.GetPublicPath();
        }
    }
}
