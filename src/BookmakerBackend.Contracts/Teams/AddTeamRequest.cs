namespace BookmakerBackend.Contracts.Teams;

/// <summary>
/// Запрос на добавление команды.
/// </summary>
public class AddTeamRequest
{
    public string? Name { get; set; }

    public string? Country { get; set; }

    public string Sport { get; set; } = null!;
}