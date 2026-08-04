using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CAM.Contracts.RepositoryContracts.Base
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool includeDeleted = false);

        IQueryable<T> FindByConditionWithDelete(Expression<Func<T, bool>> expression, bool includeDeleted = true, bool useTracking = true);
        IQueryable<T> FindAllWithDelete(bool includeDeleted = true);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool includeDeleted = false, bool useTracking = true);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteDeep(T entity);
        int Count(Expression<Func<T, bool>> expression = null, bool includeDeleted = false);
        bool IsTracked(T entity);
        void BulkCreate(List<T> entity);
        void BulkUpdate(List<T> entity);
        void BulkDeepDelete(IEnumerable<T> entity);
    }
}