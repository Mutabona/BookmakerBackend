using BookmakerBackend.Contracts.Coefficients;

namespace BookmakerBackend.AppServices.Contexts.Coefficients.Repositories;

/// <summary>
/// Репозиторий для работы с коеффициентами.
/// </summary>
public interface ICoefficientRepository
{
    /// <summary>
    /// Получает коэффициенты по идентификатору события.
    /// </summary>
    /// <param name="eventId">Идентификатор события.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция коэффициентов.</returns>
    Task<ICollection<CoefficientDto>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
}