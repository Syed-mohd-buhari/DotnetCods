using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities;
using CAM.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CAM.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
        where T : class
    {

        private bool IsAuditableEntity(IQueryable<T> entity)
        {
            return typeof(T).GetProperty("Modificationdate") != null || typeof(T).GetProperty("Creationdate") != null;
        }
       
        private IQueryable<T> ApplyAuditableFilter(IQueryable<T> entity, bool includeDeleted)
        {
            var isAuditable = IsAuditableEntity(entity); //typeof(T).BaseType == typeof(AuditableEntity);
            if (includeDeleted && !isAuditable)
                throw new ArgumentException($"{nameof(includeDeleted)} = True is applicable only to {nameof(AuditableEntity)}", nameof(includeDeleted));
            if (!includeDeleted == isAuditable)
            {
                var deletedProperty = typeof(T).GetProperty("Deleted");
                if (deletedProperty == null)
                    throw new ArgumentNullException(nameof(deletedProperty),
                        $"Check the Auditable entities, it's not contains a 'Deleted' property as a expected");

                // Create an expression tree that represents the expression  
                // 'entity.Where(x => x.Deleted == false)' 
                var pe = Expression.Parameter(typeof(T), "x");
                var left = Expression.Property(pe, deletedProperty);
                var right = Expression.Constant(false, typeof(bool?));
                var predicateBody = Expression.Equal(left, right);
                var whereCallExpression = Expression.Call(
                    typeof(Queryable),
                    "Where",
                    new[] { entity.ElementType },
                    entity.Expression,
                    Expression.Lambda<Func<T, bool>>(predicateBody, pe));

                return entity.Provider.CreateQuery<T>(whereCallExpression);

            }
            return entity;
        }

        protected RepositoryContext RepositoryContext { get; set; }


        protected ModelContext ModelContext { get; set; }


        protected RepositoryBase(ModelContext repositoryContext)
        {
            this.ModelContext = repositoryContext;
        }
        public int Count(Expression<Func<T, bool>> expression = null, bool includeDeleted = false)
        {
            return expression != null
                ? this.FindByCondition(expression, includeDeleted).Count()
                : this.FindAll(includeDeleted).Count();
        }

        public IQueryable<T> FindAll(bool includeDeleted = false)
        {
                return ApplyAuditableFilter(this.ModelContext.Set<T>(), includeDeleted).AsNoTracking();
        }
        public IQueryable<T> FindAllWithDelete(bool includeDeleted = true)
        {
            return ApplyAuditableFilter(this.ModelContext.Set<T>(), includeDeleted).AsNoTracking();
        }

        public IQueryable<T> FindByConditionWithDelete(Expression<Func<T, bool>> expression, bool includeDeleted = true, bool useTracking = false)
        {
            return ApplyAuditableFilter(this.ModelContext.Set<T>(), includeDeleted).Where(expression).AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool includeDeleted = false, bool useTracking = false)
        {
                return ApplyAuditableFilter(this.ModelContext.Set<T>(), includeDeleted).Where(expression).AsNoTracking();

        }

        public void Create(T entity)
        {
                this.ModelContext.Set<T>().Add(entity);
        }
        public void BulkCreate(List<T> entity)
        {
            this.ModelContext.Set<T>().AddRange(entity);
        }
        public void Update(T entity)
        {

                ModelContext.Entry(entity).State = EntityState.Modified;
        }
        public void BulkUpdate(List<T> entity)
        {            
            this.ModelContext.Set<T>().UpdateRange(entity);
        }

        public void DeleteDeep(T entity)
        {
     


                if (this.ModelContext.Entry(entity).State == EntityState.Detached)
                {
                    this.ModelContext.Attach(entity);
                }
                this.ModelContext.Remove(entity);
                this.ModelContext.SaveChanges();
     
        }
        public void BulkDeepDelete(IEnumerable<T> entities)
        {            
            foreach (var entity in entities)
            {
                if (this.ModelContext.Entry(entity).State == EntityState.Detached)
                {
                    this.ModelContext.Attach(entity);
                }
            }

            this.ModelContext.RemoveRange(entities);
            this.ModelContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            this.ModelContext.Set<T>().Remove(entity);
        }
        public bool IsTracked(T entity)
        {
            return this.ModelContext.Set<T>().Local.Any(e => e == entity);
        }

    }
}
