

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






//public async Task<ClassificationNode> GetClassificationNodeAsync(string projectId, bool isAreaPath)
//{            
//    var depth = 10;
//    var path = $"{projectId}/_apis/wit/classificationnodes/{(isAreaPath ? "Areas": "iterations")}?$depth={depth}&api-version=7.0";            
//    var node = await GetAsync<ClassificationNode>(path, true);
//    return node;
//}

//public async Task<AzDoClassificationNodeCreatedResponse> CreateNewClassificationNodeAsync(NewNodePayload newNodePayload)
//{
//    var path = $"{newNodePayload.projectId}/_admin/_Areas/CreateClassificationNode?useApiUrl=true&__v=5";
//    var opBody = new AzDoClassificationNodeDetailsInPayload(NodeName: newNodePayload.nodeName, ParentId: newNodePayload.parentId);
//    var operation = JsonSerializer.Serialize(opBody, jsonSerializerOptions);

//    var payload = new AzDoClassificationNodePayload(OperationData: operation, SyncWorkItemTracking: false);
//    return await PostAsync<AzDoClassificationNodePayload, AzDoClassificationNodeCreatedResponse>(path, payload, true);
//}