using CAM.BusinessManager;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Infrastucture.QueryResult;
using CAM.Exports;
using System.Linq;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.DesignComponent;

namespace CAM.WebAPI.Controllers
{

    public class DesignComponentFamilyModel
    {
        public int SystemtypeId { get; set; }
        public List<int> SubNetworkBoundaryIds { get; set; }

        public bool SupportedAllServices { get; set; }
    }

    public class DesignComponentFamilyWizardModel
    {
        public int HWOemId { get; set; }
       public decimal SWAppType { get; set; }

        public int VfNameId { get; set; }

        public int SWOemId { get; set; }
        public List<int> SubNetworkBoundaryId { get; set; }
        public int PlatformId { get; set; }
        public bool SupportedAllServices { get; set; }

    }
    public partial class DesignComponentController : CamControllerBase
    {
        [HttpGet("GetSystemType")]
        public async Task<ResultDto<SupportedAllServicesModel>> GetSystemType(int systemTypeId)
        {
            var result = await _designComponentManager.GetSystemAndSubNetworkBoundary(systemTypeId);
            return result;
        }
        [HttpGet("GetSystemTypeDestructured")]
        public async Task<ResultDto<SupportedAllServicesModel>> GetSystemType(short msOem,
            int? vfName,
            short mhOem)
        {
            var result = await _designComponentManager.GetSystemAndSubNetworkBoundary(msOem,
                 vfName,
                 mhOem);

            return result;
        }

        [HttpPost("GetDesignComponentFamilyExist")]
        public async Task<ResultDto<bool>> GetDesignComponentFamilyExist([FromBody]DesignComponentFamilyModel model)
        {
            if (model.SystemtypeId == 0)
            {
                return new ResultDto<bool>()
                {
                    Data = false
                };
            }
            if (model.SubNetworkBoundaryIds != null && model.SubNetworkBoundaryIds.Count > 0)
            {
                foreach (var subNetworkBoundaryId in model.SubNetworkBoundaryIds)
                {
                    var result = await _designComponentManager.GetDesignComponentFamilyExist(model.SystemtypeId, subNetworkBoundaryId,false);
                    if (result)
                        return new ResultDto<bool>()
                        {
                            Data = result
                        };
                }
            }
            else
            {
                var result = await _designComponentManager.GetDesignComponentFamilyExist(model.SystemtypeId, null,model.SupportedAllServices);
                if (result)
                    return new ResultDto<bool>()
                    {
                        Data = result
                    };
            }
            return new ResultDto<bool>()
            {
                Data = false,
            };

            
        }

        [HttpPost("GetDesignComponentFamilyExistDestructured")]
        public async Task<ResultDto<bool>> GetDesignComponentFamilyExistDestructured([FromBody] DesignComponentFamilyWizardModel model)
        {
            if (model.SubNetworkBoundaryId != null && model.SubNetworkBoundaryId.Count > 0)
            {
                foreach (var item in model.SubNetworkBoundaryId)
                {
                    var result = await _designComponentManager.GetDesignComponentFamilyUniqueQuery(model.HWOemId, model.SWAppType, model.VfNameId, model.SWOemId, item, model.PlatformId , model.SupportedAllServices);
                    return new ResultDto<bool>()
                    {
                        Data = result != null
                    };
                }
            }
            return new ResultDto<bool>()
            {
                Data = false,
            };
        }

        [HttpGet("GetImpact")]
        public async Task<ResultDto<ImpactSubNetworkBoundaryChanged>> GetImpact(int subNetworkBoundaryId, long systemTypeId)
        {
            var result = await _designComponentManager.GetImpact(subNetworkBoundaryId, systemTypeId);
            return new ResultDto<ImpactSubNetworkBoundaryChanged>()
            {
                Data = result
            };
        }

        [HttpGet("EditDcImpactedAreas")]
        public async Task<ResultDto> EditDcImpactedAreas(int designComponentId)
        {
            var result = await _designComponentManager.GetSystemAndSubNetworkBoundary(designComponentId);
            return new ResultDto()
            {
                Data = string.Empty
            };
        }

    }
}
