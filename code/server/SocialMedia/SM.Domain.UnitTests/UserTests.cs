using SM.Domain.Users;

namespace SM.Domain.UnitTests;

public class UserTests
{
    [Fact]
    public void GetFullName_FirstAshLastKetchum_ReturnsAshKetchum()
    {
        string expctedFullName = "Ash Ketchum";
        User user = new(Guid.CreateVersion7())
        {
            FirstName = "Ash",
            LastName = "Ketchum"
        };

        string actualFullName = user.GetFullName();

        Assert.Equal(expctedFullName, actualFullName);
    }
}
