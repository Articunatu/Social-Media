using Microsoft.EntityFrameworkCore;
using SM.Application.Database;

namespace SM.Application.UnitTests;

public static class ApplicationDbContextExtensions
{
    public static ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        return new ApplicationDbContext(options);
    }
}
