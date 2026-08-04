using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.CnfName;
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
    public class CnfNameManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly DropdownDataServiceManager _dropDownManager;


        public CnfNameManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, DropdownDataServiceManager dropownManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _dropDownManager = dropownManager;
        }
        #region UI Member function
        public ExpressionStarter<Cnfname> ApplyFilterForOracleModel(CnfNameQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Cnfname>();

            if (request.Cnfnameid != null && request.Cnfnameid.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfname>();
                foreach (var item in request.Cnfnameid)
                    predicateInner.Or(x => x.Cnfnameid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Cnfdescription != null && request.Cnfdescription.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfname>();
                foreach (var item in request.Cnfdescription)
                    predicateInner.Or(x => x.Cnfdescription == item);
                predicateResult.And(predicateInner);
            }

            if (request.Product != null && request.Product.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfname>();
                foreach (var item in request.Product)
                    predicateInner.Or(x => x.Productid.ToString() == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfname>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Cnfname>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(CnfNameQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<CnfNameDtoGrid>(new GenerateRenderForGrid<CnfNameDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Cnfname>(true);
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

        public List<CnfNameDtoGrid> MappingDto(List<CnfName> query)
        {
            var result = new List<CnfNameDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new CnfNameDtoGrid();

                    grid.CnfNameId = x.CnfNameId;
                    grid.CnfDescription = x.CnfDescription;
                    grid.Product = x.Product != null? x.Product.Description:string.Empty;
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


        public Dictionary<string, Expression<Func<CnfName, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<CnfName, object>>[]>
            {
                ["CnfHardwareId"] = new Expression<Func<CnfName, object>>[] { p => p.CnfNameId },
                ["vnfDescription"] = new Expression<Func<CnfName, object>>[] { p => p.CnfDescription },
                ["modificationUser"] = new Expression<Func<CnfName, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, CnfNameQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "cnfNameId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CnfNameId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.CnfNameId))
                    .Distinct()
                    .ToList(),

                "cnfDescription" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CnfDescription.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.CnfDescription))
                    .Distinct()
                    .ToList(),

                "product" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Product.Description.ToString().Contains(propertyFilter))
                 .Select(x => new FilterValueDto(x.Product.Description))
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

        public IQueryable<CnfName> PrepareQuery(ExpressionStarter<Cnfname> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.CnfNameRepository.FindByCondition(predicateResult)
                .Include(m => m.Product)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.CnfNameRepository.FindAll()
                 .Include(m => m.Product)
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => CnfNameMapper.GetCnfInfo(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(CnfNameCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.CnfNameRepository.FindByCondition(
               x => x.Cnfdescription.ToLower().Trim().Replace(" ", "") == dto.CnfDescription.ToLower().Trim().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Cnfnameid
                };
            }

            Cnfname entity = new Cnfname()
            {
                Cnfnameid = dto.CnfNameId,
                Cnfdescription = dto.CnfDescription,
                Productid = dto.ProductId,
            };

            _repositoryWrapper.CnfNameRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public CnfNameCreateDto GetCreatePage()
        {
            
            var dto = new CnfNameCreateDto();
            dto.ProductResource = _repositoryWrapper.ProductNameRepository.FindAll().ToDictionary(x => x.Productnameid, y => y.Description);
            return dto;

        }

        public async Task<CnfNameUpdateDto> GetUpdatedPage(long id)
        {

            var Entity = await _repositoryWrapper.CnfNameRepository.FindByCondition(x => x.Cnfnameid == id).Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new CnfNameUpdateDto();
            if (Entity != null)
            {
                var model = CnfNameMapper.GetCnfInfo(Entity);
                if (model != null)
                {
                    dto.CnfNameId = model.CnfNameId;
                    dto.CnfDescription = model.CnfDescription;
                    dto.LastModified = model.ModificationDate;
                    dto.LastModifiedBy = model.ModificationUserEntity.Email;
                    dto.ProductId = model.ProductId.Value;
                    dto.ProductResource = _repositoryWrapper.ProductNameRepository.FindAll().ToDictionary(x => x.Productnameid, y => y.Description);
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(CnfNameUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.CnfNameRepository.FindByCondition(
                   x => x.Cnfnameid != dto.CnfNameId
                   && x.Cnfdescription.ToLower().Trim().Replace(" ", "") == dto.CnfDescription.ToLower().Trim().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Cnfnameid
                };
            }
            Cnfname model = new Cnfname()
            {
                Cnfdescription = dto.CnfDescription,
                Cnfnameid = dto.CnfNameId,
                Deleted = false,
                Productid = dto.ProductId,
            };
            _repositoryWrapper.CnfNameRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.CnfNameRepository
            .FindByCondition(x => x.Cnfnameid == id)
            .SingleAsync();

            var relatedcnfClusterInfo = await _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(x => x.Cnfnameid == id).ToListAsync();

            var relatedCnfCluster = await _repositoryWrapper.CnfClusterRepository.FindByCondition(x => x.Cnfnameid == id).ToListAsync();

            if (relatedcnfClusterInfo != null && relatedcnfClusterInfo.Count > 0)
            {
                if (relatedcnfClusterInfo.Any(x => x.Cnfnameid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"CNF Name '{entity.Cnfdescription}', is Linked with CNF Cluster Info ",
                        Data = entity.Cnfnameid,
                        Warning = true
                    };
                }
            }

            if (relatedCnfCluster != null && relatedCnfCluster.Count > 0)
            {
                if (relatedCnfCluster.Any(x => x.Cnfnameid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"CNF Name '{entity.Cnfdescription}', is Linked with CNF Cluster ",
                        Data = entity.Cnfnameid,
                        Warning = true
                    };
                }
            }

            _repositoryWrapper.CnfNameRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Cnfnameid
            };
        }
        #endregion

    }
}
