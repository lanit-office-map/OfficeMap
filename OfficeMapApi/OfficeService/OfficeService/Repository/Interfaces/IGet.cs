using System.Threading.Tasks;

namespace OfficeService.Repository.Interfaces
{ 
    public interface IGet<out TEntity, in TKey>
        where TEntity : class
    {
        Task GetAsync(TKey id);
    }
}
