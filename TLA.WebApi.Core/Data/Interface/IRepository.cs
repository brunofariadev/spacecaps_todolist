using System;
using TLA.WebApi.Core.DomainObjects;

namespace TLA.WebApi.Core.Data.Interface
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
