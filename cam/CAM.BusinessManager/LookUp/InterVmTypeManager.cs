using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.InterVMType;
using CAM.DataTransferObjects.LookUp.IntraVMType;
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
    public class InterVmTypeManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;


        public InterVmTypeManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
        #region UI Member function
        public ExpressionStarter<Intervmtype> ApplyFilterForOracleModel(InterVmTypeQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Intervmtype>();

            if (request.Intervmtypeid != null && request.Intervmtypeid.Any())
            {
                var predicateInner = PredicateBuilder.New<Intervmtype>();
                foreach (var item in request.Intervmtypeid)
                    predicateInner.Or(x => x.Intervmtypeid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Interdescription != null && request.Interdescription.Any())
            {
                var predicateInner = PredicateBuilder.New<Intervmtype>();
                foreach (var item in request.Interdescription)
                    predicateInner.Or(x => x.Interdescription == item);
                predicateResult.And(predicateInner);
            }           

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Intervmtype>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Intervmtype>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(InterVmTypeQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<InterVmTypeDtoGrid>(new GenerateRenderForGrid<InterVmTypeDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Intervmtype>(true);
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

        public List<InterVmTypeDtoGrid> MappingDto(List<InterVmType> query)
        {
            var result = new List<InterVmTypeDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new InterVmTypeDtoGrid();

                    grid.InterVmTypeId = x.InterVmTypeId;
                    grid.InterDescription = x.InterDescription;
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

        public Dictionary<string, Expression<Func<InterVmType, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<InterVmType, object>>[]>
            {
                ["interVmTypeId"] = new Expression<Func<InterVmType, object>>[] { p => p.InterVmTypeId },
                ["interDescription"] = new Expression<Func<InterVmType, object>>[] { p => p.InterDescription },
                ["modificationUser"] = new Expression<Func<InterVmType, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, InterVmTypeQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "interVmTypeId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.InterVmTypeId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto (x.InterVmTypeId))
                    .Distinct()
                    .ToList(),

                "interDescription" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.InterDescription.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.InterDescription))
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

        public IQueryable<InterVmType> PrepareQuery(ExpressionStarter<Intervmtype> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.InterVmTypeRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.InterVmTypeRepository.FindAll()
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => InterVmTypeMapper.GetInterVmType(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(InterVmTypeCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.InterVmTypeRepository.FindByCondition(
               x => x.Interdescription.ToLower().Trim().Replace(" ", "") == dto.InterDescription.ToLower().Trim().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Intervmtypeid
                };
            }

            Intervmtype entity = new Intervmtype()
            {
                Interdescription = dto.InterDescription,
            };

            _repositoryWrapper.InterVmTypeRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public InterVmTypeCreateDto GetCreatePage()
        {
            
            var dto = new InterVmTypeCreateDto();
            return dto;

        }

        public async Task<InterVmTypeUpdateDto> GetUpdatedPage(long id)
        {

            var IntramTypeModel = await _repositoryWrapper.InterVmTypeRepository.FindByCondition(x => x.Intervmtypeid == id).Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new InterVmTypeUpdateDto();
            if (IntramTypeModel != null)
            {
                var vmTypeNameEntity = InterVmTypeMapper.GetInterVmType(IntramTypeModel);
                if (vmTypeNameEntity != null)
                {
                    dto.InterDescription = vmTypeNameEntity.InterDescription;
                    dto.InterVmTypeId = vmTypeNameEntity.InterVmTypeId;
                    dto.LastModified = vmTypeNameEntity.ModificationDate;
                    dto.LastModifiedBy = vmTypeNameEntity.ModificationUserEntity.Email;
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(InterVmTypeUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.InterVmTypeRepository.FindByCondition(
                   x => x.Intervmtypeid != dto.InterVmTypeId
                   && x.Interdescription.ToLower().Trim().Replace(" ", "") == dto.InterDescription.ToLower().Trim().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Intervmtypeid
                };
            }
            Intervmtype model = new Intervmtype()
            {
                Intervmtypeid = dto.InterVmTypeId,
                Interdescription = dto.InterDescription,
                Deleted  = false
            };
            _repositoryWrapper.InterVmTypeRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.InterVmTypeRepository.FindByCondition(x => x.Intervmtypeid == id).SingleAsync();
            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Intervmtypeid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Vnfvmtypenameid != 0))
                {

                    return new ResultDto
                    {
                        Info = $"{entity.Intervmtypeid}, This Id has Linked to VNF Info ",
                        Data = entity.Intervmtypeid
                    };
                }
            }
            
            _repositoryWrapper.InterVmTypeRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Intervmtypeid
            };

        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.InterVmTypeRepository.FindByCondition(x => x.Intervmtypeid == id).SingleAsync();

            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Intervmtypeid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Intervmtypeid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"Inter Type '{entity.Interdescription}', is Linked with VNF Info ",
                        Data = entity.Intervmtypeid,
                        Warning = true
                    };
                }
            }           

            _repositoryWrapper.InterVmTypeRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Intervmtypeid
            };
        }
        #endregion

    }
}
