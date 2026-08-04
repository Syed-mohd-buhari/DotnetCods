using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.BPT;
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
    public class DriverManager : GridBaseAsync<Driver, DriverDto, DriverQueryDto, Drivers>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public DriverManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Drivers> ApplyFilterForOracleModel(DriverQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Drivers>();
            var predicateInner = PredicateBuilder.New<Drivers>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Drivers>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Driver == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Drivers>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Driverid == item);
                predicateResult.And(predicateInner);
            }
            if (request.BptDriverDetails != null && request.BptDriverDetails.Any())
            {
                predicateInner = PredicateBuilder.New<Drivers>();
                foreach (var item in request.BptDriverDetails)
                    predicateInner.Or(x => x.Bptdriverdetails == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Drivers>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Drivers>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Drivers>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<DriverDto> CastObjectToDto(IQueryable<Driver> request)
        {
            return  request.Select(dto => new DriverDto()
            {
                Id = (short)dto.DriverId,
                Description = dto.DriverDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                BptDriverDetails = dto.BptDriverDetails,
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<Driver, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Driver, object>>[]>
            {
                ["description"] = new Expression<Func<Driver, object>>[] { p => p.DriverDescription },
                ["id"] = new Expression<Func<Driver, object>>[] { p => p.DriverId },
                ["bptDriverDetails"] = new Expression<Func<Driver, object>>[] { p => p.BptDriverDetails },
                ["lastModifiedBy"] = new Expression<Func<Driver, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<Driver> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.DriverDescription))
                    : request.Where(x => x.DriverDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.DriverDescription)),
                "bptDriverDetails" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.BptDriverDetails))
                    : request.Where(x => x.BptDriverDetails.Contains(propertyFilter)).Select(x => new FilterValueDto(x.BptDriverDetails)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.DriverId.ToString()))
                    : request.Where(x => x.DriverId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.DriverId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<Driver> PrepareQuery(DriverQueryDto request, ExpressionStarter<Driver> predicateResult, ExpressionStarter<Drivers> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.Driver.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.Driver.FindAll();

          return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => DriverMapper.GetDriverMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(DriverDto dto)
        {
            var entityExists = await _repositoryWrapper.Driver.FindByCondition(
               x => x.Driver == dto.Description, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Driverid
                };
            }
            Driver entity = new Driver() { DriverId = dto.Id, DriverDescription = dto.Description,BptDriverDetails = dto.BptDriverDetails };
            _repositoryWrapper.Driver.Create(DriverMapper.SetDriverMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(DriverDto dto)
        {
            var entityExists = await _repositoryWrapper.Driver.FindByCondition(
               x => x.Driverid != dto.Id && x.Driver == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Driverid
                };
            }
            Driver entity = new Driver() { DriverId = dto.Id, DriverDescription = dto.Description, BptDriverDetails = dto.BptDriverDetails };
            _repositoryWrapper.Driver.Update(DriverMapper.SetDriverMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.Driver.FindByCondition(x => x.Driverid == id).SingleAsync();
            _repositoryWrapper.Driver.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Driverid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.Driver.FindByCondition(x => x.Driverid == id).SingleAsync();
            _repositoryWrapper.Driver.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Driverid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO - verificare che non esistano relazioni
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            var entity = await _repositoryWrapper.Driver.FindByCondition(x => x.Driverid == id).SingleAsync();
            var plannedResources = _repositoryWrapper.PlannedActivityResourceDriver
                .FindByCondition(x => x.Driverid == entity.Driverid)
                .Include(x => x.Plannedactivityresource)
                .Select(x => x.Plannedactivityresource.Plannedactivityresource)
                .ToArray();
            var plannedActivities = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Driverid == entity.Driverid)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Activitystatus)
                .Include(x => x.Deliverystatus)
                .Select(x => PlannedActivityMapper.Get(x,true).toLinkedPlannedActivityName())
                .ToArray();

            if (plannedActivities.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivities });
            }

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
                        EntityName = "Driver",
                        RecordName = entity.Driver,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public DriverDto GetCreatePage()
        {
            var dto = new DriverDto();
            return dto;
        }

        public DriverDto GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.Driver.FindByCondition(x => x.Driverid == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = DriverMapper.GetDriverMapper(model);

            var dto = new DriverDto() { Id = (short)entity.DriverId, Description = entity.DriverDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email, BptDriverDetails = entity.BptDriverDetails };
            return dto;
        }

     
    }
}
