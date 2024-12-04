using BookmakerBackend.Contracts.Users;

namespace BookmakerBackend.AppServices.Contexts.Users.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Добавляет пользователя.
    /// </summary>
    /// <param name="user">Новый пользователь.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Идентификатор нового пользователя.</returns>
    Task AddAsync(UserDto user, CancellationToken cancellationToken);

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    /// <param name="request">Запрос на авторизацию <see cref="LoginUserRequest"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task<UserDto> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет пользователя по логину.
    /// </summary>
    /// <param name="username">Логин.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task DeleteAsync(string username, CancellationToken cancellationToken);

    /// <summary>
    /// Получение пользователя по логину.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные пользователя <see cref="UserDto"/></returns>
    Task<UserDto> GetByLoginAsync(string login, CancellationToken cancellationToken);
    
    /// <summary>
    /// Выполняет поиск пользователей по строке.
    /// </summary>
    /// <param name="searchString">Строка.</param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>Коллекция моделей пользователей.</returns>
    Task<ICollection<UserDto>> SearchUsersByStringAsync(string searchString, CancellationToken cancellationToken);
    
    /// <summary>
    /// Обновляет пользователя.
    /// </summary>
    /// <param name="user">Пользователь.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    Task UpdateAsync(UserDto user, CancellationToken cancellationToken);
}