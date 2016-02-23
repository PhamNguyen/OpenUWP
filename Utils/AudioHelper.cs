using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace OpenPhone.Utils
{
    public class AudioHelper
    {
        #region variable
        private static AudioVideoCaptureDevice micAudioCapture;
        private static IRandomAccessStream ranStream;
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        private static StorageFile storageFile;
        private static string audioPath;
        #endregion

        #region Method
        public static async Task StartRenderAudio(string path)
        {
            audioPath = path;
            await RecordingByAudioCapture();
        }

        public async static Task<string> StopRenderAudio()
        {
            await StopRecordingAudioCapture();
            return audioPath;
        }

        private async static Task RecordingByAudioCapture()
        {
            if (micAudioCapture == null)
            {
                micAudioCapture = await AudioVideoCaptureDevice.OpenForAudioOnlyAsync();
                micAudioCapture.AudioEncodingFormat = CameraCaptureAudioFormat.Amr;
                
            }

            storageFile = await localFolder.CreateFileAsync(audioPath, CreationCollisionOption.ReplaceExisting);
            ranStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite);
            
            await micAudioCapture.StartRecordingToStreamAsync(ranStream);
        }

        private async static Task StopRecordingAudioCapture()
        {
            if (micAudioCapture != null)
            {
                await micAudioCapture.StopRecordingAsync();
                ranStream.AsStream().Dispose();
            }
        }
        #endregion
        
    }
}
