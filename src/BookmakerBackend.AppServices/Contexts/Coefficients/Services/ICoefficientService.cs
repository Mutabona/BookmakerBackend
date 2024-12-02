using BookmakerBackend.Contracts.Coefficients;

namespace BookmakerBackend.AppServices.Contexts.Coefficients.Services;

/// <summary>
/// Сервис для работы с коеффициентами.
/// </summary>
public interface ICoefficientService
{
    /// <summary>
    /// Получает коэффициенты по идентификатору события.
    /// </summary>
    /// <param name="eventId">Идентификатор события.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция коэффициентов.</returns>
    Task<ICollection<CoefficientDto>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
}