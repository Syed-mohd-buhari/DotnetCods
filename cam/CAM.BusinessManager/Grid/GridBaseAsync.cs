using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Entities.Models.Base;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.Grid
{
    public abstract partial class GridBaseAsync<TEntity, TEntityDto, TQueryObject,T> : IGridBaseAsync<TEntity, TEntityDto, TQueryObject,T>
        where TEntity : AuditableEntity
        where TQueryObject : IQueryObject
        where TEntityDto : IGridDtoBase
        where T :class
    {
        private readonly GridCustomColumnManager _customColumnManager;
        public GridBaseAsync(GridCustomColumnManager customColumnManager, IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, out IRepositoryWrapper repositoryWrapper)
        {
            _customColumnManager = customColumnManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

        }
        public async Task<QueryResultDto<TEntityDto>> GetEnityGrid(TQueryObject request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);
            var query = PrepareQuery(request,null, predicateResult);
            OtherFilter(ref query);
            int numberOfElements = query.Count();
            query = query.ToList().AsQueryable().ApplyOrdering(request, GetColumnsMap()).ApplyPaging(request);
            
            var dataResult = CastObjectToDto(query);
            return new QueryResultDto<TEntityDto>(new GenerateRenderForGrid<TEntityDto>(_customColumnManager))
            {
                Items = dataResult,
                TotalItems = numberOfElements,
            };
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, TQueryObject request)
        {
            request.PageSize = 0;
            request.Page = 1;
            var predicateResult = ApplyFilterForOracleModel(request);
           
            var query = PrepareQuery(request , null, predicateResult);
          
            var data = (await GetFilterValueList(query, propertyName, propertyFilter)).Distinct().ToList();
            return data;
        }
        partial void OtherFilter(ref IQueryable<TEntity> data);

        public virtual IQueryable<TEntity> PrepareQuery(TQueryObject request,
            ExpressionStarter<TEntity> predicateResult , ExpressionStarter<T> oracleObject = null)
        {
              return null;
        }

        public virtual IQueryable<T> PrepareQueryOracleModel(TQueryObject request,
           ExpressionStarter<TEntity> predicateResult, ExpressionStarter<T> oracleObject = null)
        {
            return null;
        }

       
        public abstract Dictionary<string, Expression<Func<TEntity, object>>[]> GetColumnsMap();
        //public abstract ExpressionStarter<TEntity> ApplyAggregatedFilter(TQueryObject request);
        public abstract List<TEntityDto> CastObjectToDto(IQueryable<TEntity> request);
        public abstract Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<TEntity> request, string propertyName, string propertyFilter);

        public virtual ExpressionStarter<T> ApplyFilterForOracleModel(TQueryObject request)
        {
            throw new NotImplementedException();
        }
    }
}