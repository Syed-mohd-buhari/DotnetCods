using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.InizializeNewProduct;
using CAM.Infrastucture;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class InizializeNewProductController : CamControllerBase
    {
        private readonly InizializeNewProductManager _inizializeNewProductManager;
        public InizializeNewProductController(InizializeNewProductManager inizializeNewProductManager,ILoggerManager logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
        {
            _inizializeNewProductManager = inizializeNewProductManager;
        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] InizializeNewProductCreateDto dto)
        {
            try
            {
                return await _inizializeNewProductManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }


      
    }
}
