using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Report.GraphicalReport.AssetReport;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.IdentityAsIs;
using CAM.DataTransferObjects.Entita.NetworkVisualizer;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class NetworkVisualizerController : CamControllerBase
    {
        private readonly NetworkVisualizerManager _manager;
        private readonly IExportService _exportService;
        private readonly ICurrentUserService _currentUserService;
        private readonly bool _adminRoleCheck=false;
        private readonly List<string> _opcoList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        public NetworkVisualizerController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, NetworkVisualizerManager netManager
            , ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = netManager;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToString(x)).Distinct().ToList() : null;
        }
        [HttpPost("Get")]

        public QueryResultDto<NetworkVisulaizerDtoGrid> GetNetworkVirtual(IdentityAsIsDtoQuery dto)
        {
            try
            {
                if ((dto.OpCo==null) ||(dto.OpCo != null && dto.OpCo.Count == 0))
                {
                    dto.OpCo = _opcoList;
                }
                var result = _manager.FindWithCondition(dto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("GetResource")]
        public NetworkVisulaizerDto GetResourceFeilds()
        {
            try
            {
                var result = _manager.GetResource(_opcoList);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
