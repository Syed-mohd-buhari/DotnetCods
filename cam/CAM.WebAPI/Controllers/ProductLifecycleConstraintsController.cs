using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class ProductLifecycleConstraintsController : CamControllerBase
    {
        private readonly ProductLifecycleConstraintsManager _productLifecycleConstraintsManager;
        public ProductLifecycleConstraintsController(ProductLifecycleConstraintsManager productLifecycleConstraintsManager, ILoggerManager logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
        {
            _productLifecycleConstraintsManager = productLifecycleConstraintsManager;
        }

        [HttpPut(template: "SaveLifecycleConstraint")]
        public async Task<ResultDto> SaveLifecycleConstraint([FromBody] LifecycleConstraintDto data)
        {
            try
            {
                return await _productLifecycleConstraintsManager.SaveLifecycleConstraint(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPost] 
        public ConstraintInfoDto GetLifecycleCostraintInfo([FromBody] LifecycleConstraintQueryDto data)
        {
            try
            {
                return _productLifecycleConstraintsManager.GetLifecycleCostraintInfo(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }


        }
    }
}
