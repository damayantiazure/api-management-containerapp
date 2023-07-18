using 'app.bicep'

param imageName = 'neptune-webapi'
param tagName = 'latest' // readEnvironmentVariable('tagName')
param containerRegistryName = 'neptuneimages' 
param acaEnvName = 'neptune-aca' 
param uamiName = 'neptune-app-identity' 
