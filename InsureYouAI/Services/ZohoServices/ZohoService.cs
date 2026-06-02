
using InsureYouAI.DTOs.InsuranceLeadDtos;
using InsureYouAI.Entities;
using InsureYouAI.Services.ZohoServices;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace InsureYouAI.Services
{
    public class ZohoService : IZohoService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ZohoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ZohoLeadResponseDto> CreateLeadAsync(InsuranceLead lead)
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();

                var apiDomain = _configuration["Zoho:ApiDomain"];

                var requestUrl = $"{apiDomain}/crm/v8/Leads";

                var requestBody = new
                {
                    data = new[]
                    {
                        new
                        {
                            First_Name = lead.FirstName,
                            Last_Name = string.IsNullOrWhiteSpace(lead.LastName) ? "Bilinmiyor" : lead.LastName,
                            Email = lead.Email,
                            Phone = lead.Phone,
                            Company = "InsureYouAI",
                            Lead_Source = "InsureYouAI Web Form",
                            Description = $"Sigorta Türü: {lead.InsuranceType}\nMesaj: {lead.Message}"
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);

                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Zoho-oauthtoken", accessToken);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new ZohoLeadResponseDto
                    {
                        IsSuccess = false,
                        ErrorMessage = responseBody,
                        ResponseBody = responseBody
                    };
                }

                using var document = JsonDocument.Parse(responseBody);

                var data = document.RootElement.GetProperty("data")[0];

                var status = data.GetProperty("status").GetString();

                if (status == "success")
                {
                    var details = data.GetProperty("details");
                    var id = details.GetProperty("id").GetString();

                    return new ZohoLeadResponseDto
                    {
                        IsSuccess = true,
                        ZohoLeadId = id,
                        ResponseBody = responseBody
                    };
                }

                return new ZohoLeadResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = responseBody,
                    ResponseBody = responseBody
                };
            }
            catch (Exception ex)
            {
                return new ZohoLeadResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var accountsUrl = _configuration["Zoho:AccountsUrl"];
            var clientId = _configuration["Zoho:ClientId"];
            var clientSecret = _configuration["Zoho:ClientSecret"];
            var refreshToken = _configuration["Zoho:RefreshToken"];

            var tokenUrl =
                $"{accountsUrl}/oauth/v2/token" +
                $"?refresh_token={refreshToken}" +
                $"&client_id={clientId}" +
                $"&client_secret={clientSecret}" +
                $"&grant_type=refresh_token";

            var response = await _httpClient.PostAsync(tokenUrl, null);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Zoho access token alınamadı: " + responseBody);
            }

            using var document = JsonDocument.Parse(responseBody);

            return document.RootElement.GetProperty("access_token").GetString();
        }
    }
}