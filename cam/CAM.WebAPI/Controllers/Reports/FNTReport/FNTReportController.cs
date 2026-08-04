using CAM.BusinessManager.Entity.Report.FNT_Report;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.FNT_Report;
using CAM.DataTransferObjects.Entita.TsrPassThrough;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Imports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.Reports.FNTReport
{
    [Route("api/FNTReport")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class FNTReportController :  CamControllerBase
    {
        private readonly FNTReportManager _manager;
        private readonly IExportService _exportService;
        private readonly IImportService _importService;
        private CustomGridRender<ExportService> render;

        public FNTReportController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, FNTReportManager manager, IImportService importService) : base(logger, contextAccessor) 
        {
            _exportService = exportService;
            _importService = importService;
            _manager = manager;

        }

        [HttpPost("GetTemsFntReport")]
        public async Task<QueryResultDto<FNTReportDtoGrid>> GetTemsFntReport([FromBody] FNTQueryDto fntQueryDto)
        {
            try
            {
                return await _manager.FindWithCondition(fntQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
    }
}
