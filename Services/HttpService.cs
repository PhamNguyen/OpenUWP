using System.IO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenUWP.Base;
using OpenUWP.Utils;

namespace OpenUWP
{
    public class HttpService
    {
        #region Singleton

        private static object syncRoot = new object();

        private static volatile HttpService instance;

        public static HttpService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new HttpService();
                    }
                }
                return instance;
            }
        }

        private HttpService() { }
        #endregion

        public async Task<string> PostRequest(string Host, Dictionary<string, string> inputs)
        {

            try
            {
                List<KeyValuePair<string, string>> contents = new List<KeyValuePair<string, string>>();

                if (inputs != null)
                    contents = inputs.ToList();

                foreach (var content in contents)
                {
                    Debug.WriteLine(content.ToString());
                }

                HttpClient httpClient = new HttpClient();

                HttpResponseMessage responseMessage = await httpClient.PostAsync(Host, new FormUrlEncodedContent(contents));

                responseMessage.EnsureSuccessStatusCode();

                string result = await responseMessage.Content.ReadAsStringAsync();

                return result;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        public async Task<JObject> PostHttpRequest(string host, string data = null)
        {
            JObject output = null;

            try
            {
                string domain = Uri.EscapeUriString(host);

#if DEBUG
                Debug.WriteLine("Host: {0}", domain);
#endif

                var contents = new List<KeyValuePair<string, string>>();

                if (!String.IsNullOrEmpty(data))
                    contents.Add(new KeyValuePair<string, string>("data", data));

                var httpClient = new HttpClient();

                HttpResponseMessage responseMessage = await httpClient.PostAsync(domain, new FormUrlEncodedContent(contents));

                responseMessage.EnsureSuccessStatusCode();

                string result = await responseMessage.Content.ReadAsStringAsync();

                if (!String.IsNullOrEmpty(result) && result.StartsWith("{") && result.EndsWith("}"))
                {
                    output = JObject.Parse(result);
#if DEBUG
                    Debug.WriteLine("Result: \n");
                    Debug.WriteLine(output.ToString());
#endif
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception at Post Request: {0}\n", ex.Message);
#endif
            }

            return output;
        }


        public async Task<string> PostRequest(string host, string data = null)
        {

            try
            {
                string domain = Uri.EscapeUriString(host);
                Debug.WriteLine("Host: {0}", domain);

                var contents = new List<KeyValuePair<string, string>>();

                if (!String.IsNullOrEmpty(data))
                {
                    contents.Add(new KeyValuePair<string, string>("data", BaseRC4.Encrypt("153748f6d14&#@!#b6108179512d20%!@35D!#84428550a", data)));
                }

                var httpClient = new HttpClient();

                var responseMessage = await httpClient.PostAsync(domain, new FormUrlEncodedContent(contents));

                responseMessage.EnsureSuccessStatusCode();

                string result = await responseMessage.Content.ReadAsStringAsync();

                if (!String.IsNullOrEmpty(result) && result.StartsWith("{") && result.EndsWith("}"))
                {
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("Exception at Post Request: {0}\n", ex.Message);
#endif
            }

            return String.Empty;
        }
        public async Task<string> PostMultiPartRequest(string host, params PairItem[] data)
        {

            string domain = Uri.EscapeUriString(host);

            if (data != null && data.Any())
            {
                try
                {
                    var uploadingFileMultipartFormDataContent = new MultipartFormDataContent();

                    foreach (var param in data)
                    {
                        if (param.TypeOfValue == PairItem.ValueType.StringValue)
                        {
                            uploadingFileMultipartFormDataContent.Add(new StringContent(param.Value as string), param.Key);
                        }
                        else if (param.TypeOfValue == PairItem.ValueType.ByteArrayValue)
                        {
                            if (param.Value is Byte[])
                            {
                                uploadingFileMultipartFormDataContent.Add(new StreamContent(new MemoryStream(param.Value as Byte[])), param.Key, String.Format("{0}.jpg", Guid.NewGuid()));
                            }
                        }
                    }

                    var httpClient = new HttpClient();
                    var responseMessage = await httpClient.PostAsync(domain, uploadingFileMultipartFormDataContent);

                    responseMessage.EnsureSuccessStatusCode();

                    string result = await responseMessage.Content.ReadAsStringAsync();

#if DEBUG
                    Debug.WriteLine("Result: {0}\n", result);
#endif

                    if (!String.IsNullOrEmpty(result) && result.StartsWith("{") && result.EndsWith("}"))
                    {
                        return result;
                    }
                    return null;
                }
                catch (Exception exception)
                {
#if DEBUG
                    Debug.WriteLine("Exception at Post Multipart Request: {0}\n", exception.Message);
#endif
                    return null;
                }
            }
            return null;
        }


        public async Task<string> DownloadStringAsync(string url)
        {
            try
            {
                string domain = Uri.EscapeUriString(url);

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "MOBO");

                var responeMessage = await httpClient.GetAsync(domain);
                responeMessage.EnsureSuccessStatusCode();
                return await responeMessage.Content.ReadAsStringAsync(); ;
            }
            catch (Exception ex)
            {

                return String.Empty;
            }
        }

        public async Task<byte[]> DownloadAsync(string url)
        {
            try
            {

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "MOBO");

                var responeMessage = await httpClient.GetAsync(url);
                responeMessage.EnsureSuccessStatusCode();

                return await responeMessage.Content.ReadAsByteArrayAsync();

            }
            catch
            {
                return null;
            }
        }

        public async Task<JObject> DownloadJsonAsync(string url)
        {
            try
            {
                JObject data = null;

                string result = await this.DownloadStringAsync(url);

                if (result.StartsWith("<") || result.EndsWith(">"))
                    return null;

                if (!String.IsNullOrEmpty(result))
                {
                    data = JObject.Parse(result);
                }
                return data;
            }
            catch
            {
                return null;
            }
        }
    }
}
