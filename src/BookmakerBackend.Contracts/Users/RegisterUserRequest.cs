namespace BookmakerBackend.Contracts.Users;

/// <summary>
/// Запрос на создание пользователя.
/// </summary>
public class RegisterUserRequest
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }
}