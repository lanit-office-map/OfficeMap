using System;

namespace Common.Repositories
{
    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey Guid { get; set; }
    }
}
