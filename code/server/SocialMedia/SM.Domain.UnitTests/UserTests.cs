using FluentAssertions;
using SM.Domain.Users;
using SM.Domain.Users.Extensions;

namespace SM.Domain.UnitTests;

public class UserTests
{
    [Fact]
    public void GetFullName_FirstWolfgangLastGrimner_ReturnsWolfgangWhiteSpaceGrimner()
    {
        User user = new(Guid.CreateVersion7(), "greatsteiner", "Wolfgang", "Grimner", "steiner@gmail.com");

        string fullName = user.GetFullName();

        fullName.Should().Be("Wolfgang Grimner");
    }
}
