using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.OMC.AssetAsIsSdiInfo;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.AssetMapInfo;
using CAM.DataTransferObjects.QueryDto.OMC;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Entities.Mappers.Cbom;
using CAM.Entities.Models.OMC;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.OMC
{
    public class AssetAsisSdiInfoManager:BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;
        public AssetAsisSdiInfoManager(IMapper mapper,GridCustomColumnManager columnManager,IEnumerable<IRepositoryWrapper> wrappers,IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper= repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
        }

        private static ExpressionStarter<Assetasissdiinfo> ApplyFilter(AssetAsIsSdiInfoQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Assetasissdiinfo>(true);
            var predicateInner = PredicateBuilder.New<Assetasissdiinfo>(true);

            if (buildFilterDto.Assetasissdiinfoid != null && buildFilterDto.Assetasissdiinfoid.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiinfo>();
                foreach (var item in buildFilterDto.Assetasissdiinfoid)
                    predicateInner.Or(x => x.Assetasissdiinfoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Datasourcename != null && buildFilterDto.Datasourcename.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiinfo>();
                foreach (var item in buildFilterDto.Datasourcename)
                    predicateInner.Or(x => x.Datasourcename == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Datasourcetype != null && buildFilterDto.Datasourcetype.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiinfo>();
                foreach (var item in buildFilterDto.Datasourcetype)
                    predicateInner.Or(x => x.Datasourcetype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Swversion != null && buildFilterDto.Swversion.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiinfo>();
                foreach (var item in buildFilterDto.Swversion)
                    predicateInner.Or(x => x.Swversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Firmwareversion != null && buildFilterDto.Firmwareversion.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiinfo>();
                foreach (var item in buildFilterDto.Firmwareversion)
                    predicateInner.Or(x => x.Firmwareversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Manufacturer != null && buildFilterDto.Manufacturer.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiinfo>();
                foreach (var item in buildFilterDto.Manufacturer)
                    predicateInner.Or(x => x.Manufacturer == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Model != null && buildFilterDto.Model.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiinfo>();
                foreach (var item in buildFilterDto.Model)
                    predicateInner.Or(x => x.Model == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Tsrmodel != null && buildFilterDto.Tsrmodel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiinfo>();
                foreach (var item in buildFilterDto.Tsrmodel)
                    predicateInner.Or(x => x.Tsrmodel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiinfo>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Assetasissdiinfo>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, AssetAsIsSdiInfoQueryDto request)
        {
            var predicateResult = ApplyFilter(request);

            var filteredQuery = await Task.Run(() => GetAssetAsIsSdiInfoEntities(predicateResult));

            var result = propertyName switch
            {

                "assetasissdiinfoid" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Assetasissdiinfoid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Assetasissdiinfoid))
                    .Distinct()
                    .ToList(),
                "datasourcename" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Datasourcename.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Datasourcename))
                    .Distinct()
                    .ToList(),
                "datasourcetype" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Datasourcetype.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Datasourcetype))
                    .Distinct()
                    .ToList(),
                "swversion" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Swversion.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Swversion))
                    .Distinct()
                    .ToList(),
                "firmwareversion" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Firmwareversion.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Firmwareversion))
                    .Distinct()
                    .ToList(),
                "manufacturer" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Manufacturer.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Manufacturer))
                    .Distinct()
                    .ToList(),
                "model" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Model.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Model))
                    .Distinct()
                    .ToList(),
                "tsrmodel" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Tsrmodel.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Tsrmodel))
                    .Distinct()
                    .ToList(),
                "lastModifiedBy" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ModificationUserEntity.Email.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.ModificationUserEntity.Email))
                    .Distinct()
                    .ToList(),

                _ => new List<FilterValueDto>()
            };
            return result;
        }
        public IQueryable<AssetAsIsSdiInfo> GetAssetAsIsSdiInfoEntities(ExpressionStarter<Assetasissdiinfo> predicateResult)
        {
            var query = _repositoryWrapper.AssetAsIsSdiInfoRepository.FindByCondition(predicateResult)
               .Include(m => m.ModificationuserNavigation)
               .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => AssetAsIsSdiInfoMapper.Get(p)).AsQueryable();
        }
        public Dictionary<string, Expression<Func<AssetAsIsSdiInfo, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AssetAsIsSdiInfo, object>>[]>
            {
                ["assetasissdiinfoid"] = new Expression<Func<AssetAsIsSdiInfo, object>>[] { p => p.Assetasissdiinfoid },
            };
        }

        public async Task<QueryResultDto<AssetAsIsSdiInfoDtoGrid>> FindWithCondition(AssetAsIsSdiInfoQueryDto assetAsisQuerydto)
        {
            try
            {
                var predicateResult = ApplyFilter(assetAsisQuerydto);
                var rtn = new QueryResultDto<AssetAsIsSdiInfoDtoGrid>(new GenerateRenderForGrid<AssetMapInfoDtoGrid>(_columnManager)) { };
                var query = await Task.Run(() => GetAssetAsIsSdiInfoEntities(predicateResult));
                rtn.TotalItems = query.Count();
                query = query.ApplyOrdering(assetAsisQuerydto, GetColumnsMap()).ApplyPaging(assetAsisQuerydto);
                var result = _mapper.Map<IEnumerable<AssetAsIsSdiInfoDtoGrid>>(query);
                rtn.Items = result.ToArray();
                return rtn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

    }
}
