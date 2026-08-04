using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.LookUp
{
   public class EquipmentStatusManager : GridBaseAsync<EquipmentStatus, TipologicaGridDto, TipologicaQueryDto, Equipmentstatuses>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public EquipmentStatusManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper ) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

      
        public override ExpressionStarter<Equipmentstatuses> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Equipmentstatuses>();
            var predicateInner = PredicateBuilder.New<Equipmentstatuses>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Equipmentstatuses>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Equipmentstatus == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Equipmentstatuses>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Equipmentstatusid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Equipmentstatuses>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Equipmentstatuses>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Equipmentstatuses>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<EquipmentStatus> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.EquipmentStatusId,
                Description = dto.EquipmentStatusDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<EquipmentStatus, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<EquipmentStatus, object>>[]>
            {
                ["description"] = new Expression<Func<EquipmentStatus, object>>[] { p => p.EquipmentStatusDescription },
                ["id"] = new Expression<Func<EquipmentStatus, object>>[] { p => p.EquipmentStatusId },
                ["lastModifiedBy"] = new Expression<Func<EquipmentStatus, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<EquipmentStatus> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.EquipmentStatusDescription))
                : request.Where(x => x.EquipmentStatusDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.EquipmentStatusDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.EquipmentStatusId.ToString()))
                : request.Where(x => x.EquipmentStatusId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.EquipmentStatusId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<EquipmentStatus> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<EquipmentStatus> predicateResult, ExpressionStarter<Equipmentstatuses> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.EquipmentStatus.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.EquipmentStatus.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => EquipmentStatusMapper.GetEquipmentStatusMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.EquipmentStatus.FindByCondition(
               x => x.Equipmentstatus == dto.Description, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Equipmentstatusid
                };
            }
            EquipmentStatus entity = new EquipmentStatus() { EquipmentStatusId = dto.Id, EquipmentStatusDescription = dto.Description };
            _repositoryWrapper.EquipmentStatus.Create(EquipmentStatusMapper.SetEquipmentStatusMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.EquipmentStatus.FindByCondition(
               x => x.Equipmentstatusid != dto.Id && x.Equipmentstatus == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Equipmentstatusid
                };
            }
            EquipmentStatus entity = new EquipmentStatus() { EquipmentStatusId = dto.Id, EquipmentStatusDescription = dto.Description };
            _repositoryWrapper.EquipmentStatus.Update(EquipmentStatusMapper.SetEquipmentStatusMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.EquipmentStatus.FindByCondition(x => x.Equipmentstatusid == id).SingleAsync();
            _repositoryWrapper.EquipmentStatus.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Equipmentstatusid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.EquipmentStatus.FindByCondition(x => x.Equipmentstatusid == id).SingleAsync();
            _repositoryWrapper.EquipmentStatus.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Equipmentstatusid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var entities = _repositoryWrapper.VNFTransition.FindByCondition(x => x.Equipmentstatusid == id).Select(x => x.Elementname).ToArray();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "VNF Transition", Values = entities });

            var entity = await _repositoryWrapper.EquipmentStatus.FindByCondition(x => x.Equipmentstatusid == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Equipment Status",
                        RecordName = entity.Equipmentstatus,
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
            var model = _repositoryWrapper.EquipmentStatus.FindByCondition(x => x.Equipmentstatusid == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = EquipmentStatusMapper.GetEquipmentStatusMapper(model);
            var dto = new TipologicaGridDto() { Id = entity.EquipmentStatusId, Description = entity.EquipmentStatusDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }

        
    }
}
