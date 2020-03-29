namespace UserService.Repositories.Interfaces
{
    /// <summary>
    ///  Represents an interface available for updating entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public interface IUpdate<TEntity>
      where TEntity : class
    {
        /// <summary>
        ///  Update an existing entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        TEntity Update(TEntity entity);
    }
}