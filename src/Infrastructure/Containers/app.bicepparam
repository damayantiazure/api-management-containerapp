using 'app.bicep'

param imageName = 'neptune-webapi'
param tagName = 'latest' // readEnvironmentVariable('tagName')
param containerRegistryName = 'neptuneimages' 
param acaEnvName = 'neptune-aca-env' 
param uamiName = 'neptune-app-identity' 
