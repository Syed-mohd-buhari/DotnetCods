using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAM.Entities.Models.Lookup;
using CAM.DataTransferObjects.LookUp.VodafoneName;
using OracleModels.DBModels;

namespace CAM.BusinessManager.ILookUp
{
    public interface IVodafoneNameManager
    {
        QueryResultDto<VodafoneNameDtoGrid> GetEnityGrid(VodafoneNameDtoQuery request);
        Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, VodafoneNameDtoQuery request);
        IQueryable<VodafoneNames> PrepareQuery(VodafoneNameDtoQuery request,
            ExpressionStarter<VodafoneNames> predicateResult, ExpressionStarter<Vodafonenames> oracleObject = null);
        Dictionary<string, Expression<Func<VodafoneNames, object>>[]> GetColumnsMap();
        ExpressionStarter<Vodafonenames> ApplyFilterForOracleModel(VodafoneNameDtoQuery request);
        List<VodafoneNameDtoGrid> CastObjectToDto(IQueryable<VodafoneNames> request);
        Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<VodafoneNames> request, string propertyName, string propertyFilter);
        Task<ResultDto> Add(VodafoneNameDto dto);
        Task<ResultDto> Update(VodafoneNameDto dto);
        VodafoneNameDtoGrid GetUpdatePage(short id);
        VodafoneNameDtoGrid GetCreatePage();
        Task<ResultDto> GetRelatedRecords(short id);
        Task<ResultDto> Delete(short id);
        Task<ResultDto> DeleteDeep(short id);
        Task<ResultDto> CreateRiskClusterIdforVf(string vfName, int riskClusterId);
        Task<ResultDto> UpdateRiskClusterIdforVf(int riskClusterId,int riskclusterVfNameMapId,int vfId);
    }
}
