namespace FrogBot.Infrastructure.YahooFinance.Models.Responses;

public record ChartDataResponse(
    IReadOnlyList<ChartResultResponse>? Result,
    string? Error
);
