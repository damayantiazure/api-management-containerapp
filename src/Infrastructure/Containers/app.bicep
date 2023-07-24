targetScope = 'resourceGroup'

param imageName string
param tagName string
param containerRegistryName string 
param location string = resourceGroup().location
param acaEnvName string 
param uamiName string
param azureDevOpsOrg string 

resource acaEnvironment 'Microsoft.App/managedEnvironments@2022-03-01'  existing = {   name: acaEnvName }
resource uami 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' existing = { name: uamiName }

module frontendApp 'modules/http-app.bicep' = {
  name: imageName
  params: {    
    location: location
    containerAppName: imageName
    environmentName: acaEnvironment.name    
    revisionMode: 'Single'    
    hasIdentity: true
    userAssignedIdentityName: uami.name
    containerImage: '${containerRegistryName}.azurecr.io/${imageName}:${tagName}'
    containerRegistry: '${containerRegistryName}.azurecr.io'
    isPrivateRegistry: true
    containerRegistryUsername: ''
    registryPassword: ''    
    useManagedIdentityForImagePull: true
    containerPort: 80
    enableIngress: true
    isExternalIngress: true // external ingress for a vent app is still a private IP
    minReplicas: 1
    env: [
      {
        name: 'AZDO_ORG'
        value: azureDevOpsOrg
      }
    ]
  }
}
