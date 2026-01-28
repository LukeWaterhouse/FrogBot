using FrogBot.Models;
using FrogBot.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Register FrogBot services
builder.Services.Configure<TradingBotConfig>(
    builder.Configuration.GetSection("TradingBot"));

builder.Services.AddHttpClient<IStockPriceService, YahooFinanceService>();
builder.Services.AddScoped<ITradingLedgerService, AzureTableLedgerService>();
builder.Services.AddScoped<ITradingBotService, TradingBotService>();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
