using Microsoft.AspNetCore.Http;
using NeptureWebAPI.AzureDevOps.Security.Schemes;
using System;
using System.Text;

namespace NeptureWebAPI.AzureDevOps.Security
{
    // Transient in DI
    public class IdentitySupport
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ServicePrincipalTokenSupport servicePrincipalTokenSupport;
        private readonly PersonalAccessTokenSupport personalAccessTokenSupport;
        private readonly AppConfig config;

        public IdentitySupport(
            IHttpContextAccessor httpContextAccessor,
            ServicePrincipalTokenSupport servicePrincipalTokenSupport,
            PersonalAccessTokenSupport personalAccessTokenSupport,
            AppConfig config)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.servicePrincipalTokenSupport = servicePrincipalTokenSupport;
            this.personalAccessTokenSupport = personalAccessTokenSupport;
            this.config = config;
        }

        public async Task<(string, string)> GetCredentialsAsync(bool elevatedPrivilege = false)
        {
            if (elevatedPrivilege)
            {
                if (servicePrincipalTokenSupport.IsConfigured())
                {
                    return await servicePrincipalTokenSupport.GetCredentialsAsync();
                }
                if(personalAccessTokenSupport.IsConfigured())
                {
                    return personalAccessTokenSupport.GetCredentials();
                }
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
            throw new UnauthorizedAccessException("Request is denied!"); 
        }
    }
}
