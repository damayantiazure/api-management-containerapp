
param apimServiceName string
param productName string = 'neptune-product'
param apiName string = 'neptune-api'
param serviceUrl string

resource neptuneWebApi 'Microsoft.ApiManagement/service/apis@2023-03-01-preview' = {
  name: '${apimServiceName}/${apiName}'  
  properties: {
    format: 'openapi+json'
    value: loadTextContent('neptune-webapi-swagger.json')
    path: apiName
    subscriptionRequired: false
    serviceUrl: serviceUrl
  }
  
  resource policy 'policies@2023-03-01-preview' = {    
    name: 'policy'
    properties: {
      format: 'rawxml'
      value: loadTextContent('../policies/azdo-authorization-policy.xml')
    }
  }
}

resource neptuneWebApiWithProduct 'Microsoft.ApiManagement/service/products/apis@2023-03-01-preview' = {
  name: '${apimServiceName}/${productName}/${apiName}'
  dependsOn: [
    neptuneWebApi
  ]
}
