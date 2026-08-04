using CAM.BusinessManager.LookUp;
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
using CAM.DataTransferObjects.LookUp;
using CAM.Infrastucture.QueryResult;
using CAM.DataTransferObjects.LookUp.SubDomainSpoc;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class SubDomainSpocController : CamControllerBase
    {
        private readonly SubDomainSpocManager _SubDomainSpocesManager;
        private readonly ICurrentUserService _currentUserService;
        public SubDomainSpocController(SubDomainSpocManager SubDomainSpocesManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _SubDomainSpocesManager = SubDomainSpocesManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public QueryResultDto<SubDomainSpocGridDto> GetSubDomainSpoc([FromQuery] SubDomainSpocQueryDto SubDomainSpocFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var result = _SubDomainSpocesManager.FindWithCondition(SubDomainSpocFilterDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null; ;
            }
        }

        [HttpGet(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] SubDomainSpocQueryDto SubDomainSpocFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {

                var data = await _SubDomainSpocesManager.GetFilter(propertyName, propertyFilter, SubDomainSpocFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] SubDomainSpocGridDto SubDomainSpocDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _SubDomainSpocesManager.Add(SubDomainSpocDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(SubDomainSpocGridDto SubDomainSpocDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _SubDomainSpocesManager.Update(SubDomainSpocDto);

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
                return await _SubDomainSpocesManager.Delete(id);
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
                return await _SubDomainSpocesManager.DeleteDeep(id);
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
                return await _SubDomainSpocesManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public SubDomainSpocGridDto GetCreateResourceSubDomainSpoc()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _SubDomainSpocesManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public SubDomainSpocGridDto GetUpdateResourceSubDomainSpoc(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _SubDomainSpocesManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
