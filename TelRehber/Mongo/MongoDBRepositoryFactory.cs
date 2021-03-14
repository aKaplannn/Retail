using MongoDB.Driver;
namespace TelRehber.Mongo
{
    public class MongoDBRepositoryFactory : RepositoryFactory
    {
        private readonly IMongoDatabase _mongoDatabase;
        public MongoDBRepositoryFactory(IMongoClient mongoClient, string databaseName)
        {
            _mongoDatabase = mongoClient.GetDatabase(databaseName);
        }
        override
        public IRepository<TEntity> Build<TEntity>(string collectionName) where TEntity : class
        {
            return new MongoDBRepository<TEntity>(_mongoDatabase.GetCollection<TEntity>(collectionName));
        }
    }
}