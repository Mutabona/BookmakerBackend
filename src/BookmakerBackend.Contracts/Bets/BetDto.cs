namespace BookmakerBackend.Contracts.Bets;

/// <summary>
/// Модель ставки.
/// </summary>
public class BetDto
{
    public Guid? Id { get; set; }

    public Guid CoefficientId { get; set; }
    
    public string Status { get; set; }

    public string Username { get; set; } = null!;

    public decimal Amount { get; set; }
}