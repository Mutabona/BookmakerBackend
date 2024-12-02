namespace BookmakerBackend.Contracts.Coefficients;

/// <summary>
/// Модель коэффициента.
/// </summary>
public class CoefficientDto
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; }
    
    public string Type { get; set; }

    public decimal Value { get; set; }
}