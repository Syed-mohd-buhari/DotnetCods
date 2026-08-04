using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using CAM.DataTransferObjects.LookUp.LcmDeploymentStatus;

namespace CAM.BusinessManager.ILookUp
{
    public interface ILcmDeploymentStatusManager
    {
        QueryResultDto<LcmDeploymentStatusDtoGrid> GetEnityGrid(LcmDeploymentStatusDtoQuery request);
        Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, LcmDeploymentStatusDtoQuery request);
        IQueryable<LCMDeploymentStatus> PrepareQuery(LcmDeploymentStatusDtoQuery request,
            ExpressionStarter<LCMDeploymentStatus> predicateResult, ExpressionStarter<Lcmdeploymentstatus> oracleObject = null);
        Dictionary<string, Expression<Func<LCMDeploymentStatus, object>>[]> GetColumnsMap();
        ExpressionStarter<Lcmdeploymentstatus> ApplyFilterForOracleModel(LcmDeploymentStatusDtoQuery request);
        List<LcmDeploymentStatusDtoGrid> CastObjectToDto(IQueryable<LCMDeploymentStatus> request);
        Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<LCMDeploymentStatus> request, string propertyName, string propertyFilter);
        Task<ResultDto> Add(LcmDeploymentStatusDto dto);
        Task<ResultDto> Update(LcmDeploymentStatusDto dto);
        LcmDeploymentStatusDtoGrid GetUpdatePage(short id);
        //LcmDeploymentStatusDtoGrid GetCreatePage();
        Task<ResultDto> GetRelatedRecords(short id);
        Task<ResultDto> Delete(short id);
        Task<ResultDto> DeleteDeep(short id);
    }
}
