using 'app.bicep'

param imageName = 'neptune-webapi'
param tagName = 'beta2' // readEnvironmentVariable('tagName')
param containerRegistryName = 'neptuneimages' 
param acaEnvName = 'neptune-aca-env' 
param uamiName = 'neptune-app-identity' 
param appInsightName = 'neptune-app-insight'
param azureDevOpsOrg = 'moim' 
