using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TaskFlow.Domain.Constants;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Seeders;

public class UserSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserSeeder> _logger;

    public UserSeeder(UserManager<ApplicationUser> userManager, ILogger<UserSeeder> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        var users = GetSeedData();

        foreach (var seedUser in users)
        {
            var email = seedUser.Email.Trim().ToLower();
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    FirstName = seedUser.FirstName,
                    LastName = seedUser.LastName,
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createResult = await _userManager.CreateAsync(user, seedUser.Password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(x => x.Description));

                    _logger.LogError(
                        "Failed to create user {Email}: {Errors}",
                        email,
                        errors);

                    continue;
                }

                _logger.LogInformation("User {Email} created successfully.", email);
            }
            else
            {
                _logger.LogInformation("User {Email} already exists.", email);
            }

            if (!await _userManager.IsInRoleAsync(user, seedUser.RoleName))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, seedUser.RoleName);

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(
                        "Role {Role} assigned to user {Email}.",
                        seedUser.RoleName,
                        email);
                }
                else
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(x => x.Description));

                    _logger.LogError(
                        "Failed to assign role {Role} to {Email}: {Errors}",
                        seedUser.RoleName,
                        email,
                        errors
                    );
                }
            }
            else
            {
                _logger.LogInformation( "User {Email} is already in role {Role}.",
                    email, 
                    seedUser.RoleName
                );
            }
        }
    }

    private static IEnumerable<SeedUser> GetSeedData() =>
    [
        new("Admin", "Admin", "admin@admin.local", "Admin123!", RoleNames.Admin),
        new("Manager", "Manager", "manager@manager.local", "Admin123!", RoleNames.Manager),
        new("User", "User", "user@user.local", "Admin123!", RoleNames.User)
    ];

    private sealed record SeedUser(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string RoleName
    );
}