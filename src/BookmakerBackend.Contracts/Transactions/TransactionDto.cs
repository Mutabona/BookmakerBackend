namespace BookmakerBackend.Contracts.Transactions;

/// <summary>
/// Модель транзакции.
/// </summary>
public class TransactionDto
{
    public Guid? Id { get; set; }
    
    public string Type { get; set; }

    public string Username { get; set; } = null!;

    public DateTime? DateTime { get; set; }

    public decimal Amount { get; set; }
}