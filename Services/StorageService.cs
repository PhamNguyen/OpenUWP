using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace OpenUWP.Services
{
    public class StorageService
    {
        private static ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        /// <summary>
        /// Save an object to phone settings by key and value
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">key to store object</param>
        /// <param name="value">data to store</param>
        public static void SaveSetting<T>(object key, T value)
        {
            settings.Values[key.ToString()] = value;
        }

        /// <summary>
        /// Load an object from phone settings by key
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key to get oject's value</param>
        /// <returns>Data from phone setting</returns>
        public static T LoadSetting<T>(object key)
        {
            dynamic result = default(T);
            string inputKey = key.ToString();
            if (settings.Values.ContainsKey(inputKey))
            {
                result = (T)settings
                    .Values[inputKey];
            }

            return result;
        }

        /// <summary>
        /// Load an object from phone settings by key
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="key">Key to get oject's value</param>
        /// <param name="_value">Input default value</param>
        /// <returns>Data from phone setting</returns>
        public static T LoadSetting<T>(object key, T _value)
        {
            dynamic result = default(T);
            string inputKey = key.ToString();
            if (settings.Values.ContainsKey(inputKey))
            {
                result = (T)settings.Values[inputKey];
                return result;
            }
            return _value;
        }

        public static bool RemoveSetting(object key)
        {
            bool isCompleted = false;
            string inputKey = key.ToString();
            if (settings.Values.Remove(inputKey))
            {
                isCompleted = true;
            }

            return isCompleted;
        }

        public static async Task<T> LoadObjectFromXmlFileAsync<T>(string fileName)
        {
            // this reads XML content from a file ("filename") and returns an object  from the XML
            T objectFromXml = default(T);
            var serializer = new XmlSerializer(typeof(T));

            IStorageItem item = await localFolder.TryGetItemAsync(fileName);
            if (item != null)
            {
                StorageFile file = item as StorageFile;
                Stream stream = await file.OpenStreamForReadAsync();
                objectFromXml = (T)serializer.Deserialize(stream);
                stream.Dispose();
            }
            return objectFromXml;
        }

        /// <summary>
        /// Save an object to Local Folder. 
        /// To use it, you need provide file name and data to save
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="file">File name</param>
        /// <param name="data">Data to save</param>
        public static async Task SaveObjectToXml<T>(string fileName, T data)
        {
            StorageFile storageFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            Stream stream = await storageFile.OpenStreamForWriteAsync();

            using (stream)
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                //4. Create XmlWriter
                using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    //5. Serializer file
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(xmlWriter, data);
                }
            }
        }

        public static Task LoadObjectFromXmlFileAsync<T>(object savedLatestPrizedFileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save file to iso Store
        /// </summary>
        /// <param name="bytes">byte array data of the file</param>
        /// <param name="filename">display name in isostore</param>
        /// <param name="folder">folder where file is located in isostore</param>
        public static async Task SaveFile(byte[] bytes, string filename, string folder)
        {
            StorageFolder currentFolder = localFolder;

            if (!string.IsNullOrEmpty(folder))
            {
                var folders = folder.Split('/');
                foreach (var item in folders)
                {
                    currentFolder = await currentFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                }
            }

            if (!string.IsNullOrEmpty(filename))
            {
                StorageFile storageFile = await currentFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

                using (Stream stream = await storageFile.OpenStreamForWriteAsync())
                {
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// Save file to iso Store
        /// </summary>
        /// <param name="bytes">byte array data of the file</param>
        /// <param name="filename">display name in isostore</param>
        /// <param name="folder">folder where file is located in isostore</param>
        public static async Task SaveFile(Stream stream, string filename, string folder)
        {
            StorageFolder currentFolder = localFolder;

            if (!string.IsNullOrEmpty(folder))
            {
                var folders = folder.Split('/');
                foreach (var item in folders)
                {
                    if (!string.IsNullOrEmpty(item))
                        currentFolder = await currentFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                }
            }

            if (!string.IsNullOrEmpty(filename))
            {
                StorageFile storageFile = await currentFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

                using (Stream streamToSave = await storageFile.OpenStreamForWriteAsync())
                {
                    await stream.CopyToAsync(streamToSave);
                }
            }
        }

        /// <summary>
        /// Load file from IsoStore
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns>return a byte array, return null if error</returns>
        public static async Task<byte[]> LoadFile(string filePath)
        {
            StorageFolder currentFolder = localFolder;

            if (!string.IsNullOrEmpty(filePath))
            {
                var folders = filePath.Split('/');
                for (int i = 0; i < folders.Length - 1; i++)
                {
                    currentFolder = await currentFolder.GetFolderAsync(folders[i]);
                }
            }

            byte[] bytes = null;
            IStorageItem item = await currentFolder.TryGetItemAsync(filePath);
            if (item != null)
            {
                StorageFile file = item as StorageFile;
                if (file != null)
                {
                    using (Stream stream = await file.OpenStreamForReadAsync())
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            bytes = memoryStream.ToArray();
                        }
                    }
                }
            }

            return bytes;
        }
        /// <summary>
        /// Load file from IsoStore
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns>return a byte array, return null if error</returns>
        public static async Task<bool> DeleteFile(string filePath)
        {
            try
            {
                StorageFolder currentFolder = localFolder;

                if (!string.IsNullOrEmpty(filePath))
                {
                    var folders = filePath.Split('/');
                    for (int i = 0; i < folders.Length - 1; i++)
                    {
                        currentFolder = await currentFolder.GetFolderAsync(folders[i]);
                    }
                }

                IStorageItem item = await currentFolder.TryGetItemAsync(filePath);
                if (item != null)
                {
                    StorageFile file = item as StorageFile;
                    if (file != null)
                    {
                        await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("DeleteFile \"{0}\" exception: {1}", filePath, ex.Message));
                return false;
            }

            return true;
        }

        public static async Task SaveString(string filePath, string stringToWrite)
        {
            try
            {
                StorageFolder currentFolder = localFolder;

                if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(stringToWrite))
                {
                    IStorageItem item = await currentFolder.TryGetItemAsync(filePath);

                    if (item != null)
                    {
                        StorageFile file = item as StorageFile;
                        if (file != null)
                        {
                            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                        }
                    }

                    var createdFile = await currentFolder.CreateFileAsync(filePath);
                    await FileIO.WriteTextAsync(createdFile, stringToWrite);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Write String To File \"{0}\" exception: {1}", filePath, ex.Message));
            }
        }

        public static async Task<string> LoadString(string filePath)
        {
            try
            {
                StorageFolder currentFolder = localFolder;

                if (!string.IsNullOrEmpty(filePath))
                {
                    IStorageItem item = await currentFolder.TryGetItemAsync(filePath);

                    if (item != null)
                    {
                        StorageFile file = item as StorageFile;
                        if (file != null)
                        {
                            return await FileIO.ReadTextAsync(file);
                        }
                    }
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Load String From File \"{0}\" exception: {1}", filePath, ex.Message));
                return String.Empty;
            }
        }

        public static async Task<StorageFile> PickSinglePhoto()
        {
            FileOpenPicker photoPicker = GetPhotoPicker();

            return await photoPicker.PickSingleFileAsync();
        }

        public static async Task<IReadOnlyList<StorageFile>> PickMultiPhoto()
        {
            FileOpenPicker photoPicker = GetPhotoPicker();

            return await photoPicker.PickMultipleFilesAsync();
        }

        private static FileOpenPicker GetPhotoPicker()
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            return openPicker;
        }

    }
}
