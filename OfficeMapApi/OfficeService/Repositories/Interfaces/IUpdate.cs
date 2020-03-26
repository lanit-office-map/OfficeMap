namespace OfficeService.Repositories.Interfaces
{

    public interface IUpdate<TEntity>
      where TEntity : class
    {
        
        TEntity Update(TEntity entity);
    }

    
    public interface IUpdate<TEntity, TOutEntity>
      where TEntity : class
    {
        
        TEntity Update(TEntity entity, out TOutEntity output);
    }
}
