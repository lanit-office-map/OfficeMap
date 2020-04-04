namespace OfficeService.Repositories.Interfaces
{

    public interface ICreate<TEntity>
      where TEntity : class
    
    {
        TEntity Create(TEntity entity);
    }
}
