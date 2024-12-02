using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Users.Repositories;
using BookmakerBackend.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookmakerBackend.DataAccess.Repositories;

/// <inheritdoc cref="IUserRepository"/>
public class UserRepository : IUserRepository
{
    private DbContext DbContext { get; }
    private DbSet<User> Users { get; }
    private IMapper _mapper { get; }

    public UserRepository(DbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        _mapper = mapper;
        Users = DbContext.Set<User>();
    }
}