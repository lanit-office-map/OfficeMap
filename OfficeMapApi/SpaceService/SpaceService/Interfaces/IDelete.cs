using System.Threading.Tasks;

namespace SpaceService.Interfaces
{
    public interface IDelete <in TEntity>
    {
        Task DeleteAsync(TEntity entity);
    }
}
