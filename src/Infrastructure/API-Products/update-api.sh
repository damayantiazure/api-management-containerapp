#!/bin/bash


export resourceGroupName="APIM-DEVOPS-Coantapp"
export location="westeurope"
export APP_NAME="solardemo1"
export APP_ENV="dev"

echo "Updating API products..."

echo "Deploying products Bicep file..."
az deployment group create --resource-group $resourceGroupName --template-file products.bicep  --parameters products.bicepparam
