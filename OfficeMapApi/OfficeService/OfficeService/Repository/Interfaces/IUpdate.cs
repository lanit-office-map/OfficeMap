namespace OfficeService.Repository.Interfaces
{
        public interface IUpdate<TEntity>
            where TEntity : class
        {
            TEntity Update(TEntity entity);
        }
}

