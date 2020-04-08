using System.Threading.Tasks;

namespace OfficeService.Repository.Interfaces
{
    public interface ICreate<TEntity>
       where TEntity : class
    {
       Task CreateAsync(TEntity entity);
    }
}
