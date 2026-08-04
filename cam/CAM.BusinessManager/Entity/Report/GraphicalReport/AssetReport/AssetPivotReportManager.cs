using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.GraphicalReport.AssetReport
{
    public class AssetPivotReportManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _manager;
        private PlannedActivityManager _plannedActivityManager;
        private DesignComponentFamilyLifeCycleManager _designComponentFamilyLifeCycleManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly CommonManager _commonManager;
        private readonly NetworkElementsAsPlannedManager _networkElementsAsPlannedManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;

        private List<FilterValueDto> getLocations = new List<FilterValueDto>();
        public AssetPivotReportManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, IdentityAsIsManager identityAsIsManager,
            GridCustomColumnManager manager, PlannedActivityManager plannedActivityManager, DesignComponentFamilyLifeCycleManager designComponentLIfecycleManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper,
            ICurrentUserService currentUserService, CommonManager commonManager, NetworkElementsAsPlannedManager networkElementsAsPlannedManager
            , DropdownDataServiceManager dropdownDataServiceManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _plannedActivityManager = plannedActivityManager;
            _designComponentFamilyLifeCycleManager = designComponentLIfecycleManager;
            _currentUserService = currentUserService;
            _commonManager = commonManager;
            _networkElementsAsPlannedManager = networkElementsAsPlannedManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
        }

        public async Task<QueryResultDto<NetworkElementAsPlannedPivotDtoGrid>> FindWithCondition(NetworkElementAsPlannedQueryDto assetQueryDto)
        {
            var processedAssetRecord = await GetProcessedRecords(assetQueryDto);
            int totalRecordCount = processedAssetRecord != null ? processedAssetRecord.Count() : 0;
            processedAssetRecord = processedAssetRecord.OrderBy(x => x.PaImplementaionYear).ThenBy(x => x.DesignComponent)
                     .Skip((assetQueryDto.Page - 1) * assetQueryDto.PageSize).Take(assetQueryDto.PageSize).ToList();


            var rtn = new QueryResultDto<NetworkElementAsPlannedPivotDtoGrid>(new GenerateRenderForGrid<NetworkElementAsPlannedPivotDtoGrid>(_manager))
            {
                TotalItems = totalRecordCount,
                Items = processedAssetRecord
            };

            return rtn;

        }
        public async Task<List<NetworkElementAsPlannedPivotDtoGrid>> GetProcessedRecords(NetworkElementAsPlannedQueryDto assetQueryDto, bool filter = false)
        {
            var fetchEnvironement = _dropdownDataServiceManager.GetEnvironement(false)?.Result;

            var productionEnvironementId = fetchEnvironement.Where(x => x.Value.ToLower() == ConstantValueFilter.production).Select(y => y.Key)?.FirstOrDefault();

            #region // Filters Region

            if (productionEnvironementId != 0 && (assetQueryDto.Environment == null || assetQueryDto.Environment?.Any() == false))
            {
                assetQueryDto.Environment ??= new List<short>();
                assetQueryDto.Environment.Add((short)productionEnvironementId);
            }

            if (assetQueryDto.isDcfImplementation == null || assetQueryDto.isDcfImplementation?.Any() == false)
            {
                assetQueryDto.isDcfImplementation ??= new List<bool>();
                assetQueryDto.isDcfImplementation.Add(true);
            }

            if ((assetQueryDto.DeploymentStatus == null || assetQueryDto.DeploymentStatus?.Any() == false))
            {
                var deploymentStatus = _dropdownDataServiceManager.GetAssetDeploymentStatus(false)?.Result.Where(x => x.Value.ToLower() == ConstantValueFilter.InService
        || x.Value.ToLower() == ConstantValueFilter.Planned)?.ToList();

                assetQueryDto.DeploymentStatus ??= new List<short>();
                assetQueryDto.DeploymentStatus.AddRange(deploymentStatus.Select(m => m.Key));
            }


            var predicateResult = NetworkElementsAsPlannedManager.ApplyFilter(assetQueryDto);

            if (assetQueryDto.Deleted == ConstantValueFilter.isTrue)
            {
                predicateResult = predicateResult.And(x => x.Deleted == true);
            }
            #endregion
            var data =  _networkElementsAsPlannedManager.GetQueryRecords(predicateResult, false, true).Result.ToList();

            getLocations = data?.Select(x => new FilterValueDto
            {
                Text = x.Location.Location,
                Value = x.Locationid.ToString()
            })?.Distinct().ToList();

            IEnumerable<NetworkElementAsPlannedPivotDtoGrid> pivotGrid = data.Where(x => x.Lcmengineeringid != null
           && x.Lcmengineering.PlannedactivitiesLcmengineering.Any())
                      .SelectMany(y => y.Lcmengineering.PlannedactivitiesLcmengineering
                      .Where(d => assetQueryDto.PaImplementaionYear == null || !assetQueryDto.PaImplementaionYear.Any() ||
           assetQueryDto.PaImplementaionYear.Contains(d.Plannedimplementationyear.ToString()))
                      .Select(n => new NetworkElementAsPlannedPivotDtoGrid
                      {
                          VerticalFilterDto = y.Networkelementasplannedsubdomainspoc?.Count > 0?
                                              _commonManager.GetVerticaleFilterDto(y.Networkelementasplannedsubdomainspoc.Select(x => x.Subdomainspocid).ToList(), 0, false, true).ToList()
                                              : new List<FilterValueDto>(),
                          DesignComponentIndex = y.Designcomponentid,
                          DesignComponent = y.Designcomponent.toDesignComponentName(),
                          Location = y.Location.Location,
                          LocationId = (int)y.Locationid,
                          PaImplementaionYear = n.Plannedimplementationyear == 0 ? string.Empty : n.Plannedimplementationyear.ToString()

                      }));
            // Filter Assets have No PA & LCM Level PA
            var noPAandLCMAsset = data.Where(x => x.Lcmengineeringid == null ||
                                                     x.Lcmengineeringid != null && !x.Lcmengineering.PlannedactivitiesLcmengineering.Any())
                                          .Select(y => new NetworkElementAsPlannedPivotDtoGrid
                                          {
                                              VerticalFilterDto = y.Networkelementasplannedsubdomainspoc?.Count > 0 ?
                                                                  _commonManager.GetVerticaleFilterDto(y.Networkelementasplannedsubdomainspoc.Select(x => x.Subdomainspocid).ToList(), 0, false, true).ToList()
                                                                  : new List<FilterValueDto>(),
                                              DesignComponentIndex = y.Designcomponentid,
                                              DesignComponent = y.Designcomponent.toDesignComponentName(),
                                              Location = y.Location.Location,
                                              LocationId = (int)y.Locationid,
                                              PaImplementaionYear = string.Empty
                                          });


            // Merge results
            if (pivotGrid != null && pivotGrid.Any())
            {
                if (noPAandLCMAsset != null && noPAandLCMAsset.Any())
                {
                    pivotGrid = pivotGrid.Union(noPAandLCMAsset).ToList();
                }
            }
            else if (noPAandLCMAsset != null && noPAandLCMAsset.Any())
            {
                pivotGrid = noPAandLCMAsset;
            }
            var assets = pivotGrid?.GroupBy(m => new { m.DesignComponentIndex, m.PaImplementaionYear }).ToList().Select(sg => new NetworkElementAsPlannedPivotDtoGrid
            {
                DesignComponentIndex = sg.Key.DesignComponentIndex,
                DesignComponent = sg.First().DesignComponent,
                PaImplementaionYear = (!filter) ?
                (!string.IsNullOrEmpty(sg.Key.PaImplementaionYear)) ?
                $"{ConstantValueFilter.finacialYearPrefix + sg.Key.PaImplementaionYear.ToString().Substring(sg.Key.PaImplementaionYear.ToString().Length - 2)} " +
                $"| {ConstantValueFilter.Apricot}" : string.Empty :
                (!string.IsNullOrEmpty(sg.Key.PaImplementaionYear)) ?
                ConstantValueFilter.finacialYearPrefix + sg.Key.PaImplementaionYear.ToString().Substring(sg.Key.PaImplementaionYear.ToString().Length - 2) : string.Empty,

                PlannedImplementaionYear = (!string.IsNullOrEmpty(sg.Key.PaImplementaionYear)) ? sg.Key.PaImplementaionYear.ToString() : string.Empty,
                Locations = GetLocation(sg.GroupBy(x => x.DesignComponentIndex).SelectMany(t => t.Select(m => m.LocationId)).ToList()).OrderBy(m => m.Key).ToList(),
                Total = sg.GroupBy(x => x.DesignComponentIndex).SelectMany(t => t.Select(m => m.LocationId)).Count(),
                VerticalName=sg.First().VerticalFilterDto?.Count>0?string.Join(",", sg.First().VerticalFilterDto.Select(x => x.Text).ToList() ?? new List<string>()) : string.Empty,
                VerticalFilterDto=sg.First().VerticalFilterDto,

            })?.Where(x => x.Total != 0).OrderBy(x => x.PaImplementaionYear).ThenBy(x => x.DesignComponent).ToList();

            return assets;
        }
        public List<KeyValuePair<string, int>> GetLocation(List<int> pivotLocation)
        {
            List<KeyValuePair<string, int>> locationBasedAssetCount = new List<KeyValuePair<string, int>>();
            if (getLocations.Count > 0)
            {
                var assetCount = new List<KeyValuePair<string, int>>();
                foreach (var item in getLocations)
                {
                    var count = pivotLocation.Count(m => m.ToString() == item.Value.ToString());

                    if (count > 0)
                    {
                        assetCount.Add(new KeyValuePair<string, int>($"{item.Text} | {ConstantValueFilter.Apricot}",
                         count));
                    }
                    else
                    {
                        assetCount.Add(new KeyValuePair<string, int>(item.Text,
                         count));
                    }


                }
                locationBasedAssetCount = assetCount;
            }

            return locationBasedAssetCount;
        }
        public async Task<ResultDto> GetFilterDropdown(List<short> opcoList)
        {
            var deploymentStatus = _dropdownDataServiceManager.GetAssetDeploymentStatus(false)?.Result.Where(x => x.Value.ToLower() == ConstantValueFilter.InService
            || x.Value.ToLower() == ConstantValueFilter.Planned)?.ToList();

            var opco = await _dropdownDataServiceManager.GetOpcos(false, false, true,false,opcoList);

            List<KeyValuePair<short, string>> hardwareType = new List<KeyValuePair<short, string>>()
                    {
                         new KeyValuePair<short, string>(1, "Virtualized"),
                         new KeyValuePair<short, string>(2, "Non-Virtualized")
                    };


            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = new
                {
                    deploymentStatus,
                    hardwareType,
                    opco
                }
            };
        }
        public async Task<List<FilterValueDto>> GetFilterForGridLevel(string propertyName, string propertyFilter, NetworkElementAsPlannedQueryDto buildFilterDto,bool isAdmin)
        {
            var data = await GetProcessedRecords(buildFilterDto, true);

            var rtn = propertyName switch
            {
                "designComponent" => string.IsNullOrEmpty(propertyFilter)
                    ? data.Select(p => new FilterValueDto
                    {
                        Text = p.DesignComponent,
                        Value = p.DesignComponentIndex.ToString()
                    }).Distinct().ToList()
                    : data.Select(p => new FilterValueDto
                    {
                        Text = p.DesignComponent,
                        Value = p.DesignComponentIndex.ToString()
                    }).Where(m => m.Text.ToLower().Contains(propertyFilter)).Distinct().ToList(),

                "paImplementaionYear" => string.IsNullOrEmpty(propertyFilter)
                    ? data.Select(p => new FilterValueDto
                    {
                        Value = p.PlannedImplementaionYear.ToString() == "" ? "" : p.PlannedImplementaionYear,//"20" + p.PaImplementaionYear.ToString().Substring(p.PaImplementaionYear.ToString().Length - 2),
                        Text = p.PaImplementaionYear.ToString() == "" ? ConstantValueFilter.blankTextValue : p.PaImplementaionYear.ToString()
                    }).Distinct().ToList()
                    : data.Select(p => new FilterValueDto
                    {
                        Value = p.PaImplementaionYear.ToString() == "" ? "" : p.PlannedImplementaionYear,//"20" + p.PaImplementaionYear.ToString().Substring(p.PaImplementaionYear.ToString().Length - 2),
                        Text = p.PaImplementaionYear.ToString() == "" ? ConstantValueFilter.blankTextValue : p.PaImplementaionYear.ToString()
                    }).Where(m => m.Text.ToLower().Contains(propertyFilter)).Distinct().ToList(),
                "verticalName" => string.IsNullOrEmpty(propertyFilter)
                                 ? data.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                                                     .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                                                     {
                                                         Text = t.Text,
                                                         Value = t.Value
                                                     }))?.Distinct()?.ToList()
                                  :
                                  data.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                                                     .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                                                     {
                                                         Text = t.Text,
                                                         Value = t.Value
                                                     })).Where(x => x.Text.Contains(propertyFilter))?.Distinct()?.ToList(),


                _ => new List<FilterValueDto>()
            };
            if (!isAdmin && (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                rtn = rtn.Where(x => buildFilterDto.VerticalName.Contains(x.Value.ToString())).ToList();
            }

            return rtn;


        }

    }
}