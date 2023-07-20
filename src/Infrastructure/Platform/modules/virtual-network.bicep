
param location string = resourceGroup().location
param vnetName string
param addressPrefix string = '10.0.0.0/16'
param containerSubnetAddressPrefix string = '10.0.2.0/23'


resource virtualNetwork 'Microsoft.Network/virtualNetworks@2021-05-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        addressPrefix
      ]
    }
    subnets: [
      {
        name: 'default'
        properties: {
          addressPrefix: containerSubnetAddressPrefix
        }
      }
    ]
  }
}

resource vnet 'Microsoft.Network/virtualNetworks@2023-02-01' existing = {
  name: virtualNetwork.name
}

resource defaultSubnet 'Microsoft.Network/virtualNetworks/subnets@2023-02-01' existing = {
  parent: vnet
  name: 'default'
}

output defaultSubnetId string = defaultSubnet.id
