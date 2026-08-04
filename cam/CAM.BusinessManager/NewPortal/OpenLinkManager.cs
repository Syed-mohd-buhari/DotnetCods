using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.Repository;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.NewPortal
{
    public class OpenLinkManager : BaseManager
    {
        IRepositoryWrapper _repositoryWrapper;
        AuthorizedRoleManager _authorizedRoleManager;
        private static DateTime today = DateTime.Today;
        private int messagingDateRange = 1;
        private readonly ILoggerManager _loggerManager;
        private readonly CommonDapperRepository _commonDapperRepository;
        private readonly string dapperDatabaseMode = "normal";
        private readonly DapperCommonManager _dapperCommonManager;

        private static readonly DateTime Today = DateTime.Today;
        public OpenLinkManager(IEnumerable<IRepositoryWrapper> wrappers, AuthorizedRoleManager authorizedRoleManager, ILoggerManager loggerManager,
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

        #region Major Software Owner
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
 
        public async Task<ResultDto> EomEosOpenActionsForDesignContactAsync(long userId)
        {
            var result = new EomEosActionsDto();
            try
            {

                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);                            
                var OpenActionsResource = new List<OpenLinkEomAndEosDto>();
                var preferenceDetails = new Gridcustomcolumn();

                var getGridCustomColumns = await _dapperCommonManager.GetGridCustomColumnsAsync(userId);
                
                var messagingDateRangeTask = getGridCustomColumns.Where(x => x.Messagingdate != null).FirstOrDefault();
                var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate;

                preferenceDetails = getGridCustomColumns.FirstOrDefault();

                var openExpiredMessagingDate = new DateTime(today.Year, today.Month, 1).AddMonths(-messagingDateRange);

                // Product Importance - Strategic and Non-Strategic #1913 Aprl 01 2026
               // var lcmProductImportanaceId = await _dapperCommonManager.GetLcmProductImportanceIdAsync();

                if (userDetailsList?.RoleRecords?.Any(x =>
                            string.Equals(x.RoleName, "SW Product Owner", StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return new ResultDto
                    {
                        Data = new
                        {
                            OpenActionsResource,
                            WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                            MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                        }
                    };
                }

                #region Query Logic 
                var predicateResult = ApplyFilterForMajorSoftwareBuild(userId);
                var query = await GetSoftwareBasedEomEosGridRecord(  userId, predicateResult);


                // Open Action
                var expiredRecords = query
                    .Where(x =>
                        (//x.SwEndofmaintenance > openExpiredMessagingDate &&
                         x.Endofmaintenance < today)
                        ||
                        (//x.SwEndofsupport > openExpiredMessagingDate &&
                         x.Endofsupport < today)).ToList();

                #endregion

                //#1913 - Display all the records for Admin
                if (userDetailsList?.UserRoleId.Any(f => f != 1) == true)
                {
                    expiredRecords = expiredRecords.Where(f => f.MajorDesignContacts.Any(l => l == userId)).ToList();

                }

                OpenActionsResource = GroupResultForEomAndEos(expiredRecords);

                return new ResultDto
                {
                    Data = new
                    {
                        OpenActionsResource,
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

        public async Task<ResultDto> EomEosOpenActionsCountAsync(long userId)
        {
            try
            {
                var userDetails = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);
                var gridColumns = await _dapperCommonManager.GetGridCustomColumnsAsync(userId);

                var preference = gridColumns.FirstOrDefault();

                var messagingDate = gridColumns
                    .FirstOrDefault(x => x.Messagingdate != null)?.Messagingdate
                    ?? ConstantValueFilter.NewPortal_MessagingDate;

                bool isProductOwner = userDetails?.RoleRecords?.Any(x =>
                    x.RoleName.Equals("SW Product Owner", StringComparison.OrdinalIgnoreCase)) ?? false;

                if (!isProductOwner)
                {
                    return new ResultDto
                    {
                        Data = new
                        {
                            OpenActionsResource = 0,
                            WhatsGoingOnDate = preference?.Preferencedate ?? ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                            MessagingDate = messagingDate
                        }
                    };
                }

                var predicate = ApplyFilterForMajorSoftwareBuild(userId);
                var query = await GetSoftwareBasedEomEosGridRecord(userId, predicate);

                var expiredRecords = query.Where(x =>
                    x.Endofmaintenance < today ||
                    x.Endofsupport < today);

                if (userDetails?.UserRoleId.Any(x => x != 1) == true)
                { 
                    expiredRecords = expiredRecords.Where(f => f.MajorDesignContacts.Any(l => l == userId)).ToList();
                }

                return new ResultDto
                {
                    Data = new
                    {
                        OpenActionsResource = GetEomAndEosSummaryCount(expiredRecords.ToList()),
                        WhatsGoingOnDate = preference?.Preferencedate ?? ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                        MessagingDate = messagingDate
                    }
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex, ex.Message);
                throw;
            }
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
        #endregion

        #region Major Hardware 

        public async Task<List<HomePageEomEosGridDto>> HardwareBasedEomEosGridRecord( long designContactId, ExpressionStarter<Majorhardwarebuilds> predicateResult)
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
        public async Task<ResultDto> HardwareBasedEomEosOpenActionsForDesignContactAsync(long userId)
        {
           
            try
            {

                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);
                var OpenActionsResource = new List<OpenLinkEomAndEosDto>();
                var preferenceDetails = new Gridcustomcolumn();



                var getGridCustomColumns = await _dapperCommonManager.GetGridCustomColumnsAsync(userId);                
                var messagingDateRangeTask = getGridCustomColumns.Where(x => x.Messagingdate != null).FirstOrDefault();
                var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate;

                preferenceDetails = getGridCustomColumns.FirstOrDefault();

                var openExpiredMessagingDate = new DateTime(today.Year, today.Month, 1).AddMonths(-messagingDateRange);

                // Product Importance - Strategic and Non-Strategic #1913 Aprl 01 2026
                //var lcmProductImportanaceId = await _dapperCommonManager.GetLcmProductImportanceIdAsync();

                if (userDetailsList?.RoleRecords?.Any(x =>
                            string.Equals(x.RoleName, "HW Product Owner", StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return new ResultDto
                    {
                        Data = new
                        {
                            OpenActionsResource,
                            WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                            MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                        }
                    };
                }

                #region Query Logic 
                var predicateResult = ApplyFilterForMajorHardwareBuild(userId);
                var query = await HardwareBasedEomEosGridRecord( userId, predicateResult);


                // Open Action
                var expiredRecords = query
                    .Where(x =>
                        (//x.SwEndofmaintenance > openExpiredMessagingDate &&
                         x.Endofmaintenance < today)
                        ||
                        (//x.SwEndofsupport > openExpiredMessagingDate &&
                         x.Endofsupport < today)).ToList();

                #endregion

                //#1913 - Display all the records for Admin
                if (userDetailsList?.UserRoleId.Any(f => f != 1) == true)
                {
                    expiredRecords = expiredRecords.Where(f => f.MajorDesignContacts.Any(l => l == userId)).ToList();

                }

                OpenActionsResource = GroupResultForEomAndEos(expiredRecords);

                return new ResultDto
                {
                    Data = new
                    {
                        OpenActionsResource,
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

        public async Task<ResultDto> HardwareBasedEomEosOpenCountAsync(long userId)
        {

            try
            {

                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);                 
                var preferenceDetails = new Gridcustomcolumn();

                var getGridCustomColumns = await _dapperCommonManager.GetGridCustomColumnsAsync(userId);
                var messagingDateRangeTask = getGridCustomColumns.Where(x => x.Messagingdate != null).FirstOrDefault();
                var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate;

                preferenceDetails = getGridCustomColumns.FirstOrDefault();

                var openExpiredMessagingDate = new DateTime(today.Year, today.Month, 1).AddMonths(-messagingDateRange);
 
                if (userDetailsList?.RoleRecords?.Any(x =>
                            string.Equals(x.RoleName, "HW Product Owner", StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return new ResultDto
                    {
                        Data = new
                        {
                            OpenActionsResource = 0,
                            WhatsGoingOnDate = preferenceDetails != null && preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                            MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                        }
                    };
                }

                #region Query Logic 
                var predicateResult = ApplyFilterForMajorHardwareBuild(userId);
                var query = await HardwareBasedEomEosGridRecord(userId, predicateResult);

                // Open Action
                var expiredRecords = query
                    .Where(x =>
                        (//x.SwEndofmaintenance > openExpiredMessagingDate &&
                         x.Endofmaintenance < today)
                        ||
                        (//x.SwEndofsupport > openExpiredMessagingDate &&
                         x.Endofsupport < today)).ToList();

                #endregion

                //#1913 - Display all the records for Admin
                if (userDetailsList?.UserRoleId.Any(f => f != 1) == true)
                {
                    expiredRecords = expiredRecords.Where(f => f.MajorDesignContacts.Any(l => l == userId)).ToList();

                } 

                return new ResultDto
                {
                    Data = new
                    {
                        OpenActionsResource = GetEomAndEosSummaryCount(expiredRecords),
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


        #region // Life Cycle Manager

        public async Task<List<LifeCycleManagerGridDto>> LcmBasedEomEosGridRecord(long designContactId, ExpressionStarter<Lcmengineering> predicateResult)
        {
            var lcmRecords = await _repositoryWrapper.Lcmengineering
                        .FindByCondition(predicateResult)
                        .AsNoTracking()
                        .Where(x => x.Archived == false &&
                            designContactId == 0 ||
                            x.Lcmengineeringeduspoc
                                .Any(dc => dc.Lcmengineeringeduspocid == designContactId) || 
                            x.Lcmengineeringsubdomainspoc.Any(a => a.Lcmengineeringsubdomainspocid == designContactId))
                          .SelectMany(
                                lcm => lcm.PlannedactivitiesLcmengineering.DefaultIfEmpty(),
                                (lcm, pa) => new LifeCycleManagerGridDto()
                                {
                                    Oem = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                                    ProductName = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description,
                                    ProductId = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Productnameid,
                                    Eom = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance,
                                    Eos = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport,
                                    IsStrategic = lcm.Productimportance.Productimportance.ToLower().Trim() == "strategic" ? true : false,   // adjust if needed
                                    Pa = pa,
                                    Plannedcompletion = pa.Plannedcompletion,
                                    ActionDetail =
                                        $"{pa.Opco.Opco} | " +
                                        $"{pa.Plannedcompletion:MMM-yyyy} | " +
                                        $"{pa.Plannedactivityresource.Plannedactivityresource} | " +
                                        $"{pa.Deliverystatus.Deliverystatus}",

                                    PlannedDcName = DesignComponentTypeExtensionMethod
                                                .ToDesignComponentNameHomePage(pa.Designcomponent, _repositoryWrapper),

                                    CurrentDcName = DesignComponentTypeExtensionMethod
                                                .ToDesignComponentNameHomePage(lcm.Designcomponent, _repositoryWrapper)
                                })
                            .ToListAsync();

            return lcmRecords;
        }


        public async Task<List<Lcmengineering>> GetPlannedSWRecors(ExpressionStarter<Lcmengineering> predicateResult, int userId = 0)
        {
            try
            {

                var query = await _repositoryWrapper.Lcmengineering.FindByCondition(predicateResult).Include(x => x.PlannedactivitiesLcmengineering)
                  .Where(x => userId == 0 || x.Lcmengineeringsubdomainspoc.Any(t => t.Subdomainspocid == userId) || x.Lcmengineeringeduspoc.Any(t => t.Eduspocid == userId))
                  .AsNoTracking()
                  .Select(x => new Lcmengineering
                  {
                      Designcomponent = x.Designcomponent == null ? null : new Designcomponents
                      {
                          Designcomponentid = x.Designcomponentid,
                          Systemtype = x.Designcomponent.Systemtype == null ? null : new Systemtypes
                          {
                              Systemtypeid = x.Designcomponent.Systemtype.Systemtypeid,
                              Majorsoftwarebuilds = x.Designcomponent.Systemtype.Majorsoftwarebuilds == null ? null : new Majorsoftwarebuilds
                              {
                                  Productname = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname,
                                  Orgeqpmanufacturer = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer,
                                  Endofmaintenance = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance,
                                  Endofsupport = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport,
                                  Softwareversion = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion
                              }
                          },
                          //Designcomponentfamily = x.Designcomponent.Designcomponentfamily == null ? null : new Designcomponentfamilies
                          //{
                          //    Subnetworkboundary = x.Designcomponent.Designcomponentfamily.Subnetworkboundary == null ? null : new Subnetworkboundaries
                          //    {
                          //        Alias = x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias,
                          //        Description = x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description,
                          //    }
                          //}
                      },
                      Archived = x.Archived,
                      Deleted = x.Deleted,
                      Opco = x.Opco == null ? null : new Opcos
                      {
                          Opco = x.Opco.Opco,
                          Opcoid = x.Opco.Opcoid,
                      },
                      PlannedactivitiesLcmengineering = x.PlannedactivitiesLcmengineering != null && x.PlannedactivitiesLcmengineering.Count <= 0 ?
                      null : x.PlannedactivitiesLcmengineering.Select(f => new Plannedactivities
                      {
                          Deleted = f.Deleted,
                          Lcmengineeringid = f.Lcmengineeringid,
                          Plannedcompletion = f.Plannedcompletion,
                          Startdate = f.Startdate,
                          Plannedactivityresourceid = f.Plannedactivityresourceid,
                          Deliverystatus = f.Deliverystatus,
                          Deliverystatusid = f.Deliverystatusid,
                          Archived = f.Archived,
                          Opco = f.Opco,
                          Plannedactivityresource = f.Plannedactivityresource,
                          Plannedactivityid = f.Plannedactivityid,
                          Designcomponent = f.Designcomponent == null ? null : new Designcomponents
                          {
                              Designcomponentid = f.Designcomponentid.Value,
                              //Systemtype = f.Designcomponent.Systemtype == null ? null : new Systemtypes
                              //{
                              //    Systemtypeid = f.Designcomponent.Systemtype.Systemtypeid,
                              //    Majorsoftwarebuilds = f.Designcomponent.Systemtype.Majorsoftwarebuilds == null ? null : new Majorsoftwarebuilds
                              //    {
                              //        Productname = f.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname,
                              //        Orgeqpmanufacturer = f.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer,
                              //        Endofmaintenance = f.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance,
                              //        Endofsupport = f.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport,
                              //        Softwareversion = f.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion
                              //    }
                              //},
                              //Designcomponentfamily = f.Designcomponent.Designcomponentfamily == null ? null : new Designcomponentfamilies
                              //{
                              //    Subnetworkboundary = f.Designcomponent.Designcomponentfamily.Subnetworkboundary == null ? null : new Subnetworkboundaries
                              //    {
                              //        Alias = f.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias,
                              //        Description = f.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description,
                              //    }
                              //}
                          },

                      }).OrderBy(x => x.Plannedcompletion).ToList()


                  }).ToListAsync();

                return query;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<ResultDto> GetSignPostRecords(long userId)
        {
            var result = new List<PlanndActivitySoftwareUpgradDetailsDtoGrid>();

            var userDetailsList = _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);
            var swPlannedActivityRuleId = _authorizedRoleManager.GetSwPlannedActivityRulesAsync();
            var OpenActionsResource = new List<DoingSectionForEomAnsEos>();
            var preferenceDetails = new Gridcustomcolumn();

            await Task.WhenAll(userDetailsList, swPlannedActivityRuleId);

            var getGridCustomColumns = await _dapperCommonManager.GetGridCustomColumnsAsync(userId);
            var messagingDateRangeTask = getGridCustomColumns.Where(x => x.Messagingdate != null).FirstOrDefault();
            var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate;

            preferenceDetails = getGridCustomColumns.FirstOrDefault();

            var openExpiredMessagingDate = new DateTime(today.Year, today.Month, 1).AddMonths(-messagingDateRange);           

            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);
            
            var query = await LcmBasedEomEosGridRecord(userId,predicateResult);           

            #region // Conditions

            var remaining = query.ToList();
            var orderedBuckets = new List<dynamic>();

            #region // Planned Completion Past and Strategic
            /* Past Completion – Strategic */

            var paCompPastStrategic = remaining
                .Where(x => x.IsStrategic && IsPast(x.Plannedcompletion)).ToList();
            orderedBuckets.AddRange(paCompPastStrategic);
            remaining = remaining.Except(paCompPastStrategic).ToList();
            #endregion

            #region // Lcm Eos Past and startegic and No PA

            /* EOS expired – no PA – Strategic */
            var eosExpiredNoPaStrategic = remaining
                .Where(x => x.IsStrategic && NoPa(x.Pa) && /*IsEosExpired(x.Eos)*/ IsPast(x.Eos))
                .ToList();
            orderedBuckets.AddRange(eosExpiredNoPaStrategic);
            remaining = remaining.Except(eosExpiredNoPaStrategic).ToList();
            #endregion

            #region // Lcm Eos Expired and PA Created and Strategic
            /* EOS expired – PA created – Strategic */
            var eosExpiredPaStrategic = remaining
                .Where(x => x.IsStrategic && HasPa(x.Pa) && IsPast(x.Eos))
                .ToList();
            orderedBuckets.AddRange(eosExpiredPaStrategic);
            remaining = remaining.Except(eosExpiredPaStrategic).ToList();
            #endregion

            #region // PA Completion Expired and Non Strategic
            /* PA Comp past - Non Strategic */
            var pastPAComplNonStrategic = remaining
               .Where(x => !x.IsStrategic && IsPast(x.Plannedcompletion))
               .ToList();
            orderedBuckets.AddRange(pastPAComplNonStrategic);
            remaining = remaining.Except(pastPAComplNonStrategic).ToList();
            #endregion

            #region // Lcm Eos Expired and No PA and Non Strategic
            /* EOS expired – no PA – Non-Strategic */
            var eosExpiredNoPaNonStrategic = remaining
                .Where(x => !x.IsStrategic && NoPa(x) && IsPast(x.Eos))
                .ToList();
            orderedBuckets.AddRange(eosExpiredNoPaNonStrategic);
            remaining = remaining.Except(eosExpiredNoPaNonStrategic).ToList();
            #endregion

            #region // Lcm Eos Expired and PA and Non Strategic
            /*  EOS expired – PA – Non-Strategic */
            var eosExpiredPaNonStrategic = remaining
                .Where(x => !x.IsStrategic && HasPa(x) && IsPast(x.Eos))
                .ToList();
            orderedBuckets.AddRange(eosExpiredPaNonStrategic);
            remaining = remaining.Except(eosExpiredPaNonStrategic).ToList();
            #endregion

            #region // Lcm Eom Expired and startegic and No PA

            /* EOM expired – no PA – Strategic */
            var eomExpiredNoPaStrategic = remaining
                .Where(x => x.IsStrategic && NoPa(x.Pa) && /*IsEosExpired(x.Eos)*/ IsPast(x.Eom))
                .ToList();
            orderedBuckets.AddRange(eosExpiredNoPaStrategic);
            remaining = remaining.Except(eosExpiredNoPaStrategic).ToList();
            #endregion

            #region // Lcm Eom Expired and PA Created and Strategic
            /* EOS expired – PA created – Strategic */
            var eomExpiredPaStrategic = remaining
                .Where(x => x.IsStrategic && HasPa(x.Pa) && IsPast(x.Eom))
                .ToList();
            orderedBuckets.AddRange(eosExpiredPaStrategic);
            remaining = remaining.Except(eosExpiredPaStrategic).ToList();
            #endregion

            #region // Lcm Eom Expired and No PA and Non Strategic

            /* EOM expired – no PA – Non-Strategic */
            var eomExpiredNoPaNonStrategic = remaining
                .Where(x => !x.IsStrategic && NoPa(x) && IsPast(x.Eom))
                .ToList();
            orderedBuckets.AddRange(eosExpiredNoPaNonStrategic);
            remaining = remaining.Except(eosExpiredNoPaNonStrategic).ToList();
            #endregion

            #region // Lcm Eom Expired and PA and Non Strategic

            /*  EOM expired – PA – Non-Strategic */
            var eomExpiredPaNonStrategic = remaining
                .Where(x => !x.IsStrategic && HasPa(x) && IsPast(x.Eom))
                .ToList();
            orderedBuckets.AddRange(eosExpiredPaNonStrategic);
            remaining = remaining.Except(eosExpiredPaNonStrategic).ToList();
            #endregion
         
            #endregion

            result = orderedBuckets
            .GroupBy(x => x.ProductId)
            .Select(g => new PlanndActivitySoftwareUpgradDetailsDtoGrid
            {
                Oem = g.First().Product?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,
                Product = g.First().Product?.Productname?.Description,

                PlannedAction = g
                    .Where(x => x.Pa != null)
                    .OrderBy(x => x.Pa.Plannedcompletion)
                    .Select(x => new PaPlannedActionDto
                    {
                        Lcmengineeringid = x.Pa.Lcmengineeringid,
                        PaId = x.Pa.Plannedactivityid,
                        Plannedcompletion = x.Pa.Plannedcompletion,
                        ActionDetail =
                            $"{x.Pa.Opco.Opco} | " +
                            $"{x.Pa.Plannedcompletion:MMM-yyyy} | " +
                            $"{x.Pa.Plannedactivityresource?.Plannedactivityresource} | " +
                            $"{x.Pa.Deliverystatus?.Deliverystatus}",

                        PlannedDcName = DesignComponentTypeExtensionMethod
                            .ToDesignComponentNameHomePage(x.Pa.Designcomponent, _repositoryWrapper),

                        CurrentDcName = DesignComponentTypeExtensionMethod
                            .ToDesignComponentNameHomePage(x.Lcm.Designcomponent, _repositoryWrapper)
                    })
                    .ToList()
            })
            .ToList();

            return new ResultDto
            {
                Data = new
                {
                    SignPost = result,
                    WhatsGoingOnDate = preferenceDetails != null && preferenceDetails?.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                    MessagingDate = preferenceDetails != null && preferenceDetails?.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                }
            };
        }

        bool HasPa(dynamic x) => x.Pa != null;
        bool NoPa(dynamic x) => x.Pa == null;

        bool IsPast(DateTime? d) => d.HasValue && d.Value < today;
        bool IsFuture(DateTime? d) => d.HasValue && d.Value >= today;

        bool IsEosExpired(DateTime? d) =>
            d.HasValue && d.Value < today;

        bool IsEosNear(DateTime? d, DateTime? messagingDate = null) =>
            d.HasValue &&
            d >= today &&
            d <= messagingDate;

        bool IsEomExpired(dynamic x) =>
            x.Product?.Eomdate != null && x.Product.Eomdate < today;

        bool IsEomNear(DateTime? d, DateTime? messagingDate = null) =>
            d.HasValue &&
            d >= today &&
            d <= messagingDate;

        #endregion
    }

}
