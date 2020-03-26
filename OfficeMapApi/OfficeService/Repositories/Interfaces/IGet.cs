namespace OfficeService.Repositories.Interfaces
{

    public interface IGet<out TEntity, in TKey>
      where TEntity : class
    {
        
        TEntity Get(TKey id);
    }
}
