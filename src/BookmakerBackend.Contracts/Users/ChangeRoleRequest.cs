namespace BookmakerBackend.Contracts.Users;

/// <summary>
/// Запрос на изменение роли.
/// </summary>
public class ChangeRoleRequest
{
    public string Role { get; set; }
}