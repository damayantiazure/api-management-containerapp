#!/bin/bash


resourceGroupName="APIM-DEVOPS"
location="westeurope"

echo "Updating API products..."

echo "Deploying products Bicep file..."
az deployment group create --resource-group $resourceGroupName --template-file products.bicep  --parameters products.bicepparam
