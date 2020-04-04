namespace OfficeService.Repositories.Interfaces
{

    public interface IDelete<TEntity>
      where TEntity : class

    {
        void Delete(TEntity entity);
    }
}

