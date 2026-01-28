namespace FrogBot.Infrastructure.YahooFinance.Models.Responses;

public record ChartResultResponse(
    ChartMetaResponse? Meta,
    IReadOnlyList<long>? Timestamp,
    ChartIndicatorsResponse? Indicators
);
