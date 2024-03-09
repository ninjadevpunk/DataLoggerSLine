using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Data_Logger_1._3.Services
{
    public class DataService
    {
        private readonly string firebaseBaseUrl; // Set your Firebase base URL here

        public DataService(string firebaseBaseUrl)
        {
            this.firebaseBaseUrl = firebaseBaseUrl;
        }

        public async Task SendFlexiNotesLogAsync(string projectName, string applicationName) /* other parameters */
        {
            // Assuming you are using HttpClient for HTTP requests
            using (var httpClient = new HttpClient())
            {
                var apiUrl = $"{firebaseBaseUrl}/flexiNotesLogs.json"; // Adjust the URL based on your Firebase structure

                var flexiNotesLogData = new
                {
                    ProjectName = projectName,
                    ApplicationName = applicationName,
                    // Add other properties as needed
                };

                var jsonContent = JsonConvert.SerializeObject(flexiNotesLogData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content);

                // Check the response status and handle accordingly
                if (response.IsSuccessStatusCode)
                {
                    // Data sent successfully
                }
                else
                {
                    // Handle error
                }
            }
        }

    }
}
