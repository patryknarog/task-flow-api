namespace TaskFlow.Infrastructure.Seeders;

public class DatabaseSeeder
{
    private readonly RoleSeeder _roleSeeder;
    private readonly UserSeeder _userSeeder;

    public DatabaseSeeder(RoleSeeder roleSeeder, UserSeeder userSeeder)
    {
        _roleSeeder = roleSeeder;
        _userSeeder = userSeeder;
    }

    public async Task SeedAsync()
    {
        await _roleSeeder.SeedAsync();
        await _userSeeder.SeedAsync();
    }
}