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
using CAM.DataTransferObjects.Entita.NFVIBundleID;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.Entities.Mappers.Entity;
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
   public class NFVIBundleIDManager : GridBaseAsync<NFVIBundleID, NFVIBundleIDDtoGrid, NFVIBundleIDDtoQuery, Nfvibundleids>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public NFVIBundleIDManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

    
        public override ExpressionStarter<Nfvibundleids> ApplyFilterForOracleModel(NFVIBundleIDDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Nfvibundleids>();
            var predicateInner = PredicateBuilder.New<Nfvibundleids>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvibundleids>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Nfvibundleid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvibundleids>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvibundleids>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Nfvibundleidid == item);
                predicateResult.And(predicateInner);
            }
            if (request.Order != null && request.Order.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvibundleids>();
                foreach (var item in request.Order)
                    predicateInner.Or(x => x.Order == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Nfvibundleids>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Nfvibundleids>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        public override List<NFVIBundleIDDtoGrid> CastObjectToDto(IQueryable<NFVIBundleID> request)
        {
            return  request.Select(dto => new NFVIBundleIDDtoGrid()
            {
                Id = dto.NFVIBundleIDId,
                Description = dto.NFVIBundleIdDescription,
                LastModified = dto.ModificationDate,
                Order = dto.Order,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<NFVIBundleID, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<NFVIBundleID, object>>[]>
            {
                ["description"] = new Expression<Func<NFVIBundleID, object>>[] { p => p.NFVIBundleIdDescription},
                ["id"] = new Expression<Func<NFVIBundleID, object>>[] { p => p.NFVIBundleIDId },
                ["lastModifiedBy"] = new Expression<Func<NFVIBundleID, object>>[] { p => p.ModificationUserEntity.Email },
                ["order"] = new Expression<Func<NFVIBundleID, object>>[] { p => p.Order },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<NFVIBundleID> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.NFVIBundleIdDescription))
                : request.Where(x => x.NFVIBundleIdDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.NFVIBundleIdDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.NFVIBundleIDId.ToString()))
                : request.Where(x => x.NFVIBundleIDId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.NFVIBundleIDId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "order" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.Order.ToString()))
                : request.Where(x => x.Order.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Order.ToString())),
            };
        }

        public override IQueryable<NFVIBundleID> PrepareQuery(NFVIBundleIDDtoQuery request, ExpressionStarter<NFVIBundleID> predicateResult , ExpressionStarter<Nfvibundleids> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
             ? _repositoryWrapper.NFVIBundleID.FindByCondition(oraclePredicateResult)
             : _repositoryWrapper.NFVIBundleID.FindAll();
            return query.Include(m => m.CreationuserNavigation)
                .Include(m => m.ModificationuserNavigation)
                .AsEnumerable().Select(p=> NFVIBundleIDMapper.Get(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(NFVIBundleIDDtoGrid dto)
        {
            var entityExists = await _repositoryWrapper.NFVIBundleID.FindByCondition(
               x => x.Nfvibundleid.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Nfvibundleidid
                };
            }
            var order =  _repositoryWrapper.NFVIBundleID.FindAll().Count();
            NFVIBundleID entity = new NFVIBundleID() { NFVIBundleIDId = dto.Id, NFVIBundleIdDescription = dto.Description, Order= order+1 };
            _repositoryWrapper.NFVIBundleID.Create(NFVIBundleIDMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(NFVIBundleIDDtoGrid dto)
        {
            var entityExists = await _repositoryWrapper.NFVIBundleID.FindByCondition(
               x => x.Nfvibundleidid != dto.Id && x.Nfvibundleid.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Nfvibundleidid
                };
            }
            NFVIBundleID entity = new NFVIBundleID() { NFVIBundleIDId = dto.Id, NFVIBundleIdDescription = dto.Description,Order= dto.Order };
            _repositoryWrapper.NFVIBundleID.Update(NFVIBundleIDMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }


        public async Task<ResultDto> ChangeGridOrderNFVIBundleID(List<ChangeGridOrderDto> lista)
        {
            var allEntityExist = await _repositoryWrapper.NFVIBundleID.FindAll().ToListAsync();
            foreach (var item in lista)
            {
                NFVIBundleID resource = NFVIBundleIDMapper.Get(allEntityExist.Where(x => x.Nfvibundleidid == item.Id).Single());
                resource.Order = item.Order;
                _repositoryWrapper.NFVIBundleID.Update(NFVIBundleIDMapper.Set(resource));

            }
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }


        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.NFVIBundleID.FindByCondition(x => x.Nfvibundleidid == id).SingleAsync();
            _repositoryWrapper.NFVIBundleID.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Nfvibundleidid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.NFVIBundleID.FindByCondition(x => x.Nfvibundleidid == id).SingleAsync();
            _repositoryWrapper.NFVIBundleID.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Nfvibundleidid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var VNFTransition = _repositoryWrapper.VNFTransition.FindByCondition(x => x.Nfvibundleidid == id).Select(x => x.Vnftype).ToArray();
            var NetworkElementAsPlanned = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Nfvibundleidid == id).Select(x => x.Elementname).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (VNFTransition.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "VNF Transition", Values = VNFTransition });
            if (NetworkElementAsPlanned.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Planned", Values = NetworkElementAsPlanned });
            var entity = await _repositoryWrapper.NFVIBundleID.FindByCondition(x => x.Nfvibundleidid == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "NFVI Bundle Id",
                        RecordName = entity.Nfvibundleid,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public NFVIBundleIDDtoGrid GetCreatePage()
        {
            var dto = new NFVIBundleIDDtoGrid();
            return dto;
        }

        public NFVIBundleIDDtoGrid GetUpdatePage(short id)
        {
            var entity = NFVIBundleIDMapper.Get(_repositoryWrapper.NFVIBundleID.FindByCondition(x => x.Nfvibundleidid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new NFVIBundleIDDtoGrid() { Id = entity.NFVIBundleIDId, Description = entity.NFVIBundleIdDescription, LastModified = entity.ModificationDate, Order = entity.Order, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }

     
    }
}
