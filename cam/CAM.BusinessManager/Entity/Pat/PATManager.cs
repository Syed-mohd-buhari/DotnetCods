using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using System.Globalization;
using OracleModels.DBModels;
using CAM.DataTransferObjects.PAT;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using System.Linq.Expressions;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Entities.Mappers.Entity;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using Microsoft.AspNetCore.Http;
using CAM.DataTransferObjects;
using IdentityServer4.Extensions;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.BusinessManager.Entity.Pat
{
    public class PATManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _manager;
        CommonManager _commonManager;
        public PATManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager manager, IHttpContextAccessor contextAccessor, CommonManager commonManager,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _commonManager = commonManager;
        }
        private static ExpressionStarter<Plannedactivities> ApplyFilterPlannedActivity(PATExportQuery buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Plannedactivities>();

            var predicateInner = PredicateBuilder.New<Plannedactivities>();
            if (buildFilterDto?.OpCoId != null && buildFilterDto.OpCoId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto?.OpCoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.DCFId != null && buildFilterDto.DCFId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto?.DCFId)
                    predicateInner.Or(x => x.Lcmengineering.Designcomponent.Designcomponentfamilyid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ProductName != null && buildFilterDto.ProductName.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto?.ProductName)
                    predicateInner.Or(x => x.Lcmengineering.Designcomponent.Designcomponentfamily.Productname.Description.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.VerticalNameId != null && buildFilterDto.VerticalNameId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                int count = 0;

                foreach (var item in buildFilterDto?.VerticalNameId)
                    if(item.ToString() == "0")
                    {
                        count++;

                        predicateInner.Or(x => x.Lcmengineering != null && !x.Lcmengineering.Lcmengineeringsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;

                        predicateInner.Or(x => x.Lcmengineering.Lcmengineeringsubdomainspoc.Any(y => y.Subdomainspoc.AspnetuserverticalsUser
                        .Any(r => r.Organisation.Vertical.Verticalresponsibleid.ToString() == item.ToString() && r.Deleted == false /*&& r.Opcoid == x.Opcoid*/)));

                    }
                if (count > 0) predicateResult.And(predicateInner);
            }
         
            return predicateResult;
        }

        private Dictionary<string, Expression<Func<Plannedactivities, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Plannedactivities, object>>[]>
            {
                ["OpcoId"] = new Expression<Func<Plannedactivities, object>>[] { x => x.Opcoid },
                ["DCFId"] = new Expression<Func<Plannedactivities, object>>[] { x => x.Lcmengineering.Designcomponent.Designcomponentfamilyid },
                ["productName"] = new Expression<Func<Plannedactivities, object>>[] { x => x.Lcmengineering.Designcomponent.Designcomponentfamily.Productname.Description }


            };
        }

        private IQueryable<Plannedactivities> GetPA_Query(ExpressionStarter<Plannedactivities> predicateResult, List<long> plannedActivityIds)
        {
            DateTime thismonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);


            var data = (predicateResult.IsStarted ? _repositoryWrapper.PlannedActivity.FindByCondition(predicateResult).Where(p => plannedActivityIds.Contains(p.Plannedactivityid) && (p.Plannedcompletion != null && p.Plannedcompletion >= thismonth) && (p.Plannedactivityresource.Rulelinkeddc == 1))
                             .Include(p => p.Lcmengineering).ThenInclude(p => p.Opco)
                             .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                             .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Orgeqpmanufacturer)
                             .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname)
                             .Include(p => p.Plannedactivityresource)
                             .Include(p => p.Lcmengineering).ThenInclude(p => p.Designcomponent).ThenInclude(p => p.Designcomponentfamily)
                             .Include(p => p.Lcmengineering).ThenInclude(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(d => d.Orgeqpmanufacturer)
                             .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname)
                             .OrderByDescending(p => p.Modificationdate)

                             : _repositoryWrapper.PlannedActivity.FindAll().Where(p => plannedActivityIds.Contains(p.Plannedactivityid) && (p.Plannedcompletion != null && p.Plannedcompletion >= thismonth) && (p.Plannedactivityresource.Rulelinkeddc == 1))
                             .Include(p => p.Lcmengineering).ThenInclude(p => p.Opco)
                             .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                             .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Orgeqpmanufacturer)
                             .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname)
                             .Include(p => p.Plannedactivityresource)
                             .Include(p => p.Lcmengineering).ThenInclude(p => p.Designcomponent).ThenInclude(p => p.Designcomponentfamily)
                             .Include(p => p.Lcmengineering).ThenInclude(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(d => d.Orgeqpmanufacturer)
                             .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname)
                             .OrderByDescending(p => p.Modificationdate));

            return data.AsQueryable();
        }

        private Dictionary<int, string> GetReleasesValues(List<PlannedActivityReleaseModel> plannedActivityReleaseModels, List<int> months)
        {
            plannedActivityReleaseModels = plannedActivityReleaseModels.OrderBy(p => p.EndDate).ToList();
            Dictionary<int, string> keyValuePairsReleases = new Dictionary<int, string>();

            for (int i = 0; i < plannedActivityReleaseModels.Count; i++)
            {
                var item = plannedActivityReleaseModels[i];
                item.NextMajorRelease = item.NextMajorRelease != null ? item.NextMajorRelease : item.CurrentMajorRelease;

                //If PA has start and end date
                if (item.StartDate != null && item.EndDate != null)
                {
                    //To hundle monthes before first PA
                    if (keyValuePairsReleases == null || keyValuePairsReleases.Count == 0)
                    {
                        // var oldDicEndDate = (i > 0 && plannedActivityReleaseModels[i - 1] != null) ? plannedActivityReleaseModels[i - 1].StartDate.Value.AddMonths(-1) : item.StartDate.Value.AddMonths(-1);

                        var olddic = DCFPatModel.MonthsBetweenNumbers(DateTime.Today, item.StartDate.Value.AddMonths(-1)).ToDictionary(x => x, y => item.CurrentMajorRelease.Replace("MAJOR RELEASE:", "").Trim());
                        foreach (var value in olddic)
                        {
                            if (keyValuePairsReleases.ContainsKey(value.Key))
                                keyValuePairsReleases.Remove(value.Key);
                            keyValuePairsReleases.Add(value.Key, value.Value);
                        }
                    }

                    //To handle monthes that has no PA  (btw PAs)
                    if (i > 0)
                    {
                        var prevMonth = plannedActivityReleaseModels[i - 1].EndDate.Value;
                        var currentMonth = item.StartDate.Value;
                        var btwPA = DCFPatModel.MonthsBetweenNumbers(plannedActivityReleaseModels[i - 1].EndDate.Value.AddMonths(1), item.StartDate.Value).ToDictionary(x => x, y => plannedActivityReleaseModels[i - 1].NextMajorRelease.Replace("MAJOR RELEASE:", "").Trim());
                        if (btwPA != null && btwPA.Count > 1)
                        {
                            foreach (var value in btwPA)
                            {
                                if (keyValuePairsReleases.ContainsKey(value.Key))
                                    keyValuePairsReleases.Remove(value.Key);
                                keyValuePairsReleases.Add(value.Key, value.Value);
                            }
                        }
                    }

                    //To handle monthes included in PA
                    var newdic = DCFPatModel.MonthsBetweenNumbers(item.StartDate.Value, item.EndDate.Value).ToDictionary(x => x, y => item.NextMajorRelease.Replace("MAJOR RELEASE:", "").Trim());

                    foreach (var value in newdic)
                    {
                        if (keyValuePairsReleases.ContainsKey(value.Key))
                            keyValuePairsReleases.Remove(value.Key);
                        keyValuePairsReleases.Add(value.Key, value.Value);
                    }
                }
                else //To handle PA has no start date
                {
                    //To handle monthes before first PA
                    if (keyValuePairsReleases == null || keyValuePairsReleases.Count == 0)
                    {
                        keyValuePairsReleases = DCFPatModel.MonthsBetweenNumbers(DateTime.Today, item.EndDate.Value).ToDictionary(x => x, y => item.CurrentMajorRelease.Replace("MAJOR RELEASE:", "").Trim());
                        keyValuePairsReleases[keyValuePairsReleases.Last().Key] = item.NextMajorRelease.Replace("MAJOR RELEASE:", "").Trim();//(keyValuePairsReleases.Last().Key, item.NextMajorRelease.Replace("MAJOR RELEASE:", "").Trim());

                    }
                    else // To handle monthes btw previous  PA (completiom date) and of current PA 
                    {
                        var newitems = DCFPatModel.MonthsBetweenNumbers(plannedActivityReleaseModels[i - 1].EndDate.Value.AddMonths(1), item.EndDate.Value).ToDictionary(x => x, y => plannedActivityReleaseModels[i - 1].NextMajorRelease.Replace("MAJOR RELEASE:", "").Trim());
                        if (newitems != null && newitems.Count() > 0)
                        {
                            //To update current PA with correct release
                            newitems[newitems.Last().Key] = item.NextMajorRelease.Replace("MAJOR RELEASE:", "").Trim();
                        }


                        foreach (var value in newitems)
                        {
                            if (keyValuePairsReleases.ContainsKey(value.Key))
                                keyValuePairsReleases.Remove(value.Key);
                            keyValuePairsReleases.Add(value.Key, value.Value);
                        }

                    }
                }
            }

            if (keyValuePairsReleases.Count > 0 && keyValuePairsReleases.Count < 18)
            {
                var lastItem = keyValuePairsReleases.Last();
                var keys = keyValuePairsReleases.Select(k => k.Key).ToList();
                var remainingMonths = months.Where(p => !keys.Contains(p)).ToList();
                foreach (var item in remainingMonths)
                {
                    keyValuePairsReleases.Add(item, lastItem.Value.Replace(ConstantValueFilter.majorReleaseLiteral + ":", "").Trim());
                }
            }

            keyValuePairsReleases = keyValuePairsReleases.Take(months.Count).ToDictionary(x => x.Key, y => y.Value);
            return keyValuePairsReleases;
        }

        public Dictionary<short, string> GetSWOem()
        {
            return _repositoryWrapper.OriginalEquipmentManufacturer.FindAll().ToDictionary(x => x.Orgeqpmanufacturerid, y => y.Originalequipmentmanufacturer);
        }
        #region //Need to remove
        private static int SetLcmOrders(Lcmengineering model, PatBuildConstructionEnum? filter)
        {
            var order = filter == PatBuildConstructionEnum.EricssonVirtualizedBundles ?
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "vmware" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "bundle" ? 1
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "ericsson" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "vnfm" ? 2
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "ericsson" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") != "vnfm" ? 3
                         : 4

                         : filter == PatBuildConstructionEnum.HuwaeiVirtualizedBundles ?
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "vmware" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "bundle" ? 1
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "huawei" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "vnfm" ? 2
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "huawei" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") != "vnfm" ? 3
                         : 4

                         : filter == PatBuildConstructionEnum.NokiaVirtualizedBundles ?

                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "vmware" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "bundle" ? 1
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "nokia" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "vnfm" ? 2
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "nokia" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") != "vnfm" ? 3
                         : 4
                         : 1;

            return order;
        }
        #endregion


        public List<PTModel> GetOpcoDCFWithPA(List<Plannedactivities> plannedActivities, bool isBlueprintNFVI = false, bool specialSort = false, bool isEosDateEnable = false, IEnumerable<LcmEngineering> lcmEntity = null)
        {
            List<DCFPatModel> items = new List<DCFPatModel>();
            var result = new List<PTModel>();
            var colorName = new List<string>();
            var _lcmPlannedActivities = plannedActivities; 
            if (_lcmPlannedActivities != null && _lcmPlannedActivities.Count > 0)
            {
                var results = _lcmPlannedActivities.GroupBy(p => new { p.Lcmengineering.Opcoid, p.Lcmengineering.Designcomponent.Designcomponentfamilyid });

                foreach (var item in results)
                {                    

                    var lcm = item.Where(x => x.Lcmengineering != null).Select(x => x.Lcmengineering).FirstOrDefault();

                    var Lcmengineeringeduspoc = (lcm != null) ? lcm?.Lcmengineeringeduspoc?.Select(x => x?.Eduspocid).ToList() :
                       new List<int?>();

                    var paLcmOrderRecord = lcmEntity.Where(x =>lcm.Lcmengineeringid == x.LcmengineeringId).Select(r => r.Order).FirstOrDefault();

                    var lcmOpcoId = lcm?.Opcoid;

                    var data = new DCFPatModel
                    {
                        DCFId = item.Key.Designcomponentfamilyid.Value,
                        OpcoId = item.Key.Opcoid.Value,
                        DCFName = isBlueprintNFVI ? item.Select(p => p.Lcmengineering.Designcomponent.Designcomponentfamily).FirstOrDefault().toDesignComponentFamilyNameForPatNfvi(_repositoryWrapper)
                                   : item.Select(p => p.Lcmengineering.Designcomponent.Designcomponentfamily).FirstOrDefault().DCFName(_repositoryWrapper),
                        OpcoName = item.Select(p => p.Lcmengineering.Opco).FirstOrDefault().Opco,

                        VerticalResponsible = _commonManager.GenerateVerticalFilterResponse(lcm.Lcmengineeringsubdomainspoc?.Where(x => x.Deleted == false).Select(x => x.Subdomainspocid)?.ToList(), lcm.Opcoid),
                      


                        ProductName = item.Select(p => p.Lcmengineering.Designcomponent.Designcomponentfamily)
                                     .FirstOrDefault().toProductName(_repositoryWrapper),
                        PlannedActivities = item.Select(p =>
                        {

                            var lcmMajorSoftware = p.Lcmengineering.Designcomponent.Systemtype.Majorsoftwarebuilds;
                            var paMajorSoftware = p.Designcomponent?.Systemtype?.Majorsoftwarebuilds;

                            var currentRelease = lcmMajorSoftware.Softwareversion;
                            var nextRelease = paMajorSoftware?.Softwareversion ?? currentRelease;

                            var currentEom =  !isEosDateEnable ? lcmMajorSoftware.Endofmaintenance : lcmMajorSoftware.Endofsupport;
                            var nextEom = !isEosDateEnable
                                ? paMajorSoftware?.Endofmaintenance ?? currentEom
                                : paMajorSoftware?.Endofsupport ?? currentEom;

                            return new PlannedActivityReleaseModel
                            {
                                StartDate = p.Startdate,
                                EndDate = p.Plannedcompletion,
                                NextMajorRelease = nextRelease != currentRelease ? nextRelease : currentRelease,
                                CurrentMajorRelease = currentRelease,
                                CurrentEndofMaintenance = currentEom,
                                NextEndofMaintenance = nextEom != currentEom ? nextEom : currentEom
                            };
                        }).ToList(),

                        LastModifiedDate = item.Select(p => p.Lcmengineering.Modificationdate).FirstOrDefault(),
                        order = specialSort ? paLcmOrderRecord : 1
                    };
                    data.Releases = GetReleasesValues(data.PlannedActivities, data.MonthsNumbers).Values.ToList();
 
                    var formattedReleases = new List<string>();
                    for (int i = 0; i < data.Releases.Count; i++)
                    {
                        string currentRelease = data.PlannedActivities.Select(x => x.CurrentMajorRelease).FirstOrDefault();
                        string releaseNo = data.Releases[i].Replace(ConstantValueFilter.majorReleaseLiteral+":", "").Trim();
                        string format = "MMM-yy";
                        DateTime date = DateTime.ParseExact(data.Months[i], format, CultureInfo.InvariantCulture);
                        DateTime endOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

                        DateTime currentEOMDefaultDate = (!isEosDateEnable) ? new DateTime(2034, 12, 31) : 
                            DateTime.Now.AddYears(ConstantValueFilter.eosCalulatedDayCount).Date;

                        DateTime? endOfMaintenance = data.PlannedActivities.Where(x => x.CurrentMajorRelease == releaseNo)
                            .Select(x => x.CurrentEndofMaintenance).FirstOrDefault() ?? currentEOMDefaultDate;

                        DateTime? nextEndOfMaintenance = data.PlannedActivities.Where(x => x.NextMajorRelease == releaseNo)
                            .Select(x => x.NextEndofMaintenance).FirstOrDefault() ?? currentEOMDefaultDate;

                        DateTime? endDate = null;
                        DateTime? startDate = null;

                        string color = ConstantValueFilter.White;

                        if (data.PlannedActivities.Where(x => x.CurrentMajorRelease == releaseNo).Select(x => x.CurrentMajorRelease).FirstOrDefault() != null)
                        {
                            endDate = data.PlannedActivities.Where(x => x.CurrentMajorRelease == releaseNo).Select(x => x.EndDate).FirstOrDefault();
                            startDate = data.PlannedActivities.Where(x => x.CurrentMajorRelease == releaseNo).Select(x => x.StartDate).FirstOrDefault();
                            if (endOfMonth <= endOfMaintenance)
                            {
                                color = ConstantValueFilter.Green;
                            }
                            else
                                color = GetColorForDateRange(endOfMonth, startDate, endDate, ConstantValueFilter.Yellow, ConstantValueFilter.Red);
                           
                        }

                        if (data.PlannedActivities.Where(x => x.NextMajorRelease == releaseNo).Select(x => x.NextMajorRelease).FirstOrDefault() != null)
                        {
                            endDate = data.PlannedActivities.Where(x => x.NextMajorRelease == releaseNo).Select(x => x.EndDate).FirstOrDefault();
                            startDate = data.PlannedActivities.Where(x => x.NextMajorRelease == releaseNo).Select(x => x.StartDate).FirstOrDefault();
                            if (endOfMonth <= nextEndOfMaintenance)
                            {
                               color = GetColorForDateRange(endOfMonth, startDate, endDate, ConstantValueFilter.Yellow, ConstantValueFilter.Green);
                                if (color == ConstantValueFilter.Yellow)
                                {
                                    releaseNo = currentRelease + "/" + releaseNo;
                                }
                            }
                            else
                            {
                                 color = GetColorForDateRange(endOfMonth, startDate, endDate, ConstantValueFilter.Yellow, ConstantValueFilter.Red);
                                if (color == ConstantValueFilter.Yellow)
                                {
                                    releaseNo = currentRelease + "/" + releaseNo;
                                }
                            }
                        }

                        if (releaseNo.ToLower() == ConstantValueFilter.Unknown)
                        {
                            formattedReleases.Add($"{releaseNo} | {ConstantValueFilter.White}");
                        }
                        else
                        {
                            formattedReleases.Add($"{releaseNo} | {color}");
                        }
                    }


                    data.Releases = formattedReleases;

                    items.Add(data);

                }


            }

            return items.Distinct().Select(p => new PTModel()
            {
                DCFId = p.DCFId,
                DCFName = p.DCFName,
                OpcoId = p.OpcoId,
                ProductName = p.ProductName,
                OpcoName = p.OpcoName,
                VerticalName = string.Join(",", p.VerticalResponsible.ToList()?.Select(m => m.Text)?.Distinct()?.ToList() ?? new List<string>()),
                VerticalNameId = p.VerticalNameId,
                Releases = p.Releases,
                LastModifiedDate = p.LastModifiedDate,
                order = p.order,
                VerticalResponsible = p.VerticalResponsible
            }).ToList();

        }

        // Helper Method
 private  string GetColorForDateRange (DateTime endOfMonth, DateTime? startDate, DateTime? endDate, string inRangeColor, string outOfRangeColor)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                return endOfMonth >= startDate.Value && endOfMonth <= endDate.Value ? inRangeColor : outOfRangeColor;
            }
            return outOfRangeColor;
        }

        public QueryResultDto<PTModel> FindWithCondition(PATExportQuery buildFilterDto, bool isExport = false)
        {
            int totalItemsCount = 0;
            var allReportData = GetPatData(buildFilterDto, out totalItemsCount, isExport);
            var data = new DCFPatModel();
            var model = new PTModel()
            {

                DCFName = "",
                OpcoName = "",
                ProductName = "",
                Months = data.Months,
                VerticalName = "",
                VerticalNameId = default(int)

            };

            if (allReportData == null || allReportData.Count == 0)
            {
                allReportData.Add(model);
            }

            var rtn = new QueryResultDto<PTModel>(
                new GenerateRenderForGrid<PTModel>(_manager))
            {
                TotalItems = totalItemsCount
            };



            if (isExport)
            {
                buildFilterDto.PageSize = rtn.TotalItems;
            }

            rtn.Items = allReportData.ToArray();
            rtn.Items[0].Months = model.Months;


            return rtn;
        }

        public List<PTModel> GetOpcoDCFWithoutPA(List<LcmEngineering> lcmEng, List<short> opCoId, bool isBlueprintNFVI = false, bool isEosDateEnable = false)
        {
            var result = new List<PTModel>();
            List<DCFPatModel> items = new List<DCFPatModel>();
            var lcms = lcmEng;

            if (lcmEng != null && lcmEng.Count() > 0 && opCoId != null && opCoId.Count > 0)
            {
                lcmEng = lcmEng.Where(x => x.OpCo != null && opCoId.Contains(x.OpCoId.Value)).ToList();
            }

            //var monthes = 18;

            foreach (var lcm in lcms)
            {
                if (lcm.OpCo != null && lcm.DesignComponent != null && lcm.DesignComponent.DesignComponentFamily != null)
                {
                    var releaseNo = lcm.DesignComponent.SystemType.MajorSoftwareBuilds.SoftwareVersion;
                    var endOfMaintenance = (!isEosDateEnable)? lcm.DesignComponent.SystemType.MajorSoftwareBuilds.EndOfMaintenance
                        : lcm.DesignComponent.SystemType.MajorSoftwareBuilds.EndOfsupport;
                    var data = new DCFPatModel
                    {
                        DCFId = lcm.DesignComponent.DesignComponentFamilyId.Value,
                        OpcoId = lcm.OpCoId.Value,
                        DCFName = isBlueprintNFVI ? lcm.DesignComponent.DesignComponentFamily.toDesignComponentFamilyNameForPatNfvi(_repositoryWrapper) : lcm.DesignComponent.DesignComponentFamily.DCFName(_repositoryWrapper),
                        OpcoName = lcm.OpCo.OpCoDescription,
                        ProductName = lcm.DesignComponent.DesignComponentFamily.toProductName(_repositoryWrapper),
                        order = lcm.Order,
                       
                        VerticalResponsible = _commonManager.GenerateVerticalFilterResponse(lcm.LcmEngineeringSubDomainSpoc.Where(x => x.Deleted == false).Select(x => x.Subdomainspocid).ToList(), lcm.OpCoId).ToList(),
                       

                    };
                    data.LastModifiedDate = lcm.ModificationDate;
                    var colorName = new List<string>();

                    foreach (var month in data.Months)
                    {
                        string format = "MMM-yy";
                        DateTime date = DateTime.ParseExact(month, format, CultureInfo.InvariantCulture);
                        if (endOfMaintenance == null)
                            endOfMaintenance = (!isEosDateEnable) ? new DateTime(2034, 12, 31) : DateTime.Now.AddYears(ConstantValueFilter.eosCalulatedDayCount).Date;
                        if (date <= (DateTime)endOfMaintenance)
                        {
                            colorName.Add(ConstantValueFilter.Green);
                        }
                        else
                        {
                            colorName.Add(ConstantValueFilter.Red);
                        }
                    }

                    var formattedReleases = new List<string>();
                    for (int i = 0; i < colorName.Count; i++)
                    {
                        if (releaseNo.ToLower() == ConstantValueFilter.Unknown)
                        {
                            formattedReleases.Add($"{releaseNo.Replace(ConstantValueFilter.majorReleaseLiteral+":", "").Trim()} | {ConstantValueFilter.White}");
                        }
                        else
                        {
                            formattedReleases.Add($"{releaseNo.Replace(ConstantValueFilter.majorReleaseLiteral + ":", "").Trim()} | {colorName[i]}");
                        }

                    }

                    data.Releases = formattedReleases;

                    items.Add(data);
                }

            }

            return items.Distinct().Select(p => new PTModel()
            {
                DCFId = p.DCFId,
                DCFName = p.DCFName,
                OpcoId = p.OpcoId,
                OpcoName = p.OpcoName,
                ProductName = p.ProductName,
                Releases = p.Releases,
                LastModifiedDate = p.LastModifiedDate,
                order = p.order,
                VerticalName = string.Join(",", p.VerticalResponsible.ToList()?.Select(m => m.Text)?.Distinct()?.ToList() ?? new List<string>()),
                VerticalResponsible = p.VerticalResponsible,
                //lcm = p.lcm,
            }).ToList();

        }

        private static ExpressionStarter<Lcmengineering> ApplyFilter(PATExportQuery buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Lcmengineering>();

            var predicateInner = PredicateBuilder.New<Lcmengineering>();
            if (buildFilterDto?.OpCoId != null && buildFilterDto.OpCoId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OpCoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.VerticalNameId != null && buildFilterDto.VerticalNameId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                int count = 0;

                foreach (var item in buildFilterDto?.VerticalNameId)
                    if(item.ToString() == "0")
                    {
                        count++;

                        predicateInner.Or(x=>!x.Lcmengineeringsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;

                        predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(y => y.Subdomainspoc.AspnetuserverticalsUser
                        .Any(r => r.Organisation.Vertical.Verticalresponsibleid.ToString() == item.ToString() && r.Deleted == false)));
                    }
                if (count > 0) predicateResult.And(predicateInner);
            }
          
            if (buildFilterDto?.DCFId != null && buildFilterDto.DCFId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.DCFId)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamilyid == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<Lcmengineering> GetQuery(ExpressionStarter<Lcmengineering> predicateResult)
        {
            DateTime thismonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var lcms = (predicateResult.IsStarted ? _repositoryWrapper.Lcmengineering
                  .FindByCondition(predicateResult).Where(p => p.Opco != null && p.Archived == false)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Designcomponentfamily)
                  .Include(p => p.Lcmengineeringsubdomainspoc)/*.ThenInclude(x=>x.Eduspoc).ThenInclude(x=>x.Aspnetuserroles).ThenInclude(x=>x.Verticalresponsible)*/
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Orgeqpmanufacturer)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction)
                  .Include(p => p.PlannedactivitiesLcmengineering).ThenInclude(p => p.Plannedactivityresource)
                  .Include(p => p.Opco)
                  .OrderBy(P => P.Opco.Opco)

                  : _repositoryWrapper.Lcmengineering
                  .FindByCondition(p => p.Opco != null).Where(p => p.Opco != null && p.Archived == false)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Designcomponentfamily)
                  .Include(p => p.Lcmengineeringsubdomainspoc)/*.ThenInclude(x => x.Eduspoc).ThenInclude(x => x.Aspnetuserroles).ThenInclude(x => x.Verticalresponsible)*/
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Orgeqpmanufacturer)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname).Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction)
                  .Include(p => p.PlannedactivitiesLcmengineering).ThenInclude(p => p.Plannedactivityresource)
                  .Include(p => p.Opco)
                  .OrderBy(P => P.Opco.Opco)
                  );

            return lcms.AsQueryable();
        }


        private IQueryable<Lcmengineering> GetQueryWithModifiedDate(ExpressionStarter<Lcmengineering> predicateResult)
        {
            DateTime thismonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var lcms = (predicateResult.IsStarted ? _repositoryWrapper.Lcmengineering
                  .FindByCondition(predicateResult).Where(p => p.Opco != null && p.Archived == false)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Designcomponentfamily)
                  .Include(p => p.Lcmengineeringsubdomainspoc)/*.ThenInclude(x => x.Eduspoc).ThenInclude(x => x.Aspnetuserroles).ThenInclude(x => x.Verticalresponsible)*/
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Orgeqpmanufacturer)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction)
                  .Include(p => p.PlannedactivitiesLcmengineering).ThenInclude(p => p.Plannedactivityresource)
                  .Include(p => p.Opco)
                  .OrderByDescending(P => P.Modificationdate)

                  : _repositoryWrapper.Lcmengineering
                  .FindByCondition(p => p.Opco != null && p.Archived == false)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Designcomponentfamily )
                  .Include(p => p.Lcmengineeringsubdomainspoc)/*.ThenInclude(x => x.Eduspoc).ThenInclude(x => x.Aspnetuserroles).ThenInclude(x => x.Verticalresponsible)*/
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Orgeqpmanufacturer)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname).Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction)
                  .Include(p => p.PlannedactivitiesLcmengineering).ThenInclude(p => p.Plannedactivityresource)
                  .Include(p => p.Opco)
                  .OrderByDescending(P => P.Modificationdate)
                  );

            return lcms.AsQueryable();
        }


        private List<PTModel> GetPatData(PATExportQuery buildFilterDto, out int totalItemsCount, bool isExport = false, bool filter = false)
        {
            DateTime thismonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ExpressionStarter<Lcmengineering> predicateResult = ApplyFilter(buildFilterDto);

            if (buildFilterDto.VendorIds != null && buildFilterDto.VendorIds.Count > 0)
                predicateResult = predicateResult.And(p => buildFilterDto.VendorIds.Contains(p.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid));

            bool isBlueprintNFVI = false;
            bool specialSort = false;

            if (buildFilterDto.BuildConstruction != null && buildFilterDto.BuildConstruction != 0)
            {
                if (buildFilterDto.BuildConstruction == (int)PatBuildConstructionEnum.BLUEPRINTNFVI || buildFilterDto.BuildConstruction == (int)PatBuildConstructionEnum.BLUEPRINTNFCI)
                {
                    isBlueprintNFVI = true;
                }

                var buildcons = "";
                if (buildFilterDto.BuildConstruction == (int)PatBuildConstructionEnum.EricssonVirtualizedBundles ||
                    buildFilterDto.BuildConstruction == (int)PatBuildConstructionEnum.HuwaeiVirtualizedBundles ||
                    buildFilterDto.BuildConstruction == (int)PatBuildConstructionEnum.NokiaVirtualizedBundles)
                {
                    buildcons = (PatBuildConstructionEnum.BLUEPRINTNFVI).ToString().Replace(" ", "").ToLower();
                    isBlueprintNFVI = true;
                    specialSort = true;
                }
                else
                {
                    buildcons = ((PatBuildConstructionEnum)buildFilterDto.BuildConstruction).ToString().Replace(" ", "").ToLower();
                    isBlueprintNFVI = true;
                }

                predicateResult = predicateResult.And(p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(m =>
                                   m.Ismain && m.Deleted == false).Majorhardware.Buildconstruction.Buildconstruction.Replace(" ", "").ToLower() == buildcons);
            }
            //Ticket 1175 - Exclude the No-PA reocrds in PA Tracker
            predicateResult = predicateResult.And(p => p.PlannedactivitiesLcmengineering.Any(t => t.Plannedactivityresource.Rulelinkeddc
            != (int)PlannedActivityResourceEnum.No_PlannedActivity));


            IQueryable<Lcmengineering> dbQuerty;

            IEnumerable<LcmEngineering> query = new List<LcmEngineering>();
            IEnumerable<LcmEngineering> allData = new List<LcmEngineering>(); 

            if (specialSort)
            {
                dbQuerty = GetQuery(predicateResult);
                var models = dbQuerty.Select(p => LCMEngineeringMapper.GetLcmEngineeringMapper(p, true, buildFilterDto.BuildConstruction != null ? (PatBuildConstructionEnum)buildFilterDto.BuildConstruction : PatBuildConstructionEnum.AllSoftware)).ToList();
                models.ForEach(x => x.NumberOfNodes = x.CountNetworkElementReleated(false, _repositoryWrapper));
                allData = models.Where(x => x.NumberOfNodes > 0);
                if (isExport)
                {
                    query = allData.OrderBy(x => x.OpCo.OpCoDescription).ThenBy(x => x.Order).ThenByDescending(X => X.ModificationDate);
                }
                else
                {
                    query = allData.OrderBy(x => x.OpCo.OpCoDescription).ThenBy(x => x.Order).ThenByDescending(X => X.ModificationDate)
                                .Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize);
                }
            }
            else
            {
                dbQuerty = GetQueryWithModifiedDate(predicateResult);
                var models = dbQuerty.Select(p => LCMEngineeringMapper.GetLcmEngineeringMapper(p, true, buildFilterDto.BuildConstruction != null ? (PatBuildConstructionEnum)buildFilterDto.BuildConstruction : PatBuildConstructionEnum.AllSoftware)).ToList();
                models.ForEach(x => x.NumberOfNodes = x.CountNetworkElementReleated(false, _repositoryWrapper));
                allData = models.Where(x => x.NumberOfNodes > 0);
                if (isExport)
                {
                    query = allData.OrderByDescending(X => X.ModificationDate);
                }
                else
                {

                    query = allData.OrderByDescending(X => X.ModificationDate).ToList();
                }
            }
             #region // DTO Function

            DateTime currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


            var lcmIds = query.Select(p => p.LcmengineeringId).ToList();

            var plannedActivities = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Lcmengineeringid != null && x.Plannedcompletion.Value.Date >= currentDate.Date && lcmIds.Contains(x.Lcmengineeringid.Value));
            var plannedActivityIds = plannedActivities.Select(x => x.Plannedactivityid).ToList(); 

            ExpressionStarter<Plannedactivities> pa_predicateResult = ApplyFilterPlannedActivity(buildFilterDto);
            pa_predicateResult = pa_predicateResult.And(p => p.Plannedactivityresource.Rulelinkeddc != (int)(int)PlannedActivityResourceEnum.No_PlannedActivity);

            var result = GetPA_Query(pa_predicateResult, plannedActivityIds).ToList();

            var PAlcmIds = result.Select(x => x.Lcmengineeringid.Value).ToList();

            var lcmsWithoutPA = query.Where(x => !PAlcmIds.Any(y => y == x.LcmengineeringId)).ToList();



            List<PTModel> patWithoutPA = GetOpcoDCFWithoutPA(lcmsWithoutPA, buildFilterDto.OpCoId, isBlueprintNFVI, buildFilterDto.IsEosDateEnable);


            var allReportData = GetOpcoDCFWithPA(result,
                isBlueprintNFVI, specialSort , buildFilterDto.IsEosDateEnable, query);


            allReportData = ((buildFilterDto.VerticalNameId != null &&
                buildFilterDto.VerticalNameId.Count() != 0) ?
                    allReportData.Where(x => x.VerticalName != null).ToList()
               : allReportData.ToList());

            allReportData.AddRange(patWithoutPA);

            #endregion

            if (buildFilterDto.VerticalNameId ==null ||  buildFilterDto.VerticalNameId.Any(x => x.ToString() == "0") || buildFilterDto.VerticalNameId.Count() == 0)
            {
                allReportData = allReportData.ToList();
            }
            else
            {
                allReportData = allReportData.ToList().Where(x => x.VerticalName != string.Empty).ToList();
            }

            if (specialSort)
            {
                allReportData = allReportData.OrderBy(x => x.OpcoName).ThenBy(x => x.order).ThenByDescending(x => x.LastModifiedDate).ToList();
            }
            else
            {
                allReportData = allReportData.OrderByDescending(x => x.LastModifiedDate).ToList();

            }
            totalItemsCount = allReportData.Count();
            if (!filter)
            {
                allReportData = allReportData.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
            }
            return allReportData;
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, PATExportQuery buildFilterDto,bool isAdmin)
        {
            var totalCount = 0;
            var PATList = GetPatData(buildFilterDto, out totalCount, true, true);
            var result = new List<FilterValueDto>();
 
            var rtn = propertyName switch
            {
                "opCoId" => string.IsNullOrEmpty(propertyFilter)
                   ? PATList.Select(p => new FilterValueDto
                   {
                       Text = p.OpcoName,
                       Value = p.OpcoId.ToString()
                   }).Distinct().ToList()
                   : PATList.Where(x =>
                           x.OpcoName.ToLower().Contains(propertyFilter.ToLower())).Select(p => new FilterValueDto
                           {
                               Text = p.OpcoName,
                               Value = p.OpcoId.ToString()
                           }).Distinct().ToList(),

                "verticalNameId" => string.IsNullOrEmpty(propertyFilter)
                   ? PATList.SelectMany(x => x.VerticalResponsible.Select(
                    y => new FilterValueDto
                    {
                        Text = y.Text,
                        Value = y.Value
                    })).Distinct().ToList()
                    : PATList.SelectMany(x => x.VerticalResponsible.Select(
                    y => new FilterValueDto
                    {
                        Text = y.Text,
                        Value = y.Value
                    })).Where(r => r.Text.ToLower().Contains(propertyFilter.ToLower())).Distinct().ToList(),


                "dcfId" => string.IsNullOrEmpty(propertyFilter) ?
                PATList.Select(p => new FilterValueDto(p.DCFId, p.DCFName)).Distinct().ToList()

                : PATList.Where(x => x.DCFName.ToLower().Contains(propertyFilter.ToLower()))
                .Select(p => new FilterValueDto(p.DCFId, p.DCFName)).Distinct().ToList(),

                "productName" => string.IsNullOrEmpty(propertyFilter)
                           ? PATList.Select(p => new FilterValueDto
                           {
                               Text = p.ProductName,
                               Value = p.ProductName
                           }).Distinct().ToList()
                           : PATList.Where(x =>
                                   x.ProductName.Contains(propertyFilter.ToLower())).Select(p => new FilterValueDto
                                   {
                                       Text = p.ProductName,
                                       Value = p.ProductName
                                   }).Distinct().ToList(),

                _ => new List<FilterValueDto>(),
            };
            if (!isAdmin && (buildFilterDto.VerticalNameId != null && buildFilterDto.VerticalNameId.Count > 0) && propertyName == "verticalNameId")
            {
                rtn = rtn.Where(x => buildFilterDto.VerticalNameId.Contains(Convert.ToInt32(x.Value))).ToList();
            }
            return rtn;

        }

        public List<short> GetVendorsOfPredefinedFilter(int filterId)
        {
            List<short> vendors = new List<short>();
            if (filterId == (int)PatBuildConstructionEnum.EricssonVirtualizedBundles)
            {
                var vendorList = _repositoryWrapper.OriginalEquipmentManufacturer
                    .FindByCondition(x => x.Originalequipmentmanufacturer.Replace(" ", "").ToLower() == "ericsson" ||
                    x.Originalequipmentmanufacturer.Replace(" ", "").ToLower() == "vmware").Select(x => x.Orgeqpmanufacturerid).ToList();
                vendors.AddRange(vendorList);
            }

            else if (filterId == (int)PatBuildConstructionEnum.HuwaeiVirtualizedBundles)
            {
                var vendorList = _repositoryWrapper.OriginalEquipmentManufacturer
                    .FindByCondition(x => x.Originalequipmentmanufacturer.Replace(" ", "").ToLower() == "huawei" ||
                    x.Originalequipmentmanufacturer.Replace(" ", "").ToLower() == "vmware").Select(x => x.Orgeqpmanufacturerid).ToList();
                vendors.AddRange(vendorList);
            }

            else if (filterId == (int)PatBuildConstructionEnum.NokiaVirtualizedBundles)
            {
                var vendorList = _repositoryWrapper.OriginalEquipmentManufacturer
                    .FindByCondition(x => x.Originalequipmentmanufacturer.Replace(" ", "").ToLower() == "nokia" ||
                    x.Originalequipmentmanufacturer.Replace(" ", "").ToLower() == "vmware").Select(x => x.Orgeqpmanufacturerid).ToList();
                vendors.AddRange(vendorList);
            }
            return vendors;
        }
    }
}
