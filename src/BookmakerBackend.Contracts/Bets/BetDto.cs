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

    public string EventName { get; set; }
    
    public string CoefficientType { get; set; }
    
    public Guid? Team1Id { get; set; }

    public Guid? Team2Id { get; set; }
}