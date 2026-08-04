using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.LookUp;
using CAM.Entities.Models.Lookup;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
   public class VerticalResponsibleManager : GridBaseAsync<VerticalResponsible, TipologicaGridDto, TipologicaQueryDto, Verticalresponsibles>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public VerticalResponsibleManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor, 
            IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
 
        public override ExpressionStarter<Verticalresponsibles> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Verticalresponsibles>();
            var predicateInner = PredicateBuilder.New<Verticalresponsibles>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Verticalresponsibles>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Verticalresponsible == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Verticalresponsibles>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Verticalresponsibleid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Verticalresponsibles>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Verticalresponsibles>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Verticalresponsibles>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<VerticalResponsible> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = (short)dto.VerticalResponsibleId,
                Description = dto.VerticalResponsibleDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<VerticalResponsible, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<VerticalResponsible, object>>[]>
            {
                ["description"] = new Expression<Func<VerticalResponsible, object>>[] { p => p.VerticalResponsibleDescription },
                ["lastModifiedBy"] = new Expression<Func<VerticalResponsible, object>>[] { p => p.ModificationUserEntity.Email },
                ["id"] = new Expression<Func<VerticalResponsible, object>>[] { p => p.VerticalResponsibleId }
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<VerticalResponsible> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.VerticalResponsibleDescription))
                : request.Where(x => x.VerticalResponsibleDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.VerticalResponsibleDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.VerticalResponsibleId.ToString()))
                : request.Where(x => x.VerticalResponsibleId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.VerticalResponsibleId.ToString())),
            };
        }

        public override IQueryable<VerticalResponsible> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<VerticalResponsible> predicateResult , ExpressionStarter<Verticalresponsibles> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.VerticalResponsible.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.VerticalResponsible.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => VerticalResponsibleMapper.GetVerticalResponsibleMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.VerticalResponsible.FindByCondition(
               x => x.Verticalresponsible.ToLower().Replace(" ","") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Verticalresponsibleid
                };
            }
            VerticalResponsible entity = new VerticalResponsible() { VerticalResponsibleId = dto.Id, VerticalResponsibleDescription = dto.Description };
            _repositoryWrapper.VerticalResponsible.Create(VerticalResponsibleMapper.SetVerticalResponsibleMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.VerticalResponsible.FindByCondition(
               x => x.Verticalresponsibleid != dto.Id 
               && x.Verticalresponsible.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") 
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Verticalresponsibleid
                };
            }
            VerticalResponsible entity = new VerticalResponsible() { VerticalResponsibleId = dto.Id, VerticalResponsibleDescription = dto.Description };
            _repositoryWrapper.VerticalResponsible.Update(VerticalResponsibleMapper.SetVerticalResponsibleMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.VerticalResponsible.FindByCondition(x => x.Verticalresponsibleid == id).SingleAsync();
            _repositoryWrapper.VerticalResponsible.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Verticalresponsibleid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.VerticalResponsible.FindByCondition(x => x.Verticalresponsibleid == id).SingleAsync();
            _repositoryWrapper.VerticalResponsible.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Verticalresponsibleid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var SystemType = _repositoryWrapper.SystemType.FindByCondition(x => 1 == id)
                .Select(x => x.toSystemTypeName(_repositoryWrapper)).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (SystemType.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "System Type", Values = SystemType });
            var entity = await _repositoryWrapper.VerticalResponsible.FindByCondition(x => x.Verticalresponsibleid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Vertical Responsible",
                        RecordName = entity.Verticalresponsible,
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
            var entity = VerticalResponsibleMapper.GetVerticalResponsibleMapper( _repositoryWrapper.VerticalResponsible.FindByCondition(x => x.Verticalresponsibleid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = (short)entity.VerticalResponsibleId, Description = entity.VerticalResponsibleDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }
    }
}
