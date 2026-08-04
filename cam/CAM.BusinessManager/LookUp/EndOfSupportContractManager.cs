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
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CAM.BusinessManager.LookUp
{
    public class EndOfSupportContractManager : GridBaseAsync<EndOfSupportContract, TipologicaGridDto, TipologicaQueryDto, EndOfSupportContract>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public EndOfSupportContractManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<EndOfSupportContract> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.EndOfSupportContractId,
                Description = dto.EndOfSupportContractDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }



        public override Dictionary<string, Expression<Func<EndOfSupportContract, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<EndOfSupportContract, object>>[]>
            {
                ["description"] = new Expression<Func<EndOfSupportContract, object>>[] { p => p.EndOfSupportContractDescription },
                ["id"] = new Expression<Func<EndOfSupportContract, object>>[] { p => p.EndOfSupportContractId },
                ["lastModifiedBy"] = new Expression<Func<EndOfSupportContract, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<EndOfSupportContract> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.EndOfSupportContractDescription))
                : request.Where(x => x.EndOfSupportContractDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.EndOfSupportContractDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.EndOfSupportContractId.ToString()))
                : request.Where(x => x.EndOfSupportContractId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.EndOfSupportContractId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<EndOfSupportContract> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<EndOfSupportContract> predicateResult , ExpressionStarter<EndOfSupportContract> predicateResult1 = null)
        {
            var query = predicateResult.IsStarted
              ? _repositoryWrapper.EndOfSupportContract.FindByCondition(predicateResult)
              : _repositoryWrapper.EndOfSupportContract.FindAll();
            return query;
        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.EndOfSupportContract.FindByCondition(
               x => x.EndOfSupportContractId == dto.Id, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.EndOfSupportContractId
                };
            }
            EndOfSupportContract entity = new EndOfSupportContract() { EndOfSupportContractId = dto.Id, EndOfSupportContractDescription = dto.Description };
            _repositoryWrapper.EndOfSupportContract.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.EndOfSupportContract.FindByCondition(
               x => x.EndOfSupportContractId != dto.Id && x.EndOfSupportContractDescription == dto.Description && !x.Deleted).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.EndOfSupportContractId
                };
            }
            EndOfSupportContract entity = new EndOfSupportContract() { EndOfSupportContractId = dto.Id, EndOfSupportContractDescription = dto.Description };
            _repositoryWrapper.EndOfSupportContract.Update(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.EndOfSupportContract.FindByCondition(x => x.EndOfSupportContractId == id).SingleAsync();
            _repositoryWrapper.EndOfSupportContract.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.EndOfSupportContractId
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.EndOfSupportContract.FindByCondition(x => x.EndOfSupportContractId == id).SingleAsync();
            _repositoryWrapper.EndOfSupportContract.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.EndOfSupportContractId
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //da gestire
            var entities = new string[] { };
            if (entities.Length > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = entities
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
            var entity = _repositoryWrapper.EndOfSupportContract.FindByCondition(x => x.EndOfSupportContractId == id)
                .Include(x => x.ModificationUserEntity).Single();
            var dto = new TipologicaGridDto() { Id = entity.EndOfSupportContractId, Description = entity.EndOfSupportContractDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }

     
    }
}
