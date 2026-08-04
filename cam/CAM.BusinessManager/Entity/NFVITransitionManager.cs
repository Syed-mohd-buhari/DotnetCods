using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NFVITransition;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.Entity
{
    public class NFVITransitionManager : GridBaseAsync<NFVITransition, NFVITransitionDtoGrid, NFVITransitionQueryDto, Nfvitransitions>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public NFVITransitionManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager,contextAccessor,wrappers,out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Nfvitransitions> ApplyFilterForOracleModel(NFVITransitionQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Nfvitransitions>();
            var predicateInner = PredicateBuilder.New<Nfvitransitions>();
            if (request.OpCo != null && request.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.OpCo)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (request.NfviTransitionId != null && request.NfviTransitionId.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.NfviTransitionId)
                    predicateInner.Or(x => x.Nfvitransitionid == item);
                predicateResult.And(predicateInner);
            }
            if (request.NfviSiteDesignation != null && request.NfviSiteDesignation.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.NfviSiteDesignation)
                    predicateInner.Or(x => x.Nfvisitedesignation == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                if (request.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModifiedValue.StartDate);

                if (request.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (request.StatusLabMC != null && request.StatusLabMC.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.StatusLabMC)
                    predicateInner.Or(x => x.Statuslabmcid == item);
                predicateResult.And(predicateInner);
            }
            if (request.StatusLabSC != null && request.StatusLabSC.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.StatusLabSC)
                    predicateInner.Or(x => x.Statuslabscid == item);
                predicateResult.And(predicateInner);
            }
            if (request.StatusLiveMC != null && request.StatusLiveMC.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.StatusLiveMC)
                    predicateInner.Or(x => x.Statuslivemcid == item);
                predicateResult.And(predicateInner);
            }
            if (request.StatusLiveSC != null && request.StatusLiveSC.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.StatusLiveSC)
                    predicateInner.Or(x => x.Statuslivescid == item);
                predicateResult.And(predicateInner);
            }
            if (request.NextStep != null && request.NextStep.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.NextStep)
                    predicateInner.Or(x => x.Nextstep == item);
                predicateResult.And(predicateInner);
            }
            if (request.Status12KSwitch != null && request.Status12KSwitch.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.Status12KSwitch)
                    predicateInner.Or(x => x.Status12kswitchid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Spare1Json != null && request.Spare1Json.Any())
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                foreach (var item in request.Spare1Json)
                    predicateInner.Or(x => x.Spare1json == item);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Nfvitransitions>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }



            return predicateResult;
        }

        public override List<NFVITransitionDtoGrid> CastObjectToDto(IQueryable<NFVITransition> request)
        {
            if (request == null || !request.Any () )
                return new List<NFVITransitionDtoGrid>();

            return request.Select(dto => new NFVITransitionDtoGrid()
            {
                NextStep = dto.NextStep,
                Status12KSwitch = dto.NFVIStatuses12KSwitch != null ? dto.NFVIStatuses12KSwitch.NFVIStatusDescription : string.Empty,
                NfviSiteDesignation = dto.NFVISiteDesignation,
                Spare1Json = dto.Spare1Json,
                LastModified = dto.ModificationDate,
                NfviTransitionId = dto.NFVITransitionId,
                OpCo = dto.OpCo != null ?  dto.OpCo.OpCoDescription : string.Empty,
                StatusLabMC = dto.NFVIStatusesLabMC != null ?  dto.NFVIStatusesLabMC.NFVIStatusDescription : string.Empty,
                StatusLabSC = dto.NFVIStatusesLabSC != null ? dto.NFVIStatusesLabSC.NFVIStatusDescription  : string.Empty,
                StatusLiveMC = dto.NFVIStatusesLiveMC != null ? dto.NFVIStatusesLiveMC.NFVIStatusDescription : string.Empty,
                StatusLiveSC = dto.NFVIStatusesLiveSC != null ? dto.NFVIStatusesLiveSC.NFVIStatusDescription : string.Empty,
                StatusLabMCColor = dto.NFVIStatusesLabMC != null ? dto.NFVIStatusesLabMC.Color : string.Empty,
                StatusLabSCColor = dto.NFVIStatusesLabSC != null ? dto.NFVIStatusesLabSC.Color : string.Empty,
                StatusLiveMCColor = dto.NFVIStatusesLiveMC != null ? dto.NFVIStatusesLiveMC.Color : string.Empty,
                StatusLiveSCColor = dto.NFVIStatusesLiveSC != null ? dto.NFVIStatusesLiveSC.Color : string.Empty,
                Status12KSwitchColor = dto.NFVIStatuses12KSwitch != null ? dto.NFVIStatuses12KSwitch.Color : string.Empty,
                Deleted = dto.Deleted,
                LastModifiedBy = dto.ModificationUserEntity != null ? dto.ModificationUserEntity.Email : string.Empty,
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<NFVITransition, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<NFVITransition, object>>[]>
            {
                ["nfviTransitionId"] = new Expression<Func<NFVITransition, object>>[] { p => p.NFVITransitionId },
                ["opCo"] = new Expression<Func<NFVITransition, object>>[] { p => p.OpCoId },
                ["status12KSwitch"] = new Expression<Func<NFVITransition, object>>[] { p => p.NFVIStatuses12KSwitch.NFVIStatusDescription },
                ["nextStep"] = new Expression<Func<NFVITransition, object>>[] { p => p.NextStep },
                ["nfviSiteDesignation"] = new Expression<Func<NFVITransition, object>>[] { p => p.NFVISiteDesignation },
                ["statusLabMC"] = new Expression<Func<NFVITransition, object>>[] { p => p.NFVIStatusesLabMC.NFVIStatusDescription },
                ["statusLabSC"] = new Expression<Func<NFVITransition, object>>[] { p => p.NFVIStatusesLabSC.NFVIStatusDescription },
                ["statusLiveMC"] = new Expression<Func<NFVITransition, object>>[] { p => p.NFVIStatusesLiveMC.NFVIStatusDescription },
                ["statusLiveSC"] = new Expression<Func<NFVITransition, object>>[] { p => p.NFVIStatusesLiveSC.NFVIStatusDescription },
                ["spare1Json"] = new Expression<Func<NFVITransition, object>>[] { p => p.Spare1Json },
                ["lastModified"] = new Expression<Func<NFVITransition, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<NFVITransition, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<NFVITransition> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "nfviTransitionId" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.NFVITransitionId.ToString()))
                : request.Where(x => x.NFVITransitionId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.NFVITransitionId.ToString())),
                "nfviSiteDesignation" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.NFVISiteDesignation))
                : request.Where(x => x.NFVISiteDesignation.Contains(propertyFilter)).Select(x => new FilterValueDto(x.NFVISiteDesignation)),
                "nextStep" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.NextStep))
                : request.Where(x => x.NextStep.Contains(propertyFilter)).Select(x => new FilterValueDto(x.NextStep)),
                "opCo" => string.IsNullOrEmpty(propertyFilter)
                   ? request.Select(p => new FilterValueDto
                   {
                       Text = p.OpCo.OpCoDescription,
                       Value = p.OpCoId.ToString()
                   }).Distinct()
                   : request
                       .Where(x =>
                            x.OpCo.OpCoDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.OpCo.OpCoDescription,
                                    Value = p.OpCoId.ToString()
                                }).Distinct(),
                "statusLabMC" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(p => new FilterValueDto
                {
                    Text = p.NFVIStatusesLabMC.NFVIStatusDescription,
                    Value = p.StatusLabMCId.ToString()
                }).Distinct()
                : request
                    .Where(x =>
                         x.NFVIStatusesLabMC.NFVIStatusDescription.Contains(
                             propertyFilter)).Select(p => new FilterValueDto
                             {
                                 Text = p.NFVIStatusesLabMC.NFVIStatusDescription,
                                 Value = p.StatusLabMCId.ToString()
                             }).Distinct(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

                "statusLiveMC" => string.IsNullOrEmpty(propertyFilter)
                 ? request.Select(p => new FilterValueDto
                 {
                     Text = p.NFVIStatusesLiveMC.NFVIStatusDescription,
                     Value = p.StatusLiveMCId.ToString()
                 }).Distinct()
                 : request
                     .Where(x => x.NFVIStatusesLiveMC.NFVIStatusDescription.Contains(
                              propertyFilter)).Select(p => new FilterValueDto
                              {
                                  Text = p.NFVIStatusesLiveMC.NFVIStatusDescription,
                                  Value = p.StatusLiveMCId.ToString()
                              }).Distinct(),

                "statusLabSC" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto
                    {
                        Text = p.NFVIStatusesLabSC.NFVIStatusDescription,
                        Value = p.StatusLabSCId.ToString(),
                    }).Distinct()
                    : request
                        .Where(x => x.NFVIStatusesLabSC.NFVIStatusDescription.Contains(
                            propertyFilter)).Select(p => new FilterValueDto
                            {
                                Text = p.NFVIStatusesLabSC.NFVIStatusDescription.ToString(),
                                Value = p.StatusLabSCId.ToString(),
                            }).Distinct(),
                "spare1Json" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto
                    {
                        Text = p.Spare1Json,
                        Value = p.Spare1Json,
                    }).Distinct()
                    : request
                        .Where(x => x.Spare1Json.Contains(
                            propertyFilter)).Select(p => new FilterValueDto
                            {
                                Text = p.Spare1Json,
                                Value = p.Spare1Json,
                            }).Distinct(),
                "statusLiveSC" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto
                    {
                        Text = p.NFVIStatusesLiveSC.NFVIStatusDescription,
                        Value = p.StatusLabSCId.ToString(),
                    }).Distinct()
                    : request
                        .Where(x => x.NFVIStatusesLiveSC.NFVIStatusDescription.Contains(
                            propertyFilter)).Select(p => new FilterValueDto
                            {
                                Text = p.NFVIStatusesLiveSC.NFVIStatusDescription.ToString(),
                                Value = p.StatusLabSCId.ToString(),
                            }).Distinct(),

                "status12KSwitch" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto
                    {
                        Text = p.NFVIStatuses12KSwitch.NFVIStatusDescription,
                        Value = p.Status12KSwitchId.ToString(),
                    }).Distinct()
                    : request
                        .Where(x => x.NFVIStatuses12KSwitch.NFVIStatusDescription.Contains(
                            propertyFilter)).Select(p => new FilterValueDto
                            {
                                Text = p.NFVIStatuses12KSwitch.NFVIStatusDescription,
                                Value = p.Status12KSwitchId.ToString(),
                            }).Distinct(),
            };
        }

        public override IQueryable<NFVITransition> PrepareQuery(NFVITransitionQueryDto request, ExpressionStarter<NFVITransition> predicateResult , ExpressionStarter<Nfvitransitions> oracleObject = null)
        {
            var query = oracleObject.IsStarted
              ? _repositoryWrapper.NFVITransitionRepository.FindByCondition(oracleObject, request.Deleted ?? !ConstantValueFilter.isTrue)
              : _repositoryWrapper.NFVITransitionRepository.FindAll(request.Deleted ?? !ConstantValueFilter.isTrue);
            return query
                .Include(m => m.CreationuserNavigation)
                .Include(m => m.ModificationuserNavigation)
                .Include(m=>m.Status12kswitch)
                .Include(m => m.Statuslabmc)
                .Include(m => m.Statuslabsc)
                .Include(m => m.Statuslivemc)
                .Include(m => m.Statuslivesc)
                .Include(m => m.Opco)
                .AsEnumerable().Select(p=> NFVITransitionMapper.GetNFVITransitionMapper(p)).AsQueryable();
        }
       
        public async Task<ResultDto> Add(NFVITransitionDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.NFVITransitionRepository.FindByCondition(
                x => x.Opcoid == dto.OpCoId
                && x.Nfvisitedesignation == dto.NfviSiteDesignation, ConstantValueFilter.isTrue).FirstOrDefaultAsync();


            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Nfvitransitionid
                };
            }

            dto.StatusLabMCId = dto.StatusLabMCId == 0 ? null : dto.StatusLabMCId;
            dto.StatusLabSCId = dto.StatusLabSCId == 0 ? null : dto.StatusLabSCId;
            dto.StatusLiveMCId = dto.StatusLiveMCId == 0 ? null : dto.StatusLiveMCId;
            dto.StatusLiveSCId = dto.StatusLiveSCId == 0 ? null : dto.StatusLiveSCId;
            dto.StatusLiveSCId = dto.Status12KSwitchId == 0 ? null : dto.Status12KSwitchId;


            NFVITransition entity = new NFVITransition()
            {
                NextStep = dto.NextStep,
                NFVISiteDesignation = dto.NfviSiteDesignation,
                Spare1Json = dto.Spare1Json,
                Status12KSwitchId = dto.Status12KSwitchId,
                OpCoId = dto.OpCoId,
                StatusLabMCId = dto.StatusLabMCId,
                StatusLabSCId = dto.StatusLabSCId,
                StatusLiveMCId = dto.StatusLiveMCId,
                StatusLiveSCId = dto.StatusLiveSCId
            };
            _repositoryWrapper.NFVITransitionRepository.Create(NFVITransitionMapper.SetNFVITransitionMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Restore(long id)
        {
            var entity = await _repositoryWrapper.NFVITransitionRepository.FindByCondition(x => x.Nfvitransitionid == id, ConstantValueFilter.isTrue).SingleAsync();

            var anotherEntityWithSameNaturalKeyExists = await _repositoryWrapper.NFVITransitionRepository.FindByCondition(
                 x => x.Opcoid == entity.Opcoid
                && x.Nfvisitedesignation == entity.Nfvisitedesignation
                   && x.Nfvitransitionid != entity.Nfvitransitionid).FirstOrDefaultAsync();

            if (anotherEntityWithSameNaturalKeyExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = anotherEntityWithSameNaturalKeyExists.Nfvitransitionid
                };
            }
            entity.Deleted = !ConstantValueFilter.isTrue;
            entity.Deletiondate = null;

            _repositoryWrapper.NFVITransitionRepository.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Nfvitransitionid
            };
        }


        public async Task<ResultDto> Update(NFVITransitionDtoUpdate dto)
        {
            var entityExists = await _repositoryWrapper.NFVITransitionRepository.FindByCondition(
                x => x.Opcoid == dto.OpCoId
                && x.Nfvisitedesignation == dto.NfviSiteDesignation
                   && x.Nfvitransitionid != dto.NfviTransitionId, ConstantValueFilter.isTrue).FirstOrDefaultAsync();


            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryUpdateExistsDeleted : ResultMessages.EntryUpdateExists,
                    Data = entityExists.Nfvitransitionid
                };
            }

            dto.StatusLabMCId = dto.StatusLabMCId == 0 ? null : dto.StatusLabMCId;
            dto.StatusLabSCId = dto.StatusLabSCId == 0 ? null : dto.StatusLabSCId;
            dto.StatusLiveMCId = dto.StatusLiveMCId == 0 ? null : dto.StatusLiveMCId;
            dto.StatusLiveSCId = dto.StatusLiveSCId == 0 ? null : dto.StatusLiveSCId;
            dto.Status12KSwitchId = dto.Status12KSwitchId == 0 ? null : dto.Status12KSwitchId;
            try
            {
                NFVITransition entity = NFVITransitionMapper.GetNFVITransitionMapper(await _repositoryWrapper.NFVITransitionRepository.FindByCondition(x => x.Nfvitransitionid == dto.NfviTransitionId).SingleAsync());
                if (dto.StatusLiveSCId == 1 && entity.StatusLiveSCId != 1)
                {
                    var vnfEntityToUpdate = _repositoryWrapper.VNFTransition.FindByCondition(
                    x => x.Opcoid == dto.OpCoId && x.Nfvisitedesignation == dto.NfviSiteDesignation,!ConstantValueFilter.isTrue, !ConstantValueFilter.isTrue)
                        .Include(x => x.Nfvibundleid);
                    foreach (var data in vnfEntityToUpdate)
                    {
                        var nfviBundleIDEntityNext = _repositoryWrapper.NFVIBundleID.FindByCondition(x => x.Order == data.Nfvibundleid.Order + 1).SingleOrDefault();
                        data.Nfvibundleidid = nfviBundleIDEntityNext != null ? nfviBundleIDEntityNext.Nfvibundleidid : data.Nfvibundleidid;
                        _repositoryWrapper.VNFTransition.Update(data);

                    }
                }

                entity.NextStep = dto.NextStep;
                entity.NFVISiteDesignation = dto.NfviSiteDesignation;
                entity.Spare1Json = dto.Spare1Json;
                entity.Status12KSwitchId = dto.Status12KSwitchId;
                entity.OpCoId = dto.OpCoId;
                entity.StatusLabMCId = dto.StatusLabMCId;
                entity.StatusLabSCId = dto.StatusLabSCId;
                entity.StatusLiveMCId = dto.StatusLiveMCId;
                entity.StatusLiveSCId = dto.StatusLiveSCId;

                _repositoryWrapper.NFVITransitionRepository.Update(NFVITransitionMapper.SetNFVITransitionMapper(entity));

                await _repositoryWrapper.SaveAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = entity.NFVITransitionId
                };
            }
            catch(Exception e) { var t = e.Message; return new ResultDto(); }
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.NFVITransitionRepository.FindByCondition(x => x.Nfvitransitionid == id).SingleAsync();

            _repositoryWrapper.NFVITransitionRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Nfvitransitionid
            };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.NFVITransitionRepository.FindByCondition(x => x.Nfvitransitionid == id).SingleAsync();

            _repositoryWrapper.NFVITransitionRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Nfvitransitionid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            var entity = await _repositoryWrapper.NFVITransitionRepository.FindByCondition(x => x.Nfvitransitionid == id)
               .Include(x => x.Opco).SingleAsync();


            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto
                    {
                        EntityName = "NFVI Transition",
                        RecordName = entity.Opco.Opco + " - " + entity.Nfvisitedesignation,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public NFVITransitionDtoCreate GetCreatePage(List<short> _opcoList)
        {
            var opocResource = (_opcoList != null) ? _repositoryWrapper.OpCo
                .FindByCondition(x => _opcoList.Contains(x.Opcoid)) : _repositoryWrapper.OpCo.FindAll();
            var statusResource = _repositoryWrapper.NFVIStatus.FindAll();

            var nfviSiteDesignationResource = _repositoryWrapper.VNFTransition.FindAll().ToList().Select(x => x.Nfvisitedesignation).Distinct().ToList();
            var model = new NFVITransitionDtoCreate
            {
                OpCoResource = opocResource.ToDictionary(x => x.Opcoid, x => x.Opco),
                //NfviSiteDesignationResource = nfviSiteDesignationResource,
                Status12KSwitchResource = statusResource.ToDictionary(x => x.Nfvistatusid, x => new RelatedResource()
                {
                    Id = x.Color,
                    Value = x.Nfvistatus,
                }),
                StatusLabMCResource = statusResource.ToDictionary(x => x.Nfvistatusid, x => new RelatedResource()
                {
                    Id = x.Color,
                    Value = x.Nfvistatus,
                }),

                StatusLabSCResource = statusResource.ToDictionary(x => x.Nfvistatusid, x => new RelatedResource()
                {
                    Id = x.Color,
                    Value = x.Nfvistatus,
                }),
                StatusLiveMCResource = statusResource.ToDictionary(x => x.Nfvistatusid, x => new RelatedResource()
                {
                    Id = x.Color,
                    Value = x.Nfvistatus,
                }),
                StatusLiveSCResource = statusResource.ToDictionary(x => x.Nfvistatusid, x => new RelatedResource()
                {
                    Id = x.Color,
                    Value = x.Nfvistatus,
                }),

            };
            return model;


        }

        public NFVITransitionDtoUpdate GetUpdatePage(long id, List<short> _opcoList)
        {
            var entity = NFVITransitionMapper.GetNFVITransitionMapper(_repositoryWrapper.NFVITransitionRepository.FindByCondition(x => x.Nfvitransitionid == id, ConstantValueFilter.isTrue)
                .Include(x => x.ModificationuserNavigation).Single());
            var dto = new NFVITransitionDtoUpdate
            {
                NfviTransitionId = entity.NFVITransitionId,
                LastModified = entity.ModificationDate,
                Spare1Json = entity.Spare1Json,
                OpCoId = entity.OpCoId,
                NfviSiteDesignation = entity.NFVISiteDesignation,
                NextStep = entity.NextStep,
                Status12KSwitchId = entity.Status12KSwitchId,
                StatusLabMCId = entity.StatusLabMCId,
                StatusLabSCId = entity.StatusLabSCId,
                StatusLiveMCId = entity.StatusLiveMCId,
                StatusLiveSCId = entity.StatusLiveSCId,
                LastModifiedBy = entity.ModificationUserEntity.Email
            };

            #region lookUp
            var opocResource = (_opcoList != null) ? _repositoryWrapper.OpCo
                .FindByCondition(x => _opcoList.Contains(x.Opcoid)) : _repositoryWrapper.OpCo.FindAll();
            dto.OpCoResource = opocResource.ToDictionary(x => x.Opcoid, x => x.Opco);
            if (!dto.OpCoResource.ContainsKey(dto.OpCoId))
            {
                var opco = _repositoryWrapper.OpCo.FindByCondition(
                    x => x.Opcoid == dto.OpCoId, includeDeleted: ConstantValueFilter.isTrue).SingleOrDefault();
                if (opco != null)
                {
                    dto.OpCoResource.Add(opco.Opcoid, opco.Opco);
                }

            }
            var statusResource = _repositoryWrapper.NFVIStatus.FindAll();

            dto.StatusLabMCResource = statusResource.ToDictionary(x => x.Nfvistatusid, x => new RelatedResource()
            {
                Id = x.Color,
                Value = x.Nfvistatus,
            });
            if (dto.StatusLabMCId.HasValue &&
                !dto.StatusLabMCResource.ContainsKey(dto.StatusLabMCId.Value))
            {
                var data = _repositoryWrapper.NFVIStatus.FindByCondition(
                    x => x.Nfvistatusid == dto.StatusLabMCId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.StatusLabMCResource.Add(data.Nfvistatusid, new RelatedResource()
                    {
                        Id = data.Color,
                        Value = data.Nfvistatus
                    });
                }
            }
            dto.StatusLabSCResource = statusResource.ToDictionary(x => x.Nfvistatusid, x => new RelatedResource()
            {
                Id = x.Color,
                Value = x.Nfvistatus,
            });
            if (dto.StatusLabSCId.HasValue &&
                !dto.StatusLabSCResource.ContainsKey(dto.StatusLabSCId.Value))
            {
                var data = _repositoryWrapper.NFVIStatus.FindByCondition(
                    x => x.Nfvistatusid == dto.StatusLabSCId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.StatusLabSCResource.Add(data.Nfvistatusid, new RelatedResource()
                    {
                        Id = data.Color,
                        Value = data.Nfvistatus
                    });
                }
            }
            dto.StatusLiveMCResource = statusResource.ToDictionary(x => x.Nfvistatusid, x => new RelatedResource()
            {
                Id = x.Color,
                Value = x.Nfvistatus,
            });
            if (dto.StatusLiveMCId.HasValue && !dto.StatusLiveMCResource.ContainsKey(dto.StatusLiveMCId.Value))
            {
                var data = _repositoryWrapper.NFVIStatus.FindByCondition(
                    x => x.Nfvistatusid == dto.StatusLiveMCId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.StatusLiveMCResource.Add(data.Nfvistatusid, new RelatedResource()
                    {
                        Id = data.Color,
                        Value = data.Nfvistatus
                    });
                }
            }
            dto.StatusLiveSCResource = statusResource.ToDictionary(x => x.Nfvistatusid, x => new RelatedResource()
            {
                Id = x.Color,
                Value = x.Nfvistatus,
            });
            if (dto.StatusLiveSCId.HasValue && !dto.StatusLiveSCResource.ContainsKey(dto.StatusLiveSCId.Value))
            {
                var data = _repositoryWrapper.NFVIStatus.FindByCondition(
                    x => x.Nfvistatusid == dto.StatusLiveSCId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.StatusLiveSCResource.Add(data.Nfvistatusid, new RelatedResource()
                    {
                        Id = data.Color,
                        Value = data.Nfvistatus
                    });
                }
            }
            dto.Status12KSwitchResource = statusResource.ToDictionary(x => x.Nfvistatusid, x => new RelatedResource()
            {
                Id = x.Color,
                Value = x.Nfvistatus,
            });
            if (dto.Status12KSwitchId.HasValue && !dto.Status12KSwitchResource.ContainsKey(dto.Status12KSwitchId.Value))
            {
                var data = _repositoryWrapper.NFVIStatus.FindByCondition(
                    x => x.Nfvistatusid == dto.StatusLiveSCId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.Status12KSwitchResource.Add(data.Nfvistatusid, new RelatedResource()
                    {
                        Id = data.Color,
                        Value = data.Nfvistatus
                    });
                }
            }
            //var nfviSiteDesignationResource = _repositoryWrapper.VNFTransition.FindAll().Select(x => x.NFVISiteDesignation).Distinct().ToList();

            #endregion
            return dto;
        }

       
    }
}
