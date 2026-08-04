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

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
        [Authorize]
#endif
    public class OperationalRiskController : CamControllerBase
    {
        private readonly RiskManager _operationalRiskManager;
        private readonly ICurrentUserService _currentUserService;

        public OperationalRiskController(RiskManager operationalRiskManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _operationalRiskManager = operationalRiskManager;
            _currentUserService = currentUserService;
        }

       


        [HttpGet]
        public  Task<QueryResultDto<OperationalRiskDto>>  GetOperationalRisk([FromQuery] OperationalRiskQueryDto operationalRiskFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _operationalRiskManager.GetEnityGrid(operationalRiskFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>>  GetFilterResult(string propertyName, string propertyFilter, [FromQuery] OperationalRiskQueryDto operationalRiskFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                var data = _operationalRiskManager.GetFilter(propertyName, propertyFilter, operationalRiskFilterDto);                   
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }         

        }
              
        
        [HttpPost]
        public async Task<ResultDto> Create([FromBody] OperationalRiskDto operationalRiskDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {                
                return await _operationalRiskManager.Add(operationalRiskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
            
        }

        [HttpPut]
        public async Task<ResultDto> Put(OperationalRiskDto operationalRiskDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _operationalRiskManager.Update(operationalRiskDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut(template: "ChangeGridOrderOperationalRisk")]
        public async Task<ResultDto> ChangeGridOrderOperationalRisk([FromBody] List<ChangeGridOrderDto> lista)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _operationalRiskManager.ChangeGridOrderOperationalRisk(lista);

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
                return await _operationalRiskManager.Delete(id);
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
                return await _operationalRiskManager.DeleteDeep(id);
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
                return await _operationalRiskManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public OperationalRiskDto GetCreateResourceOperationalRisk()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _operationalRiskManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public OperationalRiskDto GetUpdateResourceOperationalRisk(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _operationalRiskManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
