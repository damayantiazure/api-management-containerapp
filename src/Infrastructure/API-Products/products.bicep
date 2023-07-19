
param apimServiceName string
param containerAppName string
param productName string
param apiName string
param azureDevOpsEndpoint string
param azureDevOpsEndpointKeyName string

resource apiManagementService 'Microsoft.ApiManagement/service@2023-03-01-preview' existing = {
  name: apimServiceName  
}

resource nameValueEntryForAzureDevOps 'Microsoft.ApiManagement/service/namedValues@2023-03-01-preview' = {
  name: azureDevOpsEndpointKeyName
  parent: apiManagementService
  properties: {
    displayName: azureDevOpsEndpointKeyName
    secret: false
    value: azureDevOpsEndpoint
  }
}



module neptureContainerApp 'container-apps.bicep' = {
  name: containerAppName
  params: {
    containerAppName: containerAppName
  }
}

module neptuneProducts 'neptune-product/neptune-product.bicep' = {
  name: productName
  params: {
    apimServiceName: apimServiceName
    productName: productName
    apiName: apiName
    serviceUrl: 'https://${neptureContainerApp.outputs.neptuneApiBackendFqdn}/'
  }
  dependsOn: [
    neptureContainerApp
  ]
}

output neptuneApiBackendFqdn string = neptureContainerApp.outputs.neptuneApiBackendFqdn
