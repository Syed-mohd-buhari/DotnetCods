using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Infrastucture.QueryResult;
using IdentityModel;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAM.Entities.Models.Lookup;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.QueryDto;
using OracleModels.DBModels;
using CAM.DataTransferObjects;
using CAM.Entities.Mappers.Lookup;
using CAM.Infrastucture;

namespace CAM.BusinessManager.ILookUp
{
    public interface IProductNameManager
    {
        Task<QueryResultDto<ProductNameDtoGrid>> GetEnityGrid(ProductNameDtoQuery request);
        Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, ProductNameDtoQuery request);
        IQueryable<ProductName> PrepareQuery(ProductNameDtoQuery request,
            ExpressionStarter<ProductName> predicateResult, ExpressionStarter<Productname> oracleObject = null);
        Dictionary<string, Expression<Func<ProductName, object>>[]> GetColumnsMap();
        ExpressionStarter<Productname> ApplyFilterForOracleModel(ProductNameDtoQuery request);
        List<ProductNameDtoGrid> CastObjectToDto(IQueryable<ProductName> request);
        Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<ProductName> request, string propertyName, string propertyFilter);
        Task<ResultDto> Add(ProductNameDto dto);
        Task<ResultDto> Update(ProductNameDto dto);
        ProductNameDtoGrid GetUpdatePage(short id);
        ProductNameDtoCreate GetCreatePage();
        Task<ResultDto> GetRelatedRecords(short id);
        Task<ResultDto> Delete(short id);
        Task<ResultDto> DeleteDeep(short id);
    }
}
