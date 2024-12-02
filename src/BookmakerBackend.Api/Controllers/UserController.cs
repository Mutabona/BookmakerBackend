using System.Net;
using BookmakerBackend.Api.Base;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBackend.Api.Controllers;

/// <summary>
/// Пользователи.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class UserController : BaseController
{
    
}