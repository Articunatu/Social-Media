using Models.Models;

namespace Core.Service
{
    public interface IReactionRepository
    {
        Task<IEnumerable<Reaction>> ReadAll();
        Task<Reaction> ReadSingle(int id);
        Task Create(Reaction created);
        Task Update(Reaction updated);
        Task Delete(int id);
    }
}
