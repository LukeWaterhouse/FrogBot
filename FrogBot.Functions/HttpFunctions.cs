using FrogBot.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FrogBot.Functions;

public class HttpFunctions
{
    private readonly ITradingBotService _tradingBot;
    private readonly ITradingLedgerService _ledger;
    private readonly ILogger<HttpFunctions> _logger;

    public HttpFunctions(ITradingBotService tradingBot, ITradingLedgerService ledger, ILogger<HttpFunctions> logger)
    {
        _tradingBot = tradingBot;
        _ledger = ledger;
        _logger = logger;
    }

    [Function("GetStatus")]
    public async Task<HttpResponseData> GetStatus([HttpTrigger(AuthorizationLevel.Function, "get", Route = "status")] HttpRequestData req)
    {
        var state = await _tradingBot.GetCurrentStateAsync();
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            state.CashBalance,
            Position = state.CurrentPosition,
            state.LastUpdated
        });
        return response;
    }

    [Function("GetTrades")]
    public async Task<HttpResponseData> GetTrades([HttpTrigger(AuthorizationLevel.Function, "get", Route = "trades")] HttpRequestData req)
    {
        var trades = await _ledger.GetRecentTradesAsync(30);
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(trades);
        return response;
    }

    [Function("Preview")]
    public async Task<HttpResponseData> Preview([HttpTrigger(AuthorizationLevel.Function, "get", Route = "preview")] HttpRequestData req)
    {
        var decision = await _tradingBot.MakeDailyDecisionAsync();
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(decision);
        return response;
    }

    [Function("TriggerManual")]
    public async Task<HttpResponseData> TriggerManual([HttpTrigger(AuthorizationLevel.Function, "post", Route = "trigger")] HttpRequestData req)
    {
        _logger.LogInformation("Manual trading cycle triggered");
        
        var decision = await _tradingBot.MakeDailyDecisionAsync();
        await _tradingBot.ExecuteDecisionAsync(decision);
        var state = await _tradingBot.GetCurrentStateAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            Decision = decision,
            CurrentState = state
        });
        return response;
    }
}

