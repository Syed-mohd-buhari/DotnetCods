using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

    public partial class GlossaryItemsController : CamControllerBase
    {

        private readonly GlossaryItemsManager _glossaryItemsManager;

        public GlossaryItemsController(
            GlossaryItemsManager glossaryItemsManager,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor)
            : base(logger, contextAccessor)
        {
            _glossaryItemsManager = glossaryItemsManager;
        }

        [HttpGet(template: "{id}")]
        public GlossaryItemsDto GetSingleGlossaryItems(long id)
        {
            try
            {
                return _glossaryItemsManager.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        [HttpGet("Get")]
        public async Task<IEnumerable<GlossaryItemsGridDto>> GetGlossaryItems()
        {
            try
            {
                return await _glossaryItemsManager.FindAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] GlossaryItemsDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Description) || string.IsNullOrWhiteSpace(dto.Header))
                {
                    _logger.LogError("Description and header must have a value");
                    return new ResultDto() { Info = "Description and header must have a value", Warning = true };
                }
                var result = await _glossaryItemsManager.Add(dto, forced);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(GlossaryItemsDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Description) || string.IsNullOrWhiteSpace(dto.Header))
                {
                    _logger.LogError("Description and header must have a value");
                    return new ResultDto() { Info = "Description and header must have a value", Warning = true };
                }

                var result = await _glossaryItemsManager.Update(dto, forced);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
            finally
            {

            }
        }

        [HttpPut(template: "Restore")]
        public async Task<ResultDto> Restore(long id)
        {
            try
            {
                return await _glossaryItemsManager.Restore(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(long id)
        {
            try
            {
                var result = await _glossaryItemsManager.Delete(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

    }

}


