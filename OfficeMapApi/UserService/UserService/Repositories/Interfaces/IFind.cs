using System.Collections.Generic;

namespace UserService.Repositories.Interfaces
{
    public interface IFind
    {
        /// <summary>
        /// Represents an interface available for finding entities.
        /// </summary>
        public interface IFind<TEntity, TFilter>
            where TEntity : class
        {
            /// <summary>
            /// Retrieve all entities, satisfying given filter.
            /// </summary>
            IEnumerable<TEntity> Find(TFilter filter);
        }
    }
}
