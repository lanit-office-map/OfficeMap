namespace UserService.Repositories.Interfaces
{
    /// <summary>
    /// Represents an interface used to get a shallow entity by its id.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TKey">Key type.</typeparam>
    public interface IGet<out TEntity, in TKey> where TEntity : class
    {
        /// <summary>
        /// Get an entity by id.
        /// </summary>
        /// <param name="id">Id.</param>
        TEntity Get(TKey id);
    }
}
