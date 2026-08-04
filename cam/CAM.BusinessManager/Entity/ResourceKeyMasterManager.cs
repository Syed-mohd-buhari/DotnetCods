using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ResourceKeyMaster;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.ResourcesKey;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CAM.Enum.ResourceTypeEnum;

namespace CAM.BusinessManager.Entity
{
    public class ResourceKeyMasterManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly IResourceKey _resourceKey;
        private GridCustomColumnManager _manager;
        private readonly CommonManager _commonManager;

        public ResourceKeyMasterManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper, IResourceKey resourceKey, CommonManager commonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _resourceKey = resourceKey;
            _commonManager = commonManager;
        }

        #region UIMemberFunctions

        public QueryResultDto<ResourceKeyMasterDtoGrid> FindWithCondition(ResourceKeyMasterQueryDto resourceKeyMasterFilterDto)
        {
            var predicateResult = ApplyFilter(resourceKeyMasterFilterDto);
            if (resourceKeyMasterFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            var rtn = new QueryResultDto<ResourceKeyMasterDtoGrid>(new GenerateRenderForGrid<ResourceKeyMasterDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.ResourceKeyMaster.Count(predicateResult) : _repositoryWrapper.ResourceKeyMaster.Count(),
            };
            var query = GetQuery(predicateResult, resourceKeyMasterFilterDto.Deleted ?? false).ApplyOrdering(resourceKeyMasterFilterDto, GetColumnsMap()).ApplyPaging(resourceKeyMasterFilterDto);
            var data = query.ToList();


            IEnumerable<ResourceKeyMasterDtoGrid> ResourceKeyMasterResult;


            ResourceKeyMasterResult = _mapper.Map<IEnumerable<ResourceKeyMasterDtoGrid>>(data);

            rtn.Items = ResourceKeyMasterResult.ToArray();
            return rtn;
        }
        private static ExpressionStarter<Resourcekeymaster> ApplyFilter(ResourceKeyMasterQueryDto resourceKeyMasterQueryFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Resourcekeymaster>();
            var predicateInner = PredicateBuilder.New<Resourcekeymaster>();

            if (resourceKeyMasterQueryFilterDto.ResourceKeyMasterId != null && resourceKeyMasterQueryFilterDto.ResourceKeyMasterId.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.ResourceKeyMasterId)
                    predicateInner.Or(x => x.Resourcekeymasterid == item);
                predicateResult.And(predicateInner);
            }

            if (resourceKeyMasterQueryFilterDto.OpCoId != null && resourceKeyMasterQueryFilterDto.OpCoId.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.OpCoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.LifeCycleId != null && resourceKeyMasterQueryFilterDto.LifeCycleId.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.LifeCycleId)
                    predicateInner.Or(x => x.Lifecycleid == item);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.OpCo != null && resourceKeyMasterQueryFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.OpCo)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.ResourceKey != null && resourceKeyMasterQueryFilterDto.ResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.ResourceKey)
                    predicateInner.Or(x => x.Resourcekey == item);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.ResourceType != null && resourceKeyMasterQueryFilterDto.ResourceType.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.ResourceType)
                    predicateInner.Or(x => x.Resourcetypesid == item);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.DcfId != null && resourceKeyMasterQueryFilterDto.DcfId.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.DcfId)
                    predicateInner.Or(x => x.Dcfid == item);
                predicateResult.And(predicateInner);
            }

            if (resourceKeyMasterQueryFilterDto.DesignComponentFamilyName != null && resourceKeyMasterQueryFilterDto.DesignComponentFamilyName.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.DesignComponentFamilyName)
                    predicateInner.Or(x => x.Dcfid == item);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.ElementName != null && resourceKeyMasterQueryFilterDto.ElementName.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.ElementName)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.ResourceTypesId != null && resourceKeyMasterQueryFilterDto.ResourceTypesId.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.ResourceTypesId)
                    predicateInner.Or(x => x.Resourcetypesid == item);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.CreationUser != null && resourceKeyMasterQueryFilterDto.CreationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.CreationUser)
                    predicateInner.Or(x => x.CreationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (resourceKeyMasterQueryFilterDto.ModificationUser != null && resourceKeyMasterQueryFilterDto.ModificationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                foreach (var item in resourceKeyMasterQueryFilterDto.ModificationUser)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.KeyStatus != null && resourceKeyMasterQueryFilterDto.KeyStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();

                foreach (var item in resourceKeyMasterQueryFilterDto.KeyStatus)
                    predicateInner.Or(x => x.Keystatus.ToString().ToLower().Trim() == (item == "In Use" ? "true" : "false"));
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.CreationDate != null)
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                if (resourceKeyMasterQueryFilterDto.CreationDate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= resourceKeyMasterQueryFilterDto.CreationDate.StartDate);
                if (resourceKeyMasterQueryFilterDto.CreationDate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= resourceKeyMasterQueryFilterDto.CreationDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.ModificationDate != null)
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();
                if (resourceKeyMasterQueryFilterDto.ModificationDate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= resourceKeyMasterQueryFilterDto.ModificationDate.StartDate);
                if (resourceKeyMasterQueryFilterDto.ModificationDate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= resourceKeyMasterQueryFilterDto.ModificationDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (resourceKeyMasterQueryFilterDto.BagName != null && resourceKeyMasterQueryFilterDto.BagName.Any())
            {
                predicateInner = PredicateBuilder.New<Resourcekeymaster>();

                foreach (var item in resourceKeyMasterQueryFilterDto.BagName)
                    predicateInner.Or(x => x.Buildbagid == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<ResourceKeyMaster> GetQuery(ExpressionStarter<Resourcekeymaster> predicateResult, bool includeDeleted)
        {

            var query = predicateResult.IsStarted
                ? _repositoryWrapper.ResourceKeyMaster.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Opco)
                .Include(x => x.Dcf)
                .Include(x => x.Resourcetypes)                
                : _repositoryWrapper.ResourceKeyMaster.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Opco)
                .Include(x => x.Dcf)
                .Include(x => x.Resourcetypes);
            return query.AsEnumerable().Select(x => ResourceKeyMasterMapper.Get(x)).AsQueryable();
        }

        private Dictionary<string, Expression<Func<ResourceKeyMaster, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ResourceKeyMaster, object>>[]>
            {
                ["resourceKeyMasterId"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.ResourceKeyMasterId },
                ["resourceTypesId"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.ResourceTypesId },
                ["resourceKey"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.ResourceKey },
                ["dcfId"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.DcfId },
                ["dcId"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.DcId },
                ["opcoId"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.OpcoId },
                ["opCo"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.Opco.OpCoDescription },
                ["eventId"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.EventId },
                ["elementName"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.ElementName },
                ["keyStatus"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.KeyStatus },
                ["modificationDate"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.ModificationDate },
                ["modificationUser"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationDate"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.CreationDate },
                ["creationUser"] = new Expression<Func<ResourceKeyMaster, object>>[] { p => p.CreationUserEntity.Email },
            };
        }
        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, ResourceKeyMasterQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "lifeCycleId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.LifeCycleId.ToString(), Value = p.LifeCycleId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.LifeCycleId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.LifeCycleId.ToString(), Value = p.LifeCycleId.ToString() }).Distinct().ToList(),


                "resourceKeyMasterId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.ResourceKeyMasterId.ToString(), Value = p.ResourceKeyMasterId.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.ResourceKeyMasterId.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.ResourceKeyMasterId.ToString(), Value = p.ResourceKeyMasterId.ToString() }).Distinct()
                    .ToList(),

                "opCoId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.OpcoId.ToString(), Value = p.OpcoId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.OpcoId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.OpcoId.ToString(), Value = p.OpcoId.ToString() }).Distinct().ToList(),

                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.Opco != null)
                           .Select(p => new FilterValueDto { Text = p.Opco.OpCoDescription, Value = p.OpcoId.ToString() })
                           .Distinct()
                           .ToList()
                    : query
                        .Where(x => x.Opco != null && x.Opco.OpCoDescription != null && x.Opco.OpCoDescription.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Opco.OpCoDescription, Value = p.OpcoId.ToString() })
                        .Distinct()
                        .ToList(),

                "resourceTypesId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ResourceTypesId.ToString(), Value = p.ResourceTypesId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ResourceTypesId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ResourceTypesId.ToString(), Value = p.ResourceTypesId.ToString() }).Distinct().ToList(),

                "resourceKey" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Where(x => x.ResourceKey != null)
                        .Select(p => new FilterValueDto { Text = p.ResourceKey, Value = p.ResourceKey })
                        .Distinct()
                        .ToList()
                    : query
                        .Where(x => x.ResourceKey != null && x.ResourceKey.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.ResourceKey, Value = p.ResourceKey })
                        .Distinct()
                        .ToList(),

                "elementName" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Where(x => x.ElementName != null)
                        .Select(p => new FilterValueDto { Text = p.ElementName, Value = p.ElementName })
                        .Distinct()
                        .ToList()
                    : query
                        .Where(x => x.ElementName != null && x.ElementName.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.ElementName, Value = p.ElementName })
                        .Distinct()
                        .ToList(),

                "resourceType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ResourceTypes.Name, Value = p.ResourceTypesId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ResourceKey.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ResourceTypes.Name, Value = p.ResourceTypesId.ToString() }).Distinct().ToList(),

                "dcfId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.DcfId.ToString(), Value = p.DcfId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.DcfId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.DcfId.ToString(), Value = p.DcfId.ToString() }).Distinct().ToList(),

                "dcId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.DcId.ToString(), Value = p.DcId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.DcId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.DcId.ToString(), Value = p.DcId.ToString() }).Distinct().ToList(),


                "eventId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.EventId.ToString(), Value = p.EventId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.EventId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.EventId.ToString(), Value = p.EventId.ToString() }).Distinct().ToList(),

                "keyStatus" => string.IsNullOrEmpty(propertyFilter)
                                    ? query.Select(p => new FilterValueDto { Text = p.KeyStatus == true ? "In Use" : p.KeyStatus == null ? null : "Not In Use".ToString(), Value = p.KeyStatus == true ? "In Use" : p.KeyStatus == null ? null : "Not In Use".ToString() }).Distinct().ToList()
                                    : query
                                        .Where(x => x.KeyStatus != null)
                                        .Select(p => new FilterValueDto { Text = p.KeyStatus == true ? "In Use" : p.KeyStatus == null ? null : "Not In Use".ToString(), Value = p.KeyStatus == true ? "In Use" : p.KeyStatus == null ? null : "Not In Use".ToString() }).Distinct().ToList(),

                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.CreationUserEntity.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.CreationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.CreationUserEntity.Email)).Distinct().ToList(),

                "creationDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.CreationDate.ToString(), Value = p.CreationDate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.CreationDate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.CreationDate.ToString(), Value = p.CreationDate.ToString() }).Distinct().ToList(),


                "modificationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList(),


                "modificationDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationDate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList(),

                "designComponentFamilyName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(p => new FilterValueDto
                    {
                        Text = p.DesignComponentFamily.DCFName(_repositoryWrapper),
                        Value = p.DcfId.ToString()
                    }).Distinct().ToList()
                    : query.ToList()
                        .Where(x =>
                            x.DesignComponentFamily.DCFName(_repositoryWrapper).ToUpper().Contains(
                                propertyFilter.ToUpper())).Select(p => new FilterValueDto
                                {
                                    Text = p.DesignComponentFamily.DCFName(_repositoryWrapper),
                                    Value = p.DcfId.ToString()
                                }).Distinct().ToList(),

                "bagName" => string.IsNullOrEmpty(propertyFilter)
                                 ? query.Select(p => new FilterValueDto { Text = _commonManager.GetBuildBagDescription(p.BuildBagId), Value = p.BuildBagId.ToString() }).Distinct().ToList()
                                 : query
                                     .Where(x =>
                                         x.ModificationDate.ToString().Contains(
                                             propertyFilter)).Select(p => new FilterValueDto { Text = _commonManager.GetBuildBagDescription(p.BuildBagId), Value = p.BuildBagId.ToString() }).Distinct().ToList(),

                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        #endregion

        public string GenerateResourceKey(short OpcoId, long designComponentId, int resourceTypeId)
        {
            var Dcf = _repositoryWrapper.DesignComponent.FindByCondition(
                       x => x.Designcomponentid == designComponentId).FirstOrDefault();
            var querys = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => x.Opcoid == OpcoId
                            && x.Dcfid == Dcf.Designcomponentfamilyid
                            && x.Resourcetypesid == resourceTypeId
                            && x.Keystatus == true);
            var resourceKey = querys.Select(x => x.Resourcekey).FirstOrDefault();

            if (resourceKey == null)
            {
                resourceKey = _resourceKey.GenerateRandomResourceKey(_repositoryWrapper, resourceTypeId);
            }
            return resourceKey;
        }

        /// <summary>
        /// Check for existing entries for resource keys for use cases like Virtualised or COTS Shared HW and get the existing resource key.
        /// else generate a new key and assign for the respective assets.
        /// </summary>
        /// <param name="OpcoId"></param>
        /// <param name="DesignComponentFamilyId"></param>
        /// <param name="elementName"></param>
        /// <param name="resourceTypeId"></param>
        /// <param name="virtualisedHW"></param>
        /// <returns></returns>
        public string GenerateResourceKeyForAsset(short OpcoId, long DesignComponentFamilyId, string elementName, int resourceTypeId, bool virtualisedHW)
        {
            string resourceKey = string.Empty;
            if (resourceTypeId == (int)ResourceTypesKey.SWAsset)
            {
                resourceKey = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => x.Opcoid == OpcoId
                            && x.Resourcetypesid == resourceTypeId
                            && x.Dcfid == DesignComponentFamilyId
                            && x.Elementname == elementName
                            && x.Keystatus == true).Select(x => x.Resourcekey).FirstOrDefault();
            }
            else
            {
                if (virtualisedHW)
                {
                    resourceKey = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => x.Opcoid == OpcoId
                            && x.Resourcetypesid == resourceTypeId
                            && x.Dcfid == DesignComponentFamilyId
                            && x.Keystatus == true).Select(x => x.Resourcekey).FirstOrDefault();
                }
                else
                {
                    //Check for Shared COTS Hardware - Applicable only for non-virtualised nodes.
                    string _networkConstruct = _repositoryWrapper.NetworkElementAsPlanned
                       .FindByCondition(x => x.Elementname == elementName).Select(x => x.Networkconstruct).FirstOrDefault();
                    if (_networkConstruct != null)
                    {
                        string HWResourceKey = _repositoryWrapper.NetworkElementAsPlanned
                        .FindByCondition(x => x.Networkconstruct == _networkConstruct && x.Elementname != elementName).Select(x => x.Hwresourcekey).FirstOrDefault().ToString();

                        //If HW resource key exists for the same network construct copy over the resource key to new asset,
                        // else, check for resource key from master and assign, else a new key will be generated and assigned.
                        if (HWResourceKey != string.Empty)
                        {
                            resourceKey = HWResourceKey;
                        }
                        else
                        {
                            resourceKey = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => x.Opcoid == OpcoId
                           && x.Resourcetypesid == resourceTypeId
                           && x.Elementname == elementName
                           && x.Dcfid == DesignComponentFamilyId
                           && x.Keystatus == true).Select(x => x.Resourcekey).FirstOrDefault();
                        }
                    }
                }
            }
            // by default, if resource key is not found on the above cases, assign a new key
            if (resourceKey == null || resourceKey.Trim().Equals(string.Empty))
            {
                resourceKey = _resourceKey.GenerateRandomResourceKey(_repositoryWrapper, resourceTypeId);
            }
            return resourceKey.ToString();
        }

        public string GenerateResourceKeyForIdentity(short OpcoId, long DesignComponentFamilyId, string elementName, int resourceTypeId)
        {
            var resourceKey = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => x.Opcoid == OpcoId
                         && x.Resourcetypesid == resourceTypeId
                         && x.Dcfid == DesignComponentFamilyId
                         && x.Elementname == elementName
                         && x.Keystatus == true).Select(x => x.Resourcekey).FirstOrDefault();
            if (resourceKey == null)
            {
                resourceKey = _resourceKey.GenerateRandomResourceKey(_repositoryWrapper, resourceTypeId);
            }
            return resourceKey;
        }

        public string GenerateResourceKeyForLcm(short OpcoId, long DesignComponentFamilyId,long buildBagId, int resourceTypeId )
        {
            var resourceKey = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => x.Opcoid == OpcoId
                                 && x.Resourcetypesid == resourceTypeId
                                 && x.Dcfid == DesignComponentFamilyId && x.Buildbagid == buildBagId
                                 && x.Keystatus == true).Select(x => x.Resourcekey).FirstOrDefault();
            if (resourceKey == null) 
            {
                resourceKey = _resourceKey.GenerateRandomResourceKey(_repositoryWrapper, resourceTypeId);
            }
            return resourceKey;
        }
        public string GenerateResourceKeyForComponent(short OpcoId, long DesignComponentFamilyId, long buildBagId, long componentId, int resourceTypeId)
        {
            string resourceKey  = string.Empty;
            var resourceKeyEntity = _repositoryWrapper.ResourceKeyMaster.FindByCondition(x => x.Opcoid == OpcoId
                                 && x.Resourcetypesid == resourceTypeId
                                 && x.Dcfid == DesignComponentFamilyId && x.Componentid == componentId && x.Buildbagid == buildBagId
                                // && x.Keystatus == true
                                 ) .FirstOrDefault();
            if (resourceKeyEntity == null)
            {
                resourceKey = _resourceKey.GenerateRandomResourceKey(_repositoryWrapper, resourceTypeId);
            }
            else 
            {
                if (resourceKeyEntity != null && resourceKeyEntity.Keystatus == false)
                {
                    resourceKeyEntity.Keystatus = true;
                    resourceKeyEntity.Lifecycleid = resourceKeyEntity.Lifecycleid + 1; // Need to increatement  Ex: deleted and New Add DCF Lifecyle are same
                    _repositoryWrapper.ResourceKeyMaster.Update(resourceKeyEntity);
                    _repositoryWrapper.Save();
                      _repositoryWrapper.ClearTracker();

                }
                resourceKey = resourceKeyEntity.Resourcekey;
            }
            return resourceKey;
        }
       
        public async Task<ResultDto> CreateOrUpdateResourceKey(ResourceKeyMasterDto dto)
        {
            Resourcekeymaster reskey = new Resourcekeymaster();
            string exceptionString = string.Empty;
            try
            {
                if (dto.ResourceTypesId == (int)ResourceTypesKey.Lcm)
                {
                    reskey = await _repositoryWrapper.ResourceKeyMaster.FindByCondition(
                                     x => x.Opcoid == dto.OpCoId && x.Dcfid == dto.DcfId
                                     && x.Resourcetypesid == dto.ResourceTypesId
                                     && x.Keystatus == true
                                     && x.Buildbagid == dto.BuildBagId
                                     ).FirstOrDefaultAsync();
                }
                else if (dto.ResourceTypesId == (int)ResourceTypesKey.Component)
                {
                    reskey = await _repositoryWrapper.ResourceKeyMaster.FindByCondition(
                                     x => x.Opcoid == dto.OpCoId && x.Dcfid == dto.DcfId
                                     && x.Resourcetypesid == dto.ResourceTypesId
                                     && x.Keystatus == true
                                     && x.Buildbagid == dto.BuildBagId 
                                     && x.Componentid == dto.ComponentId
                                     ).FirstOrDefaultAsync();
                }
                else
                {
                    reskey = await _repositoryWrapper.ResourceKeyMaster.FindByCondition(
                                    x => x.Opcoid == dto.OpCoId
                                    && x.Dcfid == dto.DcfId
                                    && x.Resourcetypesid == dto.ResourceTypesId
                                    && x.Keystatus == true && x.Buildbagid == dto.BuildBagId
                                    && x.Elementname == dto.ElementName).FirstOrDefaultAsync();
                }

                if (reskey != null)
                {
                    if ((reskey.Resourcekey != dto.ResourceKey) || (dto.ResourceKey != null) || (reskey.Lifecycleid != dto.LifeCycleId) || (dto.LifeCycleId != null))
                    {
                        var reskeyexists = await _repositoryWrapper.ResourceKeyMaster.FindByCondition(
                                    x => x.Resourcekey == dto.ResourceKey && x.Resourcetypesid == dto.ResourceTypesId).FirstOrDefaultAsync();
                        if (reskeyexists == null)
                        {
                            Resourcekeymaster newResourceKeyMaster = new Resourcekeymaster();
                            newResourceKeyMaster.Dcfid = dto.DcfId;
                            newResourceKeyMaster.Opcoid = dto.OpCoId;
                            newResourceKeyMaster.Resourcekey = dto.ResourceKey;
                            newResourceKeyMaster.Resourcetypesid = dto.ResourceTypesId;
                            newResourceKeyMaster.Elementname = dto.ElementName;
                            newResourceKeyMaster.Keystatus = true;
                            newResourceKeyMaster.Lifecycleid = dto.LifeCycleId;
                            newResourceKeyMaster.Buildbagid = dto.BuildBagId;
                            newResourceKeyMaster.Componentid = dto.ComponentId;
                            _repositoryWrapper.ResourceKeyMaster.Create(newResourceKeyMaster); // Add a method to create a new entity in your repository
                            _repositoryWrapper.Save();// Save changes to the database
                            await _repositoryWrapper.ClearTracker();
                            exceptionString = ResultMessages.EntryAddSuccess;
                        }
                        else
                        {
                            reskeyexists.Dcfid = dto.DcfId;
                            reskeyexists.Opcoid = dto.OpCoId;
                            reskeyexists.Elementname = dto.ElementName;
                            reskeyexists.Keystatus = true;
                            reskeyexists.Lifecycleid = dto.LifeCycleId;
                            reskeyexists.Buildbagid = dto.BuildBagId;
                            reskeyexists.Componentid = dto.ComponentId;
                            _repositoryWrapper.ResourceKeyMaster.Update(reskeyexists);
                            _repositoryWrapper.Save();
                            exceptionString = ResultMessages.EntryAddSuccess;
                            await _repositoryWrapper.ClearTracker();
                        }
                        await _repositoryWrapper.ClearTracker();
                    }

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateSuccess,
                        Data = reskey.Resourcekeymasterid.ToString()
                    };

                }
                else
                {
                    var reskeyexists = await _repositoryWrapper.ResourceKeyMaster.FindByCondition(
                                     x => x.Resourcekey == dto.ResourceKey).FirstOrDefaultAsync();
                    if (reskeyexists == null)
                    {
                        Resourcekeymaster newResourceKeyMaster = new Resourcekeymaster();
                        newResourceKeyMaster.Dcfid = dto.DcfId;
                        newResourceKeyMaster.Opcoid = dto.OpCoId;
                        newResourceKeyMaster.Resourcekey = dto.ResourceKey;
                        newResourceKeyMaster.Resourcetypesid = dto.ResourceTypesId;
                        newResourceKeyMaster.Elementname = dto.ElementName;
                        newResourceKeyMaster.Keystatus = true;
                        newResourceKeyMaster.Lifecycleid = dto.LifeCycleId;
                        newResourceKeyMaster.Buildbagid = dto.BuildBagId;
                        newResourceKeyMaster.Componentid = dto.ComponentId;
                        _repositoryWrapper.ResourceKeyMaster.Create(newResourceKeyMaster); // Add a method to create a new entity in your repository
                        _repositoryWrapper.Save();// Save changes to the database
                        await _repositoryWrapper.ClearTracker();
                        exceptionString = ResultMessages.EntryAddSuccess;
                    }
                    else
                    {
                        exceptionString = ResultMessages.EntryAddExists;
                    }
                    return new ResultDto
                    {
                        Info = exceptionString,
                        Data = dto.ResourceKey.ToString(),
                    };
                }

            }
            catch (Exception )
            {

                throw;
            }
        }

        public async Task<ResultDto> UpdateResourcekeyMasterForRefactor(string ResourceKey, long? newDCFId, short? OpCoId)
        {
            var ReskeyExistsForOldDCF = await _repositoryWrapper.ResourceKeyMaster.FindByCondition(
                                   x => x.Resourcekey == ResourceKey).FirstOrDefaultAsync();

            if (ReskeyExistsForOldDCF != null)
            {
                if (ReskeyExistsForOldDCF.Dcfid != newDCFId)
                {
                    ReskeyExistsForOldDCF.Dcfid = newDCFId;
                    ReskeyExistsForOldDCF.Opcoid = OpCoId;
                    ReskeyExistsForOldDCF.Keystatus = true;
                    _repositoryWrapper.ResourceKeyMaster.Update(ReskeyExistsForOldDCF);
                }
            }
            _repositoryWrapper.Save();
            
            await _repositoryWrapper.ClearTracker();

            return new ResultDto
            {
                Info = "Resource Key's successfully swapped",
                Data = ResourceKey.ToString(),
            };
        }

        
    }
}
