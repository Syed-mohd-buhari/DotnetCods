using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.CnfHardwareType;
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
    public class CnfHardwareTypeManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly DropdownDataServiceManager _dropDownManager;


        public CnfHardwareTypeManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, DropdownDataServiceManager dropownManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _dropDownManager = dropownManager;
        }
        #region UI Member function
        public ExpressionStarter<Cnfhardware> ApplyFilterForOracleModel(CnfHardwareTypeQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Cnfhardware>();

            if (request.Cnfhardwareid != null && request.Cnfhardwareid.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfhardware>();
                foreach (var item in request.Cnfhardwareid)
                    predicateInner.Or(x => x.Cnfhardwareid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Description != null && request.Description.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfhardware>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }            

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfhardware>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Cnfhardware>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(CnfHardwareTypeQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<CnfHardwareTypeDtoGrid>(new GenerateRenderForGrid<CnfHardwareTypeDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Cnfhardware>(true);
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

        public List<CnfHardwareTypeDtoGrid> MappingDto(List<CnfHardware> query)
        {
            var result = new List<CnfHardwareTypeDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new CnfHardwareTypeDtoGrid();

                    grid.CnfHardwareId = x.CnfHardwareId;
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


        public Dictionary<string, Expression<Func<CnfHardware, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<CnfHardware, object>>[]>
            {
                ["CnfHardwareId"] = new Expression<Func<CnfHardware, object>>[] { p => p.CnfHardwareId },
                ["vnfDescription"] = new Expression<Func<CnfHardware, object>>[] { p => p.Description },
                ["modificationUser"] = new Expression<Func<CnfHardware, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, CnfHardwareTypeQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "cnfHardwareId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CnfHardwareId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.CnfHardwareId))
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

        public IQueryable<CnfHardware> PrepareQuery(ExpressionStarter<Cnfhardware> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.CnfHardwareRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.CnfHardwareRepository.FindAll()
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => CnfHardwareMapper.GetCnfHardware(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(CnfHardwareTypeCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.CnfHardwareRepository.FindByCondition(
               x => x.Description.ToLower().Trim().Replace(" ", "") == dto.Description.ToLower().Trim().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Cnfhardwareid
                };
            }

            Cnfhardware entity = new Cnfhardware()
            {
                Cnfhardwareid = dto.CnfHardwareId,
                Description = dto.Description,
            };

            _repositoryWrapper.CnfHardwareRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public CnfHardwareTypeCreateDto GetCreatePage()
        {
            
            var dto = new CnfHardwareTypeCreateDto();
            return dto;

        }

        public async Task<CnfHardwareTypeUpdateDto> GetUpdatedPage(long id)
        {

            var cnfHardwareTypeModel = await _repositoryWrapper.CnfHardwareRepository.FindByCondition(x => x.Cnfhardwareid == id).Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new CnfHardwareTypeUpdateDto();
            if (cnfHardwareTypeModel != null)
            {
                var cnfHardwareTypeEntity = CnfHardwareMapper.GetCnfHardware(cnfHardwareTypeModel);
                if (cnfHardwareTypeEntity != null)
                {
                    dto.CnfHardwareId = cnfHardwareTypeEntity.CnfHardwareId;
                    dto.Description = cnfHardwareTypeEntity.Description;
                    dto.LastModified = cnfHardwareTypeEntity.ModificationDate;
                    dto.LastModifiedBy = cnfHardwareTypeEntity.ModificationUserEntity.Email;
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(CnfHardwareTypeUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.CnfHardwareRepository.FindByCondition(
                   x => x.Cnfhardwareid != dto.CnfHardwareId
                   && x.Description.ToLower().Trim().Replace(" ", "") == dto.Description.ToLower().Trim().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Cnfhardwareid
                };
            }
            Cnfhardware model = new Cnfhardware()
            {
                Description = dto.Description,
                Cnfhardwareid = dto.CnfHardwareId,
                Deleted = false,
            };
            _repositoryWrapper.CnfHardwareRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.CnfHardwareRepository
            .FindByCondition(x => x.Cnfhardwareid == id)
            .SingleAsync();

            var relatedcnfClusterinf = await _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(x => x.Cnfhardwareid == id).ToListAsync();

            if (relatedcnfClusterinf != null && relatedcnfClusterinf.Count > 0)
            {
                if (relatedcnfClusterinf.Any(x => x.Cnfhardwareid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"Hardware Type '{entity.Description}', is Linked with CNF CLuster Info ",
                        Data = entity.Cnfhardwareid,
                        Warning = true
                    };
                }
            }

            _repositoryWrapper.CnfHardwareRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Cnfhardwareid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            var rm = new List<ResultMessageDto>();
            var entity = await _repositoryWrapper.VnfNameRepository.FindByCondition(x => x.Vnfnameid == id).SingleAsync();
            var relatedVnfInfo = await _repositoryWrapper.VnfInfoRepository.FindByCondition(x => x.Vnfnameid == id).ToListAsync();
            var relatedVmTypeName = await _repositoryWrapper.VmTypeNameRepository.FindByCondition(x => x.Vnfnameid == id).ToListAsync();

            if (relatedVnfInfo != null && relatedVnfInfo.Count > 0)
            {
                if (relatedVnfInfo.Any(x => x.Vnfnameid != 0))
                {
                    rm.Add(new ResultMessageDto { Table = "VNF Info", Values = relatedVnfInfo.Select(x=>x.Vnfnameid.ToString()).ToArray() });                  
                }
            }
            if (relatedVmTypeName != null && relatedVmTypeName.Count > 0)
            {
                if (relatedVmTypeName.Any(x => x.Vnfnameid != 0))
                {
                    rm.Add(new ResultMessageDto { Table = "VM Type Name", Values = relatedVmTypeName.Select(x => x.Vnfnameid.ToString()).ToArray() });

                }
            }
            return new ResultDto
            {
                Warning = true,
                Info = ResultMessages.EntryDeleteNotOrphan,
                Data = new RelatedRecordsResultDto
                {
                    EntityName = "VNF Name",
                    RecordName = entity.Vnfdescription,
                    DataRelatedList = rm
                }
            };
        }
        #endregion

    }
}
