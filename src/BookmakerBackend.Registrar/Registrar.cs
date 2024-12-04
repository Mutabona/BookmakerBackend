using AutoMapper;
using BookmakerBackend.AppServices.Contexts.Bets.Repositories;
using BookmakerBackend.AppServices.Contexts.Bets.Services;
using BookmakerBackend.AppServices.Contexts.Coefficients.Repositories;
using BookmakerBackend.AppServices.Contexts.Coefficients.Services;
using BookmakerBackend.AppServices.Contexts.Events.Repositories;
using BookmakerBackend.AppServices.Contexts.Events.Services;
using BookmakerBackend.AppServices.Contexts.Teams.Repositories;
using BookmakerBackend.AppServices.Contexts.Teams.Services;
using BookmakerBackend.AppServices.Contexts.Transactions.Repositories;
using BookmakerBackend.AppServices.Contexts.Transactions.Services;
using BookmakerBackend.AppServices.Contexts.Users.Repositories;
using BookmakerBackend.AppServices.Contexts.Users.Services;
using BookmakerBackend.AppServices.Services;
using BookmakerBackend.DataAccess.Context;
using BookmakerBackend.DataAccess.Repositories;
using BookmakerBackend.Registrar.MapProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookmakerBackend.Registrar;

/// <summary>
/// Класс для регистрации компонентов в IoC-контейнере.
/// </summary>
public static class Registrar
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IBetService, BetService>();
        services.AddTransient<ITransactionService, TransactionService>();
        services.AddTransient<ITeamService, TeamService>();
        services.AddTransient<ICoefficientService, CoefficientService>();
        services.AddTransient<IEventService, EventService>();
        services.AddTransient<IJwtService, JwtService> ();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBetRepository, BetRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<ICoefficientRepository, CoefficientRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        
        services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));
        
        services.AddScoped<DbContext>(s => s.GetRequiredService<ApplicationDbContext>());

        //services.AddFluentValidation();
        
        return services;
    }

    private static MapperConfiguration GetMapperConfiguration()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CoefficientProfile>();
            cfg.AddProfile<UserProfile>();
        });
        
        configuration.AssertConfigurationIsValid();
        return configuration;
    }

    // private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    // {
    //     services.AddValidatorsFromAssemblyContaining<LoginUserRequestValidator>();
    //     services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
    //     services.AddValidatorsFromAssemblyContaining<AddBookRequestValidator>();
    //     services.AddValidatorsFromAssemblyContaining<AddNewsRequestValidator>();
    //     services.AddFluentValidationAutoValidation();
    //
    //     return services;
    // }
}