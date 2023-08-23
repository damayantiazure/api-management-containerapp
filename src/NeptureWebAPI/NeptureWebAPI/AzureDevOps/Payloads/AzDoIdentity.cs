using System.Text.Json.Serialization;

namespace NeptureWebAPI.AzureDevOps.Payloads
{    
    public record AzDoIdentity(
        [property: JsonPropertyName("entityId")] string EntityId,
        [property: JsonPropertyName("entityType")] string EntityType,
        [property: JsonPropertyName("originDirectory")] string OriginDirectory,
        [property: JsonPropertyName("originId")] string OriginId,
        [property: JsonPropertyName("localDirectory")] object LocalDirectory,
        [property: JsonPropertyName("localId")] object LocalId,
        [property: JsonPropertyName("displayName")] string DisplayName,
        [property: JsonPropertyName("scopeName")] string ScopeName,
        [property: JsonPropertyName("samAccountName")] object SamAccountName,
        [property: JsonPropertyName("active")] object Active,
        [property: JsonPropertyName("subjectDescriptor")] object SubjectDescriptor,
        [property: JsonPropertyName("department")] object Department,
        [property: JsonPropertyName("jobTitle")] object JobTitle,
        [property: JsonPropertyName("mail")] object Mail,
        [property: JsonPropertyName("mailNickname")] string MailNickname,
        [property: JsonPropertyName("physicalDeliveryOfficeName")] object PhysicalDeliveryOfficeName,
        [property: JsonPropertyName("signInAddress")] object SignInAddress,
        [property: JsonPropertyName("surname")] object Surname,
        [property: JsonPropertyName("guest")] bool Guest,
        [property: JsonPropertyName("telephoneNumber")] object TelephoneNumber,
        [property: JsonPropertyName("description")] object Description,
        [property: JsonPropertyName("isMru")] bool IsMru
    );

    public record AzDoIdentityCollection(
        [property: JsonPropertyName("queryToken")] string QueryToken,
        [property: JsonPropertyName("identities")] IReadOnlyList<AzDoIdentity> Identities,
        [property: JsonPropertyName("pagingToken")] string PagingToken
    );

    public record AzDoSearchResponse(
        [property: JsonPropertyName("results")] IReadOnlyList<AzDoIdentityCollection> Results
    );
}
