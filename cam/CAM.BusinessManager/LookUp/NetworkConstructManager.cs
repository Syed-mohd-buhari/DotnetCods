using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
    public class NetworkConstructManager : GridBaseAsync<NetworkConstruct, TipologicaGridDto, TipologicaQueryDto, Networkconstructs>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;


        public NetworkConstructManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
        }


        public override ExpressionStarter<Networkconstructs> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Networkconstructs>();
            var predicateInner = PredicateBuilder.New<Networkconstructs>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Networkconstructs>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Networkconstruct == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Networkconstructs>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Networkconstructsid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Networkconstructs>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Networkconstructs>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Networkconstructs>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<NetworkConstruct> request)
        {
            return request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.NetworkConstructsId,
                Description = dto.NetworkConstructDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<NetworkConstruct, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<NetworkConstruct, object>>[]>
            {
                ["description"] = new Expression<Func<NetworkConstruct, object>>[] { p => p.NetworkConstructDescription },
                ["id"] = new Expression<Func<NetworkConstruct, object>>[] { p => p.NetworkConstructsId },
                ["lastModifiedBy"] = new Expression<Func<NetworkConstruct, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<NetworkConstruct> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.NetworkConstructDescription)) : request.Where(x =>
                    x.NetworkConstructDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.NetworkConstructDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.NetworkConstructsId.ToString()))
                    : request.Where(x =>
                        x.NetworkConstructsId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.NetworkConstructsId.ToString())),

            };
        }

        public override IQueryable<NetworkConstruct> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<NetworkConstruct> predicateResult, ExpressionStarter<Networkconstructs> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.NetworkConstruct.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.NetworkConstruct.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => NetworkConstructMapper.GetNetworkConstructMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.NetworkConstruct.FindByCondition(
               x => x.Networkconstructsid == dto.Id, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Networkconstructsid
                };
            }
            NetworkConstruct entity = new NetworkConstruct() { NetworkConstructsId = dto.Id, NetworkConstructDescription = dto.Description };
            _repositoryWrapper.NetworkConstruct.Create(NetworkConstructMapper.SetNetworkConstructMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.NetworkConstruct.FindByCondition(
               x => x.Networkconstructsid != dto.Id && x.Networkconstruct == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Networkconstructsid
                };
            }
            NetworkConstruct entity = new NetworkConstruct() { NetworkConstructsId = dto.Id, NetworkConstructDescription = dto.Description };
            _repositoryWrapper.NetworkConstruct.Update(NetworkConstructMapper.SetNetworkConstructMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.NetworkConstruct.FindByCondition(x => x.Networkconstructsid == id).SingleAsync();
            _repositoryWrapper.NetworkConstruct.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Networkconstructsid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.NetworkConstruct.FindByCondition(x => x.Networkconstructsid == id).SingleAsync();
            _repositoryWrapper.NetworkConstruct.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Networkconstructsid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO - Verificare che non esistano relazioni
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            var entity = await _repositoryWrapper.NetworkConstruct.FindByCondition(x => x.Networkconstructsid == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Network Construct",
                        RecordName = entity.Networkconstruct,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public TipologicaGridDto GetCreatePage()
        {
            var NetworkConstructDto = new TipologicaGridDto();
            return NetworkConstructDto;
        }

        public TipologicaGridDto GetUpdatePage(short id)
        {
            var entity = NetworkConstructMapper.GetNetworkConstructMapper(_repositoryWrapper.NetworkConstruct.FindByCondition(x => x.Networkconstructsid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = entity.NetworkConstructsId, Description = entity.NetworkConstructDescription, LastModifiedBy = entity.ModificationUserEntity.Email, LastModified = entity.ModificationDate };
            return dto;
        }


    }
}
