namespace BookmakerBackend.Contracts.Users;

/// <summary>
/// Запрос на обновление пользователя.
/// </summary>
public class UpdateUserRequest
{
    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}