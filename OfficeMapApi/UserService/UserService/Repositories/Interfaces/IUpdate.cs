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

    /// <summary>
    ///  Represents an interface available for updating entity with some output.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TOutEntity">Out entity type.</typeparam>
    public interface IUpdate<TEntity, TOutEntity>
      where TEntity : class
    {
        /// <summary>
        ///  Update an existing entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="output">Output.</param>
        TEntity Update(TEntity entity, out TOutEntity output);
    }
}