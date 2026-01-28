#!/bin/bash

# Variables - update these to be unique
RESOURCE_GROUP="frogbot-rg"
LOCATION="uksouth"
STORAGE_ACCOUNT="frogbotstor$(openssl rand -hex 4)"
FUNCTION_APP="frogbot-func-$(openssl rand -hex 4)"

echo "Creating resources with:"
echo "  Storage: $STORAGE_ACCOUNT"
echo "  Function: $FUNCTION_APP"

# Login to Azure
az login

# Create Resource Group
az group create --name $RESOURCE_GROUP --location $LOCATION

# Create Storage Account (used for both Functions runtime AND your table storage)
az storage account create \
    --name $STORAGE_ACCOUNT \
    --resource-group $RESOURCE_GROUP \
    --location $LOCATION \
    --sku Standard_LRS

# Get Storage connection string
STORAGE_CONNECTION=$(az storage account show-connection-string \
    --name $STORAGE_ACCOUNT \
    --resource-group $RESOURCE_GROUP \
    --query connectionString --output tsv)

# Create Function App (Consumption plan = pay per execution)
az functionapp create \
    --name $FUNCTION_APP \
    --resource-group $RESOURCE_GROUP \
    --storage-account $STORAGE_ACCOUNT \
    --consumption-plan-location $LOCATION \
    --runtime dotnet-isolated \
    --runtime-version 8 \
    --functions-version 4 \
    --os-type Linux

# Configure app settings
az functionapp config appsettings set \
    --name $FUNCTION_APP \
    --resource-group $RESOURCE_GROUP \
    --settings \
    "ConnectionStrings:AzureStorage=$STORAGE_CONNECTION" \
    "TradingBot:InitialCapital=50" \
    "TradingBot:TakeProfitThresholdPct=2" \
    "TradingBot:BuyCandidateScoreThreshold=-2" \
    "TradingBot:MaxTradesPerWeek=3" \
    "TradingBot:CooldownDays=2" \
    "TradingBot:StopLossPct=-5" \
    "TradingBot:TransactionCostPct=0.1" \
    "TradingBot:Universe:0=SPY" \
    "TradingBot:Universe:1=QQQ" \
    "TradingBot:Universe:2=AAPL" \
    "TradingBot:Universe:3=MSFT" \
    "TradingBot:Universe:4=GOOGL" \
    "TradingBot:Universe:5=AMZN" \
    "TradingBot:Universe:6=NVDA" \
    "TradingBot:Universe:7=META" \
    "TradingBot:Universe:8=TSLA" \
    "TradingBot:Universe:9=JPM"

# Deploy the function
cd /Users/luke.waterhouse/Repos/FrogBot/FrogBot.Functions
func azure functionapp publish $FUNCTION_APP

echo ""
echo "=== Deployment Complete ==="
echo "Function App: https://$FUNCTION_APP.azurewebsites.net"
echo "Storage Account: $STORAGE_ACCOUNT"
echo ""
echo "To get your function keys:"
echo "  az functionapp keys list --name $FUNCTION_APP --resource-group $RESOURCE_GROUP"
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "ConnectionStrings:AzureStorage": "UseDevelopmentStorage=true",
    "TradingBot:InitialCapital": "50",
    "TradingBot:TakeProfitThresholdPct": "2",
    "TradingBot:BuyCandidateScoreThreshold": "-2",
    "TradingBot:MaxTradesPerWeek": "3",
    "TradingBot:CooldownDays": "2",
    "TradingBot:StopLossPct": "-5",
    "TradingBot:TransactionCostPct": "0.1",
    "TradingBot:Universe:0": "SPY",
    "TradingBot:Universe:1": "QQQ",
    "TradingBot:Universe:2": "AAPL",
    "TradingBot:Universe:3": "MSFT",
    "TradingBot:Universe:4": "GOOGL",
    "TradingBot:Universe:5": "AMZN",
    "TradingBot:Universe:6": "NVDA",
    "TradingBot:Universe:7": "META",
    "TradingBot:Universe:8": "TSLA",
    "TradingBot:Universe:9": "JPM"
  }
}

