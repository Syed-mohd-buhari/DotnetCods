using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignComponentFamily;
using CAM.DataTransferObjects.Entita.Identity;
using CAM.DataTransferObjects.Entita.IdentityAsIs;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.DataTransferObjects.Entita.ResourceKeyMaster;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static CAM.Enum.ResourceTypeEnum;

namespace CAM.BusinessManager.Entity
{
    public class IdentityAsIsManager : GridBaseAsync<IdentityAsIs, IdentityAsIsDtoGrid, IdentityAsIsDtoQuery, Identitiesasis>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _customColumnManager;
        private readonly ResourceKeyMasterManager _resourceKeyMasterManager;
        private readonly DesignComponentFamilyLifeCycleManager _designComponentFamilyLifeCycleManager;
        private readonly IMapper _mapper;
        private readonly CommonManager _commonManager;

        public IdentityAsIsManager(CommonManager commonManager,IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, IMapper mapper, ResourceKeyMasterManager resourceKeyMasterManager, DesignComponentFamilyLifeCycleManager designComponentFamilyLifeCycleManager) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _customColumnManager = columnManager;
            _mapper = mapper;
            _resourceKeyMasterManager = resourceKeyMasterManager;
            _designComponentFamilyLifeCycleManager = designComponentFamilyLifeCycleManager;
            _commonManager= commonManager;
        }


        public override ExpressionStarter<Identitiesasis> ApplyFilterForOracleModel(IdentityAsIsDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Identitiesasis>(true);
            var predicateInner = PredicateBuilder.New<Identitiesasis>(true);

            if (request.Value != null && request.Value.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.Value)
                    predicateInner.Or(x => x.Value == item);
                predicateResult.And(predicateInner);
            }

            if (request.InterfaceName != null && request.InterfaceName.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.InterfaceName)
                    predicateInner.Or(x => x.Interfacename == item);
                predicateResult.And(predicateInner);
            }

            if (request.InterfaceType != null && request.InterfaceType.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.InterfaceType)
                    predicateInner.Or(x => x.Interfacetype == item);
                predicateResult.And(predicateInner);
            }

            if (request.ResourceKey != null && request.ResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.ResourceKey)
                    predicateInner.Or(x => x.Resourcekey == item);
                predicateResult.And(predicateInner);
            }


            if (request.PreviousResourceKey != null && request.PreviousResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.PreviousResourceKey)
                    predicateInner.Or(x => x.Previousresourcekey == item);
                predicateResult.And(predicateInner);
            }



            if (request.CategoryDescription != null && request.CategoryDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.CategoryDescription)
                    predicateInner.Or(x => x.Category.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.ClassDescription != null && request.ClassDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.ClassDescription)
                    predicateInner.Or(x => x.Class.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.AssetId != null && request.AssetId.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.AssetId)
                    predicateInner.Or(x => x.Assetid == item);
                predicateResult.And(predicateInner);
            }

            if (request.TypeDescription != null && request.TypeDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.TypeDescription)
                    predicateInner.Or(x => x.Type.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            if (request.DesignComponentFamily != null && request.DesignComponentFamily.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.DesignComponentFamily)
                {
                    predicateInner.Or(x => x.Asset.Designcomponent.Designcomponentfamily.Designcomponentfamilyid == item);
                }
                predicateResult.And(predicateInner);
            }

            if (request.AssetName != null && request.AssetName.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.AssetName)
                    predicateInner.Or(x => x.Asset.Elementname == item);
                predicateResult.And(predicateInner);
            }

            //if (request.VerticalId != null && request.VerticalId.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Identitiesasis>();
            //    foreach (var item in request.VerticalId)
            //        predicateInner.Or(x => x.Asset.Designcomponent.Systemtype.Verticalresponsibleid == item);
            //    predicateResult.And(predicateInner);
            //}
            if (request.VerticalName != null && request.VerticalName.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.VerticalName)
                    predicateInner.Or(x => x.Asset.Networkelementasplannedsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser
                    .Any(m => m.Organisation.Vertical.Verticalresponsibleid.ToString() == item && m.Deleted == false /*&& m.Opcoid == x.Opcoid*/)));
                predicateResult.And(predicateInner);
            }
            if (request.OpCoId != null && request.OpCoId.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.OpCoId)
                    predicateInner.Or(x => x.Asset.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (request.OpCo != null && request.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Identitiesasis>();
                foreach (var item in request.OpCo)
                    predicateInner.Or(x => x.Asset.Opcoid.ToString() == item);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<IdentityAsIsDtoGrid> CastObjectToDto(IQueryable<IdentityAsIs> request)
        {
            return request.Select(dto => new IdentityAsIsDtoGrid()
            {
                Id = (short)dto.Id,
                Value = dto.Value,
                CategoryId = (short)dto.CategoryId,
                PreviousResourceKey = dto.PreviousResourceKey,
                ResourceKey = dto.ResourceKey,
                TypeId = (short)dto.TypeId,
                AssetId = (short)dto.AssetId,
                ClassId = (short)dto.ClassId,
                ClassDescription = dto.Class.Description,
                CategoryDescription = dto.Category.Description,
                TypeDescription = dto.Type.Description,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                AssetName = dto.Asset.ElementName,
                DesignComponentFamily = dto.Asset.DesignComponent.DesignComponentFamily.Description

            }).ToList();
        }

        public override Dictionary<string, Expression<Func<IdentityAsIs, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<IdentityAsIs, object>>[]>
            {
                ["id"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Id },
                ["value"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Value },
                ["categoryDescription"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Category != null ? p.Category.Description : default },
                ["classDescription"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Class != null ? p.Class.Description : default },
                ["typeDescription"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Type != null ? p.Type.Description : default },
                ["assetId"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.AssetId },
                ["resourceKey"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.ResourceKey },
                ["previousResourceKey"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.PreviousResourceKey },
                ["assetName"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.ElementName },
                ["designComponentFamily"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.DesignComponent.DesignComponentFamily.DCFName(_repositoryWrapper) },
                ["lastModifiedBy"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.ModificationUserEntity.Email },
                ["lastModifiedValue"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.ModificationDate },
                ["verticalId"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.DesignComponent.SystemType.VerticalResponsibleId },
                ["verticalName"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.DesignComponent.SystemType.VerticalResponsible.VerticalResponsibleDescription },

                ["opCoId"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.OpCoId },
                ["opCo"] = new Expression<Func<IdentityAsIs, object>>[] { p => p.Asset.OpCo.OpCoDescription },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<IdentityAsIs> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "id" => request.Select(x => new FilterValueDto(x.Id.ToString())),
                "value" => request.Where(x => x.Value != null).Select(x => new FilterValueDto(x.Value)),
                "resourceKey" => request.Where(x => x.ResourceKey != null).Select(x => new FilterValueDto(x.ResourceKey)),
                "previousResourceKey" => request.Where(x => x.PreviousResourceKey != null).Select(x => new FilterValueDto(x.PreviousResourceKey)),
                "categoryId" => request.Select(x => new FilterValueDto(x.CategoryId)),
                "classId" => request.Select(x => new FilterValueDto(x.ClassId)),
                "typeId" => request.Select(x => new FilterValueDto(x.TypeId)),
                "categoryDescription" => request.Where(x => x.CategoryId != null).Select(x => new FilterValueDto(x.Category.Description)),
                "classDescription" => request.Where(x => x.ClassId != null).Select(x => new FilterValueDto(x.Class.Description)),
                "typeDescription" => request.Where(x => x.TypeId != null).Select(x => new FilterValueDto(x.Type.Description)),
                "assetId" => request.Where(x => x.AssetId != 0).Select(x => new FilterValueDto(x.AssetId)),
                "lastModifiedBy" => request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "assetName" => request.Where(x => x.AssetId != 0).Select(x => new FilterValueDto(x.Asset.ElementName)),
                "designComponentFamily" => request.Where(x => x.AssetId != 0).Select(x =>
                new FilterValueDto
                {
                    Text = x.Asset.DesignComponent.DesignComponentFamily.DCFName(_repositoryWrapper),
                    Value = x.Asset.DesignComponent.DesignComponentFamilyId.ToString()
                }).Distinct().ToList(),
                "interfaceType" => request.Select(x => new FilterValueDto(x.InterfaceType)),
                "interfaceName" => request.Select(x => new FilterValueDto(x.InterfaceName)),
                "verticalId" => request.Where(x => x.AssetId != 0).Select(x =>
                new FilterValueDto
                {
                    Text = x.Asset.DesignComponent.SystemType.VerticalResponsible.VerticalResponsibleDescription,
                    Value = x.Asset.DesignComponent.SystemType.VerticalResponsibleId.ToString()
                }).Distinct().ToList(),
                "verticalName" => string.IsNullOrEmpty(propertyFilter)
                                                    ? request.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                                                    .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                                                    {
                                                        Text = t.Value,
                                                        Value = t.Key.ToString()
                                                    }))?.Distinct()?.ToList()
                                                    .Concat(request.Where(x => x.Asset.NetworkElementAsPlannedSubDomainSpoc != null && x.Asset.NetworkElementAsPlannedSubDomainSpoc.Count() <= 0)
                      .Select(x =>

                         new FilterValueDto
                         {
                             Text = "---",
                             Value = "yes",
                         }
                      )).Distinct().ToList()
                                                    :
                    request.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                                                    .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                                                    {
                                                        Text = t.Value,
                                                        Value = t.Key.ToString()
                                                    })).Where(x => x.Text.Contains(propertyFilter))?.Distinct()?.ToList()
                                                    .Concat(request.Where(x => x.Asset.NetworkElementAsPlannedSubDomainSpoc != null && x.Asset.NetworkElementAsPlannedSubDomainSpoc.Count() <= 0)
                      .Select(x =>

                         new FilterValueDto
                         {
                             Text = "---",
                             Value = "yes",
                         }
                      )).Distinct().ToList(),
                //"verticalName" => request.Where(x => x.AssetId != 0).Select(x =>
                //new FilterValueDto
                //{

                //    Text = x.Asset.DesignComponent.SystemType.VerticalResponsible.VerticalResponsibleDescription,
                //    Value = x.Asset.DesignComponent.SystemType.VerticalResponsibleId.ToString()
                //}).Distinct().ToList(),
                "opCoId" => request.Where(x => x.AssetId != 0).Select(x =>
               new FilterValueDto
               {
                   Text = x.Asset.OpCo.OpCoDescription,
                   Value = x.Asset.OpCoId.ToString()
               }).Distinct().ToList(),
                "opCo" => request.Where(x => x.AssetId != 0).Select(x =>
                new FilterValueDto
                {
                    Text = x.Asset.OpCo.OpCoDescription,
                    Value = x.Asset.OpCoId.ToString()
                }).Distinct().ToList(),
                //(x.Asset.DesignComponent.DesignComponentFamily.toDesignComponentFamilyName(_repositoryWrapper))),
            };
        }

        public override IQueryable<IdentityAsIs> PrepareQuery(IdentityAsIsDtoQuery request, ExpressionStarter<IdentityAsIs> predicateResult, ExpressionStarter<Identitiesasis> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.IdentityAsIsRepository.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.IdentityAsIsRepository.FindAll();

            var entityQuery=query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .Include(x => x.Category)
                .Include(x => x.Class)
                .Include(x => x.Type)
                .Include(x => x.Asset).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                //.Include(x => x.Asset).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Verticalresponsible)
                .Include(x => x.Asset).ThenInclude(x => x.Opco)
                .Include(x => x.Asset).ThenInclude(x => x.Networkelementasplannedsubdomainspoc)
                .AsEnumerable().Select(p => IdentityAsIsMapper.Get(p)).AsQueryable();
           
            var data = entityQuery.ToList();

            foreach (var item in data)
            {
                if (item.Asset.NetworkElementAsPlannedSubDomainSpoc?.Any() == true)
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.Asset.NetworkElementAsPlannedSubDomainSpoc?.Select(x => x?.Subdomainspocid).ToList(),
                       0, false, true)?.Distinct()?.ToDictionary(m => Convert.ToInt16(m.Value), m => m.Text);
            }

            return data.AsQueryable();
            
        }

        public QueryResultDto<IdentityAsIsDtoGrid> FindWithCondition(IdentityAsIsDtoQuery buildFilterDto)
        {
            var predicateResult = ApplyFilterForOracleModel(buildFilterDto);
            if (buildFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries

            predicateResult = predicateResult.And(x => x.Asset.Designcomponent.Deleted == false);

            var rtn = new QueryResultDto<IdentityAsIsDtoGrid>(new GenerateRenderForGrid<IdentityAsIsDtoGrid>(_customColumnManager))
            {
                TotalItems = _repositoryWrapper.IdentityAsIsRepository.Count(predicateResult)
            };

            var query = _repositoryWrapper.IdentityAsIsRepository.FindByCondition(predicateResult, buildFilterDto.Deleted ?? false)
                            .Include(x => x.Category)
                            .Include(x => x.Class)
                            .Include(x => x.Type)
                            .Include(x => x.ModificationuserNavigation)
                            .Include(x => x.CreationuserNavigation)
                            .Include(x => x.Asset).ThenInclude(x => x.Designcomponent)
                            .ThenInclude(x => x.Designcomponentfamily)
                            .Include(x => x.Asset).ThenInclude(x => x.Designcomponent)
                            //.ThenInclude(x => x.Systemtype).ThenInclude(x => x.Verticalresponsible)
                            .Include(x => x.Asset).ThenInclude(x => x.Opco)
                            .Include(x => x.Asset).ThenInclude(x => x.Networkelementasplannedsubdomainspoc)
                            .AsEnumerable()
                            .Select(p => IdentityAsIsMapper.Get(p)).AsQueryable()
                            .ApplyOrdering(buildFilterDto, GetColumnsMap()).ApplyPaging(buildFilterDto);

            var data = query.ToList();

            if (buildFilterDto.PrincipalId != 0)
            {
                var exist = data.Any(x => x.Id == buildFilterDto.PrincipalId);
                if (!exist)
                {
                    var addedResource = _repositoryWrapper.IdentityAsIsRepository.FindAll(true)
                            .Include(x => x.Category)
                            .Include(x => x.Class)
                            .Include(x => x.Type)
                            .Include(x => x.ModificationuserNavigation)
                            .Include(x => x.CreationuserNavigation)
                            .Include(x => x.Asset).ThenInclude(x => x.Designcomponent)
                            .ThenInclude(x => x.Designcomponentfamily)
                            .Include(x => x.Asset).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                            .Include(x => x.Asset).ThenInclude(x => x.Opco)
                            .Include(x => x.Asset).ThenInclude(x => x.Networkelementasplannedsubdomainspoc)
                        .Single(x => x.Id == buildFilterDto.PrincipalId);
                    data.Add(IdentityAsIsMapper.Get(addedResource));
                }
            }
            foreach (var item in data)
            {
                if (item.Asset.NetworkElementAsPlannedSubDomainSpoc?.Any() == true)
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.Asset.NetworkElementAsPlannedSubDomainSpoc?.Select(x => x?.Subdomainspocid).ToList(),
                        0, false, true)?.Distinct()?.ToDictionary(m => Convert.ToInt16(m.Value), m => m.Text);

            }

            var result = _mapper.Map<IEnumerable<IdentityAsIsDtoGrid>>(data);
            rtn.Items = result.ToArray();
            return rtn;
        }

        public async Task<ResultDto> Add(IdentityAsIsDtoCreate dto)
        {
            IdentityAsIs entity = new IdentityAsIs()
            {
                Id = dto.Id,
                Value = dto.Value,
                ResourceKey = dto.ResourceKey,
                PreviousResourceKey = dto.PreviousResourceKey,
                CategoryId = dto.CategoryId,
                ClassId = dto.ClassId,
                TypeId = dto.TypeId,
                AssetId = dto.AssetId,
                InterfaceName = dto.InterfaceName,
                InterfaceType = dto.InterfaceType
            };


            //await CreateorupdateIdentityResourceKey(dto);

            string resourceKey =  _designComponentFamilyLifeCycleManager.CreateorupdateIdentityResourceKey(dto).Result;
            entity.ResourceKey = resourceKey;

            _repositoryWrapper.IdentityAsIsRepository.Create(IdentityAsIsMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(IdentityAsIsDtoUpdate dto)
        {

            IdentityAsIs entity = new IdentityAsIs()
            {
                Id = dto.Id,
                Value = dto.Value,
                ResourceKey = dto.ResourceKey,
                PreviousResourceKey = dto.PreviousResourceKey,
                CategoryId = dto.CategoryId,
                ClassId = dto.ClassId,
                TypeId = dto.TypeId,
                AssetId = dto.AssetId,
                InterfaceName = dto.InterfaceName,
                InterfaceType = dto.InterfaceType
            };

            //Start of LCM Part3 Implementation
            var identityexists = _repositoryWrapper.IdentityAsIsRepository.FindByCondition
            (x => x.Id == dto.Id).FirstOrDefault();
            var elementname = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == dto.AssetId)
             .Select(x => x.Elementname).FirstOrDefault();
            if (identityexists != null)
            {
                if (identityexists.Resourcekey != dto.ResourceKey)
                {
                    ResourceKeyMasterDto IdentityKey = new ResourceKeyMasterDto
                    {
                        OpCoId = dto.OpcoId,
                        ResourceKey = dto.ResourceKey,
                        ElementName = elementname,
                        ResourceTypesId = (int)ResourceTypesKey.Identity,
                    };
                    await _resourceKeyMasterManager.CreateOrUpdateResourceKey(IdentityKey);

                    entity.PreviousResourceKey = identityexists.Resourcekey;

                    var dcfExists = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(
                            x => x.Opcoid == dto.OpcoId && x.Currentdetails == elementname).FirstOrDefault();

                    if (dcfExists != null)
                    {
                        dcfExists.Previousresourcekey = entity.PreviousResourceKey;
                        dcfExists.Resourcekey = dto.ResourceKey;
                        _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Update(dcfExists);
                    }

                }
            }
            //end of LCM Part3 Implementation

            _repositoryWrapper.IdentityAsIsRepository.Update(IdentityAsIsMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.IdentityAsIsRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.IdentityAsIsRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.IdentityAsIsRepository.FindByCondition(x => x.Id == id).SingleAsync();

            var elementname = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == entity.Assetid)
                                .Select(x => x.Elementname).Single();

            _repositoryWrapper.IdentityAsIsRepository.DeleteDeep(entity);

            await _repositoryWrapper.SaveAsync();
            /// LCM Part3 Requirements.
            if (entity.Resourcekey != null)
            {
                _designComponentFamilyLifeCycleManager.setResourceKeyStatus(entity.Resourcekey, (int)ResourceTypesKey.Identity);
                _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforIdentityDeletion(entity);
            }

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public ResultDto GetRelatedRecords(short id)
        {

            return new ResultDto();
        }
        

        public IdentityAsIsDtoCreate GetCreatePage(List<short> _opcoList, List<int> _verticalList)
        {
            return new IdentityAsIsDtoCreate()
            {
                CategoryResource = _repositoryWrapper.CategoryRepository.FindAll().ToDictionary(x => x.Id, x => x.Description),
                //OpCoResource = (_opcoList == null) ? _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco) :
                //            _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid))
                //            .ToDictionary(x => x.Opcoid, x => x.Opco),
                OpCoResource = (_opcoList != null && _opcoList.Any() == true) ?
                                 _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid)).ToDictionary(x => x.Opcoid, x => x.Opco)
                                 : _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco),
                #region    //Ticket 595 -#590 - Vertical Filter to be applied on design component dropdown's in LC, PA, DA etc.,

                //DcfResource = _repositoryWrapper.DesignComponentFamily.FindAll()
                //.ToDictionary(x => x.Designcomponentfamilyid, x => x.toDesignComponentFamilyName(_repositoryWrapper)),

                DcfResource = _repositoryWrapper.DesignComponentFamily.FindAll().ToList()
                .Select(x => new { x.Designcomponentfamilyid, Name = x.DCFName(_repositoryWrapper) })
                .Where(x => !string.IsNullOrEmpty(x.Name)).ToDictionary(y => y.Designcomponentfamilyid, y => y.Name),


                #endregion
                InterfaceTypes = System.Enum.GetValues(typeof(InterfaceTypesEnum)).Cast<InterfaceTypesEnum>().ToDictionary(e => (int)e, e => e.ToString())
            };
        }

        public IdentityAsIsDtoUpdate GetUpdatePage(short id, List<short> _opcoList, List<int> _verticalList)
        {
            var model = _repositoryWrapper.IdentityAsIsRepository
                                    .FindByCondition(x => x.Id == id)
                                    .Include(x => x.ModificationuserNavigation)
                                    .Include(x => x.Asset.Opco)
                                    .Include(x => x.Asset.Designcomponent.Designcomponentfamily)
                                    .Single();
            var opcoId = model.Asset.Opcoid;
            var dcfId = model.Asset.Designcomponent.Designcomponentfamilyid;
            var entity = IdentityAsIsMapper.Get(model);
            var categoryResource = _repositoryWrapper.CategoryRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);
            //var opCoResource = (_opcoList == null) ? _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco) :
            //                _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid)).ToDictionary(x => x.Opcoid, x => x.Opco);
            var opCoResource = (_opcoList != null && _opcoList.Any() == true) ?
                                 _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid)).ToDictionary(x => x.Opcoid, x => x.Opco)
                                 : _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            #region    //Ticket 595 -#590 - Vertical Filter to be applied on design component dropdown's in LC, PA, DA etc.,

            // var DcfResource = _repositoryWrapper.DesignComponentFamily.FindAll().ToDictionary(x => x.Designcomponentfamilyid, x => x.toDesignComponentFamilyName(_repositoryWrapper));


            var DcfResource = _repositoryWrapper.DesignComponentFamily.FindAll().ToList()
                .Select(x => new { x.Designcomponentfamilyid, Name = x.DCFName(_repositoryWrapper) })
                .Where(x => !string.IsNullOrEmpty(x.Name)).ToDictionary(y => y.Designcomponentfamilyid, y => y.Name);



            #endregion


            var classResource = _repositoryWrapper.ClassRepository.FindByCondition(x => x.Categoryid == entity.CategoryId).ToDictionary(x => x.Id, x => x.Description);
            var typeResource = _repositoryWrapper.TypeRepository.FindByCondition(x => x.Classid == entity.ClassId).ToDictionary(x => x.Id, x => x.Description);
            var assetResource = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == opcoId && x.Designcomponent.Designcomponentfamilyid == dcfId)
                                                        .ToDictionary(x => x.Networkelementasplannedid, x => x.Elementname);
            var InterfaceTypeResource = System.Enum.GetValues(typeof(InterfaceTypesEnum)).Cast<InterfaceTypesEnum>().ToDictionary(e => (int)e, e => e.ToString());

            return new IdentityAsIsDtoUpdate
            {
                Id = (short)entity.Id,
                Value = entity.Value,
                ResourceKey = entity.ResourceKey,
                PreviousResourceKey = entity.PreviousResourceKey,
                CategoryId = entity.CategoryId,
                ClassId = entity.ClassId,
                TypeId = entity.TypeId,
                AssetId = entity.AssetId,
                OpcoId = opcoId,
                DcfId = dcfId,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email,
                CategoryResource = categoryResource,
                OpCoResource = opCoResource,
                DcfResource = DcfResource,
                ClassResource = classResource,
                TypeResource = typeResource,
                AssetResource = assetResource,
                InterfaceTypes = InterfaceTypeResource,
                InterfaceName = entity.InterfaceName,
                InterfaceType = entity.InterfaceType
            };

        }

        public async Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, IdentityAsIsDtoQuery buildFilterDto, bool isAdmin)
        {
            buildFilterDto.PageSize = 0;
            buildFilterDto.Page = 1;
            var predicateResult = ApplyFilterForOracleModel(buildFilterDto);
            var query = PrepareQuery(buildFilterDto,null, predicateResult);
            var data = (await GetFilterValueList(query, propertyName, propertyFilter)).Distinct().ToList();
            if (!isAdmin && (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                data = data.Where(x => buildFilterDto.VerticalName.Contains(x.Value.ToString())).ToList();
            }
            return data;
        }
    }
}

