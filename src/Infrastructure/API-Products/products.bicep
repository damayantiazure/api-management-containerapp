
param apimServiceName string
param apiSwaggerUri string
param productName string = 'neptune-product'
param apiName string = 'neptune-api'

module neptuneProducts 'neptune-product/neptune-product.bicep' = {
  name: productName
  params: {
    apimServiceName: apimServiceName
    productName: productName
    apiName: apiName
    apiSwaggerUri: apiSwaggerUri
  }
}
