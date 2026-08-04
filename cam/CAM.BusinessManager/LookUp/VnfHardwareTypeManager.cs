using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.VnfHardwareType;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Entities.Mappers.Vbom;
using CAM.Entities.Models;
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
    public class VnfHardwareTypeManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public VnfHardwareTypeManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
        public ExpressionStarter<Vnfhardware> ApplyFilter(VnfHardwareTypeQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Vnfhardware>();

            if (request.Vnfhardwareid != null && request.Vnfhardwareid.Any())
            {
                var predicateInner = PredicateBuilder.New<Vnfhardware>();
                foreach (var item in request.Vnfhardwareid)
                    predicateInner.Or(x => x.Vnfhardwareid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Description != null && request.Description.Any())
            {
                var predicateInner = PredicateBuilder.New<Vnfhardware>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Vnfhardware>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Vnfhardware>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public IQueryable<VnfHardWare> GetHardwareEntities(ExpressionStarter<Vnfhardware> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.VnfHardwareRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.VnfHardwareRepository.FindAll()
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => VnfHardwareMapper.GetVnfHardWare(p)).AsQueryable();
        }
        public List<VnfHardwareTypeDtoGrid> MappingDto(List<VnfHardWare> query)
        {
            var result = new List<VnfHardwareTypeDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new VnfHardwareTypeDtoGrid();
                    grid.VnfHardwareId = x.VnfHardWareId;
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


        public Dictionary<string, Expression<Func<VnfHardWare, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<VnfHardWare, object>>[]>
            {
                ["vnfHardwareId"] = new Expression<Func<VnfHardWare, object>>[] { p => p.VnfHardWareId },
                ["description"] = new Expression<Func<VnfHardWare, object>>[] { p => p.Description },
                ["lastModifiedBy"] = new Expression<Func<VnfHardWare, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<ResultDto> FindWithCondition(VnfHardwareTypeQueryDto dto)
        {
            var predicateResult = ApplyFilter(dto);
            var rtn = new QueryResultDto<VnfHardwareTypeDtoGrid>(new GenerateRenderForGrid<VnfHardwareTypeDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => GetHardwareEntities(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);
            rtn.Items = result.ToArray();

            #region Returning all hardware_type for vbom_cluster_info: hardware_type attribute dropdown
            predicateResult = PredicateBuilder.New<Vnfhardware>(true);
            var fullQuery = await Task.Run(() => GetHardwareEntities(predicateResult));
            var allItems = MappingDto(fullQuery.ToList());
            #endregion

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
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, VnfHardwareTypeQueryDto request)
        {
            var predicateResult = ApplyFilter(request);

            var filteredQuery = await Task.Run(() => GetHardwareEntities(predicateResult));

            var result = propertyName switch
            {
                "vnfHardwareId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VnfHardWareId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.VnfHardWareId))
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
        public VnfHardwareTypeCreateDto GetCreatePage()
        {
            var dto = new VnfHardwareTypeCreateDto();
            return dto;
        }

        public async Task<VnfHardwareTypeUpdateDto> GetUpdatedPage(long id)
        {

            var vnfHardwareTypeModel = await _repositoryWrapper.VnfHardwareRepository.FindByCondition(x => x.Vnfhardwareid == id).Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new VnfHardwareTypeUpdateDto();
            if (vnfHardwareTypeModel != null)
            {
                var vnfHardwareTypeEntity = VnfHardwareMapper.GetVnfHardWare(vnfHardwareTypeModel);
                if (vnfHardwareTypeEntity != null)
                {
                    dto.VnfHardwareId = vnfHardwareTypeEntity.VnfHardWareId;
                    dto.Description = vnfHardwareTypeEntity.Description;
                    dto.LastModified = vnfHardwareTypeEntity.ModificationDate;
                    dto.LastModifiedBy = vnfHardwareTypeEntity.ModificationUserEntity.Email;
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> AddVbomHardware(VnfHardwareTypeCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.VnfHardwareRepository.FindByCondition(
               x => x.Description.ToLower().Trim().Replace(" ", "") == dto.Description.ToLower().Trim().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vnfhardwareid
                };
            }
            Vnfhardware vnfHardwareModel = new Vnfhardware
            {
                Vnfhardwareid = dto.VnfHardwareId,
                Description = dto.Description,
            };

            _repositoryWrapper.VnfHardwareRepository.Create(vnfHardwareModel);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }
        public async Task<ResultDto> UpdateVbomHardware(VnfHardwareTypeUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.VnfHardwareRepository.FindByCondition(
                   x => x.Vnfhardwareid != dto.VnfHardwareId
                   && x.Description.ToLower().Trim().Replace(" ", "") == dto.Description.ToLower().Trim().Replace(" ", "")
                    ).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vnfhardwareid
                };
            }
            Vnfhardware vnfHardwareModel = new Vnfhardware
            {
                Vnfhardwareid = dto.VnfHardwareId,
                Description = dto.Description,
                Deleted=false
            };
            _repositoryWrapper.VnfHardwareRepository.Update(vnfHardwareModel);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }
        public async Task<ResultDto> DeleteVbomHardware(long id)
        {
            var entity = await _repositoryWrapper.VnfHardwareRepository
            .FindByCondition(x => x.Vnfhardwareid == id)
            .SingleAsync();
            //instead of desc i need to check with id below
            var relatedVnfClusterInfo = await _repositoryWrapper.VnfClusterInfoRepository.FindByCondition(x => x.Hardwaretypeid == entity.Vnfhardwareid).ToListAsync();

            if (relatedVnfClusterInfo != null && relatedVnfClusterInfo.Count > 0)
            {
                //if (relatedVnfClusterInfo.Any(x => string.IsNullOrEmpty(x.Hardwaretype)))
                if (relatedVnfClusterInfo.Any(x => x.Vnfclusterinfoid != 0))
                {
                    return new ResultDto
                    {
                        //Info = $"Hardware Type '{entity.Description}', is Linked with CNF CLuster Info ",//
                        Info = ResultMessages.VnfHardwareType + entity.Description + ResultMessages.VnfHardwareTypeLinkedClusterInfo,
                        Data = entity.Vnfhardwareid,
                        Warning = true
                    };
                }
            }

            _repositoryWrapper.VnfHardwareRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vnfhardwareid
            };
        }

    }
}
