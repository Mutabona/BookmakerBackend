using BookmakerBackend.Contracts.Users;

namespace BookmakerBackend.AppServices.Services;

/// <summary>
/// Сервис для работы с jwt токенами.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Создание jwt токена.
    /// </summary>
    /// <param name="userData">Запрос на вход <see cref="LoginUserRequest"/>.</param>
    /// <param name="role">Роль пользоваетеля.</param>
    /// <returns>Токен в виде строки.</returns>
    string GetToken(LoginUserRequest userData, string role);
}