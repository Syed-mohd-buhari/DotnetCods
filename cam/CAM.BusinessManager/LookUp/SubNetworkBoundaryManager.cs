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
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.DataTransferObjects.LookUp.SubNetworkBoundary;
using CAM.Enum;
using AutoMapper;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.DataTransferObjects.Entita.DesignComponent;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Globalization;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.BusinessManager.LookUp
{
    public class SubNetworkBoundaryManager : GridBaseAsync<SubNetworkBoundary, SubNetworkBoundaryGridDto, SubNetworkBoundaryQueryDto, Subnetworkboundaries>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;
        private readonly DesignAspectManager _designManger;
        private readonly CommonManager _commonManager;

        public SubNetworkBoundaryManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, IMapper mapper, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, DesignAspectManager designManger, CommonManager commonManager) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
            _designManger = designManger;
            _commonManager = commonManager;
        }


        public override ExpressionStarter<Subnetworkboundaries> ApplyFilterForOracleModel(SubNetworkBoundaryQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Subnetworkboundaries>();
            var predicateInner = PredicateBuilder.New<Subnetworkboundaries>();

            if (request.SubNetworkBoundaryDescription != null && request.SubNetworkBoundaryDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.SubNetworkBoundaryDescription)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }
            if (request.Orphan == true)
            {
                predicateResult = predicateResult.And(x => !x.Designcomponents.Any() && !x.Designcomponentfamilies.Any());
            }
                
            if (request.SubNetworkBoundaryId != null && request.SubNetworkBoundaryId.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.SubNetworkBoundaryId)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (request.Alias != null && request.Alias.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.Alias)
                    predicateInner.Or(x => x.Alias == item);
                predicateResult.And(predicateInner);
            }

            if (request.VodafoneName != null && request.VodafoneName.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();

                foreach (var item in request.VodafoneName)
                    predicateInner.Or(x => x.Vodafonenameid == item);
                predicateResult.And(predicateInner);
            }

            if (request.AllSupportedServices != null && request.AllSupportedServices.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.AllSupportedServices)
                    predicateInner.Or(x => x.Subnetworksupportedsvr.Any(d => d.Serviceid == item));
                predicateResult.And(predicateInner);
            }

            if (request.GdprRelevant != null && request.GdprRelevant.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.GdprRelevant)
                {
                    bool? toCheck = (item) switch
                    {
                        0 => false,
                        1 => true,
                        _ => null
                    };
                    predicateInner.Or(x => x.Gdprrelevant == toCheck);
                }
                predicateResult.And(predicateInner);
            }

            if (request.InternetFacing != null && request.InternetFacing.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.InternetFacing)
                    predicateInner.Or(x => x.Internetfacing == item);
                predicateResult.And(predicateInner);
            }
            if (request.LcmPolicy != null && request.LcmPolicy.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.LcmPolicy)
                    predicateInner.Or(x => x.Lcmpolicy == item);
                predicateResult.And(predicateInner);
            }
            if (request.Criticality != null && request.Criticality.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();

                foreach (var item in request.Criticality)
                    predicateInner.Or(x => x.Criticality == item);
                predicateResult.And(predicateInner);
            }
            if (request.Pcisox != null && request.Pcisox.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.Pcisox)
                    predicateInner.Or(x => x.PciSox == item);
                predicateResult.And(predicateInner);
            }
            if (request.SecurityElement != null && request.SecurityElement.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.SecurityElement)
                    predicateInner.Or(x => x.Securityelement == item);
                predicateResult.And(predicateInner);
            }
            if (request.C3C4 != null && request.C3C4.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.C3C4)
                    predicateInner.Or(x => x.C3C4 == item);
                predicateResult.And(predicateInner);
            }
            if (request.MissionCritical != null && request.MissionCritical.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.MissionCritical)
                    predicateInner.Or(x => x.Missioncritical == item);
                predicateResult.And(predicateInner);
            }
            if (request.GdrpClassificationValue != null && request.GdrpClassificationValue.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.GdrpClassificationValue)
                    predicateInner.Or(x => x.Gdprclassification == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                if (request.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModifiedValue.StartDate);
                if (request.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            if (request.SystemFunction != null && request.SystemFunction.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.SystemFunction)
                    predicateInner.Or(x => x.Subnetwrokboundarysystemfunction.Any(d => d.Systemfunctionid == item));
                predicateResult.And(predicateInner);
            }

            if (request.CustomerWheel != null && request.CustomerWheel.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.CustomerWheel)
                    predicateInner.Or(x => x.Subnetworkboundarycustomerwheel.Any(d => d.Customerwheelid == item));
                predicateResult.And(predicateInner);
            }

            if (request.SecurityElement != null && request.SecurityElement.Any())
            {
                predicateInner = PredicateBuilder.New<Subnetworkboundaries>();
                foreach (var item in request.SecurityElement)
                    predicateInner.Or(x => x.Securityelement == item);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<SubNetworkBoundaryGridDto> CastObjectToDto(IQueryable<SubNetworkBoundary> request)
        {

            var result = request.AsEnumerable().Select(dto => new SubNetworkBoundaryGridDto()
            {
                SubNetworkBoundaryId = dto.Id,
                SubNetworkBoundaryDescription = dto.Description,
                LastModified = dto.ModificationDate,
                Deleted = dto.Deleted,
                Orphan = !dto.DesignComponentFamilies.Any(),
                LastModifiedBy = dto.ModificationUserEntity.Email,
                SupportedServicesIDs = dto.SupportedServices != null? dto.SupportedServices.Select(p => p.Id).ToList(): null,
                Alias = dto.Alias,
                AllSupportedServices = dto.SupportedServices != null ? string.Join(" | ", dto.SupportedServices.Select(fx => fx.SupportedService.Description).Distinct()): "",
                VodafoneName = dto.VodafoneName != null ? dto.VodafoneName.Description :"",
                VodafoneNameId = dto.VodafoneName != null ? (uint?)dto.VodafoneName.Id :null,
                Default = dto.Default,
                GdprRelevant = dto.GdprRelevant,
                InternetFacing = dto.InternetFacing,
                LcmPolicy = ((LCMPolicy)dto.LcmPolicy).ToString(),
                Criticality = dto.Criticality,
                GdrpClassificationValue = dto.GDPRClassification != null ? ((GDPRClassification)dto.GDPRClassification).ToString(): "",
                Pcisox = dto.Pcisox,
                C3C4 = dto.C3C4,
                MissionCritical = dto.MissionCritical,
                SystemFunction = dto.SystemFunctions == null ? "" : string.Join(" | ", dto.SystemFunctions.Select(fx => fx.SystemFunction.SystemFunctionDescription).Distinct()),
                CustomerWheel = dto.CustomerWheels == null ? "" : string.Join(" | ", dto.CustomerWheels.Select(cw => cw.CustomerWheel.Description).Distinct()),
                SecurityElement = dto.SecurityElement,
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)

            }).ToList();
            var test = result.Where(x => x.VodafoneName != null).ToList();
            return result;
        }

        public override Dictionary<string, Expression<Func<SubNetworkBoundary, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SubNetworkBoundary, object>>[]>
            {
                ["subNetworkBoundaryDescription"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.Description },
                ["subNetworkBoundaryId"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.Id },
                ["alias"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.Alias },
                ["allSupportedServices"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.SupportedServices.FirstOrDefault() != null ? p.SupportedServices.FirstOrDefault().SupportedService.Description : default },
                ["vodafoneNameId"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.VodafoneNameId },
                ["vodafoneName"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.VodafoneName != null ? p.VodafoneName.Description : default },
                ["lastModifiedBy"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.ModificationUserEntity.Email },
                ["systemFunction"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.SystemFunctions.FirstOrDefault() != null ? p.SystemFunctions.FirstOrDefault().SystemFunction.SystemFunctionDescription : default},
                ["customerWheel"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.CustomerWheels.FirstOrDefault() != null ? p.CustomerWheels.FirstOrDefault().CustomerWheel.Description : default },
                ["gdprRelevant"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.GdprRelevant },
                ["internetFacing"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.InternetFacing },
                ["lcmPolicy"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.LcmPolicy },
                ["criticality"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.Criticality },
                ["gdrpClassificationValue"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.GDPRClassification },
                ["pcisox"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.Pcisox },
                ["c3C4"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.C3C4 },
                ["missionCritical"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.MissionCritical },
                ["securityElement"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.SecurityElement },
                ["lastModifiedValue"] = new Expression<Func<SubNetworkBoundary, object>>[] { p => p.ModificationDate },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<SubNetworkBoundary> request, string propertyName, string propertyFilter)
        {
           
            return propertyName switch
            {

                "subNetworkBoundaryDescription" => request.Select(x => new FilterValueDto(x.Description)),
                "alias" => request.Select(x => new FilterValueDto(x.Alias)),
                "vodafoneName" => request.Include(p => p.VodafoneName)
                                   .Where(p => p.VodafoneName != null)
                                       .Select(p => new FilterValueDto { Text = p.VodafoneName.Description, Value = p.VodafoneNameId.ToString() }).Distinct().ToList(),
                "allSupportedServices" => request.Where(p => p.SupportedServices != null && p.SupportedServices.Count > 0).SelectMany(x => x.SupportedServices)
                .Select(p => new FilterValueDto(p.ServiceId.ToString(), p.SupportedService.Description)).Distinct().ToList(),
                "gdprRelevant" => request.Select(p => new FilterValueDto { Text = (p.GdprRelevant.HasValue ? p.GdprRelevant.Value ? "YES" : "NO" : "UNSPECIFIED"), 
                                        Value = p.GdprRelevant.HasValue ? (p.GdprRelevant.Value ? "1" : "0") : "2" }).Distinct().ToList(),
                "internetFacing" => request.Select(p => new FilterValueDto { Text = p.InternetFacing.Value ? "YES" : "NO", Value = p.InternetFacing.ToString() }).Distinct().ToList(),
                "lcmPolicy" => request.Select(p => new FilterValueDto { Text = ((LCMPolicy)p.LcmPolicy).ToString(), Value = p.LcmPolicy.ToString() }).Distinct().ToList(),
                "criticality" => request.Select(p => new FilterValueDto(p.Criticality)).Distinct().ToList(),
                "pcisox" => request.Where(p => p.Pcisox != null).Select(p => new FilterValueDto { Text = p.Pcisox.Value ? "YES" : "NO", Value = p.Pcisox.ToString() }).Distinct().ToList(),
                "c3C4" => request.Where(p => p.C3C4 != null).Select(p => new FilterValueDto { Text = p.C3C4.Value ? "YES" : "NO", Value = p.C3C4.ToString() }).Distinct().ToList(),
                "securityElement" => request.Where(p => p.SecurityElement != null).Select(p => new FilterValueDto { Text = p.SecurityElement.Value ? "YES" : "NO", Value = p.SecurityElement.ToString() }).Distinct().ToList(),
                "missionCritical" => request.Where(p => p.MissionCritical != null).Select(p => new FilterValueDto { Text = p.MissionCritical.Value ? "YES" : "NO", Value = p.MissionCritical.ToString() }).Distinct().ToList(),
                "gdrpClassificationValue" => request.Where(p => p.GDPRClassification != null)
                                       .Select(p => new FilterValueDto { Text = ((GDPRClassification)p.GDPRClassification).ToString(), Value = p.GDPRClassification.ToString() }).Distinct().ToList(),
                "subNetworkBoundaryId" => request.Select(x => new FilterValueDto(x.Id.ToString())),
                "systemFunction" => request.Where(p => p.SystemFunctions != null).SelectMany(x => x.SystemFunctions)
                            .Select(p => new FilterValueDto(p.SystemFunctionId, p.SystemFunction.SystemFunctionDescription)).Distinct().ToList(),
                "customerWheel" => request.Where(p => p.CustomerWheels != null).SelectMany(x => x.CustomerWheels).Select(p => new FilterValueDto(p.CustomerWheelId.ToString(), p.CustomerWheel.Description)).Distinct().ToList(),
                "lastModifiedBy" =>request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<SubNetworkBoundary> PrepareQuery(SubNetworkBoundaryQueryDto request,
            ExpressionStarter<SubNetworkBoundary> predicateResult, ExpressionStarter<Subnetworkboundaries> oraclePredicateResult = null)
        {
             var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.SubNetworkBoundaries.FindByCondition(oraclePredicateResult).Include(x=>x.Vodafonename)
               : _repositoryWrapper.SubNetworkBoundaries.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.Vodafonename)
                .Include(m => m.CreationuserNavigation)
                .Include(m => m.Designcomponentfamilies)
                .Include(m => m.Subnetworksupportedsvr).ThenInclude(m => m.Service)
                .Include(m => m.Subnetwrokboundarysystemfunction).ThenInclude(m => m.Systemfunction)
                .Include(m => m.Subnetworkboundarycustomerwheel).ThenInclude(m => m.Customerwheel)
                //.Where(p => p.Default == null || !p.Default.Value)
                .AsEnumerable().Select(p => SubNetworkBoundaryMapper.GetSubNetworkBoundaryMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(SubnetworkBoundaryDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.SubNetworkBoundaries.FindByCondition(
               x => x.Vodafonenameid == dto.VodafoneNameId
               && x.Description.ToLower().Replace(" ", "") == dto.SubNetworkBoundaryDescription.ToLower().Replace(" ", ""), true)
                .FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }

            dto.Order = int.Parse(dto.SubNetworkBoundaryDescription.Replace("Subnetwork", "").Trim());

            AddDefaultSubnetwork(dto.VodafoneNameId);

            SubNetworkBoundary entity = new SubNetworkBoundary()
            {
                Description = dto.SubNetworkBoundaryDescription,
                Alias = dto.Alias,
                Order = dto.Order,
                VodafoneNameId = dto.VodafoneNameId,
                GdprRelevant = dto.GdprRelevant,
                InternetFacing = dto.InternetFacing,
                LcmPolicy =dto.LcmPolicy,
                Criticality=dto.Criticality,
                SecurityElement = dto.SecurityElement,
                GDPRClassification= dto.GDPRClassification,
                Pcisox = dto.Pcisox,
                C3C4 = dto.C3C4,
                MissionCritical = dto.MissionCritical,

            };
            if (dto.SupportedServicesIDs.Any())
            {
                foreach (var item in dto.SupportedServicesIDs)
                {
                    entity.SupportedServices.Add(new SubNetworkBoundary_SupportedService() { SubNetworkId = entity.Id, ServiceId = item });
                }
            }

            if (dto.SystemFunctionsIds.Any())
            {
                foreach (var item in dto.SystemFunctionsIds)
                {
                    entity.SystemFunctions.Add(new SubnetworkBoundarySystemFunction()
                    {
                        SubnetworkBoundaryId = entity.Id,
                        SystemFunctionId = (short)item ,
                    });
                }
            }

            if (dto.CustomerWheelsIds.Any())
            {
                foreach (var item in dto.CustomerWheelsIds)
                {
                    entity.CustomerWheels.Add(new SubnetworkBoundaryCustomerWheel()
                    {
                        SubnetworkBoundaryId = entity.Id,
                        CustomerWheelId = item,
                    });
                }
            }
            var model = SubNetworkBoundaryMapper.SetSubNetworkBoundaryMapper(entity);
            _repositoryWrapper.SubNetworkBoundaries.Create(model);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = model.Id };
        }

        public async Task<ResultDto> Update(SubnetworkBoundaryDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.SubNetworkBoundaries.FindByCondition(
               x => x.Id != dto.SubNetworkBoundaryId 
               && x.Description.ToLower().Replace(" ", "") == dto.SubNetworkBoundaryDescription.ToLower().Replace(" ", "")
               && x.Vodafonenameid == dto.VodafoneNameId
               && !x.Deleted.Value)
               .FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            //  var order = dto.SubNetworkBoundaryDescription.StartsWith("Subnetwork") ? int.Parse(dto.SubNetworkBoundaryDescription.Replace("Subnetwork", "")) : (int?)null;
            dto.Order = int.Parse(dto.SubNetworkBoundaryDescription.Replace("Subnetwork", "").Trim());//Subnetwork

            AddDefaultSubnetwork(dto.VodafoneNameId);

            SubNetworkBoundary entity = new SubNetworkBoundary()
            { 
                Id = dto.SubNetworkBoundaryId,
                Description = dto.SubNetworkBoundaryDescription,
                Alias = dto.Alias,
                Default = false,
                Order = dto.Order,
                VodafoneNameId = dto.VodafoneNameId,
                GDPRClassification = dto.GDPRClassification,
                GdprRelevant = dto.GdprRelevant,
                C3C4 = dto.C3C4,
                Pcisox = dto.Pcisox,
                LcmPolicy = dto.LcmPolicy,
                Criticality = dto.Criticality,
                InternetFacing = dto.InternetFacing,
                SecurityElement = dto.SecurityElement,
                MissionCritical = dto.MissionCritical
            };
            var addedSVRs = _repositoryWrapper.SubnetworkSupportedServiceRepository.FindByCondition(p => p.Subnetworkid == dto.SubNetworkBoundaryId).ToList();
            foreach (var item in addedSVRs)
            {
                _repositoryWrapper.SubnetworkSupportedServiceRepository.DeleteDeep(item);
            }
            foreach (var item in dto.SupportedServicesIDs)
            {
                _repositoryWrapper.SubnetworkSupportedServiceRepository.Create(new Subnetworksupportedsvr() { Subnetworkid = entity.Id, Serviceid = item });
            }

            var addedCustomerWheels = _repositoryWrapper.SubnetworkCustomerWheelsRepository.FindByCondition(p => p.Subnetworkboundaryid == dto.SubNetworkBoundaryId).ToList();
            foreach (var item in addedCustomerWheels)
            {
                _repositoryWrapper.SubnetworkCustomerWheelsRepository.DeleteDeep(item);
            }
            foreach (var item in dto.CustomerWheelsIds)
            {
                _repositoryWrapper.SubnetworkCustomerWheelsRepository.Create(new Subnetworkboundarycustomerwheel() { Subnetworkboundaryid = entity.Id, Customerwheelid = item });
            }

            var addedSystemFunctions = _repositoryWrapper.SubnetworkSystemFunctionsRepository.FindByCondition(p => p.Subnetwrokboundaryid == dto.SubNetworkBoundaryId).ToList();
            foreach (var item in addedSystemFunctions)
            {
                _repositoryWrapper.SubnetworkSystemFunctionsRepository.DeleteDeep(item);
            }
            foreach (var item in dto.SystemFunctionsIds)
            {
                _repositoryWrapper.SubnetworkSystemFunctionsRepository.Create(new Subnetwrokboundarysystemfunction() { Subnetwrokboundaryid = entity.Id, Systemfunctionid = item });
            }

            _repositoryWrapper.SubNetworkBoundaries.Update(SubNetworkBoundaryMapper.SetSubNetworkBoundaryMapper(entity));
            await _repositoryWrapper.SaveAsync();

            await UpdateDesignAspect(entity);
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> UpdateDesignAspect(SubNetworkBoundary model)
        {
            var dcfEntity = await _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Subnetworkboundaryid == model.Id).FirstOrDefaultAsync();
            long id = 0;
            if (dcfEntity != null)
            {
                var designAspectEntity = await _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Designcomponentfamilyid == dcfEntity.Designcomponentfamilyid).FirstOrDefaultAsync();
                if (designAspectEntity != null)
                {
                    id = designAspectEntity.Id;
                    designAspectEntity.Criticalityrating = _designManger.ComputeCriticalityRating(designAspectEntity.Designcomponentfamilyid);
                    _repositoryWrapper.DesignAspectRepository.Update(designAspectEntity);

                }
            }
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = id
            };

        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.SubNetworkBoundaries.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.SubNetworkBoundaries.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.SubNetworkBoundaries
                .FindByCondition(x => x.Id == id)
                .Include(x => x.Subnetworksupportedsvr)
                .Include(x => x.Subnetworkboundarycustomerwheel)
                .Include(x => x.Subnetwrokboundarysystemfunction)
                
                .SingleAsync();

            if (entity.Subnetwrokboundarysystemfunction != null && entity.Subnetwrokboundarysystemfunction.Count > 0)
            {
                var systemFunctionsList = entity.Subnetwrokboundarysystemfunction.ToList();
                foreach (var toDelete in systemFunctionsList)
                {
                    _repositoryWrapper.SubnetworkSystemFunctionsRepository.Delete(toDelete);
                }    
            }

            if (entity.Subnetworkboundarycustomerwheel != null && entity.Subnetworkboundarycustomerwheel.Count > 0)
            {
                var customerwheelList = entity.Subnetworkboundarycustomerwheel.ToList();
                foreach (var toDelete in customerwheelList)
                {
                    _repositoryWrapper.SubnetworkCustomerWheelsRepository.Delete(toDelete);
                }
            }

            if (entity.Subnetworksupportedsvr != null && entity.Subnetworksupportedsvr.Count > 0)
            {
                var supportedsvrList = entity.Subnetworksupportedsvr.ToList();
                foreach (var toDelete in supportedsvrList)
                {
                    _repositoryWrapper.SubnetworkSupportedServiceRepository.Delete(toDelete);
                }
            }

            _repositoryWrapper.SubNetworkBoundaries.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }


        public async Task<ResultDto> GetRelatedRecords(short id)
        {

            var entities = _repositoryWrapper.DesignComponentFamily
                .FindByCondition(x => x.Subnetworkboundaryid == id)
                .Include(x => x.Subnetworkboundary)
                .Include(x => x.Subnetworkboundary).ThenInclude(x=>x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Select(x => x.Systemtypeidentityname)
                .ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Count(x => x != null) > 0)
                rm.Add(new ResultMessageDto() { Table = "Design Component Families", Values = entities });

            var entity = await _repositoryWrapper.SubNetworkBoundaries.FindByCondition(x => x.Id == id).SingleAsync();
            //Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries
            List<string> removeDuplicatesAndLinkedTableCheck = new List<string>()
            {
                "Designcomponents","Designcomponentfamilies" ,"Subnetworkboundarycustomerwheel","Subnetworksupportedsvr"," Subnetwrokboundarysystemfunction"
            };
            var referenceTableRecord = _commonManager.GetForeignKeyRefernceTable("Subnetworkboundaries", id, removeDuplicatesAndLinkedTableCheck);

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
                        EntityName = "SubNetwork Boundary",
                        RecordName = !string.IsNullOrEmpty(entity.Alias) ? entity.Alias : entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public SubNetworkBoundaryGridDto GetCreatePage()
        {
            var productNameResource = _repositoryWrapper.ProductNameRepository.FindAll();
            var vodafoneNameResources = _repositoryWrapper.VodafoneNameRepository.FindAll();
            var dto = new SubnetworkBoundaryDtoCreate()
            {
                SupportedServices = _repositoryWrapper.SupportedServiceRepository
                    .FindByCondition(x => x.Default == null || x.Default == false)
                    .Select(p => SupportedServiceMapper.Get(p)).ToList(),
                VodafoneNAmesResource = vodafoneNameResources.ToDictionary(x => x.Id, x => x.Description),
                SystemFunctionsResource = _repositoryWrapper.SystemFunction.FindAll()
                    .ToDictionary(k => k.Systemfunctionid, v => v.Systemfunction),
                CustomerWheelResource = _repositoryWrapper.CustomerWheelRepository.FindAll()
                    .ToDictionary(k => k.Id, v => v.Description),
                CustomerWheelsIds = new List<short>(),
                SystemFunctionsIds = new List<short>(),
                LcmPolicy = (short)LCMPolicy.TELCO
        };
            return dto;
        }
        public SubNetworkBoundaryGridDto GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.SubNetworkBoundaries.FindByCondition(x => x.Id == id)
                 .Include(x => x.ModificationuserNavigation)
                 .Include(x => x.CreationuserNavigation)
                 .Include(x => x.Subnetworksupportedsvr)
                 .Include(x => x.Designcomponentfamilies)
                 .Include(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                 .Include(x => x.Subnetworkboundarycustomerwheel).ThenInclude(x => x.Customerwheel)
                 .Single();

            var entity = SubNetworkBoundaryMapper.GetSubNetworkBoundaryMapper(model);

            entity.SystemFunctions = model.Subnetwrokboundarysystemfunction.Select(p => SubnetworkBoundarySystemFunctionMapper.Get(p)).ToList();
            entity.CustomerWheels = model.Subnetworkboundarycustomerwheel.Select(p => SubnetworkBoundaryCustomerWheelMapper.Get(p)).ToList();
            //var dto = _mapper.Map<SubnetworkBoundaryDtoUpdate>(entity);
            //if (dto.SystemFunctionsIds == null) dto.SystemFunctionsIds = new List<short>();
            //if (dto.CustomerWheelsIds == null) dto.CustomerWheelsIds = new List<short>();
            #region MyRegion
            var dto = new SubnetworkBoundaryDtoUpdate()
            {
                SubNetworkBoundaryId = entity.Id,
                SubNetworkBoundaryDescription = entity.Description,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.ToString(),
                Alias = entity.Alias,
                SupportedServices = _repositoryWrapper.SupportedServiceRepository.FindByCondition(x => x.Default == null || x.Default == false)
                .Select(p => SupportedServiceMapper.Get(p)).ToList(),
                SupportedServicesIDs = entity.SupportedServices !=null ? entity.SupportedServices.Select(p => p.ServiceId).ToList(): null,
                VodafoneNameId = entity.VodafoneNameId,
                GdprRelevant = entity.GdprRelevant,
                InternetFacing = entity.InternetFacing,
                LcmPolicy = entity.LcmPolicy,
                Criticality = entity.Criticality,
                GdrpClassificationValue = entity.GDPRClassification.ToString(),
                Pcisox = entity.Pcisox,
                C3C4 = entity.C3C4,
                MissionCritical = entity.MissionCritical,
                CustomerWheelsIds = entity.CustomerWheels != null ? entity.CustomerWheels.Select(p => (short)p.CustomerWheelId).ToList(): null,
                SystemFunctionsIds = entity.SystemFunctions != null ? entity.SystemFunctions.Select(p => p.SystemFunctionId).ToList() : null,
                SecurityElement = entity.SecurityElement,
                GDPRClassification = entity.GDPRClassification
            };

            var vodafoneNameResource = _repositoryWrapper.VodafoneNameRepository.FindAll();
            dto.VodafoneNAmesResource = vodafoneNameResource.ToDictionary(x => x.Id, x => x.Description);

            dto.VodafoneNameId = entity.VodafoneNameId;
            if (dto.VodafoneNameId.HasValue && !dto.VodafoneNAmesResource.ContainsKey(dto.VodafoneNameId.Value))
            {
                var data = _repositoryWrapper.VodafoneNameRepository.FindByCondition(
                    x => x.Id == dto.VodafoneNameId,
                    includeDeleted: true).SingleOrDefault();
                if (data != null)
                {
                    dto.VodafoneNAmesResource.Add(data.Id, data.Description);
                }
            }
                #endregion
                dto.SystemFunctionsResource = _repositoryWrapper.SystemFunction.Count() != 0 ? _repositoryWrapper.SystemFunction.FindAll()
                .ToDictionary(k => k.Systemfunctionid, v => v.Systemfunction): null;
                dto.CustomerWheelResource = _repositoryWrapper.CustomerWheelRepository.Count()!=0? _repositoryWrapper.CustomerWheelRepository.FindAll()
                .ToDictionary(k => k.Id, v => v.Description): null;
            
            return dto;
        }
       


        public Dictionary<int, string> GetServicesOfSubNetworkBoundaries(List<int> subNetworkBoundaryIds)
        {
            Dictionary<int, string> allServices = new Dictionary<int, string>();
            foreach (var item in subNetworkBoundaryIds)
            {
                var supportedServiceResources = _repositoryWrapper.SubnetworkSupportedServiceRepository
                    .FindByCondition(p => p.Subnetworkid == item)
                    .Include(p => p.Service)
                    .Select(p => p.Service)
                    .ToList();
                var services = supportedServiceResources.ToDictionary(x => x.Id, y => y.Description);
                foreach (var srv in services)
                {
                    if (!allServices.Select(x => x.Key).Contains(srv.Key))
                    {
                        allServices.Add(srv.Key, srv.Value);
                    }
                }
            }
            return allServices;

        }

        public Dictionary<string, string> GetAllProductNames()
        {

            var productNames = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x=>x.Productname != null)
                .Include(x => x.Productname)
                .Select(p => p.Productname.Description.ToUpper().Replace(" ","")).Distinct().ToDictionary(x => x, y => y);
            return productNames;
        }

        public Dictionary<int, string> GetAllVodafoneNames()
        {

            var vodafoneName = _repositoryWrapper.VodafoneNameRepository.FindAll().Distinct()
                .ToDictionary(x => x.Id, y => y.Description);
            return vodafoneName;
        }

        public string GetNewSubnetworkDescription(int? vodafoneNameId)
        {
            var subnetwork = _repositoryWrapper.SubNetworkBoundaries
                
                .FindByCondition(x=>x.Vodafonename != null 
                && x.Vodafonenameid == vodafoneNameId).Include(x=>x.Vodafonename).OrderByDescending(x => x.Order).FirstOrDefault();
            string newSubnetworkDescription = "Subnetwork2";
            if (subnetwork != null)
            {
                newSubnetworkDescription = "Subnetwork" + ((int?)subnetwork.Order + 1);
            }

            return newSubnetworkDescription;
        }

        public Subnetworkboundaries AddDefaultSubnetwork(int? vodafoneNameId)
        {
            var defaultSubnetwork = _repositoryWrapper.SubNetworkBoundaries.FindByCondition(x => x.Vodafonenameid == vodafoneNameId && x.Default == true).FirstOrDefault();

            if (defaultSubnetwork == null)
            {
                SubNetworkBoundary subnetwork = new SubNetworkBoundary() { Description = "Subnetwork1", Alias = "All Supported Services", Order = 1, VodafoneNameId = vodafoneNameId, Default = true, LcmPolicy = (short)LCMPolicy.TELCO };

                var allsupportedServiceId = _repositoryWrapper.SupportedServiceRepository.FindByCondition(x => x.Default == true).FirstOrDefault().Id;
                subnetwork.SupportedServices.Add(new SubNetworkBoundary_SupportedService() { SubNetworkId = subnetwork.Id, ServiceId = allsupportedServiceId });

                var network = SubNetworkBoundaryMapper.SetSubNetworkBoundaryMapper(subnetwork);
                _repositoryWrapper.SubNetworkBoundaries.Create(network);
                _repositoryWrapper.SaveAsync();
                return network;
            }
            else
            {
                return defaultSubnetwork;
            }
        }
    }
}
