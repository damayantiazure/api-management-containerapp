

using NeptureWebAPI.AzureDevOps.Abstract;
using NeptureWebAPI.AzureDevOps.Payloads;
using NeptureWebAPI.AzureDevOps.Security;
using System.Text.Json;

namespace NeptureWebAPI.AzureDevOps
{
    public class Client : ClientBase
    {
        private readonly ILogger<Client> logger;

        public Client(
            IHttpContextAccessor httpContextAccessor,
            JsonSerializerOptions jsonSerializerOptions,
            AppConfig appConfiguration,
            IHttpClientFactory httpClientFactory,
            IdentitySupport identitySupport,
            ILogger<Client> logger) : base(jsonSerializerOptions, httpContextAccessor, 
                appConfiguration, identitySupport, httpClientFactory)
        {
            this.logger = logger;
        }

        public async Task<string> GetAzDOOrgName()
        {
            var orgName = this.GetOrgName();
            return $"{orgName}";
        }

        public async Task<AzDoTeamCollection> GetTeamsAsync(bool mine = true, int top = 10, int skip = 0)
        {
            return await this.GetAsync<AzDoTeamCollection>($"_apis/teams?$mine={mine}&$top={top}&$skip={skip}&api-version=7.0-preview.3");
        }

        public async Task<AzDoConnectionData> GetConnectionDataAsync(bool elevated = false)
        {
            return await this.GetAsync<AzDoConnectionData>($"_apis/connectionData", elevated);
        }
        

        public async Task<AzDoGroupMembershipSlimCollection> GetGroupMembershipsAsync(string subjectDescriptor)
        {
            return await this.GetVsspAsync<AzDoGroupMembershipSlimCollection>($"_apis/graph/Memberships/{subjectDescriptor}?api-version=7.0-preview.1");
        }
    }
}
