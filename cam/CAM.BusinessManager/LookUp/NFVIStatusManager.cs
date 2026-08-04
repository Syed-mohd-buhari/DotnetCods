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
using CAM.DataTransferObjects.Entita.NFVIStatus;
using CAM.DataTransferObjects.FunctionalityDto;
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
    public class NFVIStatusManager : GridBaseAsync<NFVIStatus, NFVIStatusDtoGrid, NFVIStatusDtoQuery, Nfvistatuses>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public NFVIStatusManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }


        public override ExpressionStarter<Nfvistatuses> ApplyFilterForOracleModel(NFVIStatusDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Nfvistatuses>();
            var predicateInner = PredicateBuilder.New<Nfvistatuses>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvistatuses>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Nfvistatus == item);
                predicateResult.And(predicateInner);
            }
            if (request.Color != null && request.Color.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvistatuses>();
                foreach (var item in request.Color)
                    predicateInner.Or(x => x.Color == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvistatuses>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Nfvistatusid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvistatuses>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Nfvistatuses>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Nfvistatuses>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        public override List<NFVIStatusDtoGrid> CastObjectToDto(IQueryable<NFVIStatus> request)
        {
            return request.Select(dto => new NFVIStatusDtoGrid()
            {
                Id = dto.NFVIStatusId,
                Description = dto.NFVIStatusDescription,
                Color = dto.Color,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<NFVIStatus, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<NFVIStatus, object>>[]>
            {
                ["description"] = new Expression<Func<NFVIStatus, object>>[] { p => p.NFVIStatusDescription },
                ["id"] = new Expression<Func<NFVIStatus, object>>[] { p => p.NFVIStatusId },
                ["lastModifiedBy"] = new Expression<Func<NFVIStatus, object>>[] { p => p.ModificationUserEntity.Email },
                ["color"] = new Expression<Func<NFVIStatus, object>>[] { p => p.Color },


            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<NFVIStatus> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.NFVIStatusDescription))
                : request.Where(x => x.NFVIStatusDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.NFVIStatusDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.NFVIStatusId.ToString()))
                : request.Where(x => x.NFVIStatusId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.NFVIStatusId.ToString())),
                "color" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Color))
               : request.Where(x => x.Color.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Color)),

            };
        }

        public override IQueryable<NFVIStatus> PrepareQuery(NFVIStatusDtoQuery request, ExpressionStarter<NFVIStatus> predicateResult, ExpressionStarter<Nfvistatuses> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
             ? _repositoryWrapper.NFVIStatus.FindByCondition(oraclePredicateResult)
             : _repositoryWrapper.NFVIStatus.FindAll();
            return query.Include(m => m.CreationuserNavigation)
                .Include(m => m.ModificationuserNavigation)
                .AsEnumerable().Select(p => NFVIStatusMapper.GetNFVIStatusMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(NFVIStatusDtoGrid dto)
        {
            var entityExists = await _repositoryWrapper.NFVIStatus.FindByCondition(
               x => x.Nfvistatus.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               , true)
               .FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Nfvistatusid
                };
            }
            NFVIStatus entity = new NFVIStatus() { NFVIStatusId = dto.Id, NFVIStatusDescription = dto.Description, Color = dto.Color };
            _repositoryWrapper.NFVIStatus.Create(NFVIStatusMapper.SetNFVIStatusMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(NFVIStatusDtoGrid dto)
        {
            var entityExists = await _repositoryWrapper.NFVIStatus.FindByCondition(
               x => x.Nfvistatusid != dto.Id
               && x.Nfvistatus.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Nfvistatusid
                };
            }
            NFVIStatus entity = new NFVIStatus() { NFVIStatusId = dto.Id, NFVIStatusDescription = dto.Description, Color = dto.Color };
            _repositoryWrapper.NFVIStatus.Update(NFVIStatusMapper.SetNFVIStatusMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.NFVIStatus.FindByCondition(x => x.Nfvistatusid == id).SingleAsync();
            _repositoryWrapper.NFVIStatus.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Nfvistatusid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.NFVIStatus.FindByCondition(x => x.Nfvistatusid == id).SingleAsync();
            _repositoryWrapper.NFVIStatus.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Nfvistatusid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var NFVITransition = _repositoryWrapper.NFVITransitionRepository.FindByCondition(x => x.Statuslivescid == id).Select(x => x.Nfvisitedesignation).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (NFVITransition.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "NFVI Transition", Values = NFVITransition });

            var entity = await _repositoryWrapper.NFVIStatus.FindByCondition(x => x.Nfvistatusid == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "NFVI Status",
                        RecordName = entity.Nfvistatus,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public NFVIStatusDtoGrid GetCreatePage()
        {
            var dto = new NFVIStatusDtoGrid();
            return dto;
        }

        public NFVIStatusDtoGrid GetUpdatePage(short id)
        {
            var entity = NFVIStatusMapper.GetNFVIStatusMapper(_repositoryWrapper.NFVIStatus.FindByCondition(x => x.Nfvistatusid == id).
                Include(x => x.ModificationuserNavigation).Single());
            var dto = new NFVIStatusDtoGrid() { Id = entity.NFVIStatusId, Description = entity.NFVIStatusDescription, LastModifiedBy = entity.ModificationUserEntity.Email, LastModified = entity.ModificationDate, Color = entity.Color };
            return dto;
        }


    }
}
