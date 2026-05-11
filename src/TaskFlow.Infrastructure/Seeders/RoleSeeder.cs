using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TaskFlow.Domain.Constants;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Seeders;

public class RoleSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<RoleSeeder> _logger;

    public RoleSeeder(RoleManager<ApplicationRole> roleManager, ILogger<RoleSeeder> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        var rolesToSeed = GetRoles();

        foreach (var role in rolesToSeed)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                _logger.LogWarning("Skipped role with empty name.");
                continue;
            }

            if (await _roleManager.RoleExistsAsync(role.Name))
            {
                _logger.LogInformation("Role {RoleName} already exists.", role.Name);
                continue;
            }

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                _logger.LogInformation("Role {RoleName} created successfully.", role.Name);
                continue;
            }

            var errors = string.Join(", ", result.Errors.Select(x => x.Description));

            _logger.LogError(
                "Failed to create role {RoleName}. Errors: {Errors}",
                role.Name,
                errors
            );
        }
    }

    private static IEnumerable<ApplicationRole> GetRoles() =>
    [
        new()
        {
            Name = RoleNames.User,
            Description = "Default user with basic access."
        },
        new()
        {
            Name = RoleNames.Manager,
            Description = "Manager with team management access."
        },
        new()
        {
            Name = RoleNames.Admin,
            Description = "Administrator with full system access."
        }
    ];
}