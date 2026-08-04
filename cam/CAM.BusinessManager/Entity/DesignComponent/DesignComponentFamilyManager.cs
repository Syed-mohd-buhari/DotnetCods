using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignComponentFamily;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using CAM.BusinessManager.Entity.DesigComponent;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Entity;
using CAM.BusinessManager.LookUp;
using CAM.Entities.Mappers.Lookup;
using Microsoft.AspNetCore.Http;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.BusinessManager.Entity
{
    public class DesignComponentFamilyManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager manager;
        private readonly SupportedServiceManager _supportedServiceManager;
        private readonly CommonManager _commonManager;

        public DesignComponentFamilyManager(IEnumerable<IRepositoryWrapper> wrappers,
            SupportedServiceManager supportedServiceManager, IMapper mapper, GridCustomColumnManager manager, IRepositoryWrapper repositoryWrapper,
            IHttpContextAccessor contextAccessor, CommonManager commonManager ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _supportedServiceManager = supportedServiceManager;
            this.manager = manager;
            _commonManager = commonManager;

        }

        private async Task<IEnumerable<Designcomponentfamilies>> GetAllWithRelations()
        {
            return await _repositoryWrapper.DesignComponentFamily.GetAllWithRelations();
        }

        public async Task<IEnumerable<Designcomponentfamilies>> FindAll()
        {
            var data = _repositoryWrapper.DesignComponentFamily.FindAll();
            return data.AsEnumerable();
        }

        public async Task<ResultDto<long>> Add(DesignComponentFamilyDtoCreate dto, bool? forced = false)
        {
            var entityExists = await EntityExists(dto);

            if (entityExists != null)
            {
                return new ResultDto<long>
                {
                    Warning = true,
                    Info = ResultMessages.EntryAddExists,
                    Data = entityExists.Designcomponentfamilyid
                };
            }

            var entity = _mapper.Map<DesignComponentFamily>(dto);
            var model = DesignComponentFamilyMapper.Set(entity);
            _repositoryWrapper.DesignComponentFamily.Create(model);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto<long> { Info = ResultMessages.EntryAddSuccess, Data = model.Designcomponentfamilyid };
        }

        public async Task<Designcomponentfamilies> EntityExists(DesignComponentFamilyDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Subnetworkboundaryid == dto.SubNetworkBoundaryId
                    && x.Systemtypeidentityname == dto.SystemTypeIdentityName, true)
                .Include(x => x.Productname).OrderByDescending(x => x.Creationdate)
                .FirstOrDefaultAsync();
            return entityExists;
        }
        public async Task<Designcomponentfamilies> EntityExists(DesignComponentFamilyDtoUpdate dto)
        {
            var entityExists = await _repositoryWrapper.DesignComponentFamily.FindByCondition(
                    x => x.Subnetworkboundaryid == dto.SubNetworkBoundaryId
                    && x.Systemtypeidentityname == dto.SystemTypeIdentityName
                    && x.Designcomponentfamilyid != dto.DesignComponentFamilyId,
                    true
                ).Include(x => x.Productname)
                .OrderByDescending(x => x.Creationdate)
                .FirstOrDefaultAsync();
            return entityExists;
        }
        public bool IsSystemShared(long dcfId)
        {

            var dcf = _repositoryWrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == dcfId)
                .Include(x => x.Productname).Include(p => p.Designcomponents).ThenInclude(p => p.Systemtype)
                .ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction).FirstOrDefault();
            if (dcf != null)
            {
                var majorHardwareBuild = dcf.Designcomponents.Select(p => p.Systemtype).SelectMany(p => p.Systemtypesmajorhardwarebuilds)
                    .Select(p => p.Majorhardware).FirstOrDefault();
                if (majorHardwareBuild.Buildconstruction.Buildconstruction == ConstantValueFilter.proprietaryHw ||
                    majorHardwareBuild.Buildconstruction.Buildconstruction == ConstantValueFilter.commercialOfTheShelfCotsHw)
                {
                    var systemTypesMajorHardwareBuild = _repositoryWrapper.SystemTypesMajorHardwareBuild
                        .FindByCondition(p => p.Majorhardwareid == majorHardwareBuild.Majorhardwareid)
                        .Include(p => p.Systemtype).ThenInclude(p => p.Designcomponents).ThenInclude(p => p.Designcomponentfamily).ToList();
                    var designcomponentfamilies = systemTypesMajorHardwareBuild.Select(p => p.Systemtype)
                        .SelectMany(p => p.Designcomponents).Select(p => p.Designcomponentfamily).ToList();
                    if (designcomponentfamilies != null && designcomponentfamilies.Count > 1)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        public async Task<ResultDto> Update(DesignComponentFamilyDtoUpdate dto, bool? forced = false)
        {
            var entity = await _repositoryWrapper.DesignComponentFamily
                .FindByCondition(x => x.Designcomponentfamilyid == dto.DesignComponentFamilyId)
                .Include(x => x.Productname)
                .SingleOrDefaultAsync();
            if (entity == null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateNotExists,
                    Data = dto.DesignComponentFamilyId
                };
            }

            var entityToUpdate = _mapper.Map<DesignComponentFamily>(dto);
            entityToUpdate.Implementation = _commonManager.CheckImplementationFlag(dto.DesignComponentFamilyId );
            entityToUpdate.CountrySpecificCriticality = GetCriticalCountryValue(dto.DesignComponentFamilyId);

            var entityExists = await EntityExists(dto);

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = entityExists.Designcomponentfamilyid
                };
            }
            else
            {
                _repositoryWrapper.DesignComponentFamily.Update(DesignComponentFamilyMapper.Set(entityToUpdate));

                var designComponents = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentfamilyid == dto.DesignComponentFamilyId).ToList();
                foreach (var item in designComponents)
                {

                    if (item.Subnetworkboundaryid != dto.SubNetworkBoundaryId)
                    {
                        item.Subnetworkboundaryid = dto.SubNetworkBoundaryId;
                        _repositoryWrapper.DesignComponent.Update(item);
                    }
                }
                await _repositoryWrapper.SaveAsync();
                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess
                };
            }



        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == id).SingleAsync();
            _repositoryWrapper.DesignComponentFamily.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Designcomponentfamilyid
            };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            //Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries
            var entity = await _repositoryWrapper.DesignComponentFamily
                .FindByCondition(x => x.Designcomponentfamilyid == id)
                .SingleAsync();

            #region //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries  
            #region SoftDelete implemente Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries 
            var transientdesignComponents = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == id
            && x.Visibleflag == false
            ).ToList();
 
            if (transientdesignComponents != null && transientdesignComponents.Count() > 0)
            {
                foreach (var toDelete in transientdesignComponents)
                    _repositoryWrapper.DesignComponent.Delete(toDelete);

                await _repositoryWrapper.SaveAsync();
            }


            _repositoryWrapper.DesignComponentFamily.Delete(entity);
            #endregion
            #endregion

            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Designcomponentfamilyid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries 
            var designComponents = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == id
            && x.Deleted == false && x.Visibleflag == true)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Productname)

                .Select(x => x.toDesignComponentNameLcm(_repositoryWrapper)).ToArray();

            var designAspects = _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Designcomponentfamilyid == id
            && x.Archived == false)
                            .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                            .Select(x => x.toDesignAspectName(_repositoryWrapper)).ToArray();


            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (designComponents.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Design Components", Values = designComponents });

            if (designAspects.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Design Aspects", Values = designAspects });

            var entity = await _repositoryWrapper.DesignComponentFamily
                .FindByCondition(x => x.Designcomponentfamilyid == id)
                .Include(x => x.Subnetworkboundary)

                .Select(x => x.Systemtypeidentityname).SingleAsync();

            //Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries
            List<string> removeDuplicatesAndLinkedTableCheck = new List<string>()
            {
                "Designaspects","Designcomponents","Resourcekeymaster"
            };
            var referenceTableRecord = _commonManager.GetForeignKeyRefernceTable("Designcomponentfamilies", id, removeDuplicatesAndLinkedTableCheck);

            if (referenceTableRecord.Result != null && referenceTableRecord.Result.Count() > 0)
                rm.Add(new ResultMessageDto() { Table = _commonManager.popupTabName, Values = referenceTableRecord.Result.ToArray() });

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Design Component Family",
                        RecordName = entity,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public DesignComponentFamilyDtoCreate GetCreatePage()
        {
            var dto = new DesignComponentFamilyDtoCreate
            {
                SubNetworkBoundariesResource = _repositoryWrapper.SubNetworkBoundaries.FindAll()
                     .ToDictionary(k => k.Id, v => string.IsNullOrEmpty(v.Alias) ? v.Description : v.Alias),
                SharingTypeResource = _repositoryWrapper.SharingType.FindAll()
                   .ToDictionary(k => k.Sharingtypeid, v => v.Description),

            };
            return dto;

        }

        public DesignComponentFamilyDtoUpdate GetUpdatePage(long id)
        {

            var entity = _repositoryWrapper.DesignComponentFamily
                .FindByCondition(x => x.Designcomponentfamilyid == id, true)
                .Include(x => x.Productname)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Subnetworkboundary)
                .Include(p => p.Designcomponents).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(p => p.Designcomponents).ThenInclude(p => p.Systemtype).ThenInclude(p => p.VodafonenameNavigation)
                .Single();

            var majorsoftwarebuilds = entity.Designcomponents.Select(p => p.Systemtype).Select(p => p.Majorsoftwarebuilds).ToList(); 
            var vfNames = entity.Designcomponents.Select(x => x.Systemtype.Vodafonename);

            var subnetworks = _repositoryWrapper.SubNetworkBoundaries.FindByCondition(p => vfNames.Contains(p.Vodafonenameid)).ToList();

            var model = DesignComponentFamilyMapper.Get(entity);

            var dto = _mapper.Map<DesignComponentFamilyDtoUpdate>(model);

            dto.SubNetworkBoundariesResource = subnetworks.ToDictionary(k => k.Id, v => (string.IsNullOrEmpty(v.Alias) ? v.Description : v.Alias));


            dto.SharingTypeResource = _repositoryWrapper.SharingType.FindAll()
                  .ToDictionary(k => k.Sharingtypeid, v => v.Description);
 
            var supportedServiceResources = _repositoryWrapper.SubnetworkSupportedServiceRepository
                .FindByCondition(p => p.Subnetworkid == entity.Subnetworkboundaryid).Include(p => p.Service).Select(p => p.Service).ToList();

            dto.SubNetworkSupportedServices = supportedServiceResources.ToDictionary(x => x.Id, y => y.Description);

            return dto;
        }
        public int? ComputeCriticalityRating(bool? Pcisox, bool? C3C4, int? GDPRClassification, bool? InternetFacing,
            bool? MissionCritical, bool? SecurityElement, bool CountrySpecificCriticality)
        {
            int CriticalityRating = 0;

            if (Pcisox == true)
            {
                CriticalityRating += 1;
            }
            if (C3C4 == true)
            {
                CriticalityRating += 1;
            }
            if (GDPRClassification == 2)
            {
                CriticalityRating += 1;
            }
            if (InternetFacing == true)
            {
                CriticalityRating += 1;
            }
            if (MissionCritical == true)
            {
                CriticalityRating += 1;
            }
            if (SecurityElement == true)
            {
                CriticalityRating += 1;
            }
            if (CountrySpecificCriticality == true)
            {
                CriticalityRating += 1;
            }

            return CriticalityRating;

        }
        public QueryResultDto<DesignComponentFamilyDtoGrid> FindWithCondition(DesignComponentFamilyQueryDto designComponentFamilyFilterDto)
        {
            var predicateResult = ApplyFilter(designComponentFamilyFilterDto);
            if (designComponentFamilyFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);

            }
            if (designComponentFamilyFilterDto.Orphan == true)
            {
                predicateResult = predicateResult.And(x => !x.Designcomponents.Any());
            }


            var query = PrepareQuery(predicateResult, designComponentFamilyFilterDto.Deleted ?? false)
                .ApplyOrdering(designComponentFamilyFilterDto, GetColumnsMap());
           
            query = ((designComponentFamilyFilterDto.SystemTypeIdBasedVerticalId != null
           && designComponentFamilyFilterDto.SystemTypeIdBasedVerticalId.Count != 0) ||
           (designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue != null
           && designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue.Count != 0)) ?
            query.Where(x => x.DesignComponents.Any()) : query;

            if (designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue?.Any() == true &&
             designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto == null || x.VerticalFilterDto!=null && x.VerticalFilterDto.Count==0);
            }


            var rtn = new QueryResultDto<DesignComponentFamilyDtoGrid>(new GenerateRenderForGrid<DesignComponentFamilyDtoGrid>(manager))
            {
                TotalItems = query.Count()
            };


            query = query.ApplyPaging(designComponentFamilyFilterDto);
            var data = query.ToList();

            if (designComponentFamilyFilterDto.PrincipalId != 0)
            {
                var exist = data.Any(x => x.DesignComponentFamilyId == designComponentFamilyFilterDto.PrincipalId);
                if (!exist)
                {
                    var addedResource =
                     _repositoryWrapper.DesignComponentFamily.FindAll(true)
                        .Include(x => x.Subnetworkboundary)
                        .Include(x => x.Productname)
                        .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                        .Include(x => x.ModificationuserNavigation)
                        .Select(p => DesignComponentFamilyMapper.Get(p, true))
                         .Single(x => x.DesignComponentFamilyId == designComponentFamilyFilterDto.PrincipalId
                           );
                    if (addedResource != null)
                    {
                        var designContactList = _commonManager.GetDesignContactFromMajorSoftwareAndHardWare(addedResource.DesignComponents);
                        if (designContactList?.Any() == true)
                            addedResource.VerticalFilterDto = _commonManager.GetVerticaleNameDynamicFormat(designContactList, 0)
                                    .Select(x => new FilterValueDtoKeyValueList
                                    {
                                        Key = Convert.ToInt16(x.Value),
                                        Value = x.Text
                                    })?.Distinct().ToList();
                    }

                    if (designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue?.Any() == true)
                    {
                        if (!designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue.Contains("yes"))
                        {
                            var addedResourceDCF = addedResource.VerticalFilterDto.Where(t => designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue.Contains(t.Key.ToString()));
                            if (addedResourceDCF != null && addedResourceDCF?.Any() == true) data.Add(addedResource);
                        }

                    }
                    else data.Add(addedResource);

                }
            }
            var designComponentFamilyResult = _mapper.Map<IEnumerable<DesignComponentFamilyDtoGrid>>(data);
            rtn.Items = designComponentFamilyResult.ToArray();
            return rtn;
        }

        private IQueryable<DesignComponentFamily> PrepareQuery(ExpressionStarter<Designcomponentfamilies> predicateResult, bool includeDeleted)
        {
            var result = predicateResult.IsStarted
                    ? _repositoryWrapper.DesignComponentFamily.FindByCondition(predicateResult, includeDeleted)
                        .Include(x => x.Productname)
                        .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation)
                        .Include(x => x.Designcomponents).ThenInclude(x => x.Lcmengineering)
                        .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).
                        ThenInclude(x => x.Majorswbuildsdesigncontacts) 
                        .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                        .Include(x => x.ModificationuserNavigation)
                        .Include(x => x.Subnetworkboundary).ThenInclude(p => p.Subnetworksupportedsvr).ThenInclude(p => p.Service)
                        .Include(x => x.Sharingtype)
                    : _repositoryWrapper.DesignComponentFamily.FindAll()
                        .Include(x => x.Productname)
                        .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation)
                        .Include(x => x.Designcomponents).ThenInclude(x => x.Lcmengineering)
                        .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                        .ThenInclude(x => x.Majorswbuildsdesigncontacts) 
                        .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                        .Include(x => x.ModificationuserNavigation)
                        .Include(x => x.Subnetworkboundary).ThenInclude(p => p.Subnetworksupportedsvr).ThenInclude(p => p.Service)
                        .Include(x => x.Sharingtype);
            var data = result.AsEnumerable().Select(p => DesignComponentFamilyMapper.GetWithCountrySpecificCirticality(p, true)).ToList();

            foreach (var item in data)
            {
                var designContactList = _commonManager.GetDesignContactFromMajorSoftwareAndHardWare(item.DesignComponents);
                if (designContactList?.Any() == true)
                    item.VerticalFilterDto = _commonManager.GetVerticaleNameDynamicFormat(designContactList, 0)
                            .Select(x => new FilterValueDtoKeyValueList
                            {
                                Key = Convert.ToInt16(x.Value),
                                Value = x.Text
                            })?.Distinct().ToList();
            }

            return data.AsQueryable();

        }

        private static ExpressionStarter<Designcomponentfamilies> ApplyFilter(DesignComponentFamilyQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Designcomponentfamilies>();

            var predicateInner = PredicateBuilder.New<Designcomponentfamilies>();

            if (buildFilterDto.SystemTypeIdentityName != null && buildFilterDto.SystemTypeIdentityName.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();

                foreach (var item in buildFilterDto.SystemTypeIdentityName)
                    predicateInner.Or(x => x.Systemtypeidentityname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductName != null && buildFilterDto.ProductName.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();

                foreach (var item in buildFilterDto.ProductName)
                    predicateInner.Or(x => x.Productnameid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignComponentFamilyName != null && buildFilterDto.DesignComponentFamilyName.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                foreach (var item in buildFilterDto.DesignComponentFamilyName)
                {
                    predicateInner.Or(x => x.Designcomponentfamilyid == item);
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SharingType != null && buildFilterDto.SharingType.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();

                foreach (var item in buildFilterDto.SharingType)
                    predicateInner.Or(x => x.Sharingtypeid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Description != null && buildFilterDto.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();

                foreach (var item in buildFilterDto.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SubNetworkBoundary != null && buildFilterDto.SubNetworkBoundary.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                foreach (var item in buildFilterDto.SubNetworkBoundary)
                    predicateInner.Or(x => (string.IsNullOrEmpty(x.Subnetworkboundary.Alias) ? x.Subnetworkboundary.Description : x.Subnetworkboundary.Alias) == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.SubNetworkBoundaryId != null && buildFilterDto.SubNetworkBoundaryId.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                foreach (var item in buildFilterDto.SubNetworkBoundaryId)
                    predicateInner.Or(x => x.Subnetworkboundaryid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SupportedServices != null && buildFilterDto.SupportedServices.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                foreach (var item in buildFilterDto.SupportedServices)
                    predicateInner.Or(x => x.Subnetworkboundary.Subnetworksupportedsvr.Any(d => d.Serviceid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VodafoneName != null && buildFilterDto.VodafoneName.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                foreach (var item in buildFilterDto.VodafoneName)
                    predicateInner.Or(x => x.Designcomponents.Any(d => d.Systemtype.VodafonenameNavigation.Id == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignComponentFamilyId != null && buildFilterDto.DesignComponentFamilyId.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                foreach (var item in buildFilterDto.DesignComponentFamilyId)
                    predicateInner.Or(x => x.Designcomponentfamilyid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SystemIsShared != null && buildFilterDto.SystemIsShared.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                foreach (var item in buildFilterDto.SystemIsShared)
                    predicateInner.Or(x => x.Systemisshared == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Implementation != null && buildFilterDto.Implementation.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();

                foreach (var item in buildFilterDto.Implementation)
                    predicateInner.Or(x => x.Implementation == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);
                if (buildFilterDto.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignContact != null && buildFilterDto.DesignContact.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                foreach (var item in buildFilterDto.DesignContact)
                    if (item == "yes")
                    {
                        predicateInner.Or(x => x.Designcomponents.Any(y => y.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts != null &&
                       !y.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any()));
                    }
                    else
                    {
                        predicateInner.Or(x => x.Designcomponents.Any(x => x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any(y => y.Designcontactid.ToString() == item)));
                    }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemTypeIdBasedVerticalValue != null && buildFilterDto.SystemTypeIdBasedVerticalValue.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponentfamilies>();
                foreach (var item in buildFilterDto.SystemTypeIdBasedVerticalValue)
                    if (item == "yes")
                    {
                        predicateInner.Or(x => x.Designcomponents.Any(r => r.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts != null &&
                        !r.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any() || r.Systemtype.Systemtypesmajorhardwarebuilds
                        .Where(n => n.Ismain && !(ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction)))
                        .Any(t => t.Majorhardware.Majorhwbuildsdesigncontacts != null && !t.Majorhardware.Majorhwbuildsdesigncontacts.Any())
                        ));
                    }
                    else
                    {
                        predicateInner.Or(x => x.Designcomponents.Any(x => x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any(x => x.Designcontact.AspnetuserverticalsUser
                        .Any(x => x.Organisation.Vertical.Verticalresponsibleid.ToString() == item)) || x.Systemtype.Systemtypesmajorhardwarebuilds
                        .Where(n => n.Ismain && !(ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction)))
                        .Any(y => y.Majorhardware.Majorhwbuildsdesigncontacts.
                        Any(e => e.Designcontact.AspnetuserverticalsUser.Any(x => x.Organisation.Vertical.Verticalresponsibleid.ToString() == item)))));
                    }

                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        private Dictionary<string, Expression<Func<DesignComponentFamily, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<DesignComponentFamily, object>>[]>
            {
                ["designComponentFamilyId"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.DesignComponentFamilyId },
                ["systemTypeIdentityName"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.SystemTypeIdentityName },
                ["description"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.Description },
                ["subNetworkBoundary"] = new Expression<Func<DesignComponentFamily, object>>[] { p => string.IsNullOrEmpty(p.SubNetworkBoundary.Alias) ? p.SubNetworkBoundary.Description : p.SubNetworkBoundary.Alias },
                ["subNetworkBoundaryId"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.SubNetworkBoundaryId },
                ["implementation"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.Implementation },
                ["systemIsShared"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.SystemIsShared },
                ["lastModifiedValue"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.ModificationUserEntity.Email },
                ["sharingType"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.SharingType != null ? p.SharingType.SharingTypeDescription : default },
                ["productName"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.ProductNameNavigation != null ? p.ProductNameNavigation.Description : "" },
                ["vodafoneName"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.DesignComponents.FirstOrDefault() != null && p.DesignComponents.FirstOrDefault().SystemType.VodafoneName != null?
                p.DesignComponents.FirstOrDefault().SystemType.VodafoneName.Description : default},
                ["ProductNameId"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.ProductNameId },
                ["designComponentFamilyName"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.SystemTypeIdentityName },
                ["supportedServices"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.SubNetworkBoundary.SupportedServices.FirstOrDefault() != null?
                                                p.SubNetworkBoundary.SupportedServices.FirstOrDefault().SupportedService.Description : default },
                ["countrySpecificCriticality"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.CountrySpecificCriticality },
                ["systemTypeIdBasedVerticalId"] = new Expression<Func<DesignComponentFamily, object>>[] { p => p.DesignComponents.FirstOrDefault() != null && p.DesignComponents.FirstOrDefault().SystemType.VerticalResponsible != null?
                p.DesignComponents.FirstOrDefault().SystemType.VerticalResponsibleId  : default},

                ["systemTypeIdBasedVerticalValue"] = new Expression<Func<DesignComponentFamily, object>>[]{ p => p.DesignComponents.FirstOrDefault() != null &&
                p.DesignComponents.FirstOrDefault().SystemType.VerticalResponsible != null?
                p.DesignComponents.FirstOrDefault().SystemType.VerticalResponsible.VerticalResponsibleDescription  : default}
            };
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, DesignComponentFamilyQueryDto designComponentFamilyFilterDto,bool isAdmin)
        {
            var predicateResult = ApplyFilter(designComponentFamilyFilterDto);
            var query = PrepareQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "designComponentFamilyId" => query.Select(p => new FilterValueDto(p.DesignComponentFamilyId.ToString())).Distinct().ToList(),
                "designComponentFamilyName" => query.Select(p => new FilterValueDto
                {
                    Text = p.DCFName(_repositoryWrapper),
                    Value = p.DesignComponentFamilyId.ToString()
                }).Distinct().ToList(),
                "systemTypeIdentityName" => query.Select(p => new FilterValueDto(p.SystemTypeIdentityName)).Distinct().ToList(),
                "productName" => query.Where(x => x.ProductNameNavigation != null)
                        .Select(p => new FilterValueDto { Text = p.ProductNameNavigation != null ? p.ProductNameNavigation.Description : "", Value = p.ProductNameId.ToString() }).Distinct().ToList(),
                "description" => query.Select(p => new FilterValueDto(p.Description)).Distinct().ToList(),
                "sharingType" => query.Where(x => x.SharingType != null).Select(p => new FilterValueDto(p.SharingTypeId, p.SharingType.SharingTypeDescription)).Distinct().ToList(),
                "subNetworkBoundary" => query.Select(x => new FilterValueDto(string.IsNullOrEmpty(x.SubNetworkBoundary.Alias) ? x.SubNetworkBoundary.Description : x.SubNetworkBoundary.Alias)).Distinct().ToList(),
                "subNetworkBoundaryId" => query.Select(x => new FilterValueDto(x.SubNetworkBoundaryId)).Distinct().ToList(),
                "vodafoneName" => query.Where(p => p.DesignComponents != null && p.DesignComponents.Select(p => p.SystemType) != null && p.DesignComponents.Select(p => p.SystemType.VodafoneName) != null)
                .SelectMany(x => x.DesignComponents).Where(x => x.SystemType.VodafoneName != null)
                .Select(p => new FilterValueDto(p.SystemType.VodafoneNameId.ToString(), p.SystemType.VodafoneName.Description)).Distinct()
                .ToList(),
                "supportedServices" => query.Where(p => p.SubNetworkBoundary.SupportedServices != null && p.SubNetworkBoundary.SupportedServices.Count > 0)
                .SelectMany(x => x.SubNetworkBoundary.SupportedServices).Select(p => new FilterValueDto(p.ServiceId.ToString(), p.SupportedService.Description)).Distinct().ToList(),
                "implementation" => query.Select(p => new FilterValueDto { Text = p.Implementation ? "YES" : "NO", Value = p.Implementation.ToString() }).Distinct().ToList(),
                "countrySpecificCriticality" => query.Where(p => p.CountrySpecificCriticality != null)
                                  .Select(p => new FilterValueDto { Text = p.CountrySpecificCriticality.Value ? "YES" : "NO", Value = p.CountrySpecificCriticality.ToString() }).Distinct().ToList(),
                "systemIsShared" => query.Select(p => new FilterValueDto { Text = p.SystemIsShared ? "YES" : "NO", Value = p.SystemIsShared.ToString() }).Distinct().ToList(),
                "lastModifiedBy" => query.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

                "designContact" => query.SelectMany(x => x.DesignComponents.SelectMany(x => x.SystemType.MajorSoftwareBuilds.MajorSwBuidlsDesignContacts
                .Select(y => y.DesignContact)))?.ToList()?
                .Select(r => new FilterValueDto
                {
                    Text = r?.Email,
                    Value = r?.Id.ToString()
                })?.Distinct()?.ToList()
                .Concat(query.Where(x => x.DesignComponents.Any(y => y.SystemType.MajorSoftwareBuilds.MajorSwBuidlsDesignContacts != null && 
                y.SystemType.MajorSoftwareBuilds.MajorSwBuidlsDesignContacts.Count() <= 0))?
                       .Select(x =>
                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       ))?.Distinct()?.ToList(),
                "systemTypeIdBasedVerticalValue" => query.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                                                     .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                                                     {
                                                         Text = t.Value,
                                                         Value = t.Key.ToString()
                                                     }))?.Distinct()?.ToList()
                             .Concat(query.Where(x => x.DesignComponents.Any(y => y.SystemType.MajorSoftwareBuilds.MajorSwBuidlsDesignContacts.Any() == false
                             && y.SystemType.SystemTypesMajorHardwareBuilds.Any(m => m.MajorHardware.MajorHwBuidlsDesignContacts.Any() == false)))
                                    .Select(x =>

                                       new FilterValueDto
                                       {
                                           Text = "---",
                                           Value = "yes",
                                       }
                                    )).Distinct().ToList(),
             
                _ => new List<FilterValueDto>(),
            };
            if (!isAdmin && (designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue != null && designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue.Count > 0) && propertyName == "systemTypeIdBasedVerticalValue")
            {
                rtn = rtn.Where(x => designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue.Contains(x.Value.ToString())).ToList();
            }
            return rtn;
        }


        public async Task GenerateAssociationForDcf()
        {
            var components = await _repositoryWrapper.DesignComponentFamily
                .FindByCondition(x => x.Productname != null && x.Designcomponents.Any(s => s.Systemtype.Systemtypesmajorhardwarebuilds.Any()), false, false).AsNoTracking()
                .Include(x => x.Designcomponents)
                .Include(x => x.Productname)
                .AsNoTracking()
                .ToListAsync();
            foreach (var design in components)
            {

                var systemtypeId = design.Designcomponents.OrderByDescending(x => x.Creationdate).FirstOrDefault()?.Systemtypeid;
                var systemType = _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == systemtypeId).AsNoTracking()
                      .Include(x => x.Majorsoftwarebuilds).AsNoTracking().Include(x => x.Systemtypesmajorhardwarebuilds)
                      .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                      .AsNoTracking().FirstOrDefault();

                var mhId = systemType.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)?.Majorhardwareid;
                var majorHardwareEntity = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == mhId, true)
                     .Include(x => x.Buildconstruction).AsNoTracking().SingleOrDefault();


                var hardwareSolution = majorHardwareEntity.Hardwaresolution;
                var buildConstructionRule = majorHardwareEntity.Buildconstruction.Rule;

                design.Majorsoftwareoemid = systemType.Majorsoftwarebuilds.Orgeqpmanufacturerid;
                var ProductName = systemType.Majorsoftwarebuilds.Productname != null ? systemType.Majorsoftwarebuilds.Productname.Description : "";
                design.Majorhardwareoemid = majorHardwareEntity.Orgeqpmanufacturerid;
 
                design.Platformid = majorHardwareEntity.Platformid;
                var platform = (await _repositoryWrapper.Platform.FindByCondition(x =>
                  x.Platformid == majorHardwareEntity.Platformid).SingleOrDefaultAsync())?.Platform;

                design.Systemtypeidentityname = await Utils.STIM(_repositoryWrapper, design.Majorhardwareoemid, design.Majorsoftwareoemid, ProductName, platform, hardwareSolution, buildConstructionRule);

                _repositoryWrapper.MajorHardwareBuild.Detach();

                _repositoryWrapper.DesignComponentFamily.Update(design);
            }
            await _repositoryWrapper.SaveAsync();
        }

        public List<Opcos> GetImplementionOpcos(long designComponentFamilyId)
        {
          
            var opCos = _repositoryWrapper.OpCo.FindAll();
            var designComponentIds = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentfamilyid == designComponentFamilyId)
                .Select(p => p.Designcomponentid).Distinct().ToList();
             
            var lcms = _repositoryWrapper.Lcmengineering.FindByCondition(p => designComponentIds.Contains(p.Designcomponentid)).Include(p => p.Opco).ToList();
            var opcos = lcms.Select(p => new
            {
                Opco = p.Opco,
                NoOfNodes = p.CountNodes(false, _repositoryWrapper),
            }).Where(p => p.NoOfNodes > 0).ToList();
            return opcos.Select(p => p.Opco).GroupBy(p => p.Opcoid).Select(grp => grp.FirstOrDefault()).ToList();
        }
        public DCFImplementation GetOpcosImplementationsFlags(int designComponentFamilyId, List<long> _opcoList)
        {
            var dcf = _repositoryWrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == designComponentFamilyId).FirstOrDefault();
            var designComponents = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentfamilyid == designComponentFamilyId);
            var designComponentIds = designComponents.Select(p => p.Designcomponentid).ToList();
            var opCos = (_opcoList == null) ? _repositoryWrapper.OpCo.FindAll() :
                _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid));

            DCFImplementation dCFImplementation = new DCFImplementation();
            dCFImplementation.DCFName = dcf.Systemtypeidentityname;
            List<OpcosImplementation> output = new List<OpcosImplementation>();
            var data = _repositoryWrapper.Lcmengineering.FindByCondition(p => designComponentIds.Contains(p.Designcomponentid))
                        .Select(p => new
                        {
                            Opco = p.Opco.Opco,
                            DC = p.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper),
                            NoOfNodes = p.CountNodes(false, _repositoryWrapper),
                        }).ToList();
            if (data != null && data.Any())
            {
                var result = data.GroupBy(x => new { x.DC, x.Opco }).Select(p => new
                {
                    OpcoName = p.Key.Opco,
                    DC = p.Key.DC,
                    NodeCount = p.Sum(p => p.NoOfNodes),
                }).Where(p => p.NodeCount > 0).ToList();

                foreach (var item in opCos)
                {
                    var value = result.FirstOrDefault(p => p.OpcoName == item.Opco);
                    if (value != null)
                    {
                        output.Add(new OpcosImplementation() { OpcoName = item.Opco, DCName = value.DC, ImplementationFlag = true, ProductionNodesCount = value.NodeCount });
                    }
                }
                dCFImplementation.OpcosImplementations = output;
                return dCFImplementation;
            }
            return null;
        }

        public List<OpcosImplementationDetails> GetOpcosImplementationDetails(int designComponentFamilyId)
        {
            List<OpcosImplementationDetails> output = new List<OpcosImplementationDetails>();
            var dcf = _repositoryWrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == designComponentFamilyId).FirstOrDefault();
            var designComponents = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentfamilyid == designComponentFamilyId).Select(p => p.Designcomponentid).ToList();

            var LcmsManualInput = _repositoryWrapper.Lcmengineering.FindByCondition(p => designComponents.Contains(p.Designcomponentid) && p.Elementcount == false).Include(p => p.Opco).Include(p => p.Designcomponent).ToList();
            foreach (var item in LcmsManualInput)
            {
                output.Add(new OpcosImplementationDetails() { DCName = item.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper), NodeCountApproach = "Manual Input", OpcoName = item.Opco.Opco, NumberofNodes = item.Numberofnodes });
            }
            var LcmsElementCount = _repositoryWrapper.Lcmengineering.FindByCondition(p => designComponents.Contains(p.Designcomponentid) && p.Elementcount == true)
                .Include(p => p.Opco).Include(p => p.Designcomponent).ToList();
            foreach (var item in LcmsElementCount)
            {
                var nodes = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(p => p.Designcomponentid == item.Designcomponentid && p.Opcoid == item.Opcoid && p.Environment.Environment.ToLower() == "production").Include(p => p.Location).Include(p => p.Environment).ToList();
                foreach (var node in nodes)
                {
                    output.Add(new OpcosImplementationDetails()
                    {
                        DCName = item.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper)
                        ,
                        NodeCountApproach = "Element Count",
                        OpcoName = item.Opco.Opco,
                        Location = node.Location.Location,
                        Enviroment = node.Environment.Environment,
                        NetworkELement = node.Elementname
                    });

                }
            }
            return output;
        }

        public DCSForDCF GetDCSForDCF(int designComponentFamilyId)
        {
            var dcf = _repositoryWrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == designComponentFamilyId).FirstOrDefault();
            var designComponents = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentfamilyid == designComponentFamilyId).Include(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation);
            var designComponentIds = designComponents.Select(p => p.Designcomponentid).ToList();
            DCSForDCF dCSForDCF = new DCSForDCF();
            dCSForDCF.DCFName = dcf.Systemtypeidentityname;
            var data = designComponents.Select(p => new DCS
            {
                DCName = p.toDesignComponentNameLcm(_repositoryWrapper),
                VodafoneName = p.Systemtype.VodafonenameNavigation != null ? p.Systemtype.VodafonenameNavigation.Description : "",
            }).ToList();
            if (data != null && data.Any())
            {

                dCSForDCF.DCS = data;
                return dCSForDCF;
            }
            return null;
        }

        public async Task<ResultDto> LinkSubnetworkWithSupportedService(int designComponentFamilyId, int? supportedServiceId = null)
        {


            var entityExists = await _repositoryWrapper.DesignComponentFamily.FindByCondition(
                    x => x.Designcomponentfamilyid == designComponentFamilyId)
                .FirstOrDefaultAsync();

            if (entityExists != null)
            {
                if (supportedServiceId != null)
                {
                    var supportedSerice = _repositoryWrapper.SubnetworkSupportedServiceRepository.FindByCondition(p => p.Subnetworkid == entityExists.Subnetworkboundaryid && p.Serviceid == supportedServiceId.Value).SingleOrDefault();
                    if (supportedSerice == null)
                    {
                        Subnetworksupportedsvr model = new Subnetworksupportedsvr
                        {
                            Subnetworkid = entityExists.Subnetworkboundaryid,
                            Serviceid = supportedServiceId.Value,

                        };
                        _repositoryWrapper.SubnetworkSupportedServiceRepository.Create(model);
                        await _repositoryWrapper.SaveAsync();
                    }
                }
                var supportedServices = await _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == designComponentFamilyId)
                    .Include(p => p.Subnetworkboundary).ThenInclude(p => p.Subnetworksupportedsvr).ThenInclude(p => p.Service).ThenInclude(p => p.ModificationuserNavigation).SelectMany(p => p.Subnetworkboundary.Subnetworksupportedsvr).Select(p => p.Service).ToListAsync();

                var items = _supportedServiceManager.CastObjectToDto(supportedServices.Select(p => SupportedServiceMapper.Get(p)).AsQueryable());
                return new ResultDto
                {
                    Data = items,
                    Info = ResultMessages.EntryAddSuccess,
                    Warning = false,
                };
            }
            return new ResultDto
            {
                Info = ResultMessages.EntryAddExists,
                Warning = false,
            };
        }

        public async Task<ResultDto> LinkDCFWithNetworkFunction(int designComponentFamilyId, int? networkFunctionId = null)
        {

            //if (networkFunctionId != null)
            //{
            //    var entryexists = _repositoryWrapper.DesignComponentFamilyNetworkFunction.FindByCondition(p => p.Designcomponentfamilyid == designComponentFamilyId && p.Networkfunctionid == networkFunctionId.Value).SingleOrDefault();
            //    if (entryexists == null)
            //    {
            //        Designcomponentfamilynetworkfunction model = new Designcomponentfamilynetworkfunction
            //        {
            //            Designcomponentfamilyid = designComponentFamilyId,
            //            Networkfunctionid = networkFunctionId
            //        };
            //        _repositoryWrapper.DesignComponentFamilyNetworkFunction.Create(model);
            //        await _repositoryWrapper.SaveAsync();
            //    }
            //}
            //var networkFunctions = await _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == designComponentFamilyId)
            //   .Include(p => p.Designcomponentfamilynetworkfunction).ThenInclude(p => p.Networkfunction).ThenInclude(p => p.ModificationuserNavigation).SelectMany(p => p.Designcomponentfamilynetworkfunction).Select(p => p.Networkfunction).ToListAsync();

            //var items = _networkFunctionManager.CastObjectToDto(networkFunctions.Select(p => NetworkFunctionMapper.Get(p)).AsQueryable());
            return new ResultDto
            {
                Data = null,
                Info = ResultMessages.EntryAddSuccess,
                Warning = false,
            };

        }

        private bool GetCriticalCountryValue(long dcfId)
        {
            var designAspectList = _repositoryWrapper.DesignAspectRepository.FindByCondition(p => p.Designcomponentfamilyid == dcfId).ToList();
            if (!designAspectList.Any())
                return false;

            return designAspectList.Any(p => p.Criticalnationalinfrastructure == true);



        }
    }
}