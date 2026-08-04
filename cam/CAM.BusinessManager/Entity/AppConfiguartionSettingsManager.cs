using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AppConfigurationSettings;
using CAM.DataTransferObjects.Entita.AppSettingsConfiguration;
using CAM.DataTransferObjects.Entita.AuditLog;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
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

namespace CAM.BusinessManager.Entity
{
    public class AppSettingsConfiguartionManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public AppSettingsConfiguartionManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<AppSettingsConfigurationDto> FindWithCondition(AppSettingsConfiguartionQueryDto appConfiguartionSettingsQueryDto)
        {
            var predicateResult = ApplyFilter(appConfiguartionSettingsQueryDto);
            var rtn = new QueryResultDto<AppSettingsConfigurationDto>(new GenerateRenderForGrid<AppSettingsConfigurationDto>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.AppConfigurationSettingsRepository.Count(predicateResult) : _repositoryWrapper.AppConfigurationSettingsRepository.Count(),
            };
            var query = GetQuery(predicateResult, appConfiguartionSettingsQueryDto.Deleted ?? false).ApplyOrdering(appConfiguartionSettingsQueryDto, GetColumnsMap()).ApplyPaging(appConfiguartionSettingsQueryDto);
            var data = query.ToList();
                     
            IEnumerable <AppSettingsConfigurationDto> appConfigurationSettingsDto;

            appConfigurationSettingsDto = _mapper.Map<IEnumerable<AppSettingsConfigurationDto>>(data);

