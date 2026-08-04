using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.NewPortal;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Repository;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.AbstractionLayer
{
    public class HomePageManager : BaseManager
    {
        IRepositoryWrapper _repositoryWrapper;
        AuthorizedRoleManager _authorizedRoleManager;     
        private static DateTime today = DateTime.Today;        
        private int messagingDateRange = 1;
        private readonly ILoggerManager _loggerManager;
        private readonly CommonDapperRepository _commonDapperRepository;
        private readonly string dapperDatabaseMode = "normal";
        private readonly DapperCommonManager _dapperCommonManager;

        private readonly ProductGraphManager _productGraphManager;
        private readonly OpenLinkManager _openLinkManager;
        private readonly UpcomingLinkManager _upcomingLinkManager;
        private readonly AchivementLinkDapperManager _achivementLinkDapperManager;
        public HomePageManager(IEnumerable<IRepositoryWrapper> wrappers, AuthorizedRoleManager authorizedRoleManager, ILoggerManager loggerManager,
           IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonDapperRepository commonDapperRepository, DapperCommonManager dapperCommonManager
            , ProductGraphManager productGraphManager , OpenLinkManager openLinkManager, UpcomingLinkManager upcomingLinkManager, AchivementLinkDapperManager achivementLinkDapperManager
            ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _authorizedRoleManager = authorizedRoleManager;
            _loggerManager = loggerManager;
            _commonDapperRepository = commonDapperRepository;
            dapperDatabaseMode = GlobalDbMode.DbMode.ContainsKey(CurrentLogLevelConfig._UserName) ? GlobalDbMode.DbMode[CurrentLogLevelConfig._UserName] : "normal";
            _dapperCommonManager = dapperCommonManager;
            _productGraphManager = productGraphManager;
            _openLinkManager = openLinkManager;
            _upcomingLinkManager = upcomingLinkManager;
            _achivementLinkDapperManager =  achivementLinkDapperManager;
        }

        public async Task<ResultDto> FetchProductGraphComplaince(long userId, long PortalRoleId)
        {

            var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);
           
            var configuredRole = userDetailsList?.RoleRecords?.FirstOrDefault(x => x.PortalRoleId == PortalRoleId);
            if (configuredRole == null)
            {
                return new ResultDto
                {
                    Data = null,
                    Info = "Role not exists for logged user"
                };
            }
             
            switch (configuredRole?.PortalRoleId)
            {
                case 1:
                    return await _productGraphManager.FindByCondtionForProductComplaince(userId, userDetailsList);

                case 2:
                    return await _productGraphManager.GetHardwareProductOwnerComplaince(userId, userDetailsList);

                default:
                    return new ResultDto { Data = null };
            }

           
        }


        public async Task<ResultDto> FetchExpiredRecordForOpenLink(long userId, long PortalRoleId)
        {

            var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);

            var configuredRole = userDetailsList?.RoleRecords?.FirstOrDefault(x => x.PortalRoleId == PortalRoleId);
            if (configuredRole == null)
            {
                return new ResultDto
                {
                    Data = null,
                    Info = "Role not exists for logged user"
                };
            }

            switch (configuredRole?.PortalRoleId)
            {
                case 1:
                    return await _openLinkManager.EomEosOpenActionsForDesignContactAsync(userId);

                case 2:
                    return await _openLinkManager.HardwareBasedEomEosOpenActionsForDesignContactAsync(userId);

                default:
                    return new ResultDto { Data = null };
            }

        }

        public async Task<ResultDto> FetchFutureExpiredRecordForUpcomingLink(long userId, long PortalRoleId)
        {

            var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);

            var configuredRole = userDetailsList?.RoleRecords?.FirstOrDefault(x => x.PortalRoleId == PortalRoleId);
            if (configuredRole == null)
            {
                return new ResultDto
                {
                    Data = null,
                    Info = "Role not exists for logged user"
                };
            }

            switch (configuredRole?.PortalRoleId)
            {
                case 1:
                    return await _upcomingLinkManager.UpcomingEomEosActionsForDesignContactAsync(userId);

                case 2:
                    return await _upcomingLinkManager.HardwareBasedUpcomingEomEosActionAsync(userId);

                default:
                    return new ResultDto { Data = null };
            }
 
        }

        #region Get count for Expired and Future
        public async Task<ResultDto> GetcountForOpenLink(long userId, long PortalRoleId)
        {

            var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);

            var configuredRole = userDetailsList?.RoleRecords?.FirstOrDefault(x => x.PortalRoleId == PortalRoleId);
            if (configuredRole == null)
            {
                return new ResultDto
                {
                    Data = null,
                    Info = "Role not exists for logged user"
                };
            }

            switch (configuredRole?.PortalRoleId)
            {
                case 1:
                    return await _openLinkManager.EomEosOpenActionsCountAsync(userId);

                case 2:
                    return await _openLinkManager.HardwareBasedEomEosOpenCountAsync(userId);

                default:
                    return new ResultDto { Data = null };
            }

        }

        public async Task<ResultDto> GetcountForUpcomingLink(long userId, long PortalRoleId)
        {

            var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);

            var configuredRole = userDetailsList?.RoleRecords?.FirstOrDefault(x => x.PortalRoleId == PortalRoleId);
            if (configuredRole == null)
            {
                return new ResultDto
                {
                    Data = null,
                    Info = "Role not exists for logged user"
                };
            }

            switch (configuredRole?.PortalRoleId)
            {
                case 1:
                    return await _upcomingLinkManager.GetCountForUpcomingSoftwareActionsAsync(userId);

                case 2:
                    return await _upcomingLinkManager.GetCountForUpcomingHardwareBasedAsync(userId);

                default:
                    return new ResultDto { Data = null };
            }

        }
        #endregion
    }

}
