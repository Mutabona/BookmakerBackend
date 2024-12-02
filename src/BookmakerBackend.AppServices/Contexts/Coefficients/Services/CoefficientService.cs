using BookmakerBackend.AppServices.Contexts.Coefficients.Repositories;
using BookmakerBackend.Contracts.Coefficients;

namespace BookmakerBackend.AppServices.Contexts.Coefficients.Services;

/// <inheritdoc cref="ICoefficientService"/>
public class CoefficientService(ICoefficientRepository repository) : ICoefficientService
{
    /// <inheritdoc/>
    public async Task<ICollection<CoefficientDto>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await repository.GetByEventIdAsync(eventId, cancellationToken);
    }
}