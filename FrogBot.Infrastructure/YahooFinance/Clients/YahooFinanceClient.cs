using System.Text.Json;
using FrogBot.Infrastructure.Interfaces;
using FrogBot.Infrastructure.Models.Responses;

namespace FrogBot.Infrastructure.Clients;

public class YahooFinanceClient(HttpClient httpClient) : IYahooFinanceClient
{
    public async Task<YahooFinanceChartResponse> GetChartAsync(string symbol, string range)
    {
        var url = $"https://query1.finance.yahoo.com/v8/finance/chart/{symbol}?range={range}&interval=1d";
        return await GetResponseAsync<YahooFinanceChartResponse>(url, CancellationToken.None);
    }
    
    private async Task<T> GetResponseAsync<T>(string url, CancellationToken ct)
    {
        var response = await httpClient.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(ct);

        return JsonSerializer.Deserialize<T>(content) ?? throw new Exception("Failed to deserialize response");
    }
}