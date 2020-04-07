namespace OfficeService.Repository.Interfaces
{
    public interface IDelete<in TEntity>
    {
        void Delete(TEntity entity);
    }
}

