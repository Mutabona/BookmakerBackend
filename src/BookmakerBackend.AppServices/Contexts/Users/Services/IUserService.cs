using BookmakerBackend.Contracts.Users;

namespace BookmakerBackend.AppServices.Contexts.Users.Services;

/// <summary>
/// Сервис для работы с пользователями.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Регистрирует пользователя.
    /// </summary>
    /// <param name="request">Запрос на регистрацию.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор зарегистрированного пользователя.</returns>
    Task RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Запрос на авторизацию пользователя.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>JWT</returns>
    Task<string> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Удаляет пользователя по логину.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteUserAsync(string login, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает пользователя по логину.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель пользователя.</returns>
    Task<UserDto> GetUserByLoginAsync(string login, CancellationToken cancellationToken);

    /// <summary>
    /// Обновляет пользователя по модели запроса.
    /// </summary>
    /// <param name="login">Логин пользоваятеля.</param>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task UpdateUserAsync(string login, UpdateUserRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Изменяет роль пользователя.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="role">Новая роль.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task UpdateUserRoleAsync(string login, string role, CancellationToken cancellationToken);
    
    /// <summary>
    /// Выполняет поиск пользователей по строке.
    /// </summary>
    /// <param name="searchString">Строка.</param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>Коллекция моделей пользователей.</returns>
    Task<ICollection<UserDto>> SearchUsersByStringAsync(string searchString, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получает баланс пользователя по логину.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Баланс.</returns>
    Task<decimal> GetUserBalanceAsync(string login, CancellationToken cancellationToken);
}