using Domain.Abstraction;
using Domain.RepositoryInterfaces;
using Infrastracture.Data;
using Infrastracture.RepositoryImplementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastracture;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastracture(this IServiceCollection services)
    {
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IStadiumRepository, StadiumRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.TryAddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        return services;
    }
}
