#!/bin/bash

resourceGroupName="APIM-DEVOPS"
location="westeurope"

echo "Starting deploying the app provisioning..."



echo "Deploying app Bicep file..."
az deployment group create --resource-group $resourceGroupName --template-file 'app.bicep'  --parameters app.bicepparam