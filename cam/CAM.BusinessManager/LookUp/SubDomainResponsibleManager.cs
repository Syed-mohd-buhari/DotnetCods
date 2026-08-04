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
   public class SubDomainResponsibleManager : GridBaseAsync<SubDomainResponsible, TipologicaGridDto, TipologicaQueryDto, Subdomainresponsibles>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public SubDomainResponsibleManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
   
        public override ExpressionStarter<Subdomainresponsibles> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Subdomainresponsibles>();
            var predicateInner = PredicateBuilder.New<Subdomainresponsibles>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainresponsibles>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Subdomainresponsible == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainresponsibles>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Subdomainresponsibleid == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Subdomainresponsibles>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Subdomainresponsibles>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Subdomainresponsibles>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<SubDomainResponsible> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = (short)dto.SubDomainResponsibleId,
                Description = dto.SubDomainResponsibleDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<SubDomainResponsible, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SubDomainResponsible, object>>[]>
            {
                ["description"] = new Expression<Func<SubDomainResponsible, object>>[] { p => p.SubDomainResponsibleDescription },
                ["lastModifiedBy"] = new Expression<Func<SubDomainResponsible, object>>[] { p => p.ModificationUserEntity.Email },
                ["id"] = new Expression<Func<SubDomainResponsible, object>>[] { p => p.SubDomainResponsibleId }
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<SubDomainResponsible> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.SubDomainResponsibleDescription))
                : request.Where(x => x.SubDomainResponsibleDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.SubDomainResponsibleDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.SubDomainResponsibleId.ToString()))
                : request.Where(x => x.SubDomainResponsibleId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.SubDomainResponsibleId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<SubDomainResponsible> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<SubDomainResponsible> predicateResult , ExpressionStarter<Subdomainresponsibles> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.SubDomainResponsible.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.SubDomainResponsible.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => SubDomainResponsibleMapper.GetSubDomainResponsibleMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.SubDomainResponsible.FindByCondition(
               x => x.Subdomainresponsible.ToLower().Replace(" ","") == dto.Description.ToLower().Replace(" ",""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Subdomainresponsibleid
                };
            }
            SubDomainResponsible entity = new SubDomainResponsible() { SubDomainResponsibleId = dto.Id, SubDomainResponsibleDescription = dto.Description };
            _repositoryWrapper.SubDomainResponsible.Create(SubDomainResponsibleMapper.SetSubDomainResponsibleMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.SubDomainResponsible.FindByCondition(
               x => x.Subdomainresponsibleid != dto.Id 
               && x.Subdomainresponsible.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") 
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Subdomainresponsibleid
                };
            }
            SubDomainResponsible entity = new SubDomainResponsible() { SubDomainResponsibleId = dto.Id, SubDomainResponsibleDescription = dto.Description };
            _repositoryWrapper.SubDomainResponsible.Update(SubDomainResponsibleMapper.SetSubDomainResponsibleMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.SubDomainResponsible.FindByCondition(x => x.Subdomainresponsibleid == id).SingleAsync();
            _repositoryWrapper.SubDomainResponsible.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Subdomainresponsibleid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.SubDomainResponsible.FindByCondition(x => x.Subdomainresponsibleid == id).SingleAsync();
            _repositoryWrapper.SubDomainResponsible.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Subdomainresponsibleid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var SystemTypes = _repositoryWrapper.SystemType
                              .FindByCondition(x=>x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Select(x=>x.Designcontactid).FirstOrDefault() /*Subdomainresponsibleid*/ == id)
                              .Select(x => x.toSystemTypeName(_repositoryWrapper)).ToArray();


            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (SystemTypes.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "System Types", Values = SystemTypes });

            var entity = await _repositoryWrapper.SubDomainResponsible.FindByCondition(x => x.Subdomainresponsibleid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Sub-Domain Responsible",
                        RecordName = entity.Subdomainresponsible,
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
            var entity = SubDomainResponsibleMapper.GetSubDomainResponsibleMapper( _repositoryWrapper.SubDomainResponsible
                .FindByCondition(x => x.Subdomainresponsibleid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = (short)entity.SubDomainResponsibleId, Description = entity.SubDomainResponsibleDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }
    }
}


