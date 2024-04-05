namespace ApiSqlite.Infrastructure.Sqlite
{
    public interface IDatabaseBootstrap
    {
        void Setup();

        void EnsureDeleted();
    }
}