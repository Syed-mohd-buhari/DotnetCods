using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.VMTypeName;
using CAM.DataTransferObjects.LookUp.VmWorkloadType;
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
    public class VmTypeNameManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly DropdownDataServiceManager _dropDownManager;


        public VmTypeNameManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, DropdownDataServiceManager dropownManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _dropDownManager = dropownManager;
        }
        #region UI Member function
        public ExpressionStarter<Vmtypename> ApplyFilterForOracleModel(VmTypeNameQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Vmtypename>();

            if (request.Vmtypenameid != null && request.Vmtypenameid.Any())
            {
                var predicateInner = PredicateBuilder.New<Vmtypename>();
                foreach (var item in request.Vmtypenameid)
                    predicateInner.Or(x => x.Vmtypenameid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Vmtypedescription != null && request.Vmtypedescription.Any())
            {
                var predicateInner = PredicateBuilder.New<Vmtypename>();
                foreach (var item in request.Vmtypedescription)
                    predicateInner.Or(x => x.Vmtypedescription == item);
                predicateResult.And(predicateInner);
            }
            if (request.Vnfnameid != null && request.Vnfnameid.Any())
            {
                var predicateInner = PredicateBuilder.New<Vmtypename>();
                foreach (var item in request.Vnfnameid)
                    predicateInner.Or(x => x.Vnfnameid == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Vmtypename>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Vmtypename>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(VmTypeNameQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<VmTypeNameDtoGrid>(new GenerateRenderForGrid<VmTypeNameDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Vmtypename>(true);
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

        public List<VmTypeNameDtoGrid> MappingDto(List<VmTypeName> query)
        {
            var result = new List<VmTypeNameDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new VmTypeNameDtoGrid();

                    grid.VnfName = x.VnfName.VnfDescription;
                    grid.VmTypeDescription = x.VmTypeDescription;
                    grid.VmTypeNameId = x.VmtypeNameId;
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



        public Dictionary<string, Expression<Func<VmTypeName, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<VmTypeName, object>>[]>
            {
                ["vnfNameId"] = new Expression<Func<VmTypeName, object>>[] { p => p.VnfNameId },
                ["vmTypeDescription"] = new Expression<Func<VmTypeName, object>>[] { p => p.VmTypeDescription },
                ["vmtypeNameId"] = new Expression<Func<VmTypeName, object>>[] { p => p.VmtypeNameId },
                ["modificationUser"] = new Expression<Func<VmTypeName, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, VmTypeNameQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "vnfName" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VnfNameId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto { Text = x.VnfName.VnfDescription , Value = x.VnfNameId.ToString()})
                    .Distinct()
                    .ToList(),

                "vmTypeDescription" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VmTypeDescription.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.VmTypeDescription))
                    .Distinct()
                    .ToList(),

                "vmtypeNameId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VmtypeNameId.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.VmtypeNameId))
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

        public IQueryable<VmTypeName> PrepareQuery(ExpressionStarter<Vmtypename> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.VmTypeNameRepository.FindByCondition(predicateResult)
                .Include(m => m.Vnfname)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.VmTypeNameRepository.FindAll()
                 .Include(m => m.Vnfname)
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => VmTypeNameMapper.GetVmTypeName(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(VmTypeNameCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.VmTypeNameRepository.FindByCondition(
               x => x.Vmtypedescription.ToLower().Trim().Replace(" ", "") == dto.VmTypeDescription.ToLower().Trim().Replace(" ", "")
               && x.Vnfnameid != dto.VnfNameId
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vmtypenameid
                };
            }

            Vmtypename entity = new Vmtypename()
            {
                Vnfnameid = dto.VnfNameId,
                Vmtypedescription = dto.VmTypeDescription,
            };

            _repositoryWrapper.VmTypeNameRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public VmTypeNameCreateDto GetCreatePage()
        {
            
            var dto = new VmTypeNameCreateDto();
            dto.VnfNamResource = _repositoryWrapper.VnfNameRepository.FindAll().ToDictionary(x => x.Vnfnameid, y => y.Vnfdescription);
            return dto;

        }

        public async Task<VmTypeNameUpdateDto> GetUpdatedPage(long id)
        {

            var vmTypeNameModel = await _repositoryWrapper.VmTypeNameRepository.FindByCondition(x => x.Vmtypenameid == id).Include(x => x.Vnfname).Include(x=>x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new VmTypeNameUpdateDto();
            if (vmTypeNameModel != null)
            {
                var vmTypeNameEntity = VmTypeNameMapper.GetVmTypeName(vmTypeNameModel);
                if (vmTypeNameEntity != null)
                {
                    dto.VnfNameId = vmTypeNameEntity.VnfNameId;
                    dto.VmTypeDescription = vmTypeNameEntity.VmTypeDescription;
                    dto.VmTypeNameId = vmTypeNameEntity.VmtypeNameId;
                    dto.VnfNamResource = _repositoryWrapper.VnfNameRepository.FindAll().ToDictionary(x => x.Vnfnameid, y => y.Vnfdescription);
                    dto.LastModified = vmTypeNameEntity.ModificationDate;
                    dto.LastModifiedBy = vmTypeNameEntity.ModificationUserEntity.Email;
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(VmTypeNameUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.VmTypeNameRepository.FindByCondition(
                   x => x.Vmtypenameid != dto.VmTypeNameId
                   && x.Vmtypedescription.ToLower().Trim().Replace(" ", "") == dto.VmTypeDescription.ToLower().Trim().Replace(" ", "")
                   && x.Vnfnameid != dto.VmTypeNameId
                   && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vnfnameid
                };
            }
            Vmtypename model = new Vmtypename()
            {
                Vmtypedescription = dto.VmTypeDescription,
                Vnfnameid = dto.VnfNameId,
                Deleted = false,
                Vmtypenameid = dto.VmTypeNameId
            };
            _repositoryWrapper.VmTypeNameRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.VmTypeNameRepository.FindByCondition(x => x.Vmtypenameid == id).SingleAsync();
            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Vnfvmtypenameid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Vnfvmtypenameid != 0))
                {

                    return new ResultDto
                    {
                        Info = $"{entity.Vmtypenameid}, This Id has Linked to VNF Info ",
                        Data = entity.Vmtypenameid
                    };
                }
            }
            
            _repositoryWrapper.VmTypeNameRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vmtypenameid
            };

        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.VmTypeNameRepository
            .FindByCondition(x => x.Vmtypenameid == id)
            .SingleAsync();

            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Vnfvmtypenameid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Vnfvmtypenameid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"VM Type Name '{entity.Vnfname}', is Linked with VNF Info ",
                        Data = entity.Vmtypenameid,
                        Warning = true
                    };
                }
            }           

            _repositoryWrapper.VmTypeNameRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vmtypenameid
            };
        }
        #endregion

    }
}
