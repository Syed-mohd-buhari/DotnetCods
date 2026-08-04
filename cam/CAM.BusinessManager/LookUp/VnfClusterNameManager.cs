using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.VNFCluster;
using CAM.DataTransferObjects.LookUp.VNFName;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Entities.Mappers.Vbom;
using CAM.Entities.Models.VBom;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Drawing.Charts;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.LookUp
{
    public class VnfClusterNameManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;


        public VnfClusterNameManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
        #region UI Member function
        public ExpressionStarter<Clustername> ApplyFilterForOracleModel(VnfClusterNameQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Clustername>();

            if (request.Clusternameid != null && request.Clusternameid.Any())
            {
                var predicateInner = PredicateBuilder.New<Clustername>();
                foreach (var item in request.Clusternameid)
                    predicateInner.Or(x => x.Clusternameid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Clusterdescription != null && request.Clusterdescription.Any())
            {
                var predicateInner = PredicateBuilder.New<Clustername>();
                foreach (var item in request.Clusterdescription)
                    predicateInner.Or(x => x.Clusterdescription == item);
                predicateResult.And(predicateInner);
            }
            if (request.Clustertype != null && request.Clustertype.Any())
            {
                var predicateInner = PredicateBuilder.New<Clustername>();
                foreach (var item in request.Clustertype)
                    predicateInner.Or(x => x.Clustertype == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Clustername>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Clustername>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(VnfClusterNameQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<VnfCluserNameDtoGrid>(new GenerateRenderForGrid<VnfCluserNameDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Clustername>(true);
            var fullQuery = await Task.Run(() => PrepareQuery(predicateResult));
            var allItems = MappingDto(fullQuery.ToList());

            return new ResultDto
            {
                Data = new
                {
                    rtn.GridRender,
                    rtn.Items,
                    rtn.TotalItems,
                    allItems
                }
            };
        }

        public List<VnfCluserNameDtoGrid> MappingDto(List<ClusterName> query)
        {
            var result = new List<VnfCluserNameDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new VnfCluserNameDtoGrid();

                    grid.ClusterNameId = x.ClusterNameId;
                    grid.ClusterDescription = x.ClusterDescription;
                    grid.ClusterType = x.ClusterType;
                    grid.LastModified = x.ModificationDate;
                    grid.LastModifiedBy = x.ModificationUserEntity.Email;
                    return grid;
                }).ToList();
            }
            catch
            {
                return result;
            }

        }


        public Dictionary<string, Expression<Func<ClusterName, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ClusterName, object>>[]>
            {
                ["clusterNameId"] = new Expression<Func<ClusterName, object>>[] { p => p.ClusterNameId },
                ["clusterDescription"] = new Expression<Func<ClusterName, object>>[] { p => p.ClusterDescription },
                ["clusterType"] = new Expression<Func<ClusterName, object>>[] { p => p.ClusterType },
                ["modificationUser"] = new Expression<Func<ClusterName, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, VnfClusterNameQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "clusterNameId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ClusterNameId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.ClusterNameId))
                    .Distinct()
                    .ToList(),

                "clusterDescription" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ClusterDescription.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.ClusterDescription))
                    .Distinct()
                    .ToList(),

                "clusterType" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ClusterType.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.ClusterType))
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

        public IQueryable<ClusterName> PrepareQuery(ExpressionStarter<Clustername> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.ClusterNameRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.ClusterNameRepository.FindAll()
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => ClusterNameMapper.GetClusterName(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(VnfClusterNameCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.ClusterNameRepository.FindByCondition(
               x => x.Clusterdescription.ToLower().Trim().Replace(" ", "") == dto.ClusterDescription.ToLower().Trim().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Clusternameid
                };
            }

            Clustername entity = new Clustername()
            {
                Clusternameid = dto.ClusterNameId,
                Clustertype = dto.ClusterType,
                Clusterdescription = dto.ClusterDescription,
            };

            _repositoryWrapper.ClusterNameRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public VnfClusterNameCreateDto GetCreatePage()
        {

            var dto = new VnfClusterNameCreateDto();

            return dto;

        }

        public async Task<VnfCluserNameDtoGrid> GetUpdatedPage(long id)
        {

            var vnfClusterNameModel = await _repositoryWrapper.ClusterNameRepository.FindByCondition(x => x.Clusternameid == id).FirstOrDefaultAsync();
            var dto = new VnfCluserNameDtoGrid();
            if (vnfClusterNameModel != null)
            {
                var vnfClusterNameEntity = ClusterNameMapper.GetClusterName(vnfClusterNameModel);
                if (vnfClusterNameEntity != null)
                {
                    dto.ClusterNameId = vnfClusterNameEntity.ClusterNameId;
                    dto.ClusterDescription = vnfClusterNameEntity.ClusterDescription;
                    dto.ClusterType = vnfClusterNameEntity.ClusterType;
                    return dto;
                }
            }

            return dto;

        }

        public async Task<ResultDto> Update(VnfCluserNameUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.ClusterNameRepository.FindByCondition(
                   x => x.Clusternameid != dto.ClusterNameId
                   && x.Clusterdescription.ToLower().Trim().Replace(" ", "") == dto.ClusterDescription.ToLower().Trim().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Clusternameid
                };
            }
            Clustername model = new Clustername()
            {
                Clusterdescription = dto.ClusterDescription,
                Clustertype = dto.ClusterType,
                Clusternameid = dto.ClusterNameId,
                Deleted = false
            };
            _repositoryWrapper.ClusterNameRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.ClusterNameRepository.FindByCondition(x => x.Clusternameid == id).SingleAsync();
            var relatedVnfClusterInfo = await _repositoryWrapper.VnfClusterInfoRepository.FindByCondition(x => x.Clusternameid == id).ToListAsync();
            if (relatedVnfClusterInfo != null && relatedVnfClusterInfo.Count > 0)
            {
                if (relatedVnfClusterInfo.Any(x => x.Clusternameid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"{entity.Clusternameid}, This Id has Linked to VBOM - VNF Cluster Info ",
                        Data = entity.Clusternameid
                    };
                }
            }
            _repositoryWrapper.ClusterNameRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Clusternameid
            };

        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.ClusterNameRepository
            .FindByCondition(x => x.Clusternameid == id)
            .SingleAsync();

            var relatedVnfClusterInfo = await _repositoryWrapper.VnfClusterInfoRepository.FindByCondition(x => x.Clusternameid == id).ToListAsync();
            if (relatedVnfClusterInfo != null && relatedVnfClusterInfo.Count > 0)
            {
                if (relatedVnfClusterInfo.Any(x => x.Clusternameid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"Cluster '{entity.Clusterdescription}', is Linked with VNF Cluster Info ",
                        Data = entity.Clusternameid,
                        Warning = true
                    };
                }
                else
                {
                    //foreach (var item in relatedVnfClusterInfo)
                    //{
                    //    item.Clusternameid = null;
                    //    _repositoryWrapper.CnfClusterInfoRepository.Update(item);
                    //}

                }

            }
            await _repositoryWrapper.SaveAsync();

            _repositoryWrapper.ClusterNameRepository.DeleteDeep(entity);

            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Clusternameid
            };
        }
        #endregion

    }
}
