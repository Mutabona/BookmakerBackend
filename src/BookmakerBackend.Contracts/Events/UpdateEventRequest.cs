namespace BookmakerBackend.Contracts.Events;

/// <summary>
/// Запрос на обновление события.
/// </summary>
public class UpdateEventRequest
{
    public string Result { get; set; }

    public string Name { get; set; } = null!;
    
    public string Sport { get; set; } = null!;
    
    public DateTime DateTime { get; set; }
}