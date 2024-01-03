
using Microsoft.AspNetCore.Mvc;
using NeptureWebAPI.AzureDevOps;
using NeptureWebAPI.AzureDevOps.Payloads;

namespace NeptureWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class TeamsController : ControllerBase
    {
        private readonly Client client;
        private readonly ILogger<TeamsController> _logger;

        public TeamsController(
            Client client,
            ILogger<TeamsController> logger)
        {
            this.client = client;
            _logger = logger;
        }

        [HttpGet("loopback")]        
        public async Task<LoopbackResponse> Get()
        {
            var apiVersion = "API Version is not provided in header";

            if(Request.Headers.TryGetValue("Api-Version", out var apiVersionInHeader))
            {
                apiVersion = apiVersionInHeader;
            }
            var softwareVersion = Environment.GetEnvironmentVariable("SOFTWARE_VERSION");

            var azDoOrgName = await client.GetAzDOOrgName();

            apiVersion = apiVersion ?? "API Version is not provided in header";
            softwareVersion = softwareVersion ?? "SOFTWARE_VERSION is not provided in environment variables";

            return new LoopbackResponse(apiVersion, softwareVersion, azDoOrgName);
        }

        public record LoopbackResponse(string ApiVersion, string SoftwareVersion, string AzDoOrgName);

        [HttpGet("all")]
        public async Task<AzDoTeamCollection> GetTeamsAsync([FromQuery] int top = 10, [FromQuery] int skip = 0)
        {
            return await client.GetTeamsAsync(mine: true, top, skip);
        }

        [HttpGet("group-memberships")]
        public async Task<AzDoGroupMembershipSlimCollection> GetGroupMembershipsAsync()
        {
            var connectionData = await client.GetConnectionDataAsync();
            var subjectDescriptor = connectionData.AuthenticatedUser.SubjectDescriptor;

            return await client.GetGroupMembershipsAsync(subjectDescriptor);
        }
    }
}
