namespace BookmakerBackend.Contracts.Events;

/// <summary>
/// Модель события.
/// </summary>
public class EventDto
{
    public Guid Id { get; set; }
    
    public string Result { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public Guid? Team1Id { get; set; }

    public Guid? Team2Id { get; set; }

    public string Sport { get; set; } = null!;

    public decimal? Margin { get; set; }
}