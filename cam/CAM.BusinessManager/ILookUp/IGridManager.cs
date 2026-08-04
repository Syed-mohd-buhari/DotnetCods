using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.ILookUp
{
    public interface IGridManager<TEntity, TEntityDto, TQueryObject, T>
    where TQueryObject : IQueryObject
    where TEntityDto : IGridDtoBase
    where T : class
    {
        Task<QueryResultDto<TEntityDto>> GetEnityGrid(TQueryObject request);
        Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, TQueryObject request);
        IQueryable<TEntity> PrepareQuery(TQueryObject request,
            ExpressionStarter<TEntity> predicateResult, ExpressionStarter<T> oracleObject = null);


        Dictionary<string, Expression<Func<TEntity, object>>[]> GetColumnsMap();
        // ExpressionStarter<TEntity> ApplyAggregatedFilter(TQueryObject request);
        ExpressionStarter<T> ApplyFilterForOracleModel(TQueryObject request);


        List<TEntityDto> CastObjectToDto(IQueryable<TEntity> request);
        Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<TEntity> request, string propertyName, string propertyFilter);
    }
}
