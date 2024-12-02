namespace BookmakerBackend.Contracts.Users;

/// <summary>
/// Запрос на авторизацию пользователя.
/// </summary>
public class LoginUserRequest
{
    /// <summary>
    /// Логин.
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; }
}