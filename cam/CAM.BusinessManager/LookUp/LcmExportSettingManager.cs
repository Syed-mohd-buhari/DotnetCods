using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects;
using CAM.Entities.Mappers.Lookup;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using CAM.DataTransferObjects.LookUp.LcmExportSetting;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using CAM.BusinessManager.Entity.Report;
using CAM.Enum;
using CAM.DataTransferObjects.Entita.Report;
using System.Text.Json;
using DocumentFormat.OpenXml.Office2010.Drawing;

namespace CAM.BusinessManager.LookUp
{
    public class LcmExportSettingManager : GridBaseAsync<LcmExportSetting, LcmExportSettingDtoGrid, LcmExportSettingDtoQuery, Lcmexportsettings>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private ReportHardwareManager _reportHardwareManager;
        private ReportSoftwareManager _reportSoftwareManager;
        public LcmExportSettingManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, ReportHardwareManager reportHardwareManager, ReportSoftwareManager reportSoftwareManager) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _reportHardwareManager = reportHardwareManager;
            _reportSoftwareManager = reportSoftwareManager;
        }


        public override ExpressionStarter<Lcmexportsettings> ApplyFilterForOracleModel(LcmExportSettingDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Lcmexportsettings>();
            var predicateInner = PredicateBuilder.New<Lcmexportsettings>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmexportsettings>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmexportsettings>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (request.IsHistorical != null && request.IsHistorical.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmexportsettings>();
                foreach (var item in request.IsHistorical)
                    predicateInner.Or(x => x.Ishistorical == item);
                predicateResult.And(predicateInner);
            }

            if (request.IsDefault != null && request.IsDefault.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmexportsettings>();
                foreach (var item in request.IsDefault)
                    predicateInner.Or(x => x.Isdefault == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmexportsettings>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Lcmexportsettings>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Lcmexportsettings>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<LcmExportSettingDtoGrid> CastObjectToDto(IQueryable<LcmExportSetting> request)
        {
            return request.Select(dto => new LcmExportSettingDtoGrid()
            {
                Id = (short)dto.Id,
                Description = dto.Description,
                IsHistorical = dto.IsHistorical,
                LastModified = dto.ModificationDate,
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                LastModifiedBy = dto.ModificationUserEntity.Email,
                IsDefault = dto.IsDefault               
                
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<LcmExportSetting, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<LcmExportSetting, object>>[]>
            {
                ["description"] = new Expression<Func<LcmExportSetting, object>>[] { p => p.Description },
                ["id"] = new Expression<Func<LcmExportSetting, object>>[] { p => p.Id },
                ["lastModifiedBy"] = new Expression<Func<LcmExportSetting, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<LcmExportSetting> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ?await Task.Run(()=> request.Select(x => new FilterValueDto(x.Description)))
                    : request.Where(x => x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.Id.ToString()))
                    : request.Where(x => x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),

                "isHistorical" => string.IsNullOrEmpty(propertyFilter)
           ? request.Where(x => x.IsHistorical != null)
               .Select(p => new FilterValueDto { Text = p.IsHistorical == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(),
                   Value = p.IsHistorical.ToString() }).Distinct().ToList()
           : request
               .Where(p => p.IsHistorical != null && (p.IsHistorical.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).
               Contains(propertyFilter))
               .Select(p => new FilterValueDto { Text = p.IsHistorical == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(), 
                   Value = p.IsHistorical.ToString() }).Distinct()
               .ToList(),

                "isDefault" => string.IsNullOrEmpty(propertyFilter) ? request.Where(x => x.IsDefault != null)
               .Select(p => new FilterValueDto
               {
                   Text = p.IsDefault == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(),
                   Value = p.IsDefault.ToString()
               }).Distinct().ToList()
           : request
               .Where(p => p.IsDefault != null && (p.IsDefault.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).
               Contains(propertyFilter))
               .Select(p => new FilterValueDto
               {
                   Text = p.IsDefault == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(),
                   Value = p.IsDefault.ToString()
               }).Distinct()
               .ToList(),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

                                _ => new List<FilterValueDto>(),
            };
        }

        public override IQueryable<LcmExportSetting> PrepareQuery(LcmExportSettingDtoQuery request, ExpressionStarter<LcmExportSetting> predicateResult, ExpressionStarter<Lcmexportsettings> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
     ? _repositoryWrapper.LcmExportSettingRepository.FindByCondition(oraclePredicateResult)
     : _repositoryWrapper.LcmExportSettingRepository.FindAll();

            var lcmExportEntities = query
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable()
                .Select(LcmExportSettingMapper.Get)
                .ToList();  
            
            return lcmExportEntities.AsQueryable();

        }

        public async Task<ResultDto> Add(LcmExportSettingDtoCreate dto)
        {
            ReportHardwareQueryDto hardwareDto = new ReportHardwareQueryDto();
            ReportSoftwareQueryDto softwareDto = new ReportSoftwareQueryDto();
            IList<ReportHardwareDtoGrid> hWReportEntity = new List<ReportHardwareDtoGrid>();
            IList<ReportSoftwareDtoGrid> swReportEntity = new List<ReportSoftwareDtoGrid>();

            var entityExists = await _repositoryWrapper.LcmExportSettingRepository.FindByCondition(
                x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            if (dto.IsHistorical == true)
            {
                if (dto.ReportType == (int)ReportType.Hardware)
                {
                    hardwareDto.ViewMode = ReportViewMode.Aggregated;
                    hWReportEntity = _reportHardwareManager.FindWithCondition(hardwareDto, true).Items;
                    dto.LcmHistoricalInfoHW = JsonSerializer.Serialize(hWReportEntity);
                }
                else if(dto.ReportType == (int)ReportType.Software)
                {
                    softwareDto.ViewMode = ReportViewMode.Aggregated;
                    swReportEntity = _reportSoftwareManager.FindWithCondition(softwareDto, true).Items;
                    dto.LcmHistoricalInfoSW = JsonSerializer.Serialize(swReportEntity);
                }
                else
                {
                    softwareDto.ViewMode = ReportViewMode.Aggregated;
                    hardwareDto.ViewMode = ReportViewMode.Aggregated;

                    hWReportEntity = _reportHardwareManager.FindWithCondition(hardwareDto, true).Items;
                    dto.LcmHistoricalInfoHW = JsonSerializer.Serialize(hWReportEntity);

                    swReportEntity = _reportSoftwareManager.FindWithCondition(softwareDto, true).Items;
                    dto.LcmHistoricalInfoSW = JsonSerializer.Serialize(swReportEntity);
                }
            }
            /// Ticket 1367- Default Selection of LCM Label on LCM Export to be fixed
            if (dto.IsDefault == true)
            {
                await ResetDefaultsForEntity();
            }

            LcmExportSetting entity = new LcmExportSetting()
            {
                Id = dto.Id,
                Description = dto.Description,
                LcmHistoricalInfoSW = dto.LcmHistoricalInfoSW,
                LcmHistoricalInfoHW = dto.LcmHistoricalInfoHW,
                IsHistorical = dto.IsHistorical,
                ReportLevel = dto.IsHistorical == true ?(int)ReportViewMode.Aggregated:0,
                IsDefault = dto.IsDefault
            };

            _repositoryWrapper.LcmExportSettingRepository.Create(LcmExportSettingMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(LcmExportSettingDtoUpdate dto)
        {
            ReportHardwareQueryDto hardwareDto = new ReportHardwareQueryDto();
            ReportSoftwareQueryDto softwareDto = new ReportSoftwareQueryDto();
            IList<ReportHardwareDtoGrid> hWReportEntity = new List<ReportHardwareDtoGrid>();
            IList<ReportSoftwareDtoGrid> swReportEntity = new List<ReportSoftwareDtoGrid>();

            var entityExists = await _repositoryWrapper.LcmExportSettingRepository.FindByCondition(
                x => x.Id != dto.Id && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            if (dto.IsHistorical == true)
            {
                if (dto.ReportType == (int)ReportType.Hardware)
                {
                    hardwareDto.ViewMode = ReportViewMode.Aggregated;
                    hWReportEntity = _reportHardwareManager.FindWithCondition(hardwareDto, true).Items;
                    dto.LcmHistoricalInfoHW = JsonSerializer.Serialize(hWReportEntity);
                }
                else if (dto.ReportType == (int)ReportType.Software)
                {
                    softwareDto.ViewMode = ReportViewMode.Aggregated;
                    swReportEntity = _reportSoftwareManager.FindWithCondition(softwareDto, true).Items;
                    dto.LcmHistoricalInfoSW = JsonSerializer.Serialize(swReportEntity);
                }
                else
                {
                    softwareDto.ViewMode = ReportViewMode.Aggregated;
                    hardwareDto.ViewMode = ReportViewMode.Aggregated;

                    hWReportEntity = _reportHardwareManager.FindWithCondition(hardwareDto, true).Items;
                    dto.LcmHistoricalInfoHW = JsonSerializer.Serialize(hWReportEntity);
                    swReportEntity = _reportSoftwareManager.FindWithCondition(softwareDto, true).Items;
                    dto.LcmHistoricalInfoSW = JsonSerializer.Serialize(swReportEntity);
                }
            }
            /// Ticket 1367- Default Selection of LCM Label on LCM Export to be fixed
            if (dto.IsDefault == true)
            {
                await ResetDefaultsForEntity();
            }

            LcmExportSetting entity = new LcmExportSetting()
            {
                Id = dto.Id,
                Description = dto.Description,
                LcmHistoricalInfoSW = dto.LcmHistoricalInfoSW,
                LcmHistoricalInfoHW = dto.LcmHistoricalInfoHW,
                IsHistorical = dto.IsHistorical,
                ReportLevel = dto.IsHistorical == true? (int)ReportViewMode.Aggregated : 0,
                IsDefault = dto.IsDefault
            };

            _repositoryWrapper.LcmExportSettingRepository.Update(LcmExportSettingMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.LcmExportSettingRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.LcmExportSettingRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.LcmExportSettingRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.LcmExportSettingRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            return await Task.FromResult(new ResultDto());
        }

        public LcmExportSettingDtoCreate GetCreatePage()
        {
            return new LcmExportSettingDtoCreate();
        }

        public LcmExportSettingDtoUpdate GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.LcmExportSettingRepository.FindByCondition(x => x.Id == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = LcmExportSettingMapper.Get(model);
            return new LcmExportSettingDtoUpdate()
            {
                Id = (short)entity.Id,
                IsHistorical = entity.IsHistorical,
                Description = entity.Description,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email,
                IsDefault = entity.IsDefault

            };
        }

       /// Ticket 1367- Default Selection of LCM Label on LCM Export to be fixed <summary>
      
       /// </summary>
       /// <returns></returns>
       public async Task<bool> ResetDefaultsForEntity()
        { 
                var getPreviousDefaultLcmReport = _repositoryWrapper.LcmExportSettingRepository.FindByCondition(x => x.Isdefault == true).ToList();
                foreach (var item in getPreviousDefaultLcmReport)
                {
                    item.Isdefault = false;
                    _repositoryWrapper.LcmExportSettingRepository.Update(item);

                }
                 await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();

            return true;
 
        }

    }
}

