using NeptureWebAPI.AzureDevOps.Abstract;
using NeptureWebAPI.AzureDevOps.Payloads;
using NeptureWebAPI.Controllers;
using System;
using System.Net.Http.Headers;
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
            ILogger<Client> logger) : base(jsonSerializerOptions, httpContextAccessor, appConfiguration, httpClientFactory)
        {
            this.logger = logger;
        }

        public string GetHealthInfo()
        {
            var cred = this.GetCredentials();
            var orgName = this.GetOrgName();

            return $"{orgName} - {cred.Item1} - {cred.Item2}";
        }

        public async Task<AzDoTeamCollection> GetTeamsAsync(bool mine = true, int top = 10, int skip = 0)
        {
            return await this.GetAsync<AzDoTeamCollection>($"_apis/teams?$mine={mine}&$top={top}&$skip={skip}&api-version=7.0-preview.3");
        }

        public async Task<AzDoConnectionData> GetConnectionDataAsync()
        {
            return await this.GetAsync<AzDoConnectionData>($"_apis/connectionData");
        }

        public async Task<AzDoGroupMembershipSlimCollection> GetGroupMembershipsAsync(string subjectDescriptor)
        {
            return await this.GetVsspAsync<AzDoGroupMembershipSlimCollection>($"_apis/graph/Memberships/{subjectDescriptor}?api-version=7.0-preview.1");
        }

        public async Task<ClassificationNode> GetClassificationNodeAsync(string projectId, bool isAreaPath)
        {            
            var depth = 10;
            var path = $"{projectId}/_apis/wit/classificationnodes/{(isAreaPath ? "Areas": "iterations")}?$depth={depth}&api-version=7.0";            
            var node = await GetAsync<ClassificationNode>(path, true);
            return node;
        }

        public async Task<AzDoClassificationNodeCreatedResponse> CreateNewClassificationNodeAsync(NewNodePayload newNodePayload)
        {
            var path = $"{newNodePayload.projectId}/_admin/_Areas/CreateClassificationNode?useApiUrl=true&__v=5";
            var opBody = new AzDoClassificationNodeDetailsInPayload(NodeName: newNodePayload.nodeName, ParentId: newNodePayload.parentId);
            var operation = JsonSerializer.Serialize(opBody, jsonSerializerOptions);

            var payload = new AzDoClassificationNodePayload(OperationData: operation, SyncWorkItemTracking: false);
            return await PostAsync<AzDoClassificationNodePayload, AzDoClassificationNodeCreatedResponse>(path, payload, true);
        }

        public async Task<bool> ApplyAcksAsync(string namespaceId, AzDoAclEntryCollection[] aces)
        {
            var path = $"_apis/accesscontrollists/{namespaceId}?api-version=6.0";
            await PostAsync<AzDoAclEntryPostBody, string>(path, new AzDoAclEntryPostBody(aces), true);
            return true;
        }

        public async Task<bool> UpdateRoleAssginmentAsync(string apiVersion, string projectId, string resourceId, string seperator, string scope, AzDoRoleAssignment[] body)
        {
            var path = $"_apis/securityroles/scopes/{scope}/roleassignments/resources/{projectId}{seperator}{resourceId}?api-version={apiVersion}";
            await PutAsync<AzDoRoleAssignment[], string>(path, body, true);
            return true;
        }

        public async Task<bool> UpdateRoleInheritanceAsync(string apiVersion, string projectId, string resourceId, string seperator, string scope, bool inheritPermissions)
        {
            var path = $"_apis/securityroles/scopes/{scope}/roleassignments/resources/{projectId}{seperator}{resourceId}?api-version={apiVersion}&inheritPermissions={inheritPermissions}";
            return await PatchWithoutBodyAsync(path, true);
        }

        public async Task<AzDoTypeRoleAssignment[]> GetRoleAssignmentAsync(string apiVersion, string projectId, string resourceId, string seperator, string scope)
        {
            var path = $"_apis/securityroles/scopes/{scope}/roleassignments/resources/{projectId}{seperator}{resourceId}?api-version={apiVersion}";
            var roleAssignments = await GetAsync<AzDoTypeRoleAssignmentCollection>(path, true);
            if(roleAssignments != null && roleAssignments.Value != null)
            {
                return roleAssignments.Value.ToArray();
            }
            return new List<AzDoTypeRoleAssignment>().ToArray();
        }

        public async Task<bool> DeleteRoleAssignmentAsync(string apiVersion, string projectId, string resourceId, string seperator, string scope, string[] identities)
        {
            var path = $"_apis/securityroles/scopes/{scope}/roleassignments/resources/{projectId}{seperator}{resourceId}?api-version={apiVersion}";
            await PatchAsync<string[], string>(path, identities, true);

            return true;
        }
    }
}
