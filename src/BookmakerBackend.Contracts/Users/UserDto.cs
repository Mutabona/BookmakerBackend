namespace BookmakerBackend.Contracts.Users;

/// <summary>
/// Модель пользователя.
/// </summary>
public class UserDto
{
    public string Username { get; set; } = null!;
    
    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public decimal? Balance { get; set; }

    public string? Role { get; set; }
}