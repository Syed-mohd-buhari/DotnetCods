using CAM.BusinessManager;
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
using CAM.Exports;
using System.Linq;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using AutoMapper;
using CAM.DataTransferObjects.Entita.SoftwareBuildCompatibility;

namespace CAM.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SoftwareBuildCompatibilityController : CamControllerBase
    {
        private readonly SoftwareBuildCompatibilityManager _softwareBuildBundleManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;

        public SoftwareBuildCompatibilityController(SoftwareBuildCompatibilityManager softwareBuildBundleManager, IMapper mapper, ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService) : base(logger, contextAccessor)
        {
            _softwareBuildBundleManager = softwareBuildBundleManager;
           
        }
           

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] SoftwareBuildCompatibilityListDtoCreateUpdate dto)
        {
            try
            {
             

                var result = await _softwareBuildBundleManager.Add(dto);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        //[HttpPut]
        //public async Task<ResultDto> Put(SoftwareBuildCompatibilityListDtoCreate dto )
        //{
        //    try
        //    {
            
        //        var result = await _softwareBuildBundleManager.Update(dto);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex);
        //        return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
        //    }
        //}

      

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(SoftwareBuildCompatibilityListDtoCreate dto)
        {
            try
            {
                var result = await _softwareBuildBundleManager.DeleteDeep(dto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
 
  
    }
}
