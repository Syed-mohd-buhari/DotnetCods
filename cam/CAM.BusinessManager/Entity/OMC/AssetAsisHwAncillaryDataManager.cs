using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.OMC.AssetAsIsHwAncillaryData;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.OMC;
using CAM.Entities.Mappers.Cbom;
using CAM.Entities.Models.OMC;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.OMC
{
    public class AssetAsisHwAncillaryDataManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;
        public AssetAsisHwAncillaryDataManager(IMapper mapper,GridCustomColumnManager columnManager,IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
        }

        public IQueryable<AssetAsIsHwAncillaryData> GetAssetAsisHwAncillaryEntities(ExpressionStarter<Assetasishwancillarydata> predicateResult)
        {
            var query = _repositoryWrapper.AssetAsIsHwAncillaryDataRepository.FindByCondition(predicateResult)
                        .Include(m => m.ModificationuserNavigation);

            return query.AsEnumerable().Select(p => AssetAsIsHwAncillaryDataMapper.Get(p)).AsQueryable();

        }

        private static ExpressionStarter<Assetasishwancillarydata> ApplyFilter(AssetAsIsHwAncillaryDataQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Assetasishwancillarydata>(true);
            var predicateInner = PredicateBuilder.New<Assetasishwancillarydata>(true);

            if (buildFilterDto.Assetasishwancillarydataid != null && buildFilterDto.Assetasishwancillarydataid.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Assetasishwancillarydataid)
                    predicateInner.Or(x => x.Assetasishwancillarydataid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Networkelementasisid != null && buildFilterDto.Networkelementasisid.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Networkelementasisid)
                    predicateInner.Or(x => x.Networkelementasisid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Site != null && buildFilterDto.Site.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Site)
                    predicateInner.Or(x => x.Site == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Host != null && buildFilterDto.Host.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Host)
                    predicateInner.Or(x => x.Host == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Datasourcename != null && buildFilterDto.Datasourcename.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Datasourcename)
                    predicateInner.Or(x => x.Datasourcename == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Provider != null && buildFilterDto.Provider.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Provider)
                    predicateInner.Or(x => x.Provider == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Providertype != null && buildFilterDto.Providertype.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Providertype)
                    predicateInner.Or(x => x.Providertype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Consumer != null && buildFilterDto.Consumer.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Consumer)
                    predicateInner.Or(x => x.Consumer == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Consumertype != null && buildFilterDto.Consumertype.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Consumertype)
                    predicateInner.Or(x => x.Consumertype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Consumerrole != null && buildFilterDto.Consumerrole.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Consumerrole)
                    predicateInner.Or(x => x.Consumerrole == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Consumerrole != null && buildFilterDto.Consumerrole.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Consumerrole)
                    predicateInner.Or(x => x.Consumerrole == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Clustername != null && buildFilterDto.Clustername.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Clustername)
                    predicateInner.Or(x => x.Clustername == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Partnumber != null && buildFilterDto.Partnumber.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Partnumber)
                    predicateInner.Or(x => x.Partnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Systemtype != null && buildFilterDto.Systemtype.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Systemtype)
                    predicateInner.Or(x => x.Systemtype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Manufacturer != null && buildFilterDto.Manufacturer.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Manufacturer)
                    predicateInner.Or(x => x.Manufacturer == item);
                predicateResult.And(predicateInner);
            }
            if(buildFilterDto.Biosversion != null && buildFilterDto.Biosversion.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Biosversion)
                    predicateInner.Or(x => x.Biosversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Model != null && buildFilterDto.Model.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Model)
                    predicateInner.Or(x => x.Model == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Sku != null && buildFilterDto.Sku.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Sku)
                    predicateInner.Or(x => x.Sku == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Cpucapacity != null && buildFilterDto.Cpucapacity.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Cpucapacity)
                    predicateInner.Or(x => x.Cpucapacity == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Ephemeralstoragecapacity != null && buildFilterDto.Ephemeralstoragecapacity.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Ephemeralstoragecapacity)
                    predicateInner.Or(x => x.Ephemeralstoragecapacity == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Memorycapacity != null && buildFilterDto.Memorycapacity.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Memorycapacity)
                    predicateInner.Or(x => x.Memorycapacity == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Processorsummarymodel != null && buildFilterDto.Processorsummarymodel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Processorsummarymodel)
                    predicateInner.Or(x => x.Processorsummarymodel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Kubernetesnodetype != null && buildFilterDto.Kubernetesnodetype.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Kubernetesnodetype)
                    predicateInner.Or(x => x.Kubernetesnodetype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Managementip != null && buildFilterDto.Managementip.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Managementip)
                    predicateInner.Or(x => x.Managementip == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Kubernetesnodename != null && buildFilterDto.Kubernetesnodename.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Kubernetesnodename)
                    predicateInner.Or(x => x.Kubernetesnodename == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Kubeletversion != null && buildFilterDto.Kubeletversion.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Kubeletversion)
                    predicateInner.Or(x => x.Kubeletversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Kubernetesnodeos != null && buildFilterDto.Kubernetesnodeos.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Kubernetesnodeos)
                    predicateInner.Or(x => x.Kubernetesnodeos == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Kubernetesnoderesourcetype != null && buildFilterDto.Kubernetesnoderesourcetype.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Kubernetesnoderesourcetype)
                    predicateInner.Or(x => x.Kubernetesnoderesourcetype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Kubernetesnodestate != null && buildFilterDto.Kubernetesnodestate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.Kubernetesnodestate)
                    predicateInner.Or(x => x.Kubernetesnodestate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Kubernetesnodestatusupdatetime != null)
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                if (buildFilterDto.Kubernetesnodestatusupdatetime.StartDate != null)
                    predicateInner.And(x => x.Kubernetesnodestatusupdatetime.HasValue?x.Kubernetesnodestatusupdatetime.Value.Date>= buildFilterDto.Kubernetesnodestatusupdatetime.StartDate:true);
                if (buildFilterDto.Kubernetesnodestatusupdatetime.EndDate != null)
                    predicateInner.And(x => x.Kubernetesnodestatusupdatetime.HasValue ? x.Kubernetesnodestatusupdatetime.Value.Date <= buildFilterDto.Kubernetesnodestatusupdatetime.EndDate:false);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Assetasishwancillarydata>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        public Dictionary<string, Expression<Func<AssetAsIsHwAncillaryData, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AssetAsIsHwAncillaryData, object>>[]>
            {
                ["assetasishwancillarydataid"] = new Expression<Func<AssetAsIsHwAncillaryData, object>>[] { p => p.Assetasishwancillarydataid },
            };
        }

        public async Task<QueryResultDto<AssetAsIsHwAncillaryDataGridDto>> FindWithCondition(AssetAsIsHwAncillaryDataQueryDto hwAncillaryQueryDto)
        {
            try
            {
                var predicateResult = ApplyFilter(hwAncillaryQueryDto);
                var rtn = new QueryResultDto<AssetAsIsHwAncillaryDataGridDto>(new GenerateRenderForGrid<AssetAsIsHwAncillaryDataGridDto>(_columnManager)) { };
                var query = await Task.Run(() => GetAssetAsisHwAncillaryEntities(predicateResult));
                rtn.TotalItems = query.Count();
                query = query.ApplyOrdering(hwAncillaryQueryDto, GetColumnsMap()).ApplyPaging(hwAncillaryQueryDto);
                var result = _mapper.Map<IEnumerable<AssetAsIsHwAncillaryDataGridDto>>(query);
                rtn.Items = result.ToArray();
                return rtn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, AssetAsIsHwAncillaryDataQueryDto request)
        {
            var predicateResult = ApplyFilter(request);

            var filteredQuery = await Task.Run(() => GetAssetAsisHwAncillaryEntities(predicateResult));

            var result = propertyName switch
            {
                "assetasishwancillarydataid"=> filteredQuery.Where
                                              (x=>string.IsNullOrEmpty(propertyFilter) || x.Assetasishwancillarydataid.ToString().Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Assetasishwancillarydataid.ToString()))
                                              .Distinct()
                                              .ToList(),
                "networkelementasisid"      => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Networkelementasisid.ToString().Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Networkelementasisid.ToString()))
                                              .Distinct()
                                              .ToList(),
                "site"                      => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Site.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Site))
                                              .Distinct()
                                              .ToList(),
                "host"                      => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Host.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Host))
                                              .Distinct()
                                              .ToList(),
                "datasourcename"            => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Datasourcename.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Datasourcename))
                                              .Distinct()
                                              .ToList(),
                "provider"                  => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Provider.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Provider))
                                              .Distinct()
                                              .ToList(),
                "providertype"              => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Providertype.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Providertype))
                                              .Distinct()
                                              .ToList(),
                "consumer"                  => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Consumer.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Consumer))
                                              .Distinct()
                                              .ToList(),
                "consumertype"              => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Consumertype.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Consumertype))
                                              .Distinct()
                                              .ToList(),
                "consumerrole"              => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Consumerrole.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Consumerrole))
                                              .Distinct()
                                              .ToList(),
                "clustername"               => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Clustername.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Clustername))
                                              .Distinct()
                                              .ToList(),
                "partnumber"                => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Partnumber.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Partnumber))
                                              .Distinct()
                                              .ToList(),
                "systemtype"                => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Systemtype.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Systemtype))
                                              .Distinct()
                                              .ToList(),
                "manufacturer"              => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Manufacturer.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Manufacturer))
                                              .Distinct()
                                              .ToList(),
                "biosversion"               => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Biosversion.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Biosversion))
                                              .Distinct()
                                              .ToList()
                                              ,
                "model"                     => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Model.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Model))
                                              .Distinct()
                                              .ToList(),
                "sku"                       => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Sku.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Sku))
                                              .Distinct()
                                              .ToList(),
                "cpucapacity"               => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Cpucapacity.ToString().Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Cpucapacity.ToString()))
                                              .Distinct()
                                              .ToList(),
                "ephemeralstoragecapacity"  => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Ephemeralstoragecapacity.ToString().Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Ephemeralstoragecapacity.ToString()))
                                              .Distinct()
                                              .ToList(),
                "memorycapacity"            => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Memorycapacity.ToString().Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Memorycapacity.ToString()))
                                              .Distinct()
                                              .ToList(),
                "processorsummarymodel"     => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Processorsummarymodel.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Processorsummarymodel))
                                              .Distinct()
                                              .ToList(),
                "kubernetesnodetype"        => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Kubernetesnodetype.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Kubernetesnodetype))
                                              .Distinct()
                                              .ToList(),
                "managementip"              => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Managementip.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Managementip))
                                              .Distinct()
                                              .ToList(),
                "kubernetesnodename"        => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Kubernetesnodename.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Kubernetesnodename))
                                              .Distinct()
                                              .ToList(),
                "kubeletversion"            => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Kubeletversion.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Kubeletversion))
                                              .Distinct()
                                              .ToList(),
                "kubernetesnodeos"          => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Kubernetesnodeos.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Kubernetesnodeos))
                                              .Distinct()
                                              .ToList(),
                "kubernetesnoderesourcetype"=> filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Kubernetesnoderesourcetype.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Kubernetesnoderesourcetype))
                                              .Distinct()
                                              .ToList(),
                "kubernetesnodestate"       => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.Kubernetesnodestate.Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.Kubernetesnodestate))
                                              .Distinct()
                                              .ToList(),
                
                "lastModifiedBy"            => filteredQuery.Where
                                              (x => string.IsNullOrEmpty(propertyFilter) || x.ModificationUserEntity.Email.ToString().Contains(propertyFilter))
                                              .Select(x => new FilterValueDto(x.ModificationUserEntity.Email))
                                              .Distinct()
                                              .ToList(),

                _ => new List<FilterValueDto>()
            };
            return result;
        }

    }
}
