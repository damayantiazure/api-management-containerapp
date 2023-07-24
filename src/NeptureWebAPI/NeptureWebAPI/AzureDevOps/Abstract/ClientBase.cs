using System.Net.Http.Headers;
using System.Text.Json;

namespace NeptureWebAPI.AzureDevOps.Abstract
{
    public abstract class ClientBase
    {
        protected readonly JsonSerializerOptions jsonSerializerOptions;
        private readonly IHttpContextAccessor httpContextAccessor;
        protected readonly AppConfig appConfiguration;
        protected readonly IHttpClientFactory httpClientFactory;

        public ClientBase(
            JsonSerializerOptions jsonSerializerOptions,
            IHttpContextAccessor httpContextAccessor,
            AppConfig appConfiguration, 
            IHttpClientFactory httpClientFactory)
        {
            this.jsonSerializerOptions = jsonSerializerOptions;
            this.httpContextAccessor = httpContextAccessor;
            this.appConfiguration = appConfiguration;
            this.httpClientFactory = httpClientFactory;
        }

        protected async virtual Task<TPayload> GetAsync<TPayload>(string apiPath) where TPayload : class
        {
            return await GetCoreAsync<TPayload>(AppConfig.AZUREDEVOPSCLIENT, apiPath);
        }

        protected async virtual Task<TPayload> GetVsspAsync<TPayload>(string apiPath) where TPayload : class
        {
            return await GetCoreAsync<TPayload>(AppConfig.AZUREDEVOPS_IDENTITY_CLIENT, apiPath);
        }

        private async Task<TPayload> GetCoreAsync<TPayload>(string apiType, string apiPath) where TPayload : class
        {
            var (scheme, token) = GetCredentials();
            using HttpClient client = httpClientFactory.CreateClient(apiType);
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue(scheme, token);
            var path = $"/{appConfiguration.OrgName}/{apiPath}";
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var x = await response.Content.ReadAsStringAsync();
                var result = await response.Content.ReadFromJsonAsync<TPayload>(this.jsonSerializerOptions);
                if (result != null)
                {
                    return result;
                }
            }
            throw new InvalidOperationException($"Error: {response.StatusCode}");
        }

        protected string GetOrgName()
        {
            return appConfiguration.OrgName;
        }

        protected (string, string) GetCredentials()
        {
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null)
            {
                var request = httpContextAccessor.HttpContext.Request;
                if (request.Headers.TryGetValue("Authorization", out var authInfo) && authInfo.Any())
                {
                    var authValue = authInfo.First();
                    var authValues = $"{authValue}".Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (authValues != null && authValues.Length > 0)
                    {
                        var scheme = authValues[0];
                        var token = authValues[1];
                        return (scheme, token);
                    }
                }
            }
            return ("Invalid Scheme", "Invalid Token");
      
        }   
    }
}
