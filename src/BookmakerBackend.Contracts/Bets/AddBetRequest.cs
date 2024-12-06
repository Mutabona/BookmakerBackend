namespace BookmakerBackend.Contracts.Bets;

/// <summary>
/// Запрос на создание ставки.
/// </summary>
public class AddBetRequest
{
    public decimal Amount { get; set; }
}