using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.LookUp.AspnetUserRoleModuleMapping;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AspnetUserRoleModuleController : CamControllerBase
    {
        private readonly AspnetUserRoleModuleMappingManager _moduleManager;
        public AspnetUserRoleModuleController(AspnetUserRoleModuleMappingManager moduleManager, ILoggerManager logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
        {   
            _moduleManager = moduleManager;
        }

        [HttpGet(template: "GetUpdatePage")]
        public async Task<AspnetUserRoleModuleMappingUpdateDto> GetUpdatePage()
        {
            try
            {
                return await _moduleManager.GetUpdatedPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "Create")]
        public async Task<ResultDto> Create(AspnetUserRoleModuleMappingUpdateDto userRoleModuleMappingDto)
        {
            try
            {   
                var data = await _moduleManager.AddOrUpdateRoleModuleMapping(userRoleModuleMappingDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        
    }
}
