using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Mvc;

namespace CAM.WebAPI.Controllers
{
    public partial class SystemTypeController : CamControllerBase
    {
        [HttpGet(template: "GetCostraintInfo")]
        public ConstraintInfoDto GetCostraintInfo([FromQuery] ConstrainInfoQueryDto data)
        {
            try
            {
                return _systemTypeManager.GetCostraintInfo(data);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet(template: "GetSystemSolutionName")]
        public ResultDto GetSystemSolutionName(string nameOEM, int? majorHardwareBuilds, int? majorSoftwareBuild, [FromQuery] List<int>? majorHardwareList)
        {
            try
            {
                return new ResultDto()
                {
                    Warning = false,
                    Data = _systemTypeManager.GetSystemSolutionName(nameOEM, majorHardwareBuilds, majorSoftwareBuild, majorHardwareList),
                    Info = "success"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetAssetCategoryReleated")]

        public ResultDto<SystemTypeReleatedMajorEntity> GetAssetCategoryReleated(int assetCategoryId)
        {
            return _systemTypeManager.GetAssetCategoryReleated(assetCategoryId);
        }

        [HttpGet("GetMinorDateFromMajorEntity")]

        public async Task<ResultDto> GetMinorDateFromMajorEntity([FromQuery] int? majorSoftwareId, [FromQuery] List<long> majorHardwareIds)
        {
            return new ResultDto {
                Data = _systemTypeManager.GetMinorDateFromMajorEntity(majorSoftwareId, majorHardwareIds) };
        }

        [HttpPut(template: "ApplyDataRemediation")]
        public async Task<ResultDto<ResultDataRemediationDto>> ApplyDataRemediation([FromBody] DataRemediationDto data)
        {
           
                return await _systemTypeManager.ApplyDataRemediation(data);
           
        }

        [HttpGet(template: "GetGridGrouped")]
        public QueryResultDto<SystemTypeDtoGrouped> GetGridGrouped([FromQuery] SystemTypeQueryDto systemTypeFilterDto)
        {
            
                return _systemTypeManager.GetGridGrouped(systemTypeFilterDto);
           
        }

       

    }
}
