using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NFVIStatus;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]


#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class NFVIStatusController : CamControllerBase
    {
        private readonly NFVIStatusManager _nfviStatusManager;
        private readonly ICurrentUserService _currentUserService;
        public NFVIStatusController(NFVIStatusManager nfviStatusManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _nfviStatusManager = nfviStatusManager;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public Task<QueryResultDto<NFVIStatusDtoGrid>> GetNFVIStatus([FromQuery] NFVIStatusDtoQuery dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _nfviStatusManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] NFVIStatusDtoQuery dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {

                var data = _nfviStatusManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] NFVIStatusDtoGrid dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _nfviStatusManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(NFVIStatusDtoGrid dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _nfviStatusManager.Update(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _nfviStatusManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _nfviStatusManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                 return await _nfviStatusManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public NFVIStatusDtoGrid GetCreateResourceNFVIStatus()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _nfviStatusManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public NFVIStatusDtoGrid GetUpdateResourceNFVIStatus(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _nfviStatusManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
