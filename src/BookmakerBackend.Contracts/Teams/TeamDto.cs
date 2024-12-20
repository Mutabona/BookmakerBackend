namespace BookmakerBackend.Contracts.Teams;

/// <summary>
/// Модель команды.
/// </summary>
public class TeamDto
{
    public Guid? Id { get; set; }

    public string? Name { get; set; }

    public string? Country { get; set; }

    public string Sport { get; set; } = null!;
}