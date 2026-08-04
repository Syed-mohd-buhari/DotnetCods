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

    [Authorize]
    public class BuildConstructionController : CamControllerBase
    {
        private readonly BuildConstructionManager _buildConstructionManager;
        private readonly ICurrentUserService _currentUserService;
        public BuildConstructionController(BuildConstructionManager buildConstructionManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _buildConstructionManager = buildConstructionManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<BuildConstructionRule>> GetBuildConstruction([FromQuery] TipologicaQueryDtoRule dto)
        {
            try
            {

                /*_currentUserService.UserInRole("Admin");*/

                return _buildConstructionManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }



        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDtoRule dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {

                var data = _buildConstructionManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] BuildConstructionRule dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                       
                return await _buildConstructionManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(BuildConstructionRule dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _buildConstructionManager.Update(dto);

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
                return await _buildConstructionManager.Delete(id);
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
                return await _buildConstructionManager.DeleteDeep(id);
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
                return await _buildConstructionManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
            
        }

        [HttpGet(template: "Create")]
        public BuildConstructionRule GetCreateResourceBuildConstruction()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _buildConstructionManager.GetCreatePage();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
          
        }

        [HttpGet(template: "Update{id}")]
        public BuildConstructionRule GetUpdateResourceBuildConstruction(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _buildConstructionManager.GetUpdatePage(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
           
        }
        

        [HttpGet("GetFromBuildCostruction")]
        public async Task<ResultDto> GetFromBuildCostruction(int number)
        {
            try
            {
                int rule = await _buildConstructionManager.GetRuleFromBuildCostruction(number);
                return new ResultDto()
                {
                    Data = rule,
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
           
        }
    }
}
