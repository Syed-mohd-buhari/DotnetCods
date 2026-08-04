using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.DeploymentStatus;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsPlanned;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.LookUp
{
    public class DeploymentStatusManager : GridBaseAsync<DeploymentStatus, DeploymentStatusDtoGrid, DeploymentStatusQuery, Deploymentstatuses>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;

        public DeploymentStatusManager(IHttpContextAccessor contextAccessor, IMapper mapper, GridCustomColumnManager columnManager,
            IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
        }

     

        public override ExpressionStarter<Deploymentstatuses> ApplyFilterForOracleModel(DeploymentStatusQuery request)
        {
            var predicateResult = PredicateBuilder.New<Deploymentstatuses>();
            var predicateInner = PredicateBuilder.New<Deploymentstatuses>();

            if (request.DeploymentStatusDescription != null && request.DeploymentStatusDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymentstatuses>();
                foreach (var item in request.DeploymentStatusDescription)
                    predicateInner.Or(x => x.Deploymentstatus == item);
                predicateResult.And(predicateInner);
            }

            if (request.DefaultValue != null && request.DefaultValue.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymentstatuses>();
                foreach (var item in request.DefaultValue)
                    predicateInner.Or(x => x.Defaultvalue == item);
                predicateResult.And(predicateInner);
            }

            if (request.DeploymentStatusId != null && request.DeploymentStatusId.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymentstatuses>();
                foreach (var item in request.DeploymentStatusId)
                    predicateInner.Or(x => x.Deploymentstatusid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Rule != null && request.Rule.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymentstatuses>();
                foreach (var item in request.Rule)
                    predicateInner.Or(x => x.Rule == item);
                predicateResult.And(predicateInner);
            }

            if (request.CheckPlannedActivity != null && request.CheckPlannedActivity.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymentstatuses>();
                foreach (var item in request.CheckPlannedActivity)
                    predicateInner.Or(x => x.Checkplannedactivity == item);
                predicateResult.And(predicateInner);
            }
            if (request.PlannedActivityResourceAllowed != null && request.PlannedActivityResourceAllowed.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymentstatuses>();
                foreach (var item in request.PlannedActivityResourceAllowed)
                    predicateInner.Or(x => x.Plannedactivityresourceallowed == item);
                predicateResult.And(predicateInner);
            }
            if (request.ReadOnlyPlannedActivity != null && request.ReadOnlyPlannedActivity.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymentstatuses>();
                foreach (var item in request.ReadOnlyPlannedActivity)
                    predicateInner.Or(x => x.Readonlyplannedactivity == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymentstatuses>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Deploymentstatuses>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Deploymentstatuses>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<DeploymentStatusDtoGrid> CastObjectToDto(IQueryable<DeploymentStatus> request)
        {
            return  request.Select(dto => new DeploymentStatusDtoGrid()
            {
                DeploymentStatusId = dto.DeploymentStatusId,
                DeploymentStatusDescription = dto.DeploymentStatusDescription,
                Rule = (int)dto.Rule,
                PlannedActivityResourceAllowed = dto.toPlannedActivityResourceAllowedDescriptions(_repositoryWrapper),
                PlannedActivityResourceAllowedId = dto.toPlannedActivityResourceKeyList(),
                ReadOnlyPlannedActivity = dto.ReadOnlyPlannedActivity,
                CheckPlannedActivity = dto.CheckPlannedActivity,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                DefaultValue = dto.DefaultValue
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<DeploymentStatus, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<DeploymentStatus, object>>[]>
            {
                ["deploymentStatusDescription"] = new Expression<Func<DeploymentStatus, object>>[] { p => p.DeploymentStatusDescription },
                ["deploymentStatusId"] = new Expression<Func<DeploymentStatus, object>>[] { p => p.DeploymentStatusId },
                ["rule"] = new Expression<Func<DeploymentStatus, object>>[] { p => p.Rule },
                ["plannedActivityResourceAllowed"] = new Expression<Func<DeploymentStatus, object>>[] { p => p.toPlannedActivityResourceAllowedDescriptions(_repositoryWrapper) },
                ["plannedActivityResourceAllowedId"] = new Expression<Func<DeploymentStatus, object>>[] { p => p.toPlannedActivityResourceKeyList() },
                ["readOnlyPlannedActivity"] = new Expression<Func<DeploymentStatus, object>>[] { p => p.ReadOnlyPlannedActivity },
                ["checkPlannedActivity"] = new Expression<Func<DeploymentStatus, object>>[] { p => p.CheckPlannedActivity },
                ["defaultValue"] = new Expression<Func<DeploymentStatus, object>>[] { p => p.DefaultValue },
                ["lastModifiedBy"] = new Expression<Func<DeploymentStatus, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<DeploymentStatus> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "deploymentStatusDescription" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.DeploymentStatusDescription)) : request.Where(x =>
                    x.DeploymentStatusDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.DeploymentStatusDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "rule" => string.IsNullOrEmpty(propertyFilter)
                    ? request
                        .Select(p => new FilterValueDto { Text = p.Rule == 1 ? "YES" : "NO", Value = p.Rule.ToString() }).Distinct()
                    : request
                        .Where(x => x.Rule == 0)
                        .Select(p => new FilterValueDto { Text = p.Rule == 1 ? "YES" : "NO", Value = p.Rule.ToString() }).Distinct(),
                "plannedActivityResourceAllowed" => string.IsNullOrEmpty(propertyFilter)
                    ? request
                        .ToList()
                        .Select(x => new FilterValueDto
                        {
                            Text = x.toPlannedActivityResourceAllowedDescriptions(_repositoryWrapper),
                            Value = x.PlannedActivityResourceAllowed
                        })
                    : request.ToList().Where(x => x.toPlannedActivityResourceAllowedDescriptions(_repositoryWrapper).ToUpper().Contains(propertyFilter.ToUpper()))
                        .Select(x => new FilterValueDto
                        {
                            Text = x.toPlannedActivityResourceAllowedDescriptions(_repositoryWrapper),
                            Value = x.PlannedActivityResourceAllowed
                        }),
                "readOnlyPlannedActivity" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto { Text = p.ReadOnlyPlannedActivity == true ? "YES" : "NO", Value = p.ReadOnlyPlannedActivity.ToString() }).Distinct()
                    : request.Where(x => x.ReadOnlyPlannedActivity == false).Select(p => new FilterValueDto { Text = p.ReadOnlyPlannedActivity == true ? "YES" : "NO", Value = p.ReadOnlyPlannedActivity.ToString() }).Distinct(),
                "defaultValue" => string.IsNullOrEmpty(propertyFilter)
                     ? request.Select(p => new FilterValueDto { Text = p.DefaultValue == true ? "YES" : "NO", Value = p.DefaultValue.ToString() }).Distinct()
                     : request.Where(x => x.DefaultValue == false).Select(p => new FilterValueDto { Text = p.DefaultValue == true ? "YES" : "NO", Value = p.DefaultValue.ToString() }).Distinct(),
                "checkPlannedActivity" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto { Text = p.CheckPlannedActivity == true ? "YES" : "NO", Value = p.CheckPlannedActivity.ToString() }).Distinct()
                    : request.Where(x => x.CheckPlannedActivity == false).Select(p => new FilterValueDto { Text = p.CheckPlannedActivity == true ? "YES" : "NO", Value = p.CheckPlannedActivity.ToString() }).Distinct(),
                "deploymentStatusId" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.DeploymentStatusId.ToString()))
                    : request.Where(x =>
                        x.DeploymentStatusId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.DeploymentStatusId.ToString())),

            };
        }

        public override IQueryable<DeploymentStatus> PrepareQuery(DeploymentStatusQuery request, ExpressionStarter<DeploymentStatus> predicateResult, ExpressionStarter<Deploymentstatuses> oraclepPredicateResult = null)
        {
            var query = oraclepPredicateResult.IsStarted
                ? _repositoryWrapper.DeploymentStatus.FindByCondition(oraclepPredicateResult)
                : _repositoryWrapper.DeploymentStatus.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => DeploymentStatusMapper.GetDeploymentStatusMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(DeploymentStatusDto dto)
        {
            var entityExists = await _repositoryWrapper.DeploymentStatus.FindByCondition(
               x => x.Deploymentstatus.ToLower().Replace(" ","")  == dto.DeploymentStatusDescription.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Deploymentstatusid
                };
            }

            if (dto.DefaultValue)
            {
                await SetUndefoult();
            }
            DeploymentStatus entity = new DeploymentStatus()
            {
                DeploymentStatusId = dto.DeploymentStatusId,
                DeploymentStatusDescription = dto.DeploymentStatusDescription,
                PlannedActivityResourceAllowed = dto.toPlannedActivityResourceAllowedJoinedKeyList(),
                ReadOnlyPlannedActivity = dto.ReadOnlyPlannedActivity,
                CheckPlannedActivity = dto.CheckPlannedActivity,
                Rule = dto.Rule,
                DefaultValue = dto.DefaultValue
            };
            _repositoryWrapper.DeploymentStatus.Create(DeploymentStatusMapper.SetDeploymentStatusMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }
        private async Task SetUndefoult(short? notIncludeedId = null)
        {
            var deploymentStatuses = await _repositoryWrapper.DeploymentStatus
                                            .FindByCondition(x =>notIncludeedId == null ||  x.Deploymentstatusid != notIncludeedId)
                                            .ToListAsync();
            foreach (var deploymentStatus in deploymentStatuses)
            {
                if(deploymentStatus.Defaultvalue)
                {
                    deploymentStatus.Defaultvalue = false;
                    _repositoryWrapper.DeploymentStatus.Update(deploymentStatus);
                }
            }

            await _repositoryWrapper.SaveAsync();
        }


        public async Task<ResultDto> Update(DeploymentStatusDto dto)
        {

            if (dto.DefaultValue)
            {
                await SetUndefoult(dto.DeploymentStatusId);
            }


            DeploymentStatus entity = new DeploymentStatus()
            {
                DeploymentStatusId = dto.DeploymentStatusId,
                DeploymentStatusDescription = dto.DeploymentStatusDescription,
                PlannedActivityResourceAllowed = dto.toPlannedActivityResourceAllowedJoinedKeyList(),
                ReadOnlyPlannedActivity = dto.ReadOnlyPlannedActivity,
                CheckPlannedActivity = dto.CheckPlannedActivity,
                Rule = dto.Rule,
                DefaultValue = dto.DefaultValue
            };
            _repositoryWrapper.DeploymentStatus.Update(DeploymentStatusMapper.SetDeploymentStatusMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatusid == id).SingleAsync();
            _repositoryWrapper.DeploymentStatus.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Deploymentstatusid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatusid == id).SingleAsync();
            _repositoryWrapper.DeploymentStatus.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Deploymentstatusid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO: verificare descrizione da visualizzare
            var entities = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Deploymentstatusid == id)
                .Select(x => NetworkElementAsPlannedMapper.Get(x,true).toDescription()).ToArray();
            var entity = await _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatusid == id)
            .SingleAsync();

            var relatedSettingsUpdatePlannedActivity = _repositoryWrapper.SettingUpdatePlannedActivityAssetDeploymentStatusRepository
               .FindByCondition(x => x.Assetdeploymentstatusid == id).Select(x => x.Assetdeploymentstatusid).Any() ?
               _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Settingupdateplannedactivityassetdeploymentstatus
               .Any(f => f.Assetdeploymentstatusid == id)).AsEnumerable().Select(x => x.Settingsupdateplnactdes).ToList() : null;


            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Planned", Values = entities });

            if (relatedSettingsUpdatePlannedActivity != null && relatedSettingsUpdatePlannedActivity.ToArray().Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Related Settings Update Planned Activity", Values = relatedSettingsUpdatePlannedActivity.ToArray() });


            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Deployment Status",
                        RecordName = entity.Deploymentstatus,
                        DataRelatedList = rm
                    }
                };
            }
            else
            {
                return new ResultDto();
            }
        }

        public DeploymentStatusDto GetCreatePage()
        {
            var DeploymentStatusDto = new DeploymentStatusDto()
            {
                PlannedActivityResourceAllowedId = new List<short>(),
                PlannedActivityResources = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Foraddasset == true || x.Foreditasset == true).ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource)
            };
            return DeploymentStatusDto;
        }

        public DeploymentStatusDto GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatusid == id)
                .Include(x => x.ModificationuserNavigation).Single();

            var entity = DeploymentStatusMapper.GetDeploymentStatusMapper(model);
            var dto = new DeploymentStatusDto()
            {
                DeploymentStatusId = entity.DeploymentStatusId,
                DeploymentStatusDescription = entity.DeploymentStatusDescription,
                LastModifiedBy = entity.ModificationUserEntity.Email,
                LastModified = entity.ModificationDate,
                Rule = entity.Rule != null ? (int)entity.Rule : 0,
                CheckPlannedActivity = entity.CheckPlannedActivity,
                ReadOnlyPlannedActivity = entity.ReadOnlyPlannedActivity,
                PlannedActivityResourceAllowedId = entity.toPlannedActivityResourceKeyList(),
                PlannedActivityResources = _repositoryWrapper.PlannedActivityResourceRepository
                                            .FindByCondition(x => x.Foraddasset == true || x.Foreditasset == true)
                                            .ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource),
                DefaultValue = entity.DefaultValue,
            };
            return dto;
        }

      
    }
}
