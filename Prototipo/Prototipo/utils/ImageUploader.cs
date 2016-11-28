using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prototipo
{
    public static class ImageUploader
    {
        public static string URL = "http://wiishper.com/profilepics/upload.php";
        public static async Task<bool> UploadPic(byte[] data, string filename)
        {
            try
            {
                Debug.WriteLine("****************_*_*_*_*_*_* UPLOADING PROFILE PIC *_*_*_*_*_********************");
                var fileContent = new ByteArrayContent(data);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = filename
                };
                string boundary = "------32573574368afecdeefb";
                MultipartFormDataContent multipartContent = new MultipartFormDataContent(boundary);
                multipartContent.Add(fileContent);

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync(URL, multipartContent);

                Debug.WriteLine(response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(content);
                    return true;
                }
                Debug.WriteLine("Profile pic not uploaded");
                return false;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
