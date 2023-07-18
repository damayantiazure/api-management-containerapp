
param apimServiceName string
param apiSwaggerUri string
param productName string
param apiName string

module neptuneProducts 'neptune-product/neptune-product.bicep' = {
  name: productName
  params: {
    apimServiceName: apimServiceName
    productName: productName
    apiName: apiName
    apiSwaggerUri: apiSwaggerUri
  }
}
