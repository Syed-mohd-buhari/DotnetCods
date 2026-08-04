using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.Entita.NFVICompatibiltyReport;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using IdentityServer4.Extensions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class NFVICompatibilityAtGlanceManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _manager;
        private readonly CommonManager _commonManager;


        public NFVICompatibilityAtGlanceManager(IEnumerable<IRepositoryWrapper> wrappers,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager, GridCustomColumnManager manager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _commonManager = commonManager;
        }

        public async Task<QueryResultDto<NFVICompatibilityAtGlanceDtoGrid>> FindWithCondition(NFVICompatibilityAtGlanceQueryDto buildFilterDto, long vmVareId, bool isExport = false)
        {
            var rtn = new QueryResultDto<NFVICompatibilityAtGlanceDtoGrid>(new GenerateRenderForGrid<NFVICompatibilityAtGlanceDtoGrid>(_manager))
            {
                
            };
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = await Task.Run(() => GetQuery(predicateResult));

            var result = await GetNFVIRecords(query, vmVareId);
            rtn.TotalItems = result.Count();
            if (!isExport)
            {
                result = result.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
            }
            else
            {
                result = result.ToList();
            }
            
            rtn.Items = result.ToArray();


            return rtn;

        }

        #region //Grid View
        private ExpressionStarter<Lcmengineering> ApplyFilter(NFVICompatibilityAtGlanceQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);
            var predicateInner = PredicateBuilder.New<Lcmengineering>(true);


            if (buildFilterDto.Market?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.Market)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Application?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.Application)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productnameid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Domain?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.Domain)
                    predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser
                    .Any(m => m.Organisation.Vertical.Verticalresponsibleid == item && m.Deleted == false /*&& m.Opcoid.ToString().Contains(x.Opcoid.ToString())*/)));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignComponent?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.DesignComponent)
                    predicateInner.Or(x => x.Designcomponentid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CurrentVNF?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.CurrentVNF)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuildsid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MinimumVNF?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.MinimumVNF)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Nfvisoftwarecompatibility
                    .Any(r =>r.Nfvisoftwarecompatibilityid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedVNF?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.PlannedVNF)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any(d=>d.Designcomponent.Systemtype.Majorsoftwarebuildsid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedVNF?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.PlannedVNF)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any(d => d.Designcomponent.Systemtype.Majorsoftwarebuildsid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedUpgrade != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.PlannedUpgrade.StartDate != null)
                    predicateInner.And(x => x.PlannedactivitiesLcmengineering.Any(s => s.Plannedcompletion >= buildFilterDto.PlannedUpgrade.StartDate));
                if (buildFilterDto.PlannedUpgrade.EndDate != null)
                    predicateInner.And(x => x.PlannedactivitiesLcmengineering.Any(s=>s.Plannedcompletion <= buildFilterDto.PlannedUpgrade.EndDate));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DeleiveryStatus.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.DeleiveryStatus)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any(d => d.Plannedactivityresourceid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EduSpoc.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.EduSpoc)
                    predicateInner.Or(x => x.Lcmengineeringeduspoc.Any(e=>e.Eduspocid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubDomainSpoc.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.SubDomainSpoc)
                    predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(e => e.Subdomainspocid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OemId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.OemId)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VodafoneNameId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.VodafoneNameId)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Vodafonename == item);
                predicateResult.And(predicateInner);
            }
            #region //Status Filters
            if (buildFilterDto.ComplaintFilters?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.ComplaintFilters)
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DecommissioningFilters?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.DecommissioningFilters)
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SWUpgradePlanOkFilters?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.SWUpgradePlanOkFilters)
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SWUpgradePlanNotOkFilters?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.SWUpgradePlanNotOkFilters)
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SWUpgradeNoPlanFilters?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.SWUpgradeNoPlanFilters)
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NoMinVnfProvidedFilters?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.NoMinVnfProvidedFilters)
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NoMinVnfProvidedForPlannedDcFilters?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.NoMinVnfProvidedForPlannedDcFilters)
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedCompletionDataProvidedFilters?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.PlannedCompletionDataProvidedFilters)
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedCompletionDataNotProvidedFilters?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.PlannedCompletionDataNotProvidedFilters)
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                predicateResult.And(predicateInner);
            }
            #endregion
            return predicateResult;
        }

        public async Task<List<Lcmengineering>> GetQuery(ExpressionStarter<Lcmengineering> predicateResult)
        {
            var getLcmDeploymentStatusId = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x =>
            ConstantValueFilter.LcmDeploymentStatus.Contains(x.Description.ToLower().Trim().Replace(" ", ""))).Select(r => r.Id).ToList();

            var lcms = _repositoryWrapper.Lcmengineering.FindByCondition(predicateResult)
                .Where(p => getLcmDeploymentStatusId.Contains(p.Lcmdeploymentstatusid.Value) &&
                 p.Onsoftware == ConstantValueFilter.isTrue)
                .Include(p => p.Opco)
                .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds)
                .ThenInclude(p => p.Productname)
                .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(x=>x.VodafonenameNavigation)
                .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds)
                .ThenInclude(p => p.Orgeqpmanufacturer)
                .Include(p => p.PlannedactivitiesLcmengineering).ThenInclude(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds)
                .ThenInclude(p => p.Orgeqpmanufacturer)
                .Include(p => p.PlannedactivitiesLcmengineering).ThenInclude(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds)
                .ThenInclude(p => p.Productname)
                .Include(p => p.PlannedactivitiesLcmengineering).ThenInclude(p => p.Plannedactivityresource)
                .Include(p => p.Lcmengineeringsubdomainspoc)
                .Include(p => p.Lcmengineeringeduspoc)
                .Include(p => p.Lcmdeploymentstatus)
                .ToListAsync();
            return await lcms;
        }

        public async Task<IEnumerable<NFVICompatibilityAtGlanceDtoGrid>> GetNFVIRecords(List<Lcmengineering> lcmEntity, long vmVareId)
        {
            try
            {
                var nfviEntity = await _repositoryWrapper.NfviSoftwareCompatibilityRepository.FindByCondition(x => x.Plaftformid == vmVareId).ToListAsync();

                var result = (await Task.WhenAll(lcmEntity.Select( async lcm =>
                {
                    var grid = new NFVICompatibilityAtGlanceDtoGrid();
                    var checkLcmBasedNFVI =  nfviEntity.Where(x => x.Productid == lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Productnameid &&
                    x.Vendorid == lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid).FirstOrDefault();

                    if (checkLcmBasedNFVI != null)
                    {
                        if (lcm.Lcmdeploymentstatus.Description.ToLower().Trim() == ConstantValueFilter.inDecommission)
                        {
                            grid.Status = ConstantValueFilter.Decommissioning;
                            grid.MinimumVNF = Convert.ToDouble(checkLcmBasedNFVI.Minimumsupportedversion);
                            grid.DecommissioningFilters = lcm.Lcmengineeringid;;
                        }
                        else if (lcm.Lcmdeploymentstatus.Description.ToLower().Trim() == ConstantValueFilter.inService)
                        {
                            if (!(checkLcmBasedNFVI.Minimumsupportedversion.IsNullOrEmpty()))
                            {
                                if (CompareTheVersion(lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion, checkLcmBasedNFVI.Minimumsupportedversion))
                                {
                                    grid.Status = ConstantValueFilter.Complaint;
                                    grid.MinimumVNF = Convert.ToDouble(checkLcmBasedNFVI.Minimumsupportedversion);
                                    grid.ComplaintFilters = lcm.Lcmengineeringid;

                                }
                                else if (lcm.PlannedactivitiesLcmengineering != null && lcm.PlannedactivitiesLcmengineering.Count > 0)
                                {
                                    var PlannedProductNameId = lcm.PlannedactivitiesLcmengineering.Select(x => x.Designcomponent != null).First() ?
                                    lcm.PlannedactivitiesLcmengineering.Select(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productnameid).First() : 0;
                                    
                                    var PlannedOemId = lcm.PlannedactivitiesLcmengineering.Select(x => x.Designcomponent != null).First() ?
                                    lcm.PlannedactivitiesLcmengineering.Select(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid).First() : 0;

                                    var checkPlannedBasedNFVI = nfviEntity.Where(x =>
                                    x.Productid == PlannedProductNameId &&
                                    x.Vendorid == PlannedOemId).FirstOrDefault();

                                    if (checkPlannedBasedNFVI != null)
                                    {
                                        if (!(checkPlannedBasedNFVI.Minimumsupportedversion.IsNullOrEmpty()))
                                        {
                                            if (CompareTheVersion(lcm.PlannedactivitiesLcmengineering.Select(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion).First(), 
                                                checkPlannedBasedNFVI.Minimumsupportedversion))
                                            {
                                                grid.Status = ConstantValueFilter.SWUpgradePlanOk;
                                                grid.MinimumVNF = Convert.ToDouble(checkPlannedBasedNFVI.Minimumsupportedversion);
                                                grid.PlannedVNF = lcm.PlannedactivitiesLcmengineering.Select(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion).First();
                                                grid.PlannedUpgrade = lcm.PlannedactivitiesLcmengineering.Select(x => x.Plannedcompletion.Value).First();
                                                grid.DeleiveryStatus = lcm.PlannedactivitiesLcmengineering.Select(x => x.Plannedactivityresource.Plannedactivityresource).First();
                                                grid.SWUpgradePlanOkFilters = lcm.Lcmengineeringid;
                                                grid.PlannedCompletionDataProvidedFilters = lcm.Lcmengineeringid;
                                            }
                                            else
                                            {
                                                grid.Status = ConstantValueFilter.SWUpgradePlanNotOk;
                                                grid.MinimumVNF = Convert.ToDouble(checkPlannedBasedNFVI.Minimumsupportedversion);
                                                grid.SWUpgradePlanNotOkFilters = lcm.Lcmengineeringid;

                                            }
                                        }
                                        else
                                        {
                                            grid.Status = ConstantValueFilter.NoMinVnfProvidedForPlannedDc;
                                            grid.NoMinVnfProvidedForPlannedDcFilters = lcm.Lcmengineeringid;
                                        }

                                    }
                                }
                                else if (lcm.PlannedactivitiesLcmengineering == null || lcm.PlannedactivitiesLcmengineering.Count == 0)
                                {
                                    grid.Status = ConstantValueFilter.SWUpgradeNoPlan;
                                    grid.MinimumVNF = Convert.ToDouble(checkLcmBasedNFVI.Minimumsupportedversion);
                                    grid.SWUpgradeNoPlanFilters = lcm.Lcmengineeringid;
                                }

                            }
                            else
                            {
                                grid.Status = ConstantValueFilter.NoMinVnfProvided;
                                grid.NoMinVnfProvidedFilters = lcm.Lcmengineeringid;
                            }
                        }

                        grid.Market = lcm.Opco.Opco;
                        grid.OpcoId = lcm.Opcoid.Value;
                        grid.OemId = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid;
                        grid.OemName = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer;
                        grid.ProductNameId = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Productnameid.Value;
                        grid.Application = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description;
                        grid.Domain = _commonManager.GetVerticaleNameRes(lcm.Lcmengineeringsubdomainspoc.
                                      Select(x => x.Subdomainspocid).ToList(), lcm.Opcoid, false, true); ;
                        grid.DesignComponent = lcm.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper);
                        grid.CurrentVNF = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion;
                        grid.EduSpoc = _commonManager.GetEduAndSubDomainSpocUserEmail(lcm.Lcmengineeringeduspoc.Select(x => x.Eduspocid).ToList(), true, false);
                        grid.SubDomainSpoc = _commonManager.GetEduAndSubDomainSpocUserEmail(lcm.Lcmengineeringsubdomainspoc.Select(x => x.Subdomainspocid).ToList(), false, true);
                        grid.VodafoneNameId = lcm.Designcomponent.Systemtype.Vodafonename.Value;
                        grid.VodafoneName = lcm.Designcomponent.Systemtype.VodafonenameNavigation.Description;
                        grid.VerticalDropDown = _commonManager.GetVerticaleFilterDto(lcm.Lcmengineeringsubdomainspoc.Select(x => x.Subdomainspocid).ToList(), lcm.Opcoid, false, true)
                       .Distinct().ToList();
                        grid.OpcoDropDown = new FilterValueDto { Value = lcm.Opcoid.Value.ToString(), Text = lcm.Opco.Opco };
                        grid.OemDropDown = new FilterValueDto
                        {
                            Value = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid.ToString(),
                            Text = lcm.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer
                        };
                        grid.VfDropDown = new FilterValueDto
                        {
                            Value = lcm.Designcomponent.Systemtype.Vodafonename.ToString(),
                            Text = lcm.Designcomponent.Systemtype.VodafonenameNavigation.Description
                        };                     
                        grid.PlannedCompletionDropDown = lcm.PlannedactivitiesLcmengineering.Select(x => new FilterValueDto { Text = x.Plannedactivityid.ToString(), Value = x.Plannedcompletion.ToString() }).FirstOrDefault();
                        grid.PlannedCompletionDataNotProvidedFilters = lcm.Lcmengineeringid;
                        return grid;

                    }
                    return null;
                }))).Where(x=>x != null).ToList();
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<FilterValueDto>> GetFilteredValuesAsync(string propertyName, string propertyFilter, NFVICompatibilityAtGlanceQueryDto filterDto)
        {
            var filterCriteria = ApplyFilter(filterDto);

            var filteredQuery = await GetQuery(filterCriteria);
            
            var result = propertyName switch
            {
                "market" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Opcoid.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Opco.Opco, Value = p.Opcoid.ToString() })
                    .Distinct()
                    .ToList(),
                "application" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productnameid.ToString().Contains(propertyFilter))
                .Select(p => new FilterValueDto { Text = p.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description, 
                    Value = p.Designcomponent.Systemtype.Majorsoftwarebuilds.Productnameid.ToString() })
                .Distinct().ToList(),

                "domain" => _commonManager.GetVerticaleFilterDto(filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Lcmengineeringsubdomainspoc.Any(s=>s.Subdomainspocid.ToString().Contains(propertyFilter)))
                .SelectMany(x=>x.Lcmengineeringsubdomainspoc.Select(r=>r.Subdomainspocid)).ToList(), 0, false, true).Distinct().ToList(),

                "designComponent" =>  filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Designcomponentid.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Designcomponent.toDesignComponentName(), 
                        Value = p.Designcomponentid.ToString() })
                    .Distinct()
                    .ToList(),

                "currentVNF" =>  filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Designcomponent.Systemtype.Majorsoftwarebuildsid.ToString().Contains(propertyFilter))
                .Select(p => new FilterValueDto { Text = p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion, 
                    Value = p.Designcomponent.Systemtype.Majorsoftwarebuildsid.ToString() })
                .Distinct()
                .ToList(),

                "minimumVNF" =>  filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Nfvisoftwarecompatibility.Any(x=>x.Nfvisoftwarecompatibilityid.ToString().Contains(propertyFilter)))
                    .SelectMany(p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Nfvisoftwarecompatibility
                    .Select( s=> new FilterValueDto
                    {
                        Text = s.Minimumsupportedversion,
                        Value = s.Nfvisoftwarecompatibilityid.ToString()
                    }))
                    .Distinct()
                    .ToList(),

                "plannedVNF" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PlannedactivitiesLcmengineering.
               Any(x=>x.Designcomponent.Systemtype.Majorsoftwarebuildsid.ToString().Contains(propertyFilter)))
               .SelectMany(p => p.PlannedactivitiesLcmengineering
               .Select(s => new FilterValueDto
               {
                   Text = s.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion,
                   Value = s.Designcomponent.Systemtype.Majorsoftwarebuildsid.ToString()
               }))
               .Distinct()
               .ToList(),

                "plannedUpgrade" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PlannedactivitiesLcmengineering.
                Any(r => r.Plannedactivityid.ToString().Contains(propertyFilter)))
                .SelectMany(p => p.PlannedactivitiesLcmengineering
                .Select(s => new FilterValueDto
                {
                    Text = s.Plannedcompletion.ToString(),
                    Value = s.Plannedactivityid.ToString()
                }))
                .Distinct()
                .ToList(),

                "deleiveryStatus" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PlannedactivitiesLcmengineering.
               Any(r => r.Plannedactivityid.ToString().Contains(propertyFilter)))
               .SelectMany(p => p.PlannedactivitiesLcmengineering
               .Select(s => new FilterValueDto
               {
                   Text = s.Plannedactivityresource.Plannedactivityresource.ToString(),
                   Value = s.Plannedactivityid.ToString()
               }))
               .Distinct()
               .ToList(),
                "status" => new List<FilterValueDto>
                {
                    new FilterValueDto (ConstantValueFilter.SWUpgradeNoPlan),
                    new FilterValueDto (ConstantValueFilter.Complaint),
                    new FilterValueDto (ConstantValueFilter.SWUpgradePlanOk),
                    new FilterValueDto (ConstantValueFilter.NoMinVnfProvided),
                    new FilterValueDto (ConstantValueFilter.Decommissioning),
                    new FilterValueDto (ConstantValueFilter.SWUpgradePlanNotOk),
                    new FilterValueDto (ConstantValueFilter.NoMinVnfProvidedForPlannedDc),
                },
               
                "eduSpoc" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Lcmengineeringeduspoc.
               Any(r => r.Eduspocid.ToString().Contains(propertyFilter)))
               .SelectMany(p => p.Lcmengineeringeduspoc
               .Select(s => new FilterValueDto
               {
                   Text = s.Eduspoc.Email,
                   Value = s.Eduspocid.ToString()
               }))
               .Distinct()
               .ToList(),
                "subDomainSpoc" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Lcmengineeringsubdomainspoc.
                Any(r => r.Subdomainspocid.ToString().Contains(propertyFilter)))
                .SelectMany(p => p.Lcmengineeringsubdomainspoc
                .Select(s => new FilterValueDto
                {
                    Text = s.Subdomainspoc.Email,
                    Value = s.Subdomainspocid.ToString()
                }))
                .Distinct()
                .ToList(),

                _ => new List<FilterValueDto>(),
            };

            return result;
        }

        public bool CompareTheVersion(string currentVersion, string nfviMinVersion)
        {
            if (Double.TryParse(currentVersion, out double result))
            {
                var splitCurrentVersion = currentVersion.Split('.');
                var splitNfviVersion = nfviMinVersion.Split('.');

                if (Convert.ToDouble(splitCurrentVersion[0]) >= Convert.ToDouble(splitNfviVersion[0]))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
            
        }

        #endregion

        public async Task<ResultDto> FindWithConditionForGraphicalReport(NFVICompatibilityAtGlanceQueryDto buildFilterDto, long vmVareId, bool isOpcoWise = false, bool isProductWise = false, bool isServiceWise = false, bool isOverAllOpcoWise = false, bool isPageload = false)
        {

            var predicateResult = ApplyFilter(buildFilterDto);

            var query = await Task.Run(() => GetQuery(predicateResult));

            var result = await GetNFVIRecords(query, vmVareId);



            var overAllNFVIStatus = await GetOverAllNFVIStatus(result);

            var opcoBasedNFVIStatus = await GetOpcoBasedNFVIStatus(result);

            var vendorBasedNFVIStatus = await GetVendorBasedNFVIStatus(result);

            var VerticalBasedNFVIStatus = await GetVerticalBasedNFVIStatus(result) ;

            var vfBasedNFVIStatus = await GetVFBasedNFVIStatus(result);

            var plannedCompletionBasedNFVIStatus = await GetPlannedCompletionBasedNFVIStatus(result);

            //var opCoFilterDropDown = result.Select(x => x.OpcoDropDown).Distinct().ToList();
            //var oemFilterDropDown = result.Select(x => x.OemDropDown).Distinct().ToList();
            //var verticalFilterDropDown = result.SelectMany(x => x.VerticalDropDown).Distinct().ToList();
            //var vfFilterDropDown = result.Select(x => x.VfDropDown).Distinct().ToList();
            //var plannedCompletionFilterDropDown = result.Select(x => x.PlannedCompletionDropDown).Distinct().ToList();



            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = new
                {
                    overAllNFVIStatus,
                    opcoBasedNFVIStatus,
                    vendorBasedNFVIStatus,
                    VerticalBasedNFVIStatus,
                    vfBasedNFVIStatus,
                    plannedCompletionBasedNFVIStatus,
                    //opCoFilterDropDown,
                    //oemFilterDropDown,
                    //verticalFilterDropDown,
                    //vfFilterDropDown,
                    //plannedCompletionFilterDropDown
                }
            };

        }

        public async Task<NFVICompatibilityStatusDTO> GetOverAllNFVIStatus(IEnumerable<NFVICompatibilityAtGlanceDtoGrid> nfviEntity)
        {
            var result = new NFVICompatibilityStatusDTO();
            try
            {
                var getEntityStatus = await Task.Run(() => nfviEntity.Select(x => x.Status).ToList());

                result.Complaint = getEntityStatus.Where(x=> x == ConstantValueFilter.Complaint).Count();
                result.SWUpgradePlanOk = getEntityStatus.Where(x => x == ConstantValueFilter.SWUpgradePlanOk).Count();
                result.SWUpgradeNoPlan = getEntityStatus.Where(x => x == ConstantValueFilter.SWUpgradeNoPlan).Count();
                result.NoMinVnfProvided = getEntityStatus.Where(x => x == ConstantValueFilter.NoMinVnfProvided).Count();
                result.Decommissioning = getEntityStatus.Where(x => x == ConstantValueFilter.Decommissioning).Count();
                result.SWUpgradePlanNotOk = getEntityStatus.Where(x => x == ConstantValueFilter.SWUpgradePlanNotOk).Count();
                result.NoMinVnfProvidedForPlannedDc = getEntityStatus.Where(x => x == ConstantValueFilter.NoMinVnfProvidedForPlannedDc).Count();
                result.ComplaintFilters = await Task.Run(() => nfviEntity.Select(x => x.ComplaintFilters).Where(f=>f != 0).Distinct().ToList());
                result.DecommissioningFilters = await Task.Run(() => nfviEntity.Select(x => x.DecommissioningFilters).Distinct().ToList());
                result.SWUpgradePlanOkFilters = await Task.Run(() => nfviEntity.Select(x => x.SWUpgradePlanOkFilters).Distinct().ToList());
                result.SWUpgradeNoPlanFilters = await Task.Run(() => nfviEntity.Select(x => x.SWUpgradeNoPlanFilters).Distinct().ToList());
                result.NoMinVnfProvidedFilters = await Task.Run(() => nfviEntity.Select(x => x.NoMinVnfProvidedFilters).Distinct().ToList());
                result.SWUpgradePlanNotOkFilters = await Task.Run(() => nfviEntity.Select(x => x.SWUpgradePlanNotOkFilters).Distinct().ToList());
                result.NoMinVnfProvidedForPlannedDcFilters = await Task.Run(() => nfviEntity.Select(x => x.NoMinVnfProvidedForPlannedDcFilters).Distinct().ToList());

                return result;

            }
            catch
            {
                throw;
            }
        }

        public static async Task<List<NFVICompatibilityStatusDTO>> GetOpcoBasedNFVIStatus(IEnumerable<NFVICompatibilityAtGlanceDtoGrid> nfviEntity)
        {
            var result = new List<NFVICompatibilityStatusDTO>();
            try
            {
                var getEntityStatus = await Task.Run(() => nfviEntity.AsParallel().GroupBy(g=>new { g.Market}).Select(r =>
                {
                    var grid = new NFVICompatibilityStatusDTO();
                    grid.Opco = r.Key.Market;
                    grid.OpCoId = r.FirstOrDefault().OpcoId;
                    grid.OverAllStatusCount = r.Count();
                    grid.Complaint = r.Where(f=>f.Status == ConstantValueFilter.Complaint).Count();
                    grid.SWUpgradePlanOk = r.Where(f => f.Status == ConstantValueFilter.SWUpgradePlanOk).Count();
                    grid.SWUpgradeNoPlan = r.Where(f => f.Status == ConstantValueFilter.SWUpgradeNoPlan).Count();
                    grid.NoMinVnfProvided = r.Where(f => f.Status == ConstantValueFilter.NoMinVnfProvided).Count();
                    grid.Decommissioning = r.Where(f => f.Status == ConstantValueFilter.Decommissioning).Count();
                    grid.SWUpgradePlanNotOk = r.Where(f => f.Status == ConstantValueFilter.SWUpgradePlanNotOk).Count();
                    grid.NoMinVnfProvidedForPlannedDc = r.Where(f => f.Status == ConstantValueFilter.NoMinVnfProvidedForPlannedDc).Count();

                    return grid;
                }).ToList());

                return getEntityStatus;

            }
            catch
            {
                throw;
            }
        }
        public static async Task<List<NFVICompatibilityStatusDTO>> GetVendorBasedNFVIStatus(IEnumerable<NFVICompatibilityAtGlanceDtoGrid> nfviEntity)
        {
            var result = new List<NFVICompatibilityStatusDTO>();
            try
            {
                var getEntityStatus = await Task.Run(() => nfviEntity.AsParallel().GroupBy(g => new { g.OemName}).Select(r =>
                {
                    var grid = new NFVICompatibilityStatusDTO();
                    grid.VendorName = r.Key.OemName;
                    grid.VendorId = r.FirstOrDefault().OemId;
                    grid.OverAllStatusCount = r.Count();
                    grid.Complaint = r.Where(f => f.Status == ConstantValueFilter.Complaint).Count();
                    grid.SWUpgradePlanOk = r.Where(f => f.Status == ConstantValueFilter.SWUpgradePlanOk).Count();
                    grid.SWUpgradeNoPlan = r.Where(f => f.Status == ConstantValueFilter.SWUpgradeNoPlan).Count();
                    grid.NoMinVnfProvided = r.Where(f => f.Status == ConstantValueFilter.NoMinVnfProvided).Count();
                    grid.Decommissioning = r.Where(f => f.Status == ConstantValueFilter.Decommissioning).Count();
                    grid.SWUpgradePlanNotOk = r.Where(f => f.Status == ConstantValueFilter.SWUpgradePlanNotOk).Count();
                    grid.NoMinVnfProvidedForPlannedDc = r.Where(f => f.Status == ConstantValueFilter.NoMinVnfProvidedForPlannedDc).Count();

                    return grid;
                }).ToList());

                return getEntityStatus;

            }
            catch
            {
                throw;
            }
        }
        public static async Task<List<NFVICompatibilityStatusDTO>> GetVerticalBasedNFVIStatus(IEnumerable<NFVICompatibilityAtGlanceDtoGrid> nfviEntity)
        {
            var result = new List<NFVICompatibilityStatusDTO>();
            try
            {
                var getEntityStatus = await Task.Run(() => nfviEntity.AsParallel().Where(x=>x.Domain != "").GroupBy(g => new { g.Domain}).Select(r =>
                {
                    var grid = new NFVICompatibilityStatusDTO();
                    grid.VerticalName = r.Key.Domain;
                    grid.VerticalIdDto = r.SelectMany(x=>x.VerticalDropDown).ToList();
                    grid.OverAllStatusCount = r.Count();
                    grid.Complaint = r.Where(f => f.Status == ConstantValueFilter.Complaint).Count();
                    grid.SWUpgradePlanOk = r.Where(f => f.Status == ConstantValueFilter.SWUpgradePlanOk).Count();
                    grid.SWUpgradeNoPlan = r.Where(f => f.Status == ConstantValueFilter.SWUpgradeNoPlan).Count();
                    grid.NoMinVnfProvided = r.Where(f => f.Status == ConstantValueFilter.NoMinVnfProvided).Count();
                    grid.Decommissioning = r.Where(f => f.Status == ConstantValueFilter.Decommissioning).Count();
                    grid.SWUpgradePlanNotOk = r.Where(f => f.Status == ConstantValueFilter.SWUpgradePlanNotOk).Count();
                    grid.NoMinVnfProvidedForPlannedDc = r.Where(f => f.Status == ConstantValueFilter.NoMinVnfProvidedForPlannedDc).Count();

                    return grid;
                }).ToList());

                return getEntityStatus;

            }
            catch
            {
                throw;
            }
        }
        public static async Task<List<NFVICompatibilityStatusDTO>> GetVFBasedNFVIStatus(IEnumerable<NFVICompatibilityAtGlanceDtoGrid> nfviEntity)
        {
            var result = new List<NFVICompatibilityStatusDTO>();
            try
            {
                var getEntityStatus = await Task.Run(() => nfviEntity.AsParallel().GroupBy(g => new { g.VodafoneName }).Select(r =>
                {
                    var grid = new NFVICompatibilityStatusDTO();
                    grid.VodafoneName = r.Key.VodafoneName;
                    grid.VodafoneNameId = r.FirstOrDefault().VodafoneNameId;
                    grid.OverAllStatusCount = r.Count();
                    grid.Complaint = r.Where(f => f.Status == ConstantValueFilter.Complaint).Count();
                    grid.SWUpgradePlanOk = r.Where(f => f.Status == ConstantValueFilter.SWUpgradePlanOk).Count();
                    grid.SWUpgradeNoPlan = r.Where(f => f.Status == ConstantValueFilter.SWUpgradeNoPlan).Count();
                    grid.NoMinVnfProvided = r.Where(f => f.Status == ConstantValueFilter.NoMinVnfProvided).Count();
                    grid.Decommissioning = r.Where(f => f.Status == ConstantValueFilter.Decommissioning).Count();
                    grid.SWUpgradePlanNotOk = r.Where(f => f.Status == ConstantValueFilter.SWUpgradePlanNotOk).Count();
                    grid.NoMinVnfProvidedForPlannedDc = r.Where(f => f.Status == ConstantValueFilter.NoMinVnfProvidedForPlannedDc).Count();

                    return grid;
                }).ToList());

                return getEntityStatus;

            }
            catch
            {
                throw;
            }
        }
        public static async Task<NFVICompatibilityStatusDTO> GetPlannedCompletionBasedNFVIStatus(IEnumerable<NFVICompatibilityAtGlanceDtoGrid> nfviEntity)
        {
            var result = new NFVICompatibilityStatusDTO();
            try
            {
                var getPlannedCompletionRecords = await Task.Run(() => nfviEntity.Select(x => x.PlannedUpgrade).ToList());

                result.PlannedCompletionDataProvided = getPlannedCompletionRecords.Where(x => x.ToString() != "").Count();
                result.PlannedCompletionDataNotProvided = getPlannedCompletionRecords.Where(x => x.ToString() == "").Count();

                result.PlannedCompletionDataNotProvidedFilters = await Task.Run(() => nfviEntity.Where(f=>f.PlannedUpgrade.ToString() == "")
                .Select(x => x.PlannedCompletionDataNotProvidedFilters).Distinct().ToList());
                result.PlannedCompletionDataProvidedFilters = await Task.Run(() => nfviEntity.Where(f => f.PlannedUpgrade.ToString()! == "")
                .Select(x => x.PlannedCompletionDataProvidedFilters).Distinct().ToList());
                return result;

            }
            catch
            {
                throw;
            }
        }



    }
}