using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Users.Repositories;
using BookmakerBackend.AppServices.Exceptions;
using BookmakerBackend.AppServices.Services;
using BookmakerBackend.Contracts.Users;

namespace BookmakerBackend.AppServices.Contexts.Users.Services;

/// <inheritdoc cref="IUserService"/>
public class UserService(IUserRepository repository, IMapper mapper, IJwtService jwtService) : IUserService
{
    ///<inheritdoc/>
    public async Task RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (await IsUniqueLoginAsync(request.Username, cancellationToken))
        {
            var user = mapper.Map<UserDto>(request);
            user.Role = "User";
            user.Balance = 0;
            await repository.AddAsync(user, cancellationToken);
        }
        else
        {
            throw new LoginAlreadyExistsException();
        }
    }

    ///<inheritdoc/>
    public async Task<string> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken)
    {
        UserDto user;
        try
        {
            user = await repository.LoginAsync(request, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            throw new InvalidLoginDataException();
        }
        
        return jwtService.GetToken(request, user.Role);
    }

    ///<inheritdoc/>
    public async Task DeleteUserAsync(string login, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(login, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<UserDto> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
    {
        return await repository.GetByLoginAsync(login, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task UpdateUserAsync(string login, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByLoginAsync(login, cancellationToken);
        if (user == null) throw new EntityNotFoundException();
        user.Email = request.Email;
        user.Password = request.Password;
        user.Phone = request.Phone;
        user.BirthDate = request.BirthDate;
        user.FullName = request.FullName;
        await repository.UpdateAsync(user, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task UpdateUserRoleAsync(string login, string role, CancellationToken cancellationToken)
    {
        var user = await repository.GetByLoginAsync(login, cancellationToken);
        if (user == null) throw new EntityNotFoundException();
        user.Role = role;
        await repository.UpdateAsync(user, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<ICollection<UserDto>> SearchUsersByStringAsync(string searchString, CancellationToken cancellationToken)
    {
        return await repository.SearchUsersByStringAsync(searchString, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<decimal> GetUserBalanceAsync(string login, CancellationToken cancellationToken)
    {
        var user  = await repository.GetByLoginAsync(login, cancellationToken);
        return user.Balance.Value;
    }

    private async Task<bool> IsUniqueLoginAsync(string login, CancellationToken cancellationToken)
    {
        try
        {
            await repository.GetByLoginAsync(login, cancellationToken);
        }
        catch (EntityNotFoundException)
        {
            return true;
        }
        return false;
    }
}