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
using CAM.DataTransferObjects.LookUp.SystemNames;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class SystemNamesController : CamControllerBase
    {
        private readonly SystemNamesManager _systemNamesManager;
        private readonly ICurrentUserService _currentUserService;
        public SystemNamesController(SystemNamesManager systemNamesManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _systemNamesManager = systemNamesManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<SystemNamesDtoGrid>> GetSystemNames([FromQuery] SystemNamesQueryDto dto)
        {
            try
            {
                ///*_currentUserService.UserInRole("Admin");*/

                return _systemNamesManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] SystemNamesQueryDto dto)
        {
            ///*_currentUserService.UserInRole("Admin");*/

            try
            {

                var data = _systemNamesManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] SystemNamesDtoGrid dto)
        {
           // /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _systemNamesManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(SystemNamesDtoGrid dto)
        {
            ///*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _systemNamesManager.Update(dto);

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
            ///*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _systemNamesManager.Delete(id);
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
           // /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _systemNamesManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpGet(template: "Create")]
        public SystemNamesCreateDto GetCreateResourceSystemNames()
        {
            try
            {
                ///*_currentUserService.UserInRole("Admin");*/

                return _systemNamesManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public SystemNamesUpdateDto GetUpdateResourceSystemNames(short id)
        {
            try
            {
                ///*_currentUserService.UserInRole("Admin");*/

                return _systemNamesManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        

        //[HttpGet("GetFromBuildCostruction")]
        //public async Task<ResultDto> GetFromBuildCostruction(int number)
        //{
        //    int rule = await _systemNamesManager.GetRuleFromBuildCostruction(number);
        //    return new ResultDto()
        //    {
        //        Data = rule,
        //    };
        //}
    }
}
