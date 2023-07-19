
param apimServiceName string
param containerAppName string
param productName string
param apiName string

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
