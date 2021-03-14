namespace TelRehber.Mongo
{
    public abstract class RepositoryFactory
    {
        public abstract IRepository<TEntity> Build<TEntity>(string collectionName) where TEntity : class;
    }
}