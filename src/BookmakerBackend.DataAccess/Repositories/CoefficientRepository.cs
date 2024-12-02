using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookmakerBackend.AppServices.Contexts.Coefficients.Repositories;
using BookmakerBackend.Contracts.Coefficients;
using BookmakerBackend.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookmakerBackend.DataAccess.Repositories;

/// <inheritdoc cref="ICoefficientRepository"/>
public class CoefficientRepository : ICoefficientRepository
{
    private DbContext DbContext { get; }
    private DbSet<Coefficient> Coefficients { get; }

    private readonly IMapper _mapper;

    public CoefficientRepository(DbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        Coefficients = DbContext.Set<Coefficient>();
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<ICollection<CoefficientDto>> GetByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var coefficients = await Coefficients
            .Include(co => co.Event)
            .Where(co => co.Event.Id == eventId)
            .ProjectTo<CoefficientDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return coefficients;
    }
}