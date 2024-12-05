using System.Net;
using BookmakerBackend.Api.Base;
using BookmakerBackend.AppServices.Contexts.Transactions.Services;
using BookmakerBackend.Contracts.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookmakerBackend.Api.Controllers;

/// <summary>
/// Транзакции.
/// </summary>
[ApiController]
[Route("[controller]")]
[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
public class TransactionController(ITransactionService service) : BaseController
{
    /// <summary>
    /// Добавляет транзакцию по модели запроса.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> AddAsync(AddTransactionRequest request, CancellationToken token)
    {
        var login = GetCurrentUserUsername();
        await service.AddAsync(login, request, token);
        return Created();
    }

    /// <summary>
    /// Получает транзакции по логину пользователя.
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Коллекция моделей транзакций.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet("{login}")]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [ProducesResponseType(typeof(ICollection<TransactionDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByLoginAsync([FromRoute] string login, CancellationToken token)
    {
        var transactions = await service.GetTransactionsByUserLoginAsync(login, token);
        return Ok(transactions);
    }

    /// <summary>
    /// Получает все транзакции авторизованного пользователя.
    /// </summary>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Коллекция моделей транзакций.</returns>
    [Authorize]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ICollection<TransactionDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserTransactionsAsync(CancellationToken token)
    {
        var login = GetCurrentUserUsername();
        var transactions = await service.GetTransactionsByUserLoginAsync(login, token);
        return Ok(transactions);
    }
}