
param containerAppName string

resource neptuneContainerApp 'Microsoft.App/containerApps@2022-03-01' existing = {
  name: containerAppName  
}

output neptuneApiBackendFqdn string = neptuneContainerApp.properties.configuration.ingress.fqdn 
