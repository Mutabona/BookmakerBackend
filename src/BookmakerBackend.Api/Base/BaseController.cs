using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBackend.Api.Base;

/// <summary>
/// Базовый контроллер.
/// </summary>
public class BaseController : ControllerBase
{
    /// <summary>
    /// Получает логин аутентифицированного пользователя.
    /// </summary>
    /// <returns>Логин пользователя.</returns>
    protected string GetCurrentUserUsername()
    {
        var username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        return username;
    }

    /// <summary>
    /// Получает роль авторизированного пользователя.
    /// </summary>
    /// <returns>Роль пользователя.</returns>
    protected string GetCurrentUserRole()
    {
        var role = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        return role;
    }
}