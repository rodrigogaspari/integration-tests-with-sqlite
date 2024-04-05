﻿using Microsoft.Data.Sqlite;
using ApiSqlite.Infrastructure.Sqlite;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ApiSqlite.Infrastructure.Database
{
    public sealed class DbSession : IDisposable
    {
        private Guid _id;
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbSession(DatabaseConfig databaseConfig)
        {
            _id = Guid.NewGuid();
            Connection = new SqliteConnection(databaseConfig.Name);
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