            rtn.Items = appConfigurationSettingsDto.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Appsettingsconfiguration> ApplyFilter(AppSettingsConfiguartionQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Appsettingsconfiguration>();
            var predicateInner = PredicateBuilder.New<Appsettingsconfiguration>();

            if (buildFilterDto.AppSettingsConfiguartionId != null && buildFilterDto.AppSettingsConfiguartionId.Any())
            {
                predicateInner = PredicateBuilder.New<Appsettingsconfiguration>();
                foreach (var item in buildFilterDto.AppSettingsConfiguartionId)
                    predicateInner.Or(x => x.Appconfigurationsettingid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AppSettingsId != null && buildFilterDto.AppSettingsId.Any())
            {
                predicateInner = PredicateBuilder.New<Appsettingsconfiguration>();
                foreach (var item in buildFilterDto.AppSettingsId)
                    predicateInner.Or(x => x.Appsettingid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SettingsValue != null && buildFilterDto.SettingsValue.Any())
            {
                predicateInner = PredicateBuilder.New<Appsettingsconfiguration>();
                foreach (var item in buildFilterDto.SettingsValue)
                    predicateInner.Or(x => x.Settingsvalue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Appsettingsconfiguration>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Appsettingsconfiguration>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<AppSettingsConfiguration> GetQuery(ExpressionStarter<Appsettingsconfiguration> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.AppConfigurationSettingsRepository.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
               : _repositoryWrapper.AppConfigurationSettingsRepository.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => AppConfigurationSeetingsMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<AppSettingsConfiguration, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AppSettingsConfiguration, object>>[]>
            {
                ["appSettingsConfigurationId"] = new Expression<Func<AppSettingsConfiguration, object>>[] { p => p.AppSettingsConfigurationId },
                ["appSettingsId"] = new Expression<Func<AppSettingsConfiguration, object>>[] { p => p.AppSettingsId },
                ["settingsValue"] = new Expression<Func<AppSettingsConfiguration, object>>[] { p => p.SettingsValue },
                ["lastModifiedValue"] = new Expression<Func<AppSettingsConfiguration, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<AppSettingsConfiguration, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationUser"] = new Expression<Func<AppSettingsConfiguration, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<AppSettingsConfiguration, object>>[] { p => p.CreationDate },
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, AppSettingsConfiguartionQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "appSettingsConfigurationId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.AppSettingsConfigurationId.ToString(), Value = p.AppSettingsConfigurationId.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.AppSettingsConfigurationId.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.AppSettingsConfigurationId.ToString(), Value = p.AppSettingsConfigurationId.ToString() }).Distinct()
                   .ToList(),

                "appSettingsId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.AppSettingsId.ToString(), Value = p.AppSettingsId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.AppSettingsId.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.AppSettingsId.ToString(), Value = p.AppSettingsId.ToString() }).Distinct()
                        .ToList(),
                "settingsValue" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.SettingsValue, Value = p.SettingsValue }).Distinct().ToList()
                    : query
                        .Where(x => x.SettingsValue.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.SettingsValue, Value = p.SettingsValue }).Distinct()
                        .ToList(),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList(),


                "lastModified" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.ModificationDate.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList(),


                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        #endregion

        #region CRUD
        public AppSettingsConfigurationCreateDto GetCreatePage()
        {
            return new AppSettingsConfigurationCreateDto()
            {
                AppSettingsId = 2,
            };
        }
        public AppSettingsConfigurationUpdateDto GetUpdatePage(long id)
        {
            var appSettings = _repositoryWrapper.AppConfigurationSettingsRepository.FindByCondition(x => x.Appconfigurationsettingid == id).Include(x => x.ModificationuserNavigation).FirstOrDefault();
            AppSettingsConfigurationUpdateDto dto = new AppSettingsConfigurationUpdateDto();
            if (appSettings != null)
            {
                dto.AppSettingsConfigurationId = appSettings.Appconfigurationsettingid;
                dto.AppSettingsId = appSettings.Appsettingid;
                dto.SettingsValue = appSettings.Settingsvalue;
                dto.LastModified = appSettings.Modificationdate;
                dto.LastModifiedBy = appSettings.ModificationuserNavigation.Email;
            }
            return dto;
        }
        public async Task<ResultDto> Add(AppSettingsConfigurationCreateDto dto)
        {
            try
            {
                var entity = new AppSettingsConfiguration() 
                { 
                    AppSettingsConfigurationId = dto.AppSettingsConfigurationId, 
                    AppSettingsId = dto.AppSettingsId , 
                    SettingsValue = dto.SettingsValue
                };
                _repositoryWrapper.AppConfigurationSettingsRepository.Create(AppConfigurationSeetingsMapper.Set(entity));
                await _repositoryWrapper.SaveAsync();
                return new ResultDto { Info = ResultMessages.EntryAddSuccess };
            }
            catch (Exception ex)
            {
                return new ResultDto {Warning = true, Info = ResultMessages.EntryAddExists };
            }


        }
        public async Task<ResultDto> Update(AppSettingsConfigurationUpdateDto dto)
        {
            var AppConfigurationSettingsModel = _repositoryWrapper.AppConfigurationSettingsRepository.FindByCondition(x => x.Appconfigurationsettingid == dto.AppSettingsConfigurationId).FirstOrDefault();
            var AppConfigurationSettingsEntity = AppConfigurationSeetingsMapper.Get(AppConfigurationSettingsModel);

            if (AppConfigurationSettingsEntity != null)
            {
                AppConfigurationSettingsEntity.AppSettingsId = dto.AppSettingsId;
                AppConfigurationSettingsEntity.SettingsValue = dto.SettingsValue;
                AppConfigurationSettingsEntity.Deleted = false;

                var AppConfigurationSettingsSetEntity = AppConfigurationSeetingsMapper.Set(AppConfigurationSettingsEntity);
                _repositoryWrapper.AppConfigurationSettingsRepository.Update(AppConfigurationSettingsSetEntity);
                await _repositoryWrapper.SaveAsync();
            }


            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
                Data = AppConfigurationSettingsEntity.AppSettingsConfigurationId
            };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.AppConfigurationSettingsRepository
                .FindByCondition(x => x.Appconfigurationsettingid == id).SingleAsync();

            if (entity != null)
            {
                _repositoryWrapper.AppConfigurationSettingsRepository.Delete(entity);
                await _repositoryWrapper.SaveAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Appconfigurationsettingid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotExists,
                    Data = id
                };
            }
        }
        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.AppConfigurationSettingsRepository
               .FindByConditionWithDelete(x => x.Appconfigurationsettingid == id).SingleAsync();
            if (entity != null)
            {
                _repositoryWrapper.AppConfigurationSettingsRepository.DeleteDeep(entity);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Appconfigurationsettingid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotExists,
                    Data = id
                };
            }
        }
        #endregion

    }
}
