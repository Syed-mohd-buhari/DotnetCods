using AutoMapper;
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
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsIs;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsPlanned;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
    public class OpCosManager : GridBaseAsync<OpCo, TipologicaGridDto, TipologicaQueryDto, Opcos>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;


        public OpCosManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
        }

    
        public override ExpressionStarter<Opcos> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {

            var predicateResult = PredicateBuilder.New<Opcos>();
            var predicateInner = PredicateBuilder.New<Opcos>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Opcos>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Opco == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Opcos>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Opcos>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Opcos>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Opcos>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<OpCo> request)
        {
            return request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.OpCoId,
                Description = dto.OpCoDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<OpCo, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<OpCo, object>>[]>
            {
                ["description"] = new Expression<Func<OpCo, object>>[] { p => p.OpCoDescription },
                ["id"] = new Expression<Func<OpCo, object>>[] { p => p.OpCoId },
                ["lastModifiedBy"] = new Expression<Func<OpCo, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<OpCo> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
              "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.OpCoDescription)) : request.Where(x =>
                    x.OpCoDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.OpCoDescription)),
              "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                  : request
                      .Where(x =>
                          x.ModificationUserEntity.Email.Contains(
                              propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.OpCoId.ToString()))
                : request.Where(x =>
                    x.OpCoId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.OpCoId.ToString())),

            };
        }

        public override IQueryable<OpCo> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<OpCo> predicateResult , ExpressionStarter<Opcos> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.OpCo.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.OpCo.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => OpCoMapper.GetOpCoMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(TipologicaGridDto opCoDto)
        {
            var entityExists = await _repositoryWrapper.OpCo.FindByCondition(
               x => x.Opco.ToLower().Replace(" ","") == opCoDto.Description.ToLower().Replace(" ",""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Opcoid
                };
            }
            OpCo entity = new OpCo() { OpCoId = opCoDto.Id,OpCoDescription=opCoDto.Description };            
            _repositoryWrapper.OpCo.Create(OpCoMapper.SetOpCoMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.OpCo.FindByCondition(
               x => x.Opcoid != dto.Id 
               && x.Opco.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") 
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Opcoid
                };
            }
            OpCo entity = new OpCo() { OpCoId = dto.Id, OpCoDescription = dto.Description };
            _repositoryWrapper.OpCo.Update(OpCoMapper.SetOpCoMapper(entity));           
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.OpCo.FindByCondition(x => x.Opcoid == id).SingleAsync();
            _repositoryWrapper.OpCo.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Opcoid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.OpCo.FindByCondition(x => x.Opcoid == id)
                .Include(x => x.Damigrationstatus).FirstOrDefaultAsync();

            if(entity != null)
            {

                var daMigrationStatus = entity?.Damigrationstatus?.ToList();
                foreach (var toDelete in daMigrationStatus)
                {
                    _repositoryWrapper.DaMigrationStatusRepository.DeleteDeep(toDelete);
                }

                _repositoryWrapper.OpCo.DeleteDeep(entity);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Opcoid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotExists,
                    Data = entity.Opcoid
                };
            }
           
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var NFVITransition = _repositoryWrapper.NFVITransitionRepository.FindByCondition(x => x.Opcoid == id).Select(x => x.Nfvisitedesignation).ToArray();
            var VNFTransition = _repositoryWrapper.VNFTransition.FindByCondition(x => x.Opcoid == id).Select(x => x.Nfvisitedesignation).ToArray();
            var vBomInforEntity = _repositoryWrapper.VnfClusterInfoRepository.FindByCondition(x => x.Opcoid == id).Select(x => x.Vnfclusterinfoid.ToString()).ToArray();
            var designAspectforEntity = _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Opcoid == id).Select(x => x.Id.ToString()).ToArray();

            var Lcmengineering = _repositoryWrapper.Lcmengineering
               .FindByCondition(x => x.Opcoid == id)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x=>x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
               .Include(x => x.Opco)
               .Select(x => x.toDescription(_repositoryWrapper)).ToArray();

            //TODO - Verificare descrizioni da visualizzare
            var VolteKPI = _repositoryWrapper.VolteKPI.FindByCondition(x => x.Opcoid == id).Select(x => x.Month.ToString() +  " - " + x.Year.ToString()).ToArray();

            var plannedActivity = _repositoryWrapper.PlannedActivity
                    .FindByCondition(x => x.Opcoid == id)
                        .Include(x => x.Plannedactivityresource)
                        .Include(x => x.Activitystatus)
                        .Include(x => x.Deliverystatus)
                        .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                    .ToArray();
            var NetworkElementAsIs = _repositoryWrapper.NetworkElementAsIs
                                    .FindByCondition(x => x.Opcoid == id)
                                    .Select(x => NetworkElementAsIsMapper.Get(x).toDescription())
                                    .ToArray();
            var NetworkElementAsPlanned = _repositoryWrapper.NetworkElementAsPlanned
                                    .FindByCondition(x => x.Opcoid == id)
                                    .Select(x => NetworkElementAsPlannedMapper.Get(x, true).toDescription())
                                    .ToArray();

            var CbomReference = _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(x => x.Opcoid == id)
               .Select(x => x.Cnfclusterinfoid.ToString())
               .ToArray();


            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (NFVITransition.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "NFVI Transition", Values = NFVITransition });
            if (VNFTransition.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "VNF Transition", Values = VNFTransition });
            if (Lcmengineering.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "LCM Engineering", Values = Lcmengineering });
            if (VolteKPI.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Volte KPI", Values = VolteKPI });
            if (plannedActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivity });
            if (NetworkElementAsIs.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Is", Values = NetworkElementAsIs });
            if (NetworkElementAsPlanned.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Planned", Values = NetworkElementAsPlanned });
            if (vBomInforEntity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Virtual Bill Of Maintenance", Values = vBomInforEntity });
            if (CbomReference.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Cluster Bill Of Maintenance", Values = CbomReference });
            if (designAspectforEntity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Design Aspect", Values = designAspectforEntity });

            var entity = await _repositoryWrapper.OpCo.FindByCondition(x => x.Opcoid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "OpCo",
                        RecordName = entity.Opco,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();

        }


        public TipologicaGridDto GetCreatePage()
        {
            var opCoDto = new TipologicaGridDto();
            return opCoDto;
        }

        public TipologicaGridDto GetUpdatePage(short id)
        {
            var entity = OpCoMapper.GetOpCoMapper(_repositoryWrapper.OpCo.FindByCondition(x => x.Opcoid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = entity.OpCoId, Description = entity.OpCoDescription, LastModifiedBy = entity.ModificationUserEntity.Email, LastModified = entity.ModificationDate };
            return dto;
        }

       
    }
}
