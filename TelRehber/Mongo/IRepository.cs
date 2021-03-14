using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TelRehber.Mongo
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression);

        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression);

        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression, FindOptions findOptions);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression);

        Task InsertOneAsync(TEntity entity);

        Task InsertManyAsync(List<TEntity> entityList);

        Task ReplaceOneAsync(Expression<Func<TEntity, bool>> filterExpression, TEntity entityToUpdate, bool upsert = false);

        Task UpdateOneAsync(Expression<Func<TEntity, bool>> filterExpression, Dictionary<string, object> fieldsToUpdate, bool upsert = false);

        Task UpdateManyAsync(Expression<Func<TEntity, bool>> filterExpression, Dictionary<string, object> fieldsToUpdate, bool upsert = false);
    }
}
