using 'main.bicep'

param uamiName = 'neptune-app-identity'
param containerRegistryName = 'neptuneimages'
param keyvaultName = 'neptunesecretsmha'
param logAnalyticsName = 'neptune-log-analytics'
param appInsightName = 'neptune-app-insight'
param acaEnvName = 'neptune-aca-env'

param apimServiceName = 'neptune-apim'
param publisherEmail = 'moim.hossain@microsoft.com'
param publisherName = 'Neptune Inc.'
param sku = 'Standard'
param skuCount = 1
param vnetName = 'nepturenetwork'
