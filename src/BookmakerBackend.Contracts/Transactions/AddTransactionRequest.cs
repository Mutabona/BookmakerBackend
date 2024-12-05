namespace BookmakerBackend.Contracts.Transactions;

/// <summary>
/// Запрос на создание транзакции.
/// </summary>
public class AddTransactionRequest
{
    public string Type { get; set; }
    
    public decimal Amount { get; set; }
}