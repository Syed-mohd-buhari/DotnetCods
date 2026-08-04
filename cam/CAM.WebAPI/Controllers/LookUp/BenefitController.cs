using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class BenefitController : CamControllerBase
    {
        private readonly BenefitManager _benefitManager;
        private readonly ICurrentUserService _currentUserService;
        public BenefitController(BenefitManager benefitManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _benefitManager = benefitManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<TipologicaGridDto>> GetBenefit([FromQuery] TipologicaQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _benefitManager.GetEnityGrid(dto);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Issue happen while fetching Benefit data - \n Error Message :{ex}");
                return null;
            }
            


        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                var data = _benefitManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while filtering Benefit data - \n Error Message :{ex}");
                return null;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _benefitManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating Benefit data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _benefitManager.Update(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating Benefit data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }




        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _benefitManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting Benefit data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _benefitManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting Benefit data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _benefitManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while fetching related records of Benefit data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateResourceBenefit()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _benefitManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating Benefit data - \n Error Message :{ex}");
                return null;
            }

        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateResourceBenefit(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _benefitManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating Benefit data - \n Error Message :{ex}");
                return null;
            }

        }
    }
}
