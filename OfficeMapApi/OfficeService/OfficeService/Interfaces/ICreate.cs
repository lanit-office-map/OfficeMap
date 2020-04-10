using System.Threading.Tasks;

namespace OfficeService.Interfaces
{
    public interface ICreate<TEntity>
       where TEntity : class
    {
       Task<TEntity> CreateAsync(TEntity entity);
    }
}
