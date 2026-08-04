using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
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
    public class ActivityDetailsManager : GridBaseAsync<ActivityDetails, TipologicaGridDtoForVirtualized, TipologicaQueryDtoForVirtualized, Activitydetails>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public ActivityDetailsManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Activitydetails> ApplyFilterForOracleModel(TipologicaQueryDtoForVirtualized request)
        {
            var predicateResult = PredicateBuilder.New<Activitydetails>();
            var predicateInner = PredicateBuilder.New<Activitydetails>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Activitydetails>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Activitydetails>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Activitydetailsid == item);
                predicateResult.And(predicateInner);
            }

            if (request.ForVirtualized != null && request.ForVirtualized.Any())
            {
                predicateInner = PredicateBuilder.New<Activitydetails>();
                foreach (var item in request.ForVirtualized)
                    predicateInner.Or(x => x.Forvirtualized == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Activitydetails>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Activitydetails>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Activitydetails>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDtoForVirtualized> CastObjectToDto(IQueryable<ActivityDetails> request)
        {
            return request.Select(dto => new TipologicaGridDtoForVirtualized()
            {
                Id = (short)dto.ActivityDetailsId,
                Description = dto.ActivityDetailsDescription,
                ForVirtualized = dto.ForVirtualized,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                Deleted = dto.Deleted,
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<ActivityDetails, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ActivityDetails, object>>[]>
            {
                ["description"] = new Expression<Func<ActivityDetails, object>>[] { p => p.ActivityDetailsDescription },
                ["id"] = new Expression<Func<ActivityDetails, object>>[] { p => p.ActivityDetailsId },
                ["forVirtualized"] = new Expression<Func<ActivityDetails, object>>[] { p => p.ForVirtualized },
                ["lastModifiedBy"] = new Expression<Func<ActivityDetails, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<ActivityDetails> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.ActivityDetailsDescription))
                    : request.Where(x => x.ActivityDetailsDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.ActivityDetailsDescription)),
                "forVirtualized" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto
                    { Text = p.ForVirtualized ? "YES" : "NO", Value = p.ForVirtualized.ToString() }).Distinct()
                    : request
                        .Where(x => x.ForVirtualized == false).Select(p =>
                            new FilterValueDto { Text = p.ForVirtualized ? "YES" : "NO", Value = p.ForVirtualized.ToString() }).Distinct(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.ActivityDetailsId.ToString()))
                    : request.Where(x => x.ActivityDetailsId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ActivityDetailsId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }


        private IQueryable<Activitydetails> GetData(ExpressionStarter<Activitydetails> predicateResult)
        {
            var query = predicateResult.IsStarted
                          ? _repositoryWrapper.ActivityDetails.FindByCondition(predicateResult)
                          : _repositoryWrapper.ActivityDetails.FindAll();
            return query;
        }
        public override IQueryable<ActivityDetails> PrepareQuery(TipologicaQueryDtoForVirtualized request, ExpressionStarter<ActivityDetails> predicateResult, ExpressionStarter<Activitydetails> ordermodel)
        {

            var query = ordermodel.IsStarted
                ? _repositoryWrapper.ActivityDetails.FindByCondition(ordermodel)
                : _repositoryWrapper.ActivityDetails.GetAllWithRelations();

            return
                query.Include(m => m.CreationuserNavigation)
                .Include(m => m.ModificationuserNavigation)
                .AsEnumerable().Select(p => ActiviyDetailsMapper.GetActivityDetailsMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDtoForVirtualized dto)
        {
            var entityExists = await _repositoryWrapper.ActivityDetails.FindByCondition(
               x => x.Activitydetailsid == dto.Id, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Activitydetailsid
                };
            }
            ActivityDetails entity = new ActivityDetails() { ActivityDetailsId = dto.Id, ActivityDetailsDescription = dto.Description, ForVirtualized = dto.ForVirtualized };
            var savedObject = ActiviyDetailsMapper.SetActivityDetailsMapper(entity);
            _repositoryWrapper.ActivityDetails.Create(savedObject);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDtoForVirtualized dto)
        {
            var entityExists = await _repositoryWrapper.ActivityDetails.FindByCondition(
               x => x.Activitydetailsid != dto.Id && x.Description == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Activitydetailsid
                };
            }
            ActivityDetails entity = new ActivityDetails() { ActivityDetailsId = dto.Id, ActivityDetailsDescription = dto.Description, ForVirtualized = dto.ForVirtualized };
            var savedObject = ActiviyDetailsMapper.SetActivityDetailsMapper(entity);
            _repositoryWrapper.ActivityDetails.Update(savedObject);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.ActivityDetails.FindByCondition(x => x.Activitydetailsid == id).SingleAsync();
            _repositoryWrapper.ActivityDetails.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Activitydetailsid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.ActivityDetails.FindByCondition(x => x.Activitydetailsid == id).SingleAsync();
            _repositoryWrapper.ActivityDetails.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Activitydetailsid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO - verificare che non esistano relazioni.
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            var entity = await _repositoryWrapper.ActivityDetails.FindByCondition(x => x.Activitydetailsid == id).SingleAsync();
            var plannedResources = _repositoryWrapper.PlannedActivityResourceRepository
                .FindByCondition(x =>
                     x.Actdetailsforvrtaddasset == entity.Description ||
                    x.Activitydetailsaddasset == entity.Description ||
                     x.Actdetailsforvrteditasset == entity.Description ||
                    x.Activitydetailseditasset == entity.Description ||
                    x.Activitydetailslcm == entity.Description
                ).Select(x => x.Plannedactivityresource).ToArray();

            if (plannedResources.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "Planned Activity Resource", Values = plannedResources });
            }
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Activity Details",
                        RecordName = entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public TipologicaGridDtoForVirtualized GetCreatePage()
        {
            var dto = new TipologicaGridDtoForVirtualized();
            return dto;
        }

        public TipologicaGridDtoForVirtualized GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.ActivityDetails.FindByCondition(x => x.Activitydetailsid == id).Include(x => x.ModificationuserNavigation).Single();
            var entity = ActiviyDetailsMapper.GetActivityDetailsMapper(model);
            var dto = new TipologicaGridDtoForVirtualized()
            {
                Id = (short)entity.ActivityDetailsId,
                Description = entity.ActivityDetailsDescription,
                ForVirtualized = entity.ForVirtualized,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email
            };
            return dto;
        }


    }
}
