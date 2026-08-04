using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
    public class OperationalContractManager : GridBaseAsync<OperationalContract, TipologicaGridDto, TipologicaQueryDto, Operationalcontracts>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public OperationalContractManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Operationalcontracts> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Operationalcontracts>();
            var predicateInner = PredicateBuilder.New<Operationalcontracts>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Operationalcontracts>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Operationalcontracts>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Operationalcontracts>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Operationalcontracts>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Operationalcontracts>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<OperationalContract> request)
        {
            return request.Select(dto => new TipologicaGridDto()
            {
                Id = (short)dto.Id,
                Description = dto.Description,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<OperationalContract, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<OperationalContract, object>>[]>
            {
                ["description"] = new Expression<Func<OperationalContract, object>>[] { p => p.Description },
                ["id"] = new Expression<Func<OperationalContract, object>>[] { p => p.Id },
                ["lastModifiedBy"] = new Expression<Func<OperationalContract, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<OperationalContract> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Description))
                    : request.Where(x => x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.Id.ToString()))
                    : request.Where(x => x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<OperationalContract> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<OperationalContract> predicateResult, ExpressionStarter<Operationalcontracts> oraclePredicateResult = null)
        {


            var query = oraclePredicateResult.IsStarted
              ? _repositoryWrapper.OperationalContract.FindByCondition(oraclePredicateResult)
              : _repositoryWrapper.OperationalContract.FindAll();


            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => OperationalContractMapper.GetOperationalContractMapper(p)).AsQueryable();

        }

     

       

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.OperationalContract.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.OperationalContract.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.OperationalContract.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.OperationalContract.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO - verificare che non esistono relazioni
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            var entity = await _repositoryWrapper.OperationalContract.FindByCondition(x => x.Id == id).SingleAsync();
            var lcms = _repositoryWrapper.LCMOperationalContracts
                .FindByCondition(x => x.Operationalcontractid == entity.Id)
                .Include(x => x.Lcm).ThenInclude(x => x.Opco)
                .Include(x => x.Lcm).ThenInclude(x => x.Designcomponent)
                .Select(x => x.Lcm.toDescription(_repositoryWrapper))
                .ToArray();

            if (lcms.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "LCM Engineering", Values = lcms });
            }

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Operational Contract",
                        RecordName = entity.Description,
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

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.OperationalContract.FindByCondition(
               x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            OperationalContract entity = new OperationalContract() { Id = dto.Id, Description = dto.Description };
            _repositoryWrapper.OperationalContract.Create(OperationalContractMapper.SetOperationalContractMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public TipologicaGridDto GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.OperationalContract.FindByCondition(x => x.Id == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = OperationalContractMapper.GetOperationalContractMapper(model);
            var dto = new TipologicaGridDto()
            {
                Id = (short)entity.Id,
                Description = entity.Description,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email
            };
            return dto;
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.OperationalContract.FindByCondition(
               x => x.Id != dto.Id && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            OperationalContract entity = new OperationalContract() { Id = dto.Id, Description = dto.Description };

            _repositoryWrapper.OperationalContract.Update(OperationalContractMapper.SetOperationalContractMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }
    }
}
