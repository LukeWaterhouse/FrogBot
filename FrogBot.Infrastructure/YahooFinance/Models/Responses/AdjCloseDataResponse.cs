namespace FrogBot.Infrastructure.YahooFinance.Models.Responses;

public record AdjCloseDataResponse(
    IReadOnlyList<decimal?>? AdjClose
);
