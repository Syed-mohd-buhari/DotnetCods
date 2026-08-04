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
    public class ProgramManager : GridBaseAsync<ProgramEntity, TipologicaGridDto, TipologicaQueryDto, Program>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public ProgramManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Program> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Program>();
            var predicateInner = PredicateBuilder.New<Program>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Program>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Programdescription == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Program>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Programid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Program>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Program>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Program>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<ProgramEntity> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = (short)dto.ProgramId,
                Description = dto.ProgramDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<ProgramEntity, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ProgramEntity, object>>[]>
            {
                ["description"] = new Expression<Func<ProgramEntity, object>>[] { p => p.ProgramDescription },
                ["id"] = new Expression<Func<ProgramEntity, object>>[] { p => p.ProgramId },
                ["lastModifiedBy"] = new Expression<Func<ProgramEntity, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<ProgramEntity> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.ProgramDescription))
                    : request.Where(x => x.ProgramDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.ProgramDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.ProgramId.ToString()))
                    : request.Where(x => x.ProgramId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ProgramId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<ProgramEntity> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<ProgramEntity> predicateResult, ExpressionStarter<Program> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.ProgramRepository.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.ProgramRepository.FindAll();

          return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => ProgramMapper.GetProgramMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.ProgramRepository.FindByCondition(
               x => x.Programdescription == dto.Description, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Programid
                };
            }
            ProgramEntity entity = new ProgramEntity() { ProgramId = dto.Id, ProgramDescription = dto.Description };
            _repositoryWrapper.ProgramRepository.Create(ProgramMapper.SetProgramMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.ProgramRepository.FindByCondition(
               x => x.Programid != dto.Id && x.Programdescription == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Programid
                };
            }
            ProgramEntity entity = new ProgramEntity() { ProgramId = dto.Id, ProgramDescription = dto.Description};
            _repositoryWrapper.ProgramRepository.Update(ProgramMapper.SetProgramMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.ProgramRepository.FindByCondition(x => x.Programid == id).SingleAsync();
            _repositoryWrapper.ProgramRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Programid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.ProgramRepository.FindByCondition(x => x.Programid == id).SingleAsync();
            _repositoryWrapper.ProgramRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Programid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO - verificare che non esistano relazioni
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            var entity = await _repositoryWrapper.ProgramRepository.FindByCondition(x => x.Programid == id).SingleAsync();

            var plannedActivities = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Programid == entity.Programid)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Activitystatus)
                .Include(x => x.Deliverystatus)
                .Select(x => PlannedActivityMapper.Get(x,true).toLinkedPlannedActivityName())
                .ToArray();

            if (plannedActivities.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivities });
            }

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "ProgramEntity",
                        RecordName = entity.Programdescription,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public TipologicaGridDto GetCreatePage()
        {
            var dto = new TipologicaGridDto();
            return dto;
        }

        public TipologicaGridDto GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.ProgramRepository.FindByCondition(x => x.Programid == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = ProgramMapper.GetProgramMapper(model);

            var dto = new TipologicaGridDto() { Id = (short)entity.ProgramId, Description = entity.ProgramDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }

     
    }
}
