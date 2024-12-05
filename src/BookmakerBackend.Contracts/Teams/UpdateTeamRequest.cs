namespace BookmakerBackend.Contracts.Teams;

/// <summary>
/// Запрос на изменение команды.
/// </summary>
public class UpdateTeamRequest
{
    public string? Name { get; set; }

    public string? Country { get; set; }
}