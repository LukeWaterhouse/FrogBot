namespace FrogBot.Infrastructure.YahooFinance.Models.Responses;

public record ChartIndicatorsResponse(
    IReadOnlyList<QuoteDataResponse>? Quote,
    IReadOnlyList<AdjCloseDataResponse>? AdjClose
);
