using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.CnfCluster;
using CAM.DataTransferObjects.QueryDto.XBom.CBom;
using CAM.Entities.Mappers.Cbom;
using CAM.Entities.Models.CBom;
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

namespace CAM.BusinessManager.LookUp
{
    public class CnfClusterManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;


        public CnfClusterManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
        #region UI Member function
        public ExpressionStarter<Cnfcluster> ApplyFilterForOracleModel(CnfClusterQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Cnfcluster>();

            if (request.Cnfclusterid != null && request.Cnfclusterid.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfcluster>();
                foreach (var item in request.Cnfclusterid)
                    predicateInner.Or(x => x.Cnfclusterid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Cnfclustername != null && request.Cnfclustername.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfcluster>();
                foreach (var item in request.Cnfclustername)
                    predicateInner.Or(x => x.Cnfclustername == item);
                predicateResult.And(predicateInner);
            }
            if (request.Nodepool != null && request.Nodepool.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfcluster>();
                foreach (var item in request.Nodepool)
                    predicateInner.Or(x => x.Nodepool == item);
                predicateResult.And(predicateInner);
            }
            if (request.Alaisname != null && request.Alaisname.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfcluster>();
                foreach (var item in request.Alaisname)
                    predicateInner.Or(x => x.Alaisname == item);
                predicateResult.And(predicateInner);
            }
            if (request.Cnfname != null && request.Cnfname.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfcluster>();
                foreach (var item in request.Cnfname)
                    predicateInner.Or(x => x.Cnfnameid.ToString() == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfcluster>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Cnfcluster>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(CnfClusterQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<CnfClusterDtoGrid>(new GenerateRenderForGrid<CnfClusterDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Cnfcluster>(true);
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

        public List<CnfClusterDtoGrid> MappingDto(List<CnfCluster> query)
        {
            var result = new List<CnfClusterDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new CnfClusterDtoGrid();

                    grid.CnfClusterId = x.CnfClusterId;
                    grid.CnfClusterName = x.CnfClusterName;
                    grid.AlaisName = x.AlaisName;
                    grid.NodePool = x.NodePool;
                    grid.CnfName = x.CnfName != null? x.CnfName.CnfDescription : string.Empty;
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


        public Dictionary<string, Expression<Func<CnfCluster, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<CnfCluster, object>>[]>
            {
                ["cnfClusterId"] = new Expression<Func<CnfCluster, object>>[] { p => p.CnfClusterId },
                ["cnfClusterName"] = new Expression<Func<CnfCluster, object>>[] { p => p.CnfClusterName },
                ["nodePool"] = new Expression<Func<CnfCluster, object>>[] { p => p.NodePool },
                ["alaisName"] = new Expression<Func<CnfCluster, object>>[] { p => p.AlaisName },
                ["modificationUser"] = new Expression<Func<CnfCluster, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, CnfClusterQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "cnfClusterId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CnfClusterId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.CnfClusterId))
                    .Distinct()
                    .ToList(),

                "cnfClusterName" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CnfClusterName.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.CnfClusterName))
                    .Distinct()
                    .ToList(),

                "nodePool" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.NodePool.ToString().Contains(propertyFilter))
                 .Select(x => new FilterValueDto(x.NodePool))
                .Distinct()
                .ToList(),

                "alaisName" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.AlaisName.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.AlaisName))
                    .Distinct()
                    .ToList(),

                "cnfName" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CnfName.CnfDescription.ToString().Contains(propertyFilter))
                 .Select(x => new FilterValueDto { Text = x.CnfName.CnfDescription, Value = x.CnfName.CnfNameId.ToString()})
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

        public IQueryable<CnfCluster> PrepareQuery(ExpressionStarter<Cnfcluster> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.CnfClusterRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .Include(m => m.Cnfname)
               : _repositoryWrapper.CnfClusterRepository.FindAll()
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation)
                 .Include(m => m.Cnfname);

            return query.AsEnumerable().Select(p => CnfClusterMapper.GetCnfCluster(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(CnfClusterCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.CnfClusterRepository.FindByCondition(
               x => x.Cnfclustername.ToLower().Trim().Replace(" ", "") == dto.CnfClusterName.ToLower().Trim().Replace(" ", "") && 
               x.Nodepool.ToLower().Trim().Replace(" ", "") == dto.NodePool.ToLower().Trim().Replace(" ","") &&
               x.Alaisname.ToLower().Trim().Replace(" ", "") == dto.AlaisName.ToLower().Trim().Replace(" ", "") &&
               x.Cnfnameid == dto.CnfNameId
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Cnfclusterid
                };
            }

            Cnfcluster entity = new Cnfcluster()
            {
                Cnfclusterid = dto.CnfClusterId,
                Cnfclustername = dto.CnfClusterName,
                Cnfnameid = dto.CnfNameId,
                Nodepool = dto.NodePool,
                Alaisname = dto.AlaisName,
            };

            _repositoryWrapper.CnfClusterRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<CnfClusterCreateDto> GetCreatePage()
        {
            
            var dto = new CnfClusterCreateDto();
            var cnfResource = await _repositoryWrapper.CnfNameRepository.FindAll().ToListAsync();
            dto.CnfNameResource = cnfResource.ToDictionary(x => x.Cnfnameid, y => y.Cnfdescription);
            return dto;

        }

        public async Task<CnfClusterUpdateDto> GetUpdatedPage(long id)
        {

            var Entity = await _repositoryWrapper.CnfClusterRepository.FindByCondition(x => x.Cnfclusterid == id).Include(x => x.Cnfname).Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new CnfClusterUpdateDto();
            if (Entity != null)
            {
                var model = CnfClusterMapper.GetCnfCluster(Entity);
                if (model != null)
                {
                    dto.CnfClusterId = model.CnfClusterId;
                    dto.CnfClusterName = model.CnfClusterName;
                    dto.NodePool = model.NodePool;
                    dto.AlaisName = model.AlaisName;
                    dto.CnfNameId = model.CnfNameId != null? model.CnfNameId.Value: 0;
                    dto.LastModified = model.ModificationDate;
                    dto.LastModifiedBy = model.ModificationUserEntity.Email;
                    dto.CnfNameResource = _repositoryWrapper.CnfNameRepository.FindAll().ToDictionary(x => x.Cnfnameid, y => y.Cnfdescription);
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(CnfClusterUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.CnfClusterRepository.FindByCondition(
                   x => x.Cnfclusterid != dto.CnfClusterId
                   && x.Cnfclustername.ToLower().Trim().Replace(" ", "") == dto.CnfClusterName.ToLower().Trim().Replace(" ", "")
                   && x.Alaisname.ToLower().Trim().Replace(" ", "") == dto.AlaisName.ToLower().Trim().Replace(" ", "")
                   && x.Nodepool.ToLower().Trim().Replace(" ", "") == dto.NodePool.ToLower().Trim().Replace(" ", "")
                   && x.Cnfnameid == dto.CnfNameId
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Cnfclusterid
                };
            }
            Cnfcluster model = new Cnfcluster()
            {
                Alaisname = dto.AlaisName,
                Nodepool = dto.NodePool,
                Cnfclustername = dto.CnfClusterName,
                Cnfclusterid = dto.CnfClusterId,
                Cnfnameid = dto.CnfNameId,
                Deleted = false,
            };
            _repositoryWrapper.CnfClusterRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.CnfClusterRepository
            .FindByCondition(x => x.Cnfclusterid == id)
            .SingleAsync();

            var relatedcnfPodInfo = await _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(x => x.Cnfclusterid == id).ToListAsync();

            if (relatedcnfPodInfo != null && relatedcnfPodInfo.Count > 0)
            {
                if (relatedcnfPodInfo.Any(x => x.Cnfclusterid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"CNF Cluster Name '{entity.Cnfclustername}', is Linked with CNF Pod Info ",
                        Data = entity.Cnfclusterid,
                        Warning = true
                    };
                }
            }

            _repositoryWrapper.CnfClusterRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Cnfclusterid
            };
        }
        #endregion

    }
}
