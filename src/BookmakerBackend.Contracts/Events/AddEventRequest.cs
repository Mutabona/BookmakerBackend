namespace BookmakerBackend.Contracts.Events;

/// <summary>
/// Запрос на добавление события.
/// </summary>
public class AddEventRequest
{
    public string Name { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public Guid? Team1Id { get; set; }

    public Guid? Team2Id { get; set; }

    public string Sport { get; set; } = null!;

    public decimal? Margin { get; set; }
}