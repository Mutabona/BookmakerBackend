using System.Net;
using BookmakerBackend.Api.Base;
using BookmakerBackend.AppServices.Contexts.Users.Services;
using BookmakerBackend.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBackend.Api.Controllers;

/// <summary>
/// Пользователи.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class UserController(IUserService service) : BaseController
{
    /// <summary>
    /// Получает ифнормацию об авторизаванном пользователе.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель пользователя.</returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetUserInfoAsync(CancellationToken cancellationToken)
    {
        var login = GetCurrentUserUsername();
        var userInfo = await service.GetUserByLoginAsync(login, cancellationToken);
        return Ok(userInfo);
    }

    /// <summary>
    /// Получает информацию о пользователе по логину.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Модель пользователя.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("{login}")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
    {
        var userInfo = await service.GetUserByLoginAsync(login, cancellationToken);
        return Ok(userInfo);
    }

    /// <summary>
    /// Обновляет пользователя по модели запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var login = GetCurrentUserUsername();
        await service.UpdateUserAsync(login, request, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Изменяет роль пользователя.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="role">Роль.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("{login}/Role")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> ChangeUserRoleAsync(string login, string role, CancellationToken cancellationToken)
    {
        await service.UpdateUserRoleAsync(login, role, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Выполняет поиск пользователей по строке.
    /// </summary>
    /// <param name="search">Строка.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция моделей пользователей.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("Search/{search}")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ICollection<UserDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SearchByStringAsync(string search, CancellationToken cancellationToken)
    {
        var users = await service.SearchUsersByStringAsync(search, cancellationToken);
        return Ok(users);
    }

    /// <summary>
    /// Получает баланс пользователя.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Баланс пользователя.</returns>
    [Authorize]
    [HttpGet("Balance")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBalanceAsync(CancellationToken cancellationToken)
    {
        var login = GetCurrentUserUsername();
        var balance = await service.GetUserBalanceAsync(login, cancellationToken);
        return Ok(balance);
    }
}