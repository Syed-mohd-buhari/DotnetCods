using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.OMC.AssetAsisSdiSwitchInfo;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.OMC;
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
    public class AssetAsisSdiSwitchInfoManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;
        public AssetAsisSdiSwitchInfoManager(GridCustomColumnManager columnManager,IMapper mapper,IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
        }

        private static ExpressionStarter<Assetasissdiswitchinfo> ApplyFilter(AssetAsisSdiSwitchInfoQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Assetasissdiswitchinfo>(true);
            var predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>(true);

            if (buildFilterDto.Assetasissdiswitchinfoid != null && buildFilterDto.Assetasissdiswitchinfoid.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Assetasissdiswitchinfoid)
                    predicateInner.Or(x => x.Assetasissdiswitchinfoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Datasourcename != null && buildFilterDto.Datasourcename.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Datasourcename)
                    predicateInner.Or(x => x.Datasourcename == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchname != null && buildFilterDto.Switchname.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchname)
                    predicateInner.Or(x => x.Switchname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchid != null && buildFilterDto.Switchid.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchid)
                    predicateInner.Or(x => x.Switchid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchadminstate != null && buildFilterDto.Switchadminstate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchadminstate)
                    predicateInner.Or(x => x.Switchadminstate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchuniqueid != null && buildFilterDto.Switchuniqueid.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchuniqueid)
                    predicateInner.Or(x => x.Switchuniqueid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchrole != null && buildFilterDto.Switchrole.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchrole)
                    predicateInner.Or(x => x.Switchrole == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchrack != null && buildFilterDto.Switchrack.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchrack)
                    predicateInner.Or(x => x.Switchrack == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchlabel != null && buildFilterDto.Switchlabel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchlabel)
                    predicateInner.Or(x => x.Switchlabel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchserialnumber != null && buildFilterDto.Switchserialnumber.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchserialnumber)
                    predicateInner.Or(x => x.Switchserialnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchopsstate != null && buildFilterDto.Switchopsstate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchopsstate)
                    predicateInner.Or(x => x.Switchopsstate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchnetwork != null && buildFilterDto.Switchnetwork.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchnetwork)
                    predicateInner.Or(x => x.Switchnetwork == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchmanufacturer != null && buildFilterDto.Switchmanufacturer.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchmanufacturer)
                    predicateInner.Or(x => x.Switchmanufacturer == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchmodel != null && buildFilterDto.Switchmodel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchmodel)
                    predicateInner.Or(x => x.Switchmodel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchipaddress != null && buildFilterDto.Switchipaddress.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchipaddress)
                    predicateInner.Or(x => x.Switchipaddress == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Switchswversion != null && buildFilterDto.Switchswversion.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.Switchswversion)
                    predicateInner.Or(x => x.Switchswversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Assetasissdiswitchinfo>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, AssetAsisSdiSwitchInfoQueryDto request)
        {
            var predicateResult = ApplyFilter(request);

            var filteredQuery = await Task.Run(() => GetAssetAsisSdiSwitchInfoEntities(predicateResult));

            var result = propertyName switch
            {
                "assetasissdiswitchinfoid" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Assetasissdiswitchinfoid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Assetasissdiswitchinfoid))
                    .Distinct()
                    .ToList(),
                "datasourcename" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Datasourcename.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Datasourcename))
                    .Distinct()
                    .ToList(),
                "switchname" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchname.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Switchname))
                    .Distinct()
                    .ToList(),
                "switchid" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchid.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Switchid))
                    .Distinct()
                    .ToList(),
                "switchadminstate" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchadminstate.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Switchadminstate))
                    .Distinct()
                    .ToList(),
                "switchuniqueid" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchuniqueid.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Switchuniqueid))
                    .Distinct()
                    .ToList(),
                "switchrole" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchrole.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Switchrole))
                    .Distinct()
                    .ToList(),
                "switchrack" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchrack.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Switchrack))
                    .Distinct()
                    .ToList(),
                "switchlabel" => filteredQuery
             .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchlabel.Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.Switchlabel))
                .Distinct()
                .ToList(),
                "switchserialnumber" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchserialnumber.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Switchserialnumber))
                    .Distinct()
                    .ToList(),
                "switchopsstate" => filteredQuery
             .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchopsstate.Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.Switchopsstate))
                .Distinct()
                .ToList(),
                "switchnetwork" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchnetwork.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Switchnetwork))
                    .Distinct()
                    .ToList(),
                "switchmanufacturer" => filteredQuery
             .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchmanufacturer.Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.Switchmanufacturer))
                .Distinct()
                .ToList(),
                "switchmodel" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchmodel.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Switchmodel))
                    .Distinct()
                    .ToList(),
                "switchipaddress" => filteredQuery
             .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchipaddress.Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.Switchipaddress))
                .Distinct()
                .ToList(),
                "switchswversion" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Switchswversion.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Switchswversion))
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
        public IQueryable<AssetAsIsSdiSwitchInfo> GetAssetAsisSdiSwitchInfoEntities(ExpressionStarter<Assetasissdiswitchinfo> predicateResult)
        {
            var query = _repositoryWrapper.AssetAsIsSdiSwitchInfoRepository.FindByCondition(predicateResult)
               .Include(m => m.ModificationuserNavigation)
               .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => AssetAsIsSdiSwitchInfoMapper.Get(p)).AsQueryable();
        }
        public Dictionary<string, Expression<Func<AssetAsIsSdiSwitchInfo, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AssetAsIsSdiSwitchInfo, object>>[]>
            {
                ["assetasissdiswitchinfoid"] = new Expression<Func<AssetAsIsSdiSwitchInfo, object>>[] { p => p.Assetasissdiswitchinfoid },
            };
        }

        public async Task<QueryResultDto<AssetAsIsSdiSwitchInfoGridDto>> FindWithCondition(AssetAsisSdiSwitchInfoQueryDto assetAsisQuerydto)
        {
            try
            {
                var predicateResult = ApplyFilter(assetAsisQuerydto);
                var rtn = new QueryResultDto<AssetAsIsSdiSwitchInfoGridDto>(new GenerateRenderForGrid<AssetAsIsSdiSwitchInfoGridDto>(_columnManager)) { };
                var query = await Task.Run(() => GetAssetAsisSdiSwitchInfoEntities(predicateResult));
                rtn.TotalItems = query.Count();
                query = query.ApplyOrdering(assetAsisQuerydto, GetColumnsMap()).ApplyPaging(assetAsisQuerydto);
                var result = _mapper.Map<IEnumerable<AssetAsIsSdiSwitchInfoGridDto>>(query);
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
