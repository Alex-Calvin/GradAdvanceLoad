using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using DataProcessor.Configuration;
using DataProcessor.Models;

namespace DataProcessor.Services
{
    public interface IApiService
    {
        Task<ApiResponse> ProcessUserRecordAsync(UserRecord userRecord, int recordIndex, TimeSpan elapsedTime);
    }

    public class ApiService : IApiService
    {
        private readonly ApiSettings _apiSettings;
        private readonly RestClient _httpClient;

        public ApiService(ApiSettings apiSettings)
        {
            _apiSettings = apiSettings;
            _httpClient = new RestClient(_apiSettings.BaseUrl);
        }

        public async Task<ApiResponse> ProcessUserRecordAsync(UserRecord userRecord, int recordIndex, TimeSpan elapsedTime)
        {
            var request = new RestRequest(_apiSettings.Endpoint, Method.Post);
            request.AddHeader("Authorization", $"Token token={_apiSettings.ApiKey}");

            request.AddParameter("first_name", userRecord.FirstName);
            request.AddParameter("last_name", userRecord.LastName);
            request.AddParameter("email", userRecord.Email);
            request.AddParameter("external_id", userRecord.ExternalId.PadLeft(10, '0'));

            var response = await _httpClient.ExecuteAsync(request);
            
            Console.WriteLine($"Record {recordIndex} - {userRecord.ExternalId.PadLeft(10, '0')}: {response.Content} at {elapsedTime.TotalMilliseconds}ms");

            return new ApiResponse
            {
                IsSuccess = response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.MethodNotAllowed,
                ShouldRetry = response.StatusCode == HttpStatusCode.TooManyRequests,
                StatusCode = response.StatusCode,
                Content = response.Content
            };
        }
    }

    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public bool ShouldRetry { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; } = string.Empty;
    }
} 