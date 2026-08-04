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
using CAM.DataTransferObjects.LookUp.Practice;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PracticeController : CamControllerBase
    {
        private readonly PracticeManager _practiceManager;
        private readonly ICurrentUserService _currentUserService;
        public PracticeController(PracticeManager practiceManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _practiceManager = practiceManager;
            _currentUserService = currentUserService;
        }

        [HttpPost("Get")]
        public Task<QueryResultDto<PracticeGridDto>> GetPractice([FromBody] PracticeQueryDto dto)
        {
            try
            {
                return _practiceManager.FindWithCondition(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] PracticeQueryDto dto)
        {

            try
            {

                var data = _practiceManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] PracticeGridDto dto)
        {

            try
            {
                return await _practiceManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(PracticeGridDto dto)
        {
            try
            {
                return await _practiceManager.Update(dto);

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
                return await _practiceManager.Delete(id);
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
                return await _practiceManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }       

        [HttpGet(template: "CreatePage")]
        public PracticeGridDto GetCreateResourcePractice()
        {
            try
            {
                return _practiceManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "UpdatePage")]
        public PracticeGridDto GetUpdateResourcePractice(long id)
        {
            try
            {
                return _practiceManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
