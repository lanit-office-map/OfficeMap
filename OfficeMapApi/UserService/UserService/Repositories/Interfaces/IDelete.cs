namespace UserService.Repositories.Interfaces
{
    /// <summary>
    ///  Represents an interface available for deleting an entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public interface IDelete<in TEntity>
    {
        /// <summary>
        ///  Delete an entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        void Delete(TEntity entity);
    }
}