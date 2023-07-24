
using Microsoft.AspNetCore.Mvc;
using NeptureWebAPI.AzureDevOps;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using NeptureWebAPI.AzureDevOps.Payloads;

namespace NeptureWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public string Get()
        {
            return client.GetHealthInfo();
        }

        [HttpGet("all")]
        public async Task<AzDoTeamCollection> GetTeamsAsync()
        {
            return await client.GetTeamsAsync();
        }
    }
}
