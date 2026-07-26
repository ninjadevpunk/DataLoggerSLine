using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using static Data_Logger_1._3.Services.EntityReader;

namespace Data_Logger_1._3.Services.CommandLogic
{
    public class PasswordResetService
    {
        private readonly IDataService _dataService;

        public PasswordResetService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            try
            {
                // Check local DB first.
                if (await _dataService.EmailExists(email))
                {
                    using var client = new HttpClient();

                    var payload = new
                    {
                        Email = email
                    };

                    var json = JsonConvert.SerializeObject(payload);

                    using var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://datalogger.space/api/password-reset", content);

                    response.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException httpEx)
            {
                await _dataService.HandleExceptionAsync(httpEx, "RequestPasswordResetAsync()");
                return false;
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, "RequestPasswordResetAsync()");
                return false;
            }

            return true;
        }
    }
}
