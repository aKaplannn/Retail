using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TelRehber.Mongo
{
    public class MongoDBRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _mongoCollection;

        public MongoDBRepository(IMongoCollection<TEntity> mongoCollection)
        {
            _mongoCollection = mongoCollection;
        }

        public async Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            await _mongoCollection.DeleteOneAsync<TEntity>(filterExpression);
        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            await _mongoCollection.DeleteManyAsync<TEntity>(filterExpression);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _mongoCollection.AsQueryable<TEntity>();
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return await _mongoCollection.Find<TEntity>(filterExpression).Limit(1).SingleOrDefaultAsync<TEntity>();
        }

        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return await _mongoCollection.Find<TEntity>(filterExpression).ToListAsync();
        }

        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression, FindOptions findOptions)
        {
            SortDefinition<TEntity> sort = null;

            if (findOptions.SortDefinition != null)
                if (findOptions.SortDefinition.Any())
                {
                    List<SortDefinition<TEntity>> sortDefinitionList = new List<SortDefinition<TEntity>>();

                    foreach (SortDefinitionItem sortDefinitionItem in findOptions.SortDefinition)
                        if (!sortDefinitionItem.Descending)
                            sortDefinitionList.Add(Builders<TEntity>.Sort.Ascending(sortDefinitionItem.FieldName));
                        else
                            sortDefinitionList.Add(Builders<TEntity>.Sort.Descending(sortDefinitionItem.FieldName));

                    sort = Builders<TEntity>.Sort.Combine(sortDefinitionList);
                }

            if (sort == null)
                return await _mongoCollection.Find<TEntity>(filterExpression).Skip(findOptions.Skip).Limit(findOptions.Limit).ToListAsync();
            else
                return await _mongoCollection.Find<TEntity>(filterExpression).Sort(sort).Skip(findOptions.Skip).Limit(findOptions.Limit).ToListAsync();
        }



        public async Task InsertOneAsync(TEntity entity)
        {
            await _mongoCollection.InsertOneAsync(entity);
        }

        public async Task InsertManyAsync(List<TEntity> entityList)
        {
            await _mongoCollection.InsertManyAsync(entityList);
        }

        public async Task ReplaceOneAsync(Expression<Func<TEntity, bool>> filterExpression, TEntity entityToUpdate, bool upsert = false)
        {
            await _mongoCollection.ReplaceOneAsync<TEntity>(filterExpression, entityToUpdate, options: new ReplaceOptions { IsUpsert = upsert });
        }

        public async Task UpdateOneAsync(Expression<Func<TEntity, bool>> filterExpression, Dictionary<string, object> fieldsToUpdate, bool upsert = false)
        {
            var update = Builders<TEntity>.Update;
            var updates = new List<UpdateDefinition<TEntity>>();

            foreach (KeyValuePair<string, object> entry in fieldsToUpdate)
                updates.Add(update.Set(entry.Key, entry.Value));

            await _mongoCollection.UpdateOneAsync<TEntity>(filterExpression, update.Combine(updates), new UpdateOptions { IsUpsert = upsert });
        }

        public async Task UpdateManyAsync(Expression<Func<TEntity, bool>> filterExpression, Dictionary<string, object> fieldsToUpdate, bool upsert = false)
        {
            var update = Builders<TEntity>.Update;
            var updates = new List<UpdateDefinition<TEntity>>();

            foreach (KeyValuePair<string, object> entry in fieldsToUpdate)
                updates.Add(update.Set(entry.Key, entry.Value));

            await _mongoCollection.UpdateManyAsync<TEntity>(filterExpression, update.Combine(updates), new UpdateOptions { IsUpsert = upsert });
        }

    }
}
