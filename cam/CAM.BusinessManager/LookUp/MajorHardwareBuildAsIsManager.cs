using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ReportScheduler;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.MajorHardwareBuildAsIs;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Spreadsheet;
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
    public class MajorHardwareBuildAsIsManager : BaseManager
    {
        private readonly GridCustomColumnManager _columnManager;
        private readonly DropdownDataServiceManager _dropdownManager;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public MajorHardwareBuildAsIsManager(DropdownDataServiceManager dropdownManager,IMapper mapper, GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper= repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
            _dropdownManager= dropdownManager;
        }

        public ExpressionStarter<Majorhardwarebuildasis> ApplyFilter(MajorHardwareBuildAsIsQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Majorhardwarebuildasis>(true);

            if (request.MajorHardwareBuildAsisId != null && request.MajorHardwareBuildAsisId.Any())
            {
                var predicateInner = PredicateBuilder.New<Majorhardwarebuildasis>();
                foreach (var item in request.MajorHardwareBuildAsisId)
                    predicateInner.Or(x => x.Majorhardwarebuildasisid == item);
                predicateResult.And(predicateInner);
            }
            if (request.HardwareSolution != null && request.HardwareSolution.Any())
            {
                var predicateInner = PredicateBuilder.New<Majorhardwarebuildasis>();
                foreach (var item in request.HardwareSolution)
                    predicateInner.Or(x => x.Hardwaresolution == item);
                predicateResult.And(predicateInner);
            }
            if (request.HardwareType != null && request.HardwareType.Any())
            {
                var predicateInner = PredicateBuilder.New<Majorhardwarebuildasis>();
                foreach (var item in request.HardwareType)
                    predicateInner.Or(x => x.Hardwaretype == item);
                predicateResult.And(predicateInner);
            }
            if (request.OrgEqpManuFacturerDesc != null && request.OrgEqpManuFacturerDesc.Any())
            {
                var predicateInner = PredicateBuilder.New<Majorhardwarebuildasis>();
                foreach (var item in request.OrgEqpManuFacturerDesc)
                    predicateInner.Or(x => Convert.ToString(x.Orgeqpmanufacturer.Orgeqpmanufacturerid) == item);
                predicateResult.And(predicateInner);
            }
            if (request.PlatformDesc != null && request.PlatformDesc.Any())
            {
                var predicateInner = PredicateBuilder.New<Majorhardwarebuildasis>();
                foreach (var item in request.PlatformDesc)
                    predicateInner.Or(x => Convert.ToString(x.Platform.Platformid) == item);
                predicateResult.And(predicateInner);
            }
            if (request.BuildConstructionDesc != null && request.BuildConstructionDesc.Any())
            {
                var predicateInner = PredicateBuilder.New<Majorhardwarebuildasis>();
                foreach (var item in request.BuildConstructionDesc)
                    predicateInner.Or(x => Convert.ToString(x.Buildconstruction.Buildconstructionid) == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Majorhardwarebuildasis>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Majorhardwarebuildasis>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        public async Task<ResultDto> FindWithCondition(MajorHardwareBuildAsIsQueryDto majorHwQuerydto)
        {
            try
            {
                var predicateResult = ApplyFilter(majorHwQuerydto);
                var rtn = new QueryResultDto<MajorHardwareBuildAsIsDtoGrid>(new GenerateRenderForGrid<MajorHardwareBuildAsIsDtoGrid>(_columnManager))
                {

                };
                var query = await Task.Run(() => GetMajorHardwareBuildAsIsEntities(predicateResult));
                rtn.TotalItems = query.Count();
                query = query.ApplyOrdering(majorHwQuerydto, GetColumnsMap()).ApplyPaging(majorHwQuerydto);
                var result = _mapper.Map<IEnumerable<MajorHardwareBuildAsIsDtoGrid>>(query);
                rtn.Items = result.ToArray();

                predicateResult = PredicateBuilder.New<Majorhardwarebuildasis>();
                var allItems = await Task.Run(() => GetMajorHardwareBuildAsIsEntities(predicateResult));

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
            catch(Exception ex)
            {
                return new ResultDto
                {
                    Warning =true,Info= ex.Message
                };
            }
            
        }

        public IQueryable<MajorHardwareBuildAsIs> GetMajorHardwareBuildAsIsEntities(ExpressionStarter<Majorhardwarebuildasis> predicateResult)
        {
            var query = _repositoryWrapper.MajorHardwareBuildAsIs.FindByCondition(predicateResult)
               .Include(m => m.Buildconstruction)
               .Include(m => m.Platform)
               .Include(m => m.Orgeqpmanufacturer)
               .Include(m => m.Hardwaresolutionreource)
               .Include(m => m.ModificationuserNavigation)
               .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => MajorHardwareBuildAsIsMapper.GetMajorHardwareBuildAsIsMapper(p)).AsQueryable();
        }

        public Dictionary<string, Expression<Func<MajorHardwareBuildAsIs, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<MajorHardwareBuildAsIs, object>>[]>
            {
                ["majorHardwareBuildAsIsId"] = new Expression<Func<MajorHardwareBuildAsIs, object>>[] { p => p.MajorHardwareBuildAsIsId },
                ["orgEqpmanuFacturerId"] = new Expression<Func<MajorHardwareBuildAsIs, object>>[] { p => p.OriginalEquipmentManufacturerId },
                ["platformId"] = new Expression<Func<MajorHardwareBuildAsIs, object>>[] { p => p.PlatformId },
                ["buildConstructionId"] = new Expression<Func<MajorHardwareBuildAsIs, object>>[] { p => p.BuildConstructionId },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, MajorHardwareBuildAsIsQueryDto request)
        {
            var predicateResult = ApplyFilter(request);

            var filteredQuery = await Task.Run(() => GetMajorHardwareBuildAsIsEntities(predicateResult));

            var result = propertyName switch
            {
                "majorHardwareBuildAsisId" => filteredQuery
                    .Select(x => new FilterValueDto(x.MajorHardwareBuildAsIsId))
                    .Distinct()
                    .ToList(),
                "orgEqpManuFacturerDesc" => filteredQuery
                    .Select(x => new FilterValueDto{Text=x.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription,Value=x.OriginalEquipmentManufacturerId.ToString()})
                    .Distinct()
                    .ToList(),
               
                "platformDesc" => filteredQuery
                    .Select(x => new FilterValueDto { Text = x.Platform.PlatformDescription, Value = x.PlatformId.ToString() })
                    .Distinct()
                    .ToList(),
                "buildConstructionDesc" => filteredQuery
                    .Select(x => new FilterValueDto { Text = x.BuildConstruction.BuildConstructionDescription, Value = x.BuildConstructionId.ToString() })
                    .Distinct()
                    .ToList(),
                "hardwareSolution" => filteredQuery
                    .Select(x => new FilterValueDto(x.HardwareSolution))
                    .Distinct()
                    .ToList(),
                "hardwareType" => filteredQuery
                    .Select(x => new FilterValueDto(x.HardwareType))
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

        public MajorHardwareBuildAsIsCreateDto GetCreatePage()
        {
            var createDto = new MajorHardwareBuildAsIsCreateDto();
            createDto.OrgEqpmanuFacturerResources = _dropdownManager.GetFilterValueVendorResource();
            createDto.PlatformResources = _dropdownManager.GetAllPlatorm();
            createDto.BuildConstructionResources = _dropdownManager.GetAllBuildConstruction();
            return createDto;
        }
        public async Task<MajorHardwareBuildAsIsCreateDto> GetUpdatedPage(long id)
        {
            var updateDto = new MajorHardwareBuildAsIsCreateDto();
            
            var updateEntityExists = _repositoryWrapper.MajorHardwareBuildAsIs.FindByCondition(x => x.Majorhardwarebuildasisid == id)
                .Include(x => x.ModificationuserNavigation)
                                     .FirstOrDefault();

            if (updateEntityExists != null)
            {
                updateDto.MajorHardwareBuildAsisId = updateEntityExists.Majorhardwarebuildasisid;
                updateDto.HardwareSolution = updateEntityExists.Hardwaresolution;
                updateDto.HardwareType = updateEntityExists.Hardwaretype;
                updateDto.BuildConstructionId = updateEntityExists.Buildconstructionid;
                updateDto.PlatformId = updateEntityExists.Platformid;
                updateDto.OrgEqpManuFacturerId = updateEntityExists.Orgeqpmanufacturerid;

                updateDto.LastModifiedBy = updateEntityExists.ModificationuserNavigation.Email;
                updateDto.LastModified = updateEntityExists.ModificationuserNavigation.Modificationdate;
            }
            updateDto.OrgEqpmanuFacturerResources = _dropdownManager.GetFilterValueVendorResource();
            updateDto.PlatformResources = _dropdownManager.GetAllPlatorm();
            updateDto.BuildConstructionResources =  _dropdownManager.GetAllBuildConstruction();
            return updateDto;
        }

        public async Task<ResultDto> Add(MajorHardwareBuildAsIsCreateDto dto)
        {
            var existingEntities = await _repositoryWrapper
                .MajorHardwareBuildAsIs
                .FindByCondition(
                x => x.Hardwaretype.ToLower().Replace(" ", "") == dto.HardwareType.ToLower().Replace(" ", "") && 
                     x.Orgeqpmanufacturerid==dto.OrgEqpManuFacturerId  &&
                     x.Platformid==dto.PlatformId
                     ).ToListAsync();

            if (existingEntities.Any())
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryAlreadyExists
                };
            }
            var newEntity = new Majorhardwarebuildasis
            {
                Hardwaretype = dto.HardwareType,
                Hardwaresolution = dto.HardwareSolution,
                Orgeqpmanufacturerid = (short)dto.OrgEqpManuFacturerId,
                Platformid = (short)dto.PlatformId,
                Buildconstructionid = (short)dto.BuildConstructionId,
            };

            _repositoryWrapper.MajorHardwareBuildAsIs.Create(newEntity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto { Warning = false, Info = ResultMessages.EntryAddSuccess };
        }
        public async Task<ResultDto> Update(MajorHardwareBuildAsIsCreateDto dto)
        {
            var existingEntities = await _repositoryWrapper
                .MajorHardwareBuildAsIs
                .FindByCondition(
                x => x.Hardwaretype.ToLower().Replace(" ", "") == dto.HardwareType.ToLower().Replace(" ", "") &&
                     x.Orgeqpmanufacturerid == dto.OrgEqpManuFacturerId &&
                     x.Platformid == dto.PlatformId
                     ).ToListAsync();

                if (existingEntities.Any(x => x.Majorhardwarebuildasisid != dto.MajorHardwareBuildAsisId))
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryAlreadyExists,
                        Data = dto.MajorHardwareBuildAsisId
                    };
                }
                var entityToUpdate = (existingEntities.Count >0) ? existingEntities
                    .FirstOrDefault(x => x.Majorhardwarebuildasisid == dto.MajorHardwareBuildAsisId) :
                    _repositoryWrapper.MajorHardwareBuildAsIs.FindByCondition(x => x.Majorhardwarebuildasisid == dto.MajorHardwareBuildAsisId).FirstOrDefault();

                if (entityToUpdate == null)
                {
                    return new ResultDto { Warning = true,  Info = ResultMessages.EntryNotFound };
                }
                entityToUpdate.Hardwaretype = dto.HardwareType;
                entityToUpdate.Hardwaresolution = dto.HardwareSolution;
                entityToUpdate.Orgeqpmanufacturerid = (short)dto.OrgEqpManuFacturerId;
                entityToUpdate.Platformid = (short)dto.PlatformId;
                entityToUpdate.Buildconstructionid = (short)dto.BuildConstructionId;

                _repositoryWrapper.MajorHardwareBuildAsIs.Update(entityToUpdate);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto { Warning = false, Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.MajorHardwareBuildAsIs.FindByCondition(x => x.Majorhardwarebuildasisid == id).SingleAsync();
            _repositoryWrapper.MajorHardwareBuildAsIs.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Majorhardwarebuildasisid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var entities = _repositoryWrapper.AssetHardwareAncillaryRepository.FindByCondition(x => x.Majorhardwarebuildasisid == id)
                .Include(x => x.Majorhardwarebuildasis).ThenInclude(x=>x.Platform)
                .Include(x => x.Majorhardwarebuildasis).ThenInclude(x => x.Orgeqpmanufacturer)
                .Select(x => x.Majorhardwarebuildasis.Orgeqpmanufacturer.Originalequipmentmanufacturer+" - " +x.Majorhardwarebuildasis.Hardwaretype + " - " + x.Majorhardwarebuildasis.Platform.Platform).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Asset Hardware Ancillary", Values = entities });

            var majorHwEntity = _repositoryWrapper.MajorHardwareBuildAsIs.FindByCondition(x => x.Majorhardwarebuildasisid == id)
                                .Include(x=>x.Platform)
                                .SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Major Hardware Build Asis",
                        RecordName = majorHwEntity.Result.Hardwaretype+ "-" +majorHwEntity.Result.Platform.Platform,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }
    }
}
