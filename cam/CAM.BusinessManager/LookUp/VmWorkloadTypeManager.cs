using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.VmWorkloadType;
using CAM.DataTransferObjects.LookUp.VNFCluster;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Entities.Mappers.Vbom;
using CAM.Entities.Models.VBom;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Drawing.Charts;
using ICSharpCode.SharpZipLib.Zip;
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
    public class VmWorkloadTypeManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;


        public VmWorkloadTypeManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
        #region UI Member function
        public ExpressionStarter<Vmworkloadtype> ApplyFilterForOracleModel(VmWorkloadTypeQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Vmworkloadtype>();

            if (request.Vmworkloadtypeid != null && request.Vmworkloadtypeid.Any())
            {
                var predicateInner = PredicateBuilder.New<Vmworkloadtype>();
                foreach (var item in request.Vmworkloadtypeid)
                    predicateInner.Or(x => x.Vmworkloadtypeid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Description != null && request.Description.Any())
            {
                var predicateInner = PredicateBuilder.New<Vmworkloadtype>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }           

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Vmworkloadtype>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Vmworkloadtype>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(VmWorkloadTypeQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<VmWorkloadTypeDtoGrid>(new GenerateRenderForGrid<VmWorkloadTypeDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Vmworkloadtype>(true);
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

        public List<VmWorkloadTypeDtoGrid> MappingDto(List<VmWorkLoadType> query)
        {
            var result = new List<VmWorkloadTypeDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new VmWorkloadTypeDtoGrid();

                    grid.VmWorkLoadTypeId = x.VmworkLoadTypeId;
                    grid.Description = x.Description;
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


        public Dictionary<string, Expression<Func<VmWorkLoadType, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<VmWorkLoadType, object>>[]>
            {
                ["vmworkLoadTypeId"] = new Expression<Func<VmWorkLoadType, object>>[] { p => p.VmworkLoadTypeId },
                ["description"] = new Expression<Func<VmWorkLoadType, object>>[] { p => p.Description },
                ["modificationUser"] = new Expression<Func<VmWorkLoadType, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, VmWorkloadTypeQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "vmWorkLoadTypeId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VmworkLoadTypeId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto (x.VmworkLoadTypeId))
                    .Distinct()
                    .ToList(),

                "description" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Description.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.Description))
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

        public IQueryable<VmWorkLoadType> PrepareQuery(ExpressionStarter<Vmworkloadtype> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.VnfWorkLoadTypeRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.VnfWorkLoadTypeRepository.FindAll()
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => VmWorkLoadTypeMapper.GetVmWorkLoadType(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(VmWorkloadTypeCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.VnfWorkLoadTypeRepository.FindByCondition(
               x => x.Description.ToLower().Trim().Replace(" ", "") == dto.Description.ToLower().Trim().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vmworkloadtypeid
                };
            }

            Vmworkloadtype entity = new Vmworkloadtype()
            {
                Description = dto.Description,
            };

            _repositoryWrapper.VnfWorkLoadTypeRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public VmWorkloadTypeCreateDto GetCreatePage()
        {
            
            var dto = new VmWorkloadTypeCreateDto();
            return dto;

        }

        public async Task<VmWorkloadTypeUpdateDto> GetUpdatedPage(long id)
        {

            var IntramTypeModel = await _repositoryWrapper.VnfWorkLoadTypeRepository.FindByCondition(x => x.Vmworkloadtypeid == id)
                .Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new VmWorkloadTypeUpdateDto();
            if (IntramTypeModel != null)
            {
                var vmTypeNameEntity = VmWorkLoadTypeMapper.GetVmWorkLoadType(IntramTypeModel);
                if (vmTypeNameEntity != null)
                {
                    dto.Description = vmTypeNameEntity.Description;
                    dto.VmWorkLoadTypeId = vmTypeNameEntity.VmworkLoadTypeId;
                    dto.LastModified = vmTypeNameEntity.ModificationDate;
                    dto.LastModifiedBy = vmTypeNameEntity.ModificationUserEntity.Email;
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(VmWorkloadTypeUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.VnfWorkLoadTypeRepository.FindByCondition(
                   x => x.Vmworkloadtypeid != dto.VmWorkLoadTypeId
                   && x.Description.ToLower().Trim().Replace(" ", "") == dto.Description.ToLower().Trim().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vmworkloadtypeid
                };
            }
            Vmworkloadtype model = new Vmworkloadtype()
            {
                Description = dto.Description,
                Vmworkloadtypeid = dto.VmWorkLoadTypeId,
                Deleted =  false,
            };
            _repositoryWrapper.VnfWorkLoadTypeRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.VnfWorkLoadTypeRepository.FindByCondition(x => x.Vmworkloadtypeid == id).SingleAsync();
            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Vmworkloadtypeid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Vnfvmtypenameid != 0))
                {

                    return new ResultDto
                    {
                        Info = $"{entity.Vmworkloadtypeid}, This Id has Linked to VNF Info ",
                        Data = entity.Vmworkloadtypeid
                    };
                }
            }
            
            _repositoryWrapper.VnfWorkLoadTypeRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vmworkloadtypeid
            };

        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.VnfWorkLoadTypeRepository.FindByCondition(x => x.Vmworkloadtypeid == id).SingleAsync();

            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Vmworkloadtypeid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Vmworkloadtypeid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"VM Workload Type '{entity.Description}', is Linked with VNF Info ",
                        Data = entity.Vmworkloadtypeid,
                        Warning = true
                    };
                }
            }           

            _repositoryWrapper.VnfWorkLoadTypeRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vmworkloadtypeid
            };
        }
        #endregion

    }
}
