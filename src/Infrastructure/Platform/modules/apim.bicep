
param apimServiceName string
param location string = resourceGroup().location
param publisherEmail string 
param publisherName string 
param sku string
param skuCount int

resource apiManagementService 'Microsoft.ApiManagement/service@2023-03-01-preview' = {
  name: apimServiceName
  location: location
  sku: {
    name: sku
    capacity: skuCount
  }
  properties: {
    publisherName: publisherName
    publisherEmail: publisherEmail
  }
}


module servicePolicies 'api-policies/azdo-authorization-policy.bicep' = {
  name: '${apiManagementService.name}-service-policy'  
  params: {
    apimServiceName: apimServiceName
  }  
}
