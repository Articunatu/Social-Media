using Bogus;
using Models.Models;

namespace Core.Data
{
    public class FakeDataGenerator
    {
        public IEnumerable<Account> GenerateFakeAccounts()
        {
            var fakePersonGenerator = new Faker<Account>()
                .RuleFor(a => a.Id, f => f.Random.Guid())
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName);

            var fakePersons = fakePersonGenerator.Generate(10);
            return fakePersons;
        }
    }
}
