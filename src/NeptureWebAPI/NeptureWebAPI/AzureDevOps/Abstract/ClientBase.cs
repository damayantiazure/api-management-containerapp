

using System.Net.Http.Headers;
using System.Text;
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

        protected async virtual Task<TResponsePayload> PostAsync<TRequestPayload, TResponsePayload>(
            string apiPath, TRequestPayload payload, bool elevate = false) 
            where TRequestPayload : class 
            where TResponsePayload : class
        {
            return await PostCoreAsync<TRequestPayload, TResponsePayload>(AppConfig.AZUREDEVOPSCLIENT, apiPath, payload, elevate);
        }

        protected async virtual Task<TPayload> GetAsync<TPayload>(string apiPath, bool elevate = false) where TPayload : class
        {
            return await GetCoreAsync<TPayload>(AppConfig.AZUREDEVOPSCLIENT, apiPath, elevate);
        }

        protected async virtual Task<TPayload> GetVsspAsync<TPayload>(string apiPath, bool elevate = false) where TPayload : class
        {
            return await GetCoreAsync<TPayload>(AppConfig.AZUREDEVOPS_IDENTITY_CLIENT, apiPath, elevate);
        }

        private async Task<TPayload> GetCoreAsync<TPayload>(
            string apiType, string apiPath, bool elevate = false) where TPayload : class
        {
            var (scheme, token) = GetCredentials(elevate);
            using HttpClient client = httpClientFactory.CreateClient(apiType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
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

        private async Task<TResponsePayload> PostCoreAsync<TRequestPayload, TResponsePayload>(
            string apiType, string apiPath, TRequestPayload payload, bool elevate = false) 
            where TRequestPayload : class
            where TResponsePayload : class
        {
            var (scheme, token) = GetCredentials(elevate);
            using HttpClient client = httpClientFactory.CreateClient(apiType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
            var path = $"/{appConfiguration.OrgName}/{apiPath}";
            
            using var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync<TRequestPayload>(memoryStream, payload, this.jsonSerializerOptions);

            var jsonContent = new StringContent(Encoding.UTF8.GetString(memoryStream.ToArray()) , Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, path) 
            {
                Content = jsonContent
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var x = await response.Content.ReadAsStringAsync();
                var result = await response.Content.ReadFromJsonAsync<TResponsePayload>(this.jsonSerializerOptions);
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

        protected (string, string) GetCredentials(bool elevate = false)
        {
            if(elevate)
            {
                var base64Credential = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", appConfiguration.Pat)));
                var scheme = "Basic";
                return (scheme, base64Credential);
            }
            else if (httpContextAccessor != null && httpContextAccessor.HttpContext != null)
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
