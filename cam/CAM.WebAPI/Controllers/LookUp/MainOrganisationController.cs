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
using CAM.BusinessManager.LookUp;
using CAM.DataTransferObjects.LookUp.MainOrganisation;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class MainOrganisationController : CamControllerBase
    {
        private readonly MainOrganisationManager _mainOrgManager;
        private readonly ICurrentUserService _currentUserService;
        public MainOrganisationController(MainOrganisationManager mainOrgManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _mainOrgManager = mainOrgManager;
            _currentUserService = currentUserService;
        }

        [HttpPost("Get")]
        public Task<QueryResultDto<MainOrganisatioinGridDto>> GetMainOrg([FromBody] MainOrganisationQueryDto dto)
        {
            try
            {
                return _mainOrgManager.FindWithCondition(dto);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] MainOrganisationQueryDto dto)
        {

            try
            {

                var data = _mainOrgManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] MainOrganisatioinGridDto dto)
        {

            try
            {
                return await _mainOrgManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(MainOrganisatioinGridDto dto)
        {
            try
            {
                return await _mainOrgManager.Update(dto);

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
                return await _mainOrgManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(long id)
        {

            try
            {
                return await _mainOrgManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }        

        [HttpGet(template: "CreatePage")]
        public MainOrganisatioinGridDto GetCreateResourceMainOrg()
        {
            try
            {
                return _mainOrgManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "UpdatePage")]
        public MainOrganisatioinGridDto GetUpdateResourceMainOrg(long id)
        {
            try
            {
                return _mainOrgManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
