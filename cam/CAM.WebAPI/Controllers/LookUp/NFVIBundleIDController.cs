using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Infrastucture.QueryResult;
using CAM.BusinessManager.LookUp;
using CAM.DataTransferObjects.Entita.NFVIBundleID;
using CAM.DataTransferObjects.LookUp;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]


#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class NFVIBundleIDController : CamControllerBase
    {
        private readonly NFVIBundleIDManager _nfviBundleIDManager;
        private readonly ICurrentUserService _currentUserService;
        public NFVIBundleIDController(NFVIBundleIDManager nfviBundleIDManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _nfviBundleIDManager = nfviBundleIDManager;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public Task<QueryResultDto<NFVIBundleIDDtoGrid>> GetNFVIBundleID([FromQuery] NFVIBundleIDDtoQuery dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _nfviBundleIDManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] NFVIBundleIDDtoQuery dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {

                var data = _nfviBundleIDManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] NFVIBundleIDDtoGrid dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _nfviBundleIDManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(NFVIBundleIDDtoGrid dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _nfviBundleIDManager.Update(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut(template: "ChangeGridOrderNFVIBundleID")]
        public async Task<ResultDto> ChangeGridOrderNFVIBundleID([FromBody] List<ChangeGridOrderDto> lista)
        {
            try
            {
                return await _nfviBundleIDManager.ChangeGridOrderNFVIBundleID(lista);
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
                return await _nfviBundleIDManager.Delete(id);
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
                return await _nfviBundleIDManager.DeleteDeep(id);
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
                return await _nfviBundleIDManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public NFVIBundleIDDtoGrid GetCreateResourceActivityStatus()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _nfviBundleIDManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public NFVIBundleIDDtoGrid GetUpdateResourceActivityStatus(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _nfviBundleIDManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
