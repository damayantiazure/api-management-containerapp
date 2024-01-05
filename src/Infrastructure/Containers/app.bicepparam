using 'app.bicep'

var appname = readEnvironmentVariable('APP_NAME')
var appEnv = readEnvironmentVariable('APP_ENV')



param uamiName = 'solardemo1-uami-dev'

param imageName = 'neptune-webapi'
param tagName = readEnvironmentVariable('TAG_NAME')
param containerRegistryName = '${appname}contregistry${appEnv}'
param acaEnvName = '${appname}-appenv-${appEnv}'

param appInsightName = '${appname}-appinsights-${appEnv}'
param azureDevOpsOrg = 'moim' 

