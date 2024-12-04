using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookmakerBackend.AppServices.Contexts.Users.Repositories;
using BookmakerBackend.AppServices.Exceptions;
using BookmakerBackend.Contracts.Users;
using BookmakerBackend.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookmakerBackend.DataAccess.Repositories;

/// <inheritdoc cref="IUserRepository"/>
public class UserRepository : IUserRepository
{
    private DbContext DbContext { get; }
    private DbSet<User> Users { get; }
    private IMapper Mapper { get; }

    public UserRepository(DbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        Mapper = mapper;
        Users = DbContext.Set<User>();
    }

    ///<inheritdoc/>
    public async Task AddAsync(UserDto user, CancellationToken cancellationToken)
    {
        var userEntity = Mapper.Map<User>(user);
        await Users.AddAsync(userEntity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<UserDto> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await Users.Where(x => x.Password == request.Password && x.Username == request.Username).FirstOrDefaultAsync(cancellationToken);

        if (user == null) throw new EntityNotFoundException();
        
        return Mapper.Map<UserDto>(user);
    }

    ///<inheritdoc/>
    public async Task DeleteAsync(string username, CancellationToken cancellationToken)
    {
        var user = await Users.Where(x => x.Username == username).FirstOrDefaultAsync(cancellationToken);
        if (user == null) throw new EntityNotFoundException();
        Users.Remove(user);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<UserDto> GetByLoginAsync(string login, CancellationToken cancellationToken)
    {
        var user = await Users.AsNoTracking().Where(x => x.Username == login).FirstOrDefaultAsync(cancellationToken);
        if (user == null) throw new EntityNotFoundException();
        
        return Mapper.Map<UserDto>(user);
    }

    ///<inheritdoc/>
    public async Task<ICollection<UserDto>> SearchUsersByStringAsync(string searchString, CancellationToken cancellationToken)
    {
        var users = await Users
            .AsNoTracking()
            .Where(x => x.FullName.ToLower().Contains(searchString.ToLower()) || x.Username.ToLower().Contains(searchString.ToLower()))
            .ProjectTo<UserDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return users;
    }

    ///<inheritdoc/>
    public async Task UpdateAsync(UserDto user, CancellationToken cancellationToken)
    {
        var userEntity = Mapper.Map<User>(user);
        Users.Update(userEntity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}