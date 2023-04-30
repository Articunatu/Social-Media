using API.ViewModels;
using Core.Service;
using Models.Models;
using Xunit;

namespace Test
{
    public class AccountTest
    {
        readonly IRepository<Account> _repo;

        public AccountTest(IRepository<Account> repo)
        {
            _repo = repo;
        }

        [Fact]
        public void Guid_Returns_Profile_With_Fullname_Anders_Salo_Verified_True_And_Tag_andross38()
        {
            //Arrange
            ProfileModel expectedProfile = new()
            {
                Tag = "andross38",
                Fullname = "Anders Salo",
                ProfilePicture = null
            };

            ProfilePageModel profilePage = new(expectedProfile, null);

            //Act
            Guid guid = Guid.Parse("f0218ae7-f567-4ea0-bd6b-5c7e9e91665b");
            var actualProfile = _repo.ReadSingle(guid);
        }
    }
}
