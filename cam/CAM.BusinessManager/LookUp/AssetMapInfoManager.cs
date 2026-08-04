using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.AssetMapInfo;
using CAM.DataTransferObjects.LookUp.MajorHardwareBuildAsIs;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.OMC;
using CAM.Entities.Models;
using CAM.Entities.Models.OMC;
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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.LookUp
{
    public class AssetMapInfoManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;
        public AssetMapInfoManager(IMapper mapper,GridCustomColumnManager columnManager,IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
        }

        public ExpressionStarter<Assetmapinfo> ApplyFilter(AssetMapInfoQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Assetmapinfo>(true);

            if (request.AssetMapInfoId != null && request.AssetMapInfoId.Any())
            {
                var predicateInner = PredicateBuilder.New<Assetmapinfo>();
                foreach (var item in request.AssetMapInfoId)
                    predicateInner.Or(x => x.Assetmapinfoid == item);
                predicateResult.And(predicateInner);
            }
            if (request.OmcAssetName != null && request.OmcAssetName.Any())
            {
                var predicateInner = PredicateBuilder.New<Assetmapinfo>();
                foreach (var item in request.OmcAssetName)
                    predicateInner.Or(x => x.Omcassetname == item);
                predicateResult.And(predicateInner);
            }
            if (request.TemsAssetName != null && request.TemsAssetName.Any())
            {
                var predicateInner = PredicateBuilder.New<Assetmapinfo>();
                foreach (var item in request.TemsAssetName)
                    predicateInner.Or(x => x.Temsassetname == item);
                predicateResult.And(predicateInner);
            }
            if (request.EnmAssetName != null && request.EnmAssetName.Any())
            {
                var predicateInner = PredicateBuilder.New<Assetmapinfo>();
                foreach (var item in request.EnmAssetName)
                    predicateInner.Or(x => x.Enmassetname == item);
                predicateResult.And(predicateInner);
            }
            if (request.Site != null && request.Site.Any())
            {
                var predicateInner = PredicateBuilder.New<Assetmapinfo>();
                foreach (var item in request.Site)
                    predicateInner.Or(x => x.Site == item);
                predicateResult.And(predicateInner);
            }
            if (request.DataSourceName != null && request.DataSourceName.Any())
            {
                var predicateInner = PredicateBuilder.New<Assetmapinfo>();
                foreach (var item in request.DataSourceName)
                    predicateInner.Or(x => x.Datasourcename == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Assetmapinfo>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Assetmapinfo>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public IQueryable<AssetMapInfo> GetAssetMapInfoEntities(ExpressionStarter<Assetmapinfo> predicateResult)
        {
            var query = _repositoryWrapper.AssetMapInfoRepository.FindByCondition(predicateResult)
               .Include(m => m.ModificationuserNavigation)
               .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => AssetMapInfoMapper.Get(p)).AsQueryable();
        }

        public Dictionary<string, Expression<Func<AssetMapInfo, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AssetMapInfo, object>>[]>
            {
                ["assetMapInfoId"] = new Expression<Func<AssetMapInfo, object>>[] { p => p.Assetmapinfoid },
            };
        }

        public async Task<QueryResultDto<AssetMapInfoDtoGrid>> FindWithCondition(AssetMapInfoQueryDto assetMapInfoQuerydto)
        {
            try
            {
                var predicateResult = ApplyFilter(assetMapInfoQuerydto);
                var rtn = new QueryResultDto<AssetMapInfoDtoGrid>(new GenerateRenderForGrid<AssetMapInfoDtoGrid>(_columnManager))
                {

                };
                var query = await Task.Run(() => GetAssetMapInfoEntities(predicateResult));
                rtn.TotalItems = query.Count();
                query = query.ApplyOrdering(assetMapInfoQuerydto, GetColumnsMap()).ApplyPaging(assetMapInfoQuerydto);
                var result = _mapper.Map<IEnumerable<AssetMapInfoDtoGrid>>(query);
                rtn.Items = result.ToArray();
                return rtn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, AssetMapInfoQueryDto request)
        {
            var predicateResult = ApplyFilter(request);

            var filteredQuery = await Task.Run(() => GetAssetMapInfoEntities(predicateResult));
        
            var result = propertyName switch
            {
                "assetMapInfoId" => filteredQuery
                    .Select(x => new FilterValueDto(x.Assetmapinfoid))
                    .Distinct()
                    .ToList(),
                "dataSourceName" => filteredQuery
                    .Select(x => new FilterValueDto (x.Datasourcename))
                    .Distinct()
                    .ToList(),
                "site" => filteredQuery
                    .Select(x => new FilterValueDto (x.Site))
                    .Distinct()
                    .ToList(),
                "enmAssetName" => filteredQuery
                    .Select(x => new FilterValueDto (x.Enmassetname))
                    .Distinct()
                    .ToList(),
                "temsAssetName" => filteredQuery
                .Select(x => new FilterValueDto(x.Temsassetname))
                .Distinct()
                .ToList(),
                "omcAssetName" => filteredQuery
                    .Select(x => new FilterValueDto(x.Omcassetname))
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


        public async Task<ResultDto> Add(AsseMapInfoCreateDto dto)
        {
            var existingEntities = await _repositoryWrapper
                .AssetMapInfoRepository
                .FindByCondition(
                x => x.Omcassetname.ToLower().Replace(" ", "") == dto.OmcAssetName.ToLower().Replace(" ", "") &&
                     x.Enmassetname.ToLower().Replace(" ", "") == dto.EnmAssetName.ToLower().Replace(" ", "") &&
                     x.Temsassetname.ToLower().Replace(" ", "") == dto.TemsAssetName.ToLower().Replace(" ", "") &&
                     x.Site.ToLower().Replace(" ", "") == dto.Site.ToLower().Replace(" ", "") &&
                     x.Datasourcename.ToLower().Replace(" ", "") == dto.DataSourceName.ToLower().Replace(" ", "") 
                     ).ToListAsync();

            if (existingEntities.Any())
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryAlreadyExists
                };
            }
            var newEntity = new Assetmapinfo
            {
                Omcassetname = dto.OmcAssetName,
                Enmassetname = dto.EnmAssetName,
                Site = dto.Site,
                Datasourcename =dto.DataSourceName,
                Temsassetname=dto.TemsAssetName
            };

            _repositoryWrapper.AssetMapInfoRepository.Create(newEntity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }
        public async Task<ResultDto> Update(AsseMapInfoCreateDto dto)
        {
            var existingEntities = await _repositoryWrapper
               .AssetMapInfoRepository
               .FindByCondition(
               x => x.Omcassetname.ToLower().Replace(" ", "") == dto.OmcAssetName.ToLower().Replace(" ", "") ||
                    x.Enmassetname.ToLower().Replace(" ", "") == dto.EnmAssetName.ToLower().Replace(" ", "") ||
                    x.Temsassetname.ToLower().Replace(" ", "") == dto.TemsAssetName.ToLower().Replace(" ", "") ||
                    x.Site.ToLower().Replace(" ", "") == dto.Site.ToLower().Replace(" ", "") ||
                    x.Datasourcename.ToLower().Replace(" ", "") == dto.DataSourceName.ToLower().Replace(" ", "")
                    ).ToListAsync();

            //if (existingEntities.Any(x => x.Assetmapinfoid != dto.AssetMapInfoId))
            //{
            //    return new ResultDto
            //    {
            //        Warning = false,
            //        Info = ResultMessages.EntryAlreadyExists,
            //        Data = dto.AssetMapInfoId
            //    };
            //}
            if( existingEntities.Any(x => x.Assetmapinfoid != dto.AssetMapInfoId && (x?.Omcassetname?.ToLower()?.Replace(" ", "") == dto?.OmcAssetName?.ToLower()?.Replace(" ", ""))) )
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = dto.OmcAssetName + " - " +ResultMessages.EntryAlreadyExists,
                    Data = dto.AssetMapInfoId
                };
            }
            //else if (existingEntities.Any(x => x.Assetmapinfoid != dto.AssetMapInfoId && (x?.Enmassetname?.ToLower().Replace(" ", "") == dto?.EnmAssetName?.ToLower()?.Replace(" ", ""))))
            //{
            //    return new ResultDto
            //    {
            //        Warning = true,
            //        Info = dto.EnmAssetName + " - " + ResultMessages.EntryAlreadyExists,
            //        Data = dto.AssetMapInfoId
            //    };
            //}
            else if (existingEntities.Any(x => x.Assetmapinfoid != dto.AssetMapInfoId && (x.Temsassetname?.ToLower()?.Replace(" ", "") == dto?.TemsAssetName?.ToLower()?.Replace(" ", ""))))
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = dto.TemsAssetName + " - " + ResultMessages.EntryAlreadyExists,
                    Data = dto.AssetMapInfoId
                };
            }
            //else if (existingEntities.Any(x => x.Assetmapinfoid != dto.AssetMapInfoId && (x?.Site?.ToLower()?.Replace(" ", "") == dto?.Site?.ToLower()?.Replace(" ", ""))))
            //{
            //    return new ResultDto
            //    {
            //        Warning = true,
            //        Info = dto.Site + " - " + ResultMessages.EntryAlreadyExists,
            //        Data = dto.AssetMapInfoId
            //    };
            //}
            //else if (existingEntities.Any(x => x.Assetmapinfoid != dto.AssetMapInfoId && (x?.Datasourcename?.ToLower()?.Replace(" ", "") == dto?.DataSourceName?.ToLower()?.Replace(" ", ""))))
            //{
            //    return new ResultDto
            //    {
            //        Warning = true,
            //        Info = dto.DataSourceName + " - " + ResultMessages.EntryAlreadyExists,
            //        Data = dto.AssetMapInfoId
            //    };
            //}

            var entityToUpdate = (existingEntities.Count > 0) ? existingEntities.FirstOrDefault(x => x.Assetmapinfoid == dto.AssetMapInfoId)
                                : _repositoryWrapper.AssetMapInfoRepository.FindByCondition(x => x.Assetmapinfoid == dto.AssetMapInfoId).FirstOrDefault();

            if (entityToUpdate == null)
            {
                return new ResultDto { Warning = true, Info = ResultMessages.EntryNotFound };
            }
            entityToUpdate.Omcassetname = dto.OmcAssetName;
            entityToUpdate.Enmassetname = dto.EnmAssetName;
            entityToUpdate.Site = dto.Site;
            entityToUpdate.Datasourcename = dto.DataSourceName;
            entityToUpdate.Temsassetname= dto.TemsAssetName;

            _repositoryWrapper.AssetMapInfoRepository.Update(entityToUpdate);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Warning = false, Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.AssetMapInfoRepository.FindByCondition(x => x.Assetmapinfoid == id).SingleAsync();
            _repositoryWrapper.AssetMapInfoRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Assetmapinfoid
            };
        }

        public AsseMapInfoCreateDto GetCreatePage()
        {
            var createDto = new AsseMapInfoCreateDto();
            return createDto;
        }
        public async Task<AsseMapInfoCreateDto> GetUpdatedPage(long id)
        {
            var updateDto = new AsseMapInfoCreateDto();

            var updateEntityExists = _repositoryWrapper.AssetMapInfoRepository.FindByCondition(x => x.Assetmapinfoid == id)
                                    .Include(x => x.ModificationuserNavigation)
                                     .FirstOrDefault();

            if (updateEntityExists != null)
            {
                updateDto.AssetMapInfoId = updateEntityExists.Assetmapinfoid;
                updateDto.OmcAssetName = updateEntityExists.Omcassetname;
                updateDto.EnmAssetName = updateEntityExists.Enmassetname;
                updateDto.TemsAssetName = updateEntityExists.Temsassetname;
                updateDto.Site = updateEntityExists.Site;
                updateDto.DataSourceName = updateEntityExists.Datasourcename;
                updateDto.LastModifiedBy = updateEntityExists.ModificationuserNavigation.Email;
                updateDto.LastModified = updateEntityExists.ModificationuserNavigation.Modificationdate;
            }
            return updateDto;
        }

    }
}
