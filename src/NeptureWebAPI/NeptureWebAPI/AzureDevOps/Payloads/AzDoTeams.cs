using System.Text.Json.Serialization;

namespace NeptureWebAPI.AzureDevOps.Payloads
{

    public record AzDoTeamCollection(
        [property: JsonPropertyName("value")] IReadOnlyList<AzDoTeam> Value,
        [property: JsonPropertyName("count")] int Count
    );

    public record AzDoTeam(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("identityUrl")] string IdentityUrl
    );


}
