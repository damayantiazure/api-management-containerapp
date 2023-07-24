
namespace NeptureWebAPI
{
    public class AppConfig
    {
        public const string AZUREDEVOPSCLIENT = "AZUREDEVOPSCLIENT";
        public const string AZDO_URI = "https://dev.azure.com";
        private const string AZDO_ORG_KEY = "AZDO_ORG";
        private string orgName;
        public AppConfig()
        {
            var orgName = System.Environment.GetEnvironmentVariable(AZDO_ORG_KEY);
            ArgumentNullException.ThrowIfNullOrEmpty(orgName, $"Environment variable {AZDO_ORG_KEY} is not set");
            this.orgName = orgName;
        }

        public string OrgName => orgName;
    }
}