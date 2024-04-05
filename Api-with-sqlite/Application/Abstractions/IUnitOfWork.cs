using System;

namespace ApiSqlite.Application.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
