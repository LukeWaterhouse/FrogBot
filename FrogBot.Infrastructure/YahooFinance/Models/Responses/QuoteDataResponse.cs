namespace FrogBot.Infrastructure.YahooFinance.Models.Responses;

public record QuoteDataResponse(
    IReadOnlyList<decimal?>? Low,
    IReadOnlyList<decimal?>? Open,
    IReadOnlyList<decimal?>? Close,
    IReadOnlyList<decimal?>? High,
    IReadOnlyList<long?>? Volume
);
