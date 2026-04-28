using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SM.Application.Database;

namespace SM.Application;

public static class DependencyInjection
{
    // ...existing code...
    public static void SeedDatabase(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        ApplicationDbContextSeeder.Seed(db);
    }
    // ...existing code...
}
