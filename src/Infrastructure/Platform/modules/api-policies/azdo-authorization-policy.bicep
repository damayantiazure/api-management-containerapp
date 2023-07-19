
param apimServiceName string

resource policy 'Microsoft.ApiManagement/service/policies@2023-03-01-preview' = {
  name: '${apimServiceName}/policy'
  properties: {
    format: 'rawxml'
    value: loadTextContent('azdo-authorization-policy.xml')
  }
}
