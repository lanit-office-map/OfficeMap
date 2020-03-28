namespace UserService.Repositories.Interfaces
{
    /// <summary>
    /// Represents an interface available for creating a new entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public interface ICreate<TEntity> where TEntity : class
    {
        /// <summary>
        /// Create a new entity.
        /// </summary>
        /// <param name="entity">New entity.</param>
        void Create(TEntity entity);
    }
}
