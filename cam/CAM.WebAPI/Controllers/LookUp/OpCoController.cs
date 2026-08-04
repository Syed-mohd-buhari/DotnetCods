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
    public class OpCoController : CamControllerBase
    {
        private readonly OpCosManager _opCoManager;
        private readonly ICurrentUserService _currentUserService;

        public OpCoController(OpCosManager opCoManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _opCoManager = opCoManager;
            _currentUserService = currentUserService;
        }

       


        [HttpGet]
        public  Task<QueryResultDto<TipologicaGridDto>>  GetOpCo([FromQuery] TipologicaQueryDto opCoFilterDto)
        {
            try
            {
               // /*/*_currentUserService.UserInRole("Admin");*/*/

                return _opCoManager.GetEnityGrid(opCoFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>>  GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDto opCoFilterDto)
        {
           // /*/*_currentUserService.UserInRole("Admin");*/*/

            try
            {
                var data = _opCoManager.GetFilter(propertyName, propertyFilter, opCoFilterDto);                   
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }         

        }
              
        
        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDto opCoDto)
        {
            ///*/*_currentUserService.UserInRole("Admin");*/*/

            try
            {                
                return await _opCoManager.Add(opCoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
            
        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDto opCoDto)
        {
           // /*/*_currentUserService.UserInRole("Admin");*/*/

            try
            {
                return await _opCoManager.Update(opCoDto);

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
            ///*/*_currentUserService.UserInRole("Admin");*/*/

            try
            {
                return await _opCoManager.Delete(id);
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
            ///*/*_currentUserService.UserInRole("Admin");*/*/

            try
            {
                return await _opCoManager.DeleteDeep(id);
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
                return await _opCoManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateResourceOpCo()
        {
            try
            {
                ///*/*_currentUserService.UserInRole("Admin");*/*/

                return _opCoManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateResourceOpCo(short id)
        {
            try
            {
               ///*/*_currentUserService.UserInRole("Admin");*/*/

                return _opCoManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
