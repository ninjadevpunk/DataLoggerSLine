using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Data_Logger_1._3.Services
{
    public class ImageKitService
    {
        private const string PrivateKey = "private_V0jvPQzQdQr2ifAWasNay7RloQ0=";
        private const string UploadUrl = "https://upload.imagekit.io/api/v1/files/upload";

        public static async Task<(string imageUrl, string fileId)> UploadImageAsync(string filePath)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(300);

                string credentials = $"{PrivateKey}:";
                string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

                var request = new MultipartFormDataContent();
                var imageBytes = await File.ReadAllBytesAsync(filePath);
                var fileContent = new ByteArrayContent(imageBytes);
                request.Add(fileContent, "file", Path.GetFileName(filePath));

                request.Add(new StringContent(Path.GetFileName(filePath)), "fileName");

                var response = await client.PostAsync(UploadUrl, request);
                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"ImageKit upload failed: {errorResponse}");
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseBody);
                string imageUrl = json["url"]?.ToString();
                string fileId = json["fileId"]?.ToString();

                return (imageUrl, fileId);
            }
        }

        public static string GetProcessedImageUrl(string uploadedImageUrl)
        {
            return $"{uploadedImageUrl}?tr=ar-1-1,w-50,r-3,fo-auto,q-100,cp-true,e-sharpen,lo-true,f-png";
        }

        public static async Task<string> DownloadImageAsync(string imageUrl, string savePath)
        {
            using var client = new HttpClient();

            var response = await client.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();

            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(savePath, imageBytes);

            return savePath;
        }

        public static async Task DeleteImageAsync(string fileId)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);

                string credentials = $"{PrivateKey}:";
                string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

                string deleteUrl = $"https://api.imagekit.io/v1/files/{fileId}";

                var response = await client.DeleteAsync(deleteUrl);
                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"ImageKit delete failed: {errorResponse}");
                }
            }
        }
    }

}
