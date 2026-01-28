namespace FrogBot.Infrastructure.YahooFinance.Models.Responses;

public record ChartMetaResponse(
    string? Symbol,
    string? Currency,
    string? Range
);
