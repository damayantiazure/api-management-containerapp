
namespace NeptureWebAPI
{
    public class AppConfig
    {
        private const string APPINSIGHT_CONN_STR_KEY = "APPINSIGHT_CONN_STR";
        public const string AZUREDEVOPSCLIENT = "AZUREDEVOPSCLIENT";
        public const string AZUREDEVOPS_IDENTITY_CLIENT = "AZUREDEVOPS_IDENTITY_CLIENT";
        public const string AZDO_URI = "https://dev.azure.com";
        public const string AZDO_IDENTITY_URI = "https://vssps.dev.azure.com";
        private const string AZDO_ORG_KEY = "AZDO_ORG";
        private string orgName;
        private string appInsightConnStr;
        public AppConfig()
        {
            var orgName = System.Environment.GetEnvironmentVariable(AZDO_ORG_KEY);
            ArgumentNullException.ThrowIfNullOrEmpty(orgName, $"Environment variable {AZDO_ORG_KEY} is not set");
            this.orgName = orgName;
            this.appInsightConnStr = (appInsightConnStr != null) ? appInsightConnStr : "NOT-SET";
        }

        public static string? GetAppInsightsConnStrFromEnv()
        {
            return System.Environment.GetEnvironmentVariable(APPINSIGHT_CONN_STR_KEY);
        }

        public string OrgName => orgName;
        public string AppInsightConnStr => appInsightConnStr;
    }
}