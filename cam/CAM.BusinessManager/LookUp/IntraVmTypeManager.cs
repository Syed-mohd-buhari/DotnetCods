using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
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
    public class IntraVmTypeManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;


        public IntraVmTypeManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
        #region UI Member function
        public ExpressionStarter<Intravmtype> ApplyFilterForOracleModel(IntraVmTypeQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Intravmtype>();

            if (request.Intravmtypeid != null && request.Intravmtypeid.Any())
            {
                var predicateInner = PredicateBuilder.New<Intravmtype>();
                foreach (var item in request.Intravmtypeid)
                    predicateInner.Or(x => x.Intravmtypeid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Intradescription != null && request.Intradescription.Any())
            {
                var predicateInner = PredicateBuilder.New<Intravmtype>();
                foreach (var item in request.Intradescription)
                    predicateInner.Or(x => x.Intradescription == item);
                predicateResult.And(predicateInner);
            }           

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Intravmtype>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Intravmtype>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(IntraVmTypeQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<IntraVmTypeDtoGrid>(new GenerateRenderForGrid<IntraVmTypeDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);
            rtn.Items = result.ToArray();

            predicateResult = PredicateBuilder.New<Intravmtype>(true);
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

        public List<IntraVmTypeDtoGrid> MappingDto(List<IntraVmType> query )
        {
            var result = new List<IntraVmTypeDtoGrid>();
            try
            {
               return result = query.Select(x =>
                {
                    var grid = new IntraVmTypeDtoGrid();

                    grid.IntraVmTypeId = x.IntraVmTypeId;
                    grid.IntraDescription = x.IntraDescription;
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

        public Dictionary<string, Expression<Func<IntraVmType, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<IntraVmType, object>>[]>
            {
                ["intraVmTypeId"] = new Expression<Func<IntraVmType, object>>[] { p => p.IntraVmTypeId },
                ["intraDescription"] = new Expression<Func<IntraVmType, object>>[] { p => p.IntraDescription },
                ["modificationUser"] = new Expression<Func<IntraVmType, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, IntraVmTypeQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "intraVmTypeId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.IntraVmTypeId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto (x.IntraVmTypeId))
                    .Distinct()
                    .ToList(),

                "intraDescription" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.IntraDescription.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.IntraDescription))
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

        public IQueryable<IntraVmType> PrepareQuery(ExpressionStarter<Intravmtype> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.IntraVmTypeRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.IntraVmTypeRepository.FindAll()
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => IntarVmTypeMapper.GetIntarVmTypee(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(IntraVmTypeCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.IntraVmTypeRepository.FindByCondition(
               x => x.Intradescription.ToLower().Trim().Replace(" ", "") == dto.IntraDescription.ToLower().Trim().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Intravmtypeid
                };
            }

            Intravmtype entity = new Intravmtype()
            {
                Intradescription = dto.IntraDescription,
            };

            _repositoryWrapper.IntraVmTypeRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public IntraVmTypeCreateDto GetCreatePage()
        {
            
            var dto = new IntraVmTypeCreateDto();
            return dto;

        }

        public async Task<IntraVmTypeUpdateDto> GetUpdatedPage(long id)
        {

            var IntramTypeModel = await _repositoryWrapper.IntraVmTypeRepository.FindByCondition(x => x.Intravmtypeid == id)
                .Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new IntraVmTypeUpdateDto();
            if (IntramTypeModel != null)
            {
                var vmTypeNameEntity = IntarVmTypeMapper.GetIntarVmTypee(IntramTypeModel);
                if (vmTypeNameEntity != null)
                {
                    dto.IntraDescription = vmTypeNameEntity.IntraDescription;
                    dto.IntraVmTypeId = vmTypeNameEntity.IntraVmTypeId;
                    dto.LastModified = vmTypeNameEntity.ModificationDate;
                    dto.LastModifiedBy = vmTypeNameEntity.ModificationUserEntity.Email;

                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(IntraVmTypeUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.IntraVmTypeRepository.FindByCondition(
                   x => x.Intravmtypeid != dto.IntraVmTypeId
                   && x.Intradescription.ToLower().Trim().Replace(" ", "") == dto.IntraDescription.ToLower().Trim().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Intravmtypeid
                };
            }
            Intravmtype model = new Intravmtype()
            {
                Intravmtypeid = dto.IntraVmTypeId,
                Intradescription = dto.IntraDescription,
                Deleted = false,
            };
            _repositoryWrapper.IntraVmTypeRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.IntraVmTypeRepository.FindByCondition(x => x.Intravmtypeid == id).SingleAsync();
            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Intravmtypeid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Vnfvmtypenameid != 0))
                {

                    return new ResultDto
                    {
                        Info = $"{entity.Intravmtypeid}, This Id has Linked to VNF Info ",
                        Data = entity.Intravmtypeid
                    };
                }
            }
            
            _repositoryWrapper.IntraVmTypeRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Intravmtypeid
            };

        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.IntraVmTypeRepository.FindByCondition(x => x.Intravmtypeid == id).SingleAsync();

            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Intravmtypeid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Intravmtypeid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"Intra VM Type '{entity.Intradescription}', is Linked to VNF Info ",
                        Data = entity.Intravmtypeid,
                        Warning = true
                    };
                }
            }           

            _repositoryWrapper.IntraVmTypeRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Intravmtypeid
            };
        }
        #endregion

    }
}
