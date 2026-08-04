using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.Repository;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.NewPortal
{
    public class UpcomingLinkManager : BaseManager
    {
        IRepositoryWrapper _repositoryWrapper;
        AuthorizedRoleManager _authorizedRoleManager;
        private static DateTime today = DateTime.Today;
        private int messagingDateRange = 1;
        private readonly ILoggerManager _loggerManager;
        private readonly CommonDapperRepository _commonDapperRepository;
        private readonly string dapperDatabaseMode = "normal";
        private readonly DapperCommonManager _dapperCommonManager;

        public UpcomingLinkManager(IEnumerable<IRepositoryWrapper> wrappers, AuthorizedRoleManager authorizedRoleManager, ILoggerManager loggerManager,
           IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonDapperRepository commonDapperRepository, DapperCommonManager dapperCommonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _authorizedRoleManager = authorizedRoleManager;
            _loggerManager = loggerManager;
            _commonDapperRepository = commonDapperRepository;
            dapperDatabaseMode = GlobalDbMode.DbMode.ContainsKey(CurrentLogLevelConfig._UserName) ? GlobalDbMode.DbMode[CurrentLogLevelConfig._UserName] : "normal";
            _dapperCommonManager = dapperCommonManager;
        }
        #region  apply filter

        private static ExpressionStarter<Majorsoftwarebuilds> ApplyFilterForMajorSoftwareBuild(long userId)
        {
            var predicate = PredicateBuilder.New<Majorsoftwarebuilds>(true);

            if (userId != 0)
            {
                predicate.And(x =>
                    x.Majorswbuildsdesigncontacts.Any(d =>
                        d.Designcontactid == userId));
            }

            return predicate;
        }

        private static ExpressionStarter<Majorhardwarebuilds> ApplyFilterForMajorHardwareBuild(long userId)
        {
            var predicate = PredicateBuilder.New<Majorhardwarebuilds>(true);

            if (userId != 0)
            {
                predicate.And(x =>
                    x.Majorhwbuildsdesigncontacts.Any(d =>
                        d.Designcontactid == userId));
            }

            return predicate;
        }

        #endregion
        public async Task<List<HomePageEomEosGridDto>> GetSoftwareBasedEomEosGridRecord(long designContactId, ExpressionStarter<Majorsoftwarebuilds> predicateResult)
        {
            var lcmRecords = await _repositoryWrapper.MajorSoftwareBuild
                        .FindByCondition(predicateResult)
                        .AsNoTracking()
                        .Where(x =>
                            designContactId == 0 ||
                            x.Majorswbuildsdesigncontacts
                                .Any(dc => dc.Designcontactid == designContactId))
                          .Select(x => new HomePageEomEosGridDto
                          {
                              ProductAndPlatform = x.Productname.Description,
                              Oem = x.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                              Version = x.Softwareversion,
                              Endofmaintenance = x.Endofmaintenance,
                              Endofsupport = x.Endofsupport,
                              MajorId = x.Majorsoftwarebuildsid,
                              MajorDesignContacts = (x.Majorswbuildsdesigncontacts != null && x.Majorswbuildsdesigncontacts.Count > 0) ? x.Majorswbuildsdesigncontacts.Select(m => m.Designcontactid).ToList() : null,

                          })
                        .ToListAsync();

            return lcmRecords;
        }

        public async Task<List<HomePageEomEosGridDto>> HardwareBasedEomEosGridRecord(long designContactId, ExpressionStarter<Majorhardwarebuilds> predicateResult)
        {
            var lcmRecords = await _repositoryWrapper.MajorHardwareBuild
                        .FindByCondition(predicateResult)
                        .AsNoTracking()
                        .Where(x =>
                            designContactId == 0 ||
                            x.Majorhwbuildsdesigncontacts
                                .Any(dc => dc.Designcontactid == designContactId))
                          .Select(x => new HomePageEomEosGridDto
                          {

                              ProductAndPlatform = x.Platform.Platform,
                              Oem = x.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                              Version = x.Hardwaretype,
                              Endofmaintenance = x.Endofmaintenance,
                              Endofsupport = x.Endofsupport,
                              MajorId = x.Majorhardwareid,
                              MajorDesignContacts = (x.Majorhwbuildsdesigncontacts != null && x.Majorhwbuildsdesigncontacts.Count > 0) ? x.Majorhwbuildsdesigncontacts.Select(m => m.Designcontactid).ToList() : null,


                          })
                        .ToListAsync();

            return lcmRecords;
        }

        public int GetEomAndEosSummaryCount(List<HomePageEomEosGridDto> data)
        {
            return data
                .GroupBy(x => new
                {
                    x.ProductAndPlatform,
                    x.Oem
                })
                .Count();
        }
        public List<OpenLinkEomAndEosDto> GroupResultForEomAndEos(List<HomePageEomEosGridDto> data)
        {
            return data
                .GroupBy(x => new
                {
                    x.ProductAndPlatform,
                    x.Oem
                })
                .Select(productGroup => new OpenLinkEomAndEosDto
                {
                    ProductAndPlatfrom = $"{productGroup.Key.ProductAndPlatform}-{productGroup.Key.Oem}",
                    OEM = productGroup.Key.Oem,

                    Versions = productGroup
                        .GroupBy(v => v.Version)
                        .Select(versionGroup => new
                        {
                            VersionGroup = versionGroup,
                            Eom = versionGroup.Select(x => x.Endofmaintenance).FirstOrDefault(),
                            Eos = versionGroup.Select(x => x.Endofsupport).FirstOrDefault()
                        })
                        .OrderBy(x => x.Eom == null)
                        .ThenBy(x => x.Eom)
                        .Select(x => new HomePageVersionDto
                        {
                            Version = x.VersionGroup.Key,
                            MajorId = x.VersionGroup
                                .Select(v => (long)v.MajorId)
                                .FirstOrDefault(),
                            OpCo = string.Join(",",
                                x.VersionGroup
                                    .Select(v => v.Opco)
                                    .Distinct()),
                            EomDate = x.Eom?.ToString("dd-MM-yyyy"),
                            EosDate = x.Eos?.ToString("dd-MM-yyyy")
                        })
                        .ToList()
                })
                .ToList();
        }
        public async Task<ResultDto> UpcomingEomEosActionsForDesignContactAsync(long userId)
        {
            var result = new EomEosActionsDto();
            try
            {

                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);
                var preferenceDetails = new Gridcustomcolumn();
                var UpcomingActionsResource = new List<OpenLinkEomAndEosDto>();

                var getGridCustomColumns = await _dapperCommonManager.GetGridCustomColumnsAsync(userId);
                 
                var messagingDateRangeTask = getGridCustomColumns.Where(x => x.Messagingdate != null).FirstOrDefault();
                var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate;

                preferenceDetails = getGridCustomColumns.FirstOrDefault();

                var upcomingMessagingDate = new DateTime(today.Year, today.Month, 1).AddMonths(messagingDateRange);


                // Product Importance - Strategic and Non-Strategic #1913 Aprl 01 2026 
               // var lcmProductImportanaceId = await _dapperCommonManager.GetLcmProductImportanceIdAsync();

                if (userDetailsList?.RoleRecords?.Any(x =>
                           string.Equals(x.RoleName, "SW Product Owner", StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return new ResultDto
                    {
                        Data = new
                        {
                            UpcomingActionsResource,
                            WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                            MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                        }
                    };
                }

                #region Query Logic 
                var predicateResult = ApplyFilterForMajorSoftwareBuild(userId);
                var query = await GetSoftwareBasedEomEosGridRecord(userId, predicateResult);

                // Upcoming Action
                var upcomingRecords = query
                    .Where(x =>
                        (x.Endofmaintenance >= today &&
                         x.Endofmaintenance <= upcomingMessagingDate)
                        ||
                        (x.Endofsupport >= today &&
                         x.Endofsupport <= upcomingMessagingDate)).ToList();

                #endregion

                //#1913 - Display all the records for Admin
                if (userDetailsList?.UserRoleId.Any(f => f != 1) == true)
                {

                    upcomingRecords = upcomingRecords.Where(f => f.MajorDesignContacts.Any(l => l == userId)).ToList();
                }

                UpcomingActionsResource = GroupResultForEomAndEos(upcomingRecords);

                return new ResultDto
                {
                    Data = new
                    {

                        UpcomingActionsResource,
                        WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                        MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                    }
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
      

        #region Hardware 
        public async Task<ResultDto> HardwareBasedUpcomingEomEosActionAsync(long userId)
        {
            var result = new EomEosActionsDto();
            try
            {

                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);
                var preferenceDetails = new Gridcustomcolumn();
                var UpcomingActionsResource = new List<OpenLinkEomAndEosDto>();

                var getGridCustomColumns = await _dapperCommonManager.GetGridCustomColumnsAsync(userId);

                var messagingDateRangeTask = getGridCustomColumns.Where(x => x.Messagingdate != null).FirstOrDefault();
                var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate;

                preferenceDetails = getGridCustomColumns.FirstOrDefault();

                var upcomingMessagingDate = new DateTime(today.Year, today.Month, 1).AddMonths(messagingDateRange);


                // Product Importance - Strategic and Non-Strategic #1913 Aprl 01 2026 
                // var lcmProductImportanaceId = await _dapperCommonManager.GetLcmProductImportanceIdAsync();

                if (userDetailsList?.RoleRecords?.Any(x =>
                           string.Equals(x.RoleName, "HW Product Owner", StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return new ResultDto
                    {
                        Data = new
                        {
                            UpcomingActionsResource,
                            WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                            MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                        }
                    };
                }

                #region Query Logic 
                var predicateResult = ApplyFilterForMajorHardwareBuild(userId);
                var query = await HardwareBasedEomEosGridRecord(userId, predicateResult);

                // Upcoming Action
                var upcomingRecords = query
                    .Where(x =>
                        (x.Endofmaintenance >= today &&
                         x.Endofmaintenance <= upcomingMessagingDate)
                        ||
                        (x.Endofsupport >= today &&
                         x.Endofsupport <= upcomingMessagingDate)).ToList();

                #endregion

                //#1913 - Display all the records for Admin
                if (userDetailsList?.UserRoleId.Any(f => f != 1) == true)
                {

                    upcomingRecords = upcomingRecords.Where(f => f.MajorDesignContacts.Any(l => l == userId)).ToList();
                }

                UpcomingActionsResource = GroupResultForEomAndEos(upcomingRecords);

                return new ResultDto
                {
                    Data = new
                    {

                        UpcomingActionsResource,
                        WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                        MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                    }
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<ResultDto> GetCountForUpcomingHardwareBasedAsync(long userId)
        {
            var result = new EomEosActionsDto();
            try
            {

                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);
                var preferenceDetails = new Gridcustomcolumn();
                
                var getGridCustomColumns = await _dapperCommonManager.GetGridCustomColumnsAsync(userId);

                var messagingDateRangeTask = getGridCustomColumns.Where(x => x.Messagingdate != null).FirstOrDefault();
                var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate;

                preferenceDetails = getGridCustomColumns.FirstOrDefault();

                var upcomingMessagingDate = new DateTime(today.Year, today.Month, 1).AddMonths(messagingDateRange);


                // Product Importance - Strategic and Non-Strategic #1913 Aprl 01 2026 
                // var lcmProductImportanaceId = await _dapperCommonManager.GetLcmProductImportanceIdAsync();

                if (userDetailsList?.RoleRecords?.Any(x =>
                           string.Equals(x.RoleName, "HW Product Owner", StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return new ResultDto
                    {
                        Data = new
                        {
                            UpcomingActionsResource =0,
                            WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                            MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                        }
                    };
                }

                #region Query Logic 
                var predicateResult = ApplyFilterForMajorHardwareBuild(userId);
                var query = await HardwareBasedEomEosGridRecord(userId, predicateResult);

                // Upcoming Action
                var upcomingRecords = query
                    .Where(x =>
                        (x.Endofmaintenance >= today &&
                         x.Endofmaintenance <= upcomingMessagingDate)
                        ||
                        (x.Endofsupport >= today &&
                         x.Endofsupport <= upcomingMessagingDate)).ToList();

                #endregion

                //#1913 - Display all the records for Admin
                if (userDetailsList?.UserRoleId.Any(f => f != 1) == true)
                {

                    upcomingRecords = upcomingRecords.Where(f => f.MajorDesignContacts.Any(l => l == userId)).ToList();
                } 

                return new ResultDto
                {
                    Data = new
                    {

                        UpcomingActionsResource = GetEomAndEosSummaryCount(upcomingRecords) ,
                        WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                        MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                    }
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region Get Upcoming link count detail
        public async Task<ResultDto> GetCountForUpcomingSoftwareActionsAsync(long userId)
        {
            var result = new EomEosActionsDto();
            try
            {

                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);
                var preferenceDetails = new Gridcustomcolumn();                
                var getGridCustomColumns = await _dapperCommonManager.GetGridCustomColumnsAsync(userId);

                var messagingDateRangeTask = getGridCustomColumns.Where(x => x.Messagingdate != null).FirstOrDefault();
                var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate;

                preferenceDetails = getGridCustomColumns.FirstOrDefault();

                var upcomingMessagingDate = new DateTime(today.Year, today.Month, 1).AddMonths(messagingDateRange);


                // Product Importance - Strategic and Non-Strategic #1913 Aprl 01 2026 
                // var lcmProductImportanaceId = await _dapperCommonManager.GetLcmProductImportanceIdAsync();

                if (userDetailsList?.RoleRecords?.Any(x =>
                           string.Equals(x.RoleName, "SW Product Owner", StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return new ResultDto
                    {
                        Data = new
                        {
                            UpcomingActionsResource =0,
                            WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                            MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                        }
                    };
                }

                #region Query Logic 
                var predicateResult = ApplyFilterForMajorSoftwareBuild(userId);
                var query = await GetSoftwareBasedEomEosGridRecord(userId, predicateResult);

                // Upcoming Action
                var upcomingRecords = query
                    .Where(x =>
                        (x.Endofmaintenance >= today &&
                         x.Endofmaintenance <= upcomingMessagingDate)
                        ||
                        (x.Endofsupport >= today &&
                         x.Endofsupport <= upcomingMessagingDate)).ToList();

                #endregion

                //#1913 - Display all the records for Admin
                if (userDetailsList?.UserRoleId.Any(f => f != 1) == true)
                {

                    upcomingRecords = upcomingRecords.Where(f => f.MajorDesignContacts.Any(l => l == userId)).ToList();
                } 

                return new ResultDto
                {
                    Data = new
                    {

                        UpcomingActionsResource = GetEomAndEosSummaryCount(upcomingRecords) ,
                        WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                        MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                    }
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }



        #endregion

    }

}
