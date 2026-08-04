using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.LookUp.SubNetworkBoundary;
using CAM.DataTransferObjects.LookUp.VodafoneName;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using System.Transactions;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using System.Collections.Immutable;
using DocumentFormat.OpenXml.Office2010.Excel;
using AutoMapper.Configuration.Annotations;
using CAM.Entities.Models;
using IdentityServer4.Extensions;
using System.Globalization;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.Spreadsheet;
using CAM.DataTransferObjects.LookUp.LcmDeploymentStatus;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.MapConfiguration;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;

namespace CAM.BusinessManager.LookUp
{
    public class LcmDeploymentStatusManager : BaseManager, ILcmDeploymentStatusManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;

        public LcmDeploymentStatusManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IMapper mapper, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
        }
        public async Task<ResultDto> Add(LcmDeploymentStatusDto dto)
        {
            var entityExists = new Lcmdeploymentstatus();
            try
            {
                entityExists = await _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(
                  x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
                  , true).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted != null ? ( entityExists.Deleted.Value ?  ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists):"",
                    Data = entityExists.Id
                };
            }

            LCMDeploymentStatus entity = new LCMDeploymentStatus()
            {
                Id = dto.Id,
                Description = dto.Description,
            };
            var lcmDeploymentStatus = LCMDeploymentStatusMapper.SetLCMDeploymentStatusMapper(entity);

            _repositoryWrapper.LcmDeploymentStatusRepository.Create(lcmDeploymentStatus);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }
        public async Task<ResultDto> Update(LcmDeploymentStatusDto dto)
        {
            var entityExists = await _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(
                   x => x.Id != dto.Id
                   && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted != null? (entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists):"",
                    Data = entityExists.Id
                };
            }
            LCMDeploymentStatus entity = new LCMDeploymentStatus()
            {
                Id = dto.Id,
                Description = dto.Description,
            };
            var updatedLcmDeploymentStatus = LCMDeploymentStatusMapper.SetLCMDeploymentStatusMapper(entity);
            _repositoryWrapper.LcmDeploymentStatusRepository.Update(updatedLcmDeploymentStatus);
            _repositoryWrapper.Save();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public ExpressionStarter<Lcmdeploymentstatus> ApplyFilterForOracleModel(LcmDeploymentStatusDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Lcmdeploymentstatus>();
            var predicateInner = PredicateBuilder.New<Lcmdeploymentstatus>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmdeploymentstatus>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmdeploymentstatus>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmdeploymentstatus>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Lcmdeploymentstatus>();
                if (request.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModifiedValue.StartDate);
                if (request.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Lcmdeploymentstatus>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        public List<LcmDeploymentStatusDtoGrid> CastObjectToDto(IQueryable<LCMDeploymentStatus> request)
        {
            var model = request.ToList().Select(dto => new LcmDeploymentStatusDtoGrid()
            {
                Id = dto.Id,
                Description = dto.Description,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                Deleted = dto.Deleted,
                Orphan = !_repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Id == dto.Id).SelectMany(x => x.Lcmengineering).Any(),
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            }).ToList();
            return model;
        }
        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.LcmDeploymentStatusRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.LcmDeploymentStatusRepository
            .FindByCondition(x => x.Id == id)
            .SingleAsync();

            var relatedLcms = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmdeploymentstatusid == id).ToList();
            foreach (var item in relatedLcms)
            {
                item.Lcmdeploymentstatusid = null;
                _repositoryWrapper.Lcmengineering.Update(item);
                await _repositoryWrapper.SaveAsync();
            }
            _repositoryWrapper.LcmDeploymentStatusRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public Dictionary<string, Expression<Func<LCMDeploymentStatus, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<LCMDeploymentStatus, object>>[]>
            {
                ["Description"] = new Expression<Func<LCMDeploymentStatus, object>>[] { p => p.Description },
                ["Id"] = new Expression<Func<LCMDeploymentStatus, object>>[] { p => p.Id },
                ["lastModifiedBy"] = new Expression<Func<LCMDeploymentStatus, object>>[] { p => p.ModificationUserEntity.Email },
                ["lcmEngineerings"] = new Expression<Func<LCMDeploymentStatus, object>>[] { p => p.Lcmengineerings },
            };
        }

        public QueryResultDto<LcmDeploymentStatusDtoGrid> GetEnityGrid(LcmDeploymentStatusDtoQuery request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            if (request.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            if (request.Orphan == true)
            {
                predicateResult.And(x => !x.Lcmengineering.Any());
            }

            var query = PrepareQuery(request, null, predicateResult);

            int numberOfElements = query.Count();
            query = query.ToList().AsQueryable().ApplyOrdering(request, GetColumnsMap()).ApplyPaging(request);
            var dataResult = CastObjectToDto(query);
            return new QueryResultDto<LcmDeploymentStatusDtoGrid>(new GenerateRenderForGrid<LcmDeploymentStatusDtoGrid>(_columnManager))
            {
                Items = dataResult,
                TotalItems = numberOfElements,
            };
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, LcmDeploymentStatusDtoQuery request)
        {
            request.PageSize = 0;
            request.Page = 1;
            var predicateResult = ApplyFilterForOracleModel(request);

            var query = PrepareQuery(request, null, predicateResult);

            var data = (await GetFilterValueList(query, propertyName, propertyFilter)).Distinct().ToList();
            return data;
        }

        public async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<LCMDeploymentStatus> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Description)) : request.Where(x =>
                   x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                        : request
                            .Where(x =>
                                x.ModificationUserEntity.Email.Contains(
                                    propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

                "id" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(x => new FilterValueDto(x.Id.ToString()))
                        : request.Where(x =>
                            x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),

                //"softwareApplicationTypes" => string.IsNullOrEmpty(propertyFilter)
                //        ? request.Where(p => p.SoftwareApplicationTypes != null && p.SoftwareApplicationTypes.Count > 0)
                //        .SelectMany(x => x.SoftwareApplicationTypes).Select(p => new FilterValueDto(p.Id.ToString(), p.Description)).Distinct().ToList()
                //        : request.Where(x => x.SoftwareApplicationTypes != null && x.SoftwareApplicationTypes.Any(s => s.Description.ToUpper()
                //        .Contains(propertyFilter.ToUpper())))
                //        .SelectMany(x => x.SoftwareApplicationTypes)
                //        .Select(p => new FilterValueDto(p.Id.ToString(), p.Description)).Distinct()
                //        .ToList(),
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {

            var realtedLcms = _repositoryWrapper.LcmDeploymentStatusRepository
               .FindByCondition(x => x.Id == id).Include(x => x.Lcmengineering)
               .SelectMany(x => x.Lcmengineering).ToList();

            var relatedSettingsUpdatePlannedActivity = _repositoryWrapper.SettingUpdatePlannedActivityLcmDeploymentStatusRepository
                .FindByCondition(x => x.Lcmdeploymentstatusid == id).Select(x => x.Lcmdeploymentstatusid).Any() ?
                _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Settingupdateplannedactivitylcmdeploymentstatus
                .Any(f=>f.Lcmdeploymentstatusid == id)).AsEnumerable().Select(x=>x.Settingsupdateplnactdes).ToList():null;



            Designcomponents designcomponent;
            List<string> lcmEngineeringRelated = new List<string>();
            foreach (var item in realtedLcms)
            {
                designcomponent = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == item.Designcomponentid).FirstOrDefault();
                lcmEngineeringRelated.Add(CAM.Entities.Mappers.Entity.DesignComponentMapper
               .GetDesignComponentMapper(designcomponent, false).toDesignComponentNameLcmFromDCModel(_repositoryWrapper));
            }



            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            if (lcmEngineeringRelated != null && lcmEngineeringRelated.ToArray().Length > 0)
                rm.Add(new ResultMessageDto() { Table = "LCMs Related", Values = lcmEngineeringRelated.ToArray() });
           if (relatedSettingsUpdatePlannedActivity != null && relatedSettingsUpdatePlannedActivity.ToArray().Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Related Settings Update Planned Activity", Values = relatedSettingsUpdatePlannedActivity.ToArray() });

            var entity = await _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Id == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "LCM Deployment Status",
                        RecordName = entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public IQueryable<LCMDeploymentStatus> PrepareQuery(LcmDeploymentStatusDtoQuery request, ExpressionStarter<LCMDeploymentStatus> predicateResult, ExpressionStarter<Lcmdeploymentstatus> oracleObject = null)
        {
            var query = oracleObject.IsStarted
               ? _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(oracleObject)
               .Include(x => x.Lcmengineering)
               : _repositoryWrapper.LcmDeploymentStatusRepository.FindAll();

            return query
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .Include(x => x.Lcmengineering)
                .AsEnumerable().Select(p => LCMDeploymentStatusMapper.GetLCMDeploymentStatusMapper(p)).AsQueryable();
        }

        LcmDeploymentStatusDtoGrid ILcmDeploymentStatusManager.GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Id == id)
                        .Include(x => x.ModificationuserNavigation)
                        .Single();

            var entity = LCMDeploymentStatusMapper.GetLCMDeploymentStatusMapper(model);

            var dto = new LcmDeploymentStatusDtoUpdate()
            {
                Id = entity.Id,
                Description = entity.Description,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email.ToString(),
            };
            return dto;
        }

        //LcmDeploymentStatusDtoGrid ILcmDeploymentStatusManager.GetCreatePage()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
