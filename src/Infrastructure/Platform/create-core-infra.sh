#!/bin/bash

resourceGroupName="APIM-DEVOPS"
location="westeurope"

echo "Starting Infrastructure provisioning..."


echo "Creating resource group..."
az group create --name $resourceGroupName --location $location

echo "Deploying main Bicep file..."
#az deployment group create --confirm-with-what-if --resource-group $resourceGroupName --template-file main.bicep  --parameters main.bicepparam

az deployment group create --resource-group $resourceGroupName --template-file main.bicep  --parameters main.bicepparam