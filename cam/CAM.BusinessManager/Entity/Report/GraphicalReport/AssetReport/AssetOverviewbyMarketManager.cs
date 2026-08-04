using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.GraphicalReport.AssetReport
{
    public class AssetOverviewbyMarketManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _manager;
        private DropdownDataServiceManager _dropdownDataServiceManager;

        public AssetOverviewbyMarketManager(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper,
            GridCustomColumnManager manager, DropdownDataServiceManager dropdownDataServiceManager,
            IHttpContextAccessor contextAccessor
            ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
        }

        #region // Grid Functions
        private IQueryable<Networkelementsasplanned> GetQuery(ExpressionStarter<Networkelementsasplanned> predicateResult)
        {
            var assetQuery = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult)
              //.Include(x => x.Networkelementasplannedsubdomainspoc).ThenInclude(x => x.Subdomainspoc).ThenInclude(x => x.Aspnetuserroles).ThenInclude(x => x.Verticalresponsible)
              .Include(x => x.Networkelementasplannedsubdomainspoc).ThenInclude(x => x.Subdomainspoc).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical)
                 .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Environment)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetworksupportedsvr).ThenInclude(x => x.Service)

                .Include(x => x.Opco);

            return assetQuery;

        }
        public async Task<ResultDto> FindWithConditionQueryCheck(AssetOverviewbyMarketQueryDto buildFilterDto,List<int> userVertical=null,bool isAdmin=false)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var returnQueryResultDto = new QueryResultDto<AssetOverviewbyMarketGridDto>(new GenerateRenderForGrid<AssetOverviewbyMarketGridDto>(_manager))
            {
            };

            var iQueryableAssetRecords = await Task.Run(() => GetQuery(predicateResult).AsQueryable().ApplyOrdering(buildFilterDto, GetColumnsMap(), "Modificationdate").ToList());

            #region // Domain Filters

            if (buildFilterDto.SupportService != null && buildFilterDto.SupportService.Any())
            {
                iQueryableAssetRecords = iQueryableAssetRecords.Where(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Subnetworksupportedsvr.Any(y => buildFilterDto.SupportService.Contains(y.Serviceid))).ToList();
            }
            if (buildFilterDto.VerticalId != null && buildFilterDto.VerticalId.Any())
            {
                iQueryableAssetRecords = iQueryableAssetRecords.Where(x => x.Networkelementasplannedsubdomainspoc.Any(y => y.Subdomainspoc.AspnetuserverticalsUser
                .Any(s => /*s.Opcoid == x.Opcoid && */s.Deleted == false && s.Organisation!= null && s.Organisation.Vertical != null
                && buildFilterDto.VerticalId.Contains((short)s.Organisation.Vertical.Verticalresponsibleid)))).ToList();
            }
            if (buildFilterDto.HwBuild != null && buildFilterDto.HwBuild.Any())
            {
                iQueryableAssetRecords = iQueryableAssetRecords.Where(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => buildFilterDto.HwBuild.Contains((long)y.Majorhardware.Buildconstructionid))).ToList();
            }

            #endregion


            var processAssetRecord = iQueryableAssetRecords.ToList().SelectMany(x =>
            {
                var assingElementGraphDtoList =
                x.Networkelementasplannedsubdomainspoc.Where(m => m.Deleted == false)
                .SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Where(t => /*t.Opcoid == x.Opcoid && */t.Deleted == false &&
                t.Organisation.Vertical != null &&
                    (buildFilterDto.VerticalId == null || !buildFilterDto.VerticalId.Any() ||
                    (buildFilterDto.VerticalId?.Any(m => m == t.Organisation.Vertical.Verticalresponsibleid) ?? false))
                    )
                .DistinctBy(n => n.Organisation.Vertical.Verticalresponsibleid).Select(m => new AssetOverviewbyMarketGridDto()
                {

                    OemVendor = x?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,
                    OemVendorId = (short)x?.Orgeqpmanufacturerid,
                    NetworkElementsAsPlannedId = x.Networkelementasplannedid,
                    NetworkElementsPlannedName = Convert.ToString(x?.Elementname),
                    OpcoId = x.Opcoid,
                    OpCoDescrption = x?.Opco?.Opco,
                    VerticalId = (short)m.Organisation?.Vertical?.Verticalresponsibleid,
                    VerticalDescrption = m.Organisation?.Vertical?.Verticalresponsible,
                    ProductNameNeInstances = x?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Productname?.Description,
                    Environmentid = (long)x?.Environmentid,
                    Environment = x?.Environment.Environment,
                    SupportServiceId = (long)x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Subnetworksupportedsvr.Select(y => y.Serviceid).FirstOrDefault(),
                    SupportService = x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Subnetworksupportedsvr.Select(y => y.Service.Description).FirstOrDefault(),
                    DCFId = x.Designcomponent.Designcomponentfamilyid,
                    DCFDescription = x.Designcomponent.toDesignComponentFamily(),
                    HWBuildConsId = (long)x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware.Buildconstruction.Buildconstructionid).FirstOrDefault(),
                    HWBuildCons = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware.Buildconstruction.Buildconstruction).FirstOrDefault(),
                    SystemTypeId = x.Designcomponent.Systemtypeid,
                    SystemTypeDescription = x.Designcomponent.Systemtype.SystemTypeNameForDC(_repositoryWrapper)
                    //SystemTypeDescription = x.Designcomponent.Systemtype.sys.SystemTypeNameForDC(_repositoryWrapper).

                }));

                return assingElementGraphDtoList;
            });

            if(!isAdmin && userVertical!=null && userVertical.Count > 0)
            {
                processAssetRecord = processAssetRecord.Where(x => userVertical.Contains(x.VerticalId));
            }
            var groupByAssetRecords = processAssetRecord.Where(x => x != null).Select(x => x).ToList().GroupBy(x => new
            {
                x?.VerticalDescrption,
                x?.OemVendor,
                x?.ProductNameNeInstances,
                x?.OpCoDescrption,
                x.OpcoId
            }).
              Select(x => new AssetOverviewbyMarketGridDto()
              {
                  VerticalDescrption = x?.Key?.VerticalDescrption.ToString(),
                  OemVendor = x?.Key?.OemVendor,
                  ProductNameNeInstances = x?.Key?.ProductNameNeInstances,
                  OpcoId = x.Key.OpcoId,
                  OpCoDescrption = x?.Select(x => x?.OpCoDescrption).FirstOrDefault(),
                  NetworkElementCount = x.Where(x => x.OpcoId != 0).Count()
              });

            var item = groupByAssetRecords.ToList().Where(x => x.NetworkElementCount != 0).ToList();


            var dropdown = new Object();
            ///Chirsh Feedback Task - 1433 AssetOverViewByMarket - Return all Dropdown value When Filter item is Empty    
            if ((item.Count > 0) && ((buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Any()) || (buildFilterDto.OemVendorId != null && buildFilterDto.OemVendorId.Any()) || (buildFilterDto.VerticalId != null && buildFilterDto.VerticalId.Any()) ||
                (buildFilterDto.HwBuild != null && buildFilterDto.HwBuild.Any())
                || (buildFilterDto.SupportService != null && buildFilterDto.SupportService.Any()) || (buildFilterDto.DcfId != null && buildFilterDto.DcfId.Any()) ||
                (buildFilterDto.EnvironmentId != null && buildFilterDto.EnvironmentId.Any())))
            {
                dropdown = await GetAllResourcesFilterDropDown(processAssetRecord);
            }
            else
            {
                dropdown = await GetAllResourcesDropDown();

            }



            return new ResultDto
            {
                Data = new
                {
                    GridRender = returnQueryResultDto.GridRender,
                    TotalItems = item.Count(),
                    Item = item.ToList(),
                    allResource = dropdown,
                }
            };

        }
        private static ExpressionStarter<Networkelementsasplanned> ApplyFilter(AssetOverviewbyMarketQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasplanned>();

            var predicateInner = PredicateBuilder.New<Networkelementsasplanned>();

            predicateInner = predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc
            .Any(t => t.Subdomainspoc.AspnetuserverticalsUser
            .Any(m => /*m.Opcoid == x.Opcoid &&*/ m.Deleted == false && m.Organisation != null && m.Organisation.Vertical != null)));
            predicateResult = predicateResult.And(predicateInner);

            predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
            predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(m => m.Subdomainspoc != null));
            predicateResult.And(predicateInner);

            predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
            predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(x => x.Subdomainspoc.Isdesigncontact == true
            ));

            predicateResult.And(predicateInner);

            predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
            predicateInner.Or(x => x.Opco.Opco != null);
            predicateResult.And(predicateInner);

            if (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.OpcoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OemVendorId != null && buildFilterDto.OemVendorId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.OemVendorId)
                    predicateInner.Or(x => x.Orgeqpmanufacturerid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VerticalId != null && buildFilterDto.VerticalId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.VerticalId)
                    predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(y => y.Subdomainspoc.AspnetuserverticalsUser
                    .Any(d => /*d.Opcoid == x.Opcoid &&*/ d.Deleted == false && d.Organisation.Vertical.Verticalresponsibleid == item)));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HwBuild != null && buildFilterDto.HwBuild.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.HwBuild)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => y.Majorhardware.Buildconstructionid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SupportService != null && buildFilterDto.SupportService.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.SupportService)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Subnetworksupportedsvr.Any(y => y.Serviceid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DcfId != null && buildFilterDto.DcfId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.DcfId)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamilyid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EnvironmentId != null && buildFilterDto.EnvironmentId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.EnvironmentId)
                    predicateInner.Or(x => x.Environmentid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemTypetId != null && buildFilterDto.SystemTypetId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.SystemTypetId)
                    predicateInner.Or(x => x.Designcomponent.Systemtypeid == item);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        private Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]>
            {
                ["oemVendor"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer },
                ["oemVendorId"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Orgeqpmanufacturerid },
                ["productNameNeInstances"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description },
                ["networkElementsAsPlannedId"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Networkelementasplannedid },
                ["networkElementsPlannedName"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname },
                ["opcoId"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Opcoid },
                ["opCoDescrption"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Opco.Opco },
                ["networkElementCount"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Networkelementasplannedid },

            };
        }
        #endregion


        #region //Export Function

        public async Task<QueryResultDto<AssetOverviewbyMarketGridDto>> FindWithConditionForExport(AssetOverviewbyMarketQueryDto buildFilterDto)
        {

            var predicateResult = ApplyFilter(buildFilterDto);

            var returnQueryResultDto = new QueryResultDto<AssetOverviewbyMarketGridDto>(new GenerateRenderForGrid<AssetOverviewbyMarketGridDto>(_manager))
            {
            };

            var iQueryableAssetRecords = await Task.Run(() => GetQuery(predicateResult).AsQueryable().ApplyOrdering(buildFilterDto, GetColumnsMap(), "Modificationdate").ToList());

            #region // Domain Filters

            if (buildFilterDto.SupportService != null && buildFilterDto.SupportService.Any())
            {
                iQueryableAssetRecords = iQueryableAssetRecords.Where(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Subnetworksupportedsvr.Any(y => buildFilterDto.SupportService.Contains(y.Serviceid))).ToList();
            }
            if (buildFilterDto.VerticalId != null && buildFilterDto.VerticalId.Any())
            {
                iQueryableAssetRecords = iQueryableAssetRecords.Where(x => x.Networkelementasplannedsubdomainspoc.Any(y => y.Subdomainspoc.AspnetuserverticalsUser
                .Any(s => /*s.Opcoid == x.Opcoid && */s.Deleted == false && s.Organisation != null && s.Organisation.Vertical != null
                && buildFilterDto.VerticalId.Contains((short)s.Organisation.Vertical.Verticalresponsibleid)))).ToList();
            }
            if (buildFilterDto.HwBuild != null && buildFilterDto.HwBuild.Any())
            {
                iQueryableAssetRecords = iQueryableAssetRecords.Where(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => buildFilterDto.HwBuild.Contains((long)y.Majorhardware.Buildconstructionid))).ToList();
            }

            #endregion


            var processAssetRecord = iQueryableAssetRecords.ToList().SelectMany(x =>
            {
                var assingElementGraphDtoList =
                x.Networkelementasplannedsubdomainspoc.Where(m => m.Deleted == false)
                .SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Where(t => /*t.Opcoid == x.Opcoid && */t.Deleted == false &&
                t.Organisation != null && t.Organisation.Vertical != null &&
                    (buildFilterDto.VerticalId == null || !buildFilterDto.VerticalId.Any() ||
                    (buildFilterDto.VerticalId?.Any(m => m == t.Organisation.Vertical.Verticalresponsibleid) ?? false))
                    )
                .DistinctBy(n => n.Organisation.Vertical.Verticalresponsibleid).Select(m => new AssetOverviewbyMarketGridDto()
                {
                    OemVendor = x?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,
                    OemVendorId = (short)x?.Orgeqpmanufacturerid,
                    NetworkElementsAsPlannedId = x.Networkelementasplannedid,
                    NetworkElementsPlannedName = Convert.ToString(x?.Elementname),
                    OpcoId = x.Opcoid,
                    OpCoDescrption = x?.Opco?.Opco,
                    VerticalId = (short)m.Organisation.Vertical.Verticalresponsibleid,
                    VerticalDescrption = m.Organisation.Vertical.Verticalresponsible,
                    ProductNameNeInstances = x?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Productname?.Description,
                    Environmentid = (long)x?.Environmentid,
                    Environment = x?.Environment.Environment,
                    SupportServiceId = (long)x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Subnetworksupportedsvr.Select(y => y.Serviceid).FirstOrDefault(),
                    SupportService = x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Subnetworksupportedsvr.Select(y => y.Service.Description).FirstOrDefault(),
                    DCFId = x.Designcomponent.Designcomponentfamilyid,
                    DCFDescription = x.Designcomponent.toDesignComponentFamily(),
                    HWBuildConsId = (long)x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware.Buildconstruction.Buildconstructionid).FirstOrDefault(),
                    HWBuildCons = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware.Buildconstruction.Buildconstruction).FirstOrDefault(),
                    SystemTypeId = x.Designcomponent.Systemtypeid,
                    SystemTypeDescription = x.Designcomponent.Systemtype.SystemTypeNameForDC(_repositoryWrapper)

                }));
                return assingElementGraphDtoList;
            });

            var dropdown = new Object();

            if ((buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Any()) || (buildFilterDto.OemVendorId != null && buildFilterDto.OemVendorId.Any()) || (buildFilterDto.VerticalId != null && buildFilterDto.VerticalId.Any()) ||
                (buildFilterDto.HwBuild != null && buildFilterDto.HwBuild.Any())
                || (buildFilterDto.SupportService != null && buildFilterDto.SupportService.Any()) || (buildFilterDto.DcfId != null && buildFilterDto.DcfId.Any()) || (buildFilterDto.EnvironmentId != null && buildFilterDto.EnvironmentId.Any()))
            {
                dropdown = await GetAllResourcesFilterDropDown(processAssetRecord);
            }
            else
            {
                dropdown = await GetAllResourcesDropDown();

            }

            var groupByAssetRecords = processAssetRecord.Where(x => x != null).Select(x => x).ToList().GroupBy(x => new
            {
                x?.VerticalDescrption,
                x?.OemVendor,
                x?.ProductNameNeInstances,
                x?.OpCoDescrption,
                x.OpcoId
            }).
               Select(x => new AssetOverviewbyMarketGridDto()
               {
                   VerticalDescrption = x?.Key?.VerticalDescrption.ToString(),
                   OemVendor = x?.Key?.OemVendor,
                   ProductNameNeInstances = x?.Key?.ProductNameNeInstances,
                   OpcoId = x.Key.OpcoId,
                   OpCoDescrption = x?.Select(x => x?.OpCoDescrption).FirstOrDefault(),
                   NetworkElementCount = x.Where(x => x.OpcoId != 0).Count(),
                   DCFDescription = x?.Select(x => x?.DCFDescription).FirstOrDefault(),
                   Environment = x?.Select(x => x?.Environment).FirstOrDefault(),
                   HWBuildCons = x?.Select(x => x?.HWBuildCons).FirstOrDefault(),
                   SupportService = x?.Select(x => x?.SupportService).FirstOrDefault(),
                   SystemTypeDescription = x?.Select(x => x.SystemTypeDescription).FirstOrDefault(),
               });

            var item = groupByAssetRecords.ToList().Where(x => x.NetworkElementCount != 0).ToList();

            foreach (var col in ConstantValueFilter.assetOverViewExport)
            {
                returnQueryResultDto.GridRender.Render.Find(x => x.PropertyName.ToLower() == col.ToLower()).Show = true;
            }

            returnQueryResultDto.TotalItems = item.Count();
            returnQueryResultDto.Items = item.ToList();

            return returnQueryResultDto;

        }

        #endregion

        public async Task<ResultDto> GetAllResourcesDropDown()
        {
            try
            {
                var buildFilterDto = new DesignComponentFamilyQueryDto();
                var allOpco = await _dropdownDataServiceManager.GetOpcos(false, false, true);
                var allVertical = await _dropdownDataServiceManager.GetAllVerticalResponse(false, false, true,false,true,null);
                var allBuildCons = await _dropdownDataServiceManager.GetHWBuildConstruction(false, false, true);
                var allSupport = await _dropdownDataServiceManager.GetAllSupportedServices(false, false, true);
                var allVendor = await _dropdownDataServiceManager.GetVendorResource(false, false, true);
                var allDcf = await _dropdownDataServiceManager.GetAllDesignComponentFamilyName(buildFilterDto);
                var allEnvironment = await _dropdownDataServiceManager.GetEnvironmentNode(false, false, true);
                var allSystemType = _dropdownDataServiceManager.GetSystemType().Result;

                return new ResultDto
                {
                    Data = new
                    {
                        allOpcos = allOpco.Data,
                        allVertical = allVertical.Data,
                        allVendor = allVendor.Data,
                        allBuildCons = allBuildCons.Data,
                        allSupport = allSupport.Data,
                        allDcf = allDcf.Data,
                        allEnvironment = allEnvironment.Data,
                        allSystemType = allSystemType?.SystemTypeResource
                    }

                };

            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Data = ex.Message
                };
            }

        }

        public async Task<ResultDto> GetAllResourcesFilterDropDown(IEnumerable<AssetOverviewbyMarketGridDto> data)
        {
            try
            {

                var newVar = new DropdownKeyValueList();

                var allResource = await Task.Run(() => data?.ToList().Select(x => new AssetOverviewbyMarketDto
                {
                    Opco = (x.OpCoDescrption != null) ? new DropdownKeyValueList { Key = x.OpcoId, Value = x.OpCoDescrption } : newVar,
                    Vendor = (x.OemVendor != null) ? new DropdownKeyValueList { Key = x.OemVendorId, Value = x.OemVendor } : newVar,
                    Vertical = (x.VerticalDescrption != null) ? new DropdownKeyValueList { Key = x.VerticalId, Value = x.VerticalDescrption } : newVar,
                    HWBuildCons = (x.HWBuildCons != null) ? new DropdownKeyValueList { Key = (short)x.HWBuildConsId, Value = x.HWBuildCons } : newVar,
                    SupportService = (x.SupportService != null) ? new DropdownKeyValueList { Key = (short)x.SupportServiceId, Value = x.SupportService } : newVar,
                    DCF = (x.DCFDescription != null) ? new DropdownKeyValueList { Id = (short)x.DCFId, Description = x.DCFDescription } : newVar,
                    Environment = (x.Environment != null) ? new DropdownKeyValueList { Key = (short)x.Environmentid, Value = x.Environment } : newVar,
                    SystemType = (x.SystemTypeDescription != null) ? new DropdownKeyValueList { Key = (short)x.SystemTypeId, Value = x.SystemTypeDescription } : newVar,

                }).Distinct().ToList());


                return new ResultDto
                {
                    Data = new
                    {
                        allOpcos = allResource?.Select(x => x.Opco)?.DistinctBy(x => x.Key)?.ToList(),
                        allVertical = allResource?.Select(x => x.Vertical)?.DistinctBy(x => x.Key)?.ToList(),
                        allVendor = allResource.Select(x => x.Vendor)?.DistinctBy(x => x.Key)?.ToList(),
                        allBuildCons = allResource.Select(x => x.HWBuildCons)?.DistinctBy(x => x.Key)?.ToList(),
                        allSupport = allResource.Select(x => x.SupportService)?.DistinctBy(x => x.Key)?.ToList(),
                        allDcf = allResource.Select(x => x.DCF)?.DistinctBy(x => x.Id)?.ToList(),
                        allEnvironment = allResource.Select(x => x.Environment)?.DistinctBy(x => x.Key)?.ToList(),
                        allSystemType = allResource.Select(x => x.SystemType)?.DistinctBy(x => x.Key)?.ToList(),
                    }
                };

            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Data = ex.Message
                };
            }

        }
    }
}