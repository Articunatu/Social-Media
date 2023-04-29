
using Models.Models;

namespace Core.Service
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> ReadAll();
        Task<T> ReadSingle(Guid id);
        Task Create(T created);
        Task Update(T updated);
        Task Delete(Guid id);
    }

    public interface IReactionRepository
    {
        Task<IEnumerable<Reaction>> ReadAll();
        Task<Reaction> ReadSingle(int id);
        Task Create(Reaction created);
        Task Update(Reaction updated);
        Task Delete(int id);
    }
}
