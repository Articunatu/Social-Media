
namespace SM.Application.Abstractions
{
    public interface IBaserRepository<Entity> where Entity : class
    {
        Entity GetEntityById();
        IEnumerable<Entity> GetAllEntities();
        Task Create(); 
        Task Delete(); 
        Task Update(); 
    }
}
