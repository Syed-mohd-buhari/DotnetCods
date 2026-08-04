using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ServicePlan;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Identity;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class ServicePlanManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly PlannedActivityManager _plannedActivityManager;
        private readonly CommonManager _commonManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public ServicePlanManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, IMapper mapper, PlannedActivityManager plannedActivityManager,
            CommonManager commonManager, ILoggerManager logger,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) :base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
            _plannedActivityManager = plannedActivityManager;
            _commonManager = commonManager;
            _logger = logger;
        }
        #region UI Member function
        public async Task<QueryResultDto<ServicePlanPaGridDto>> FindWithCondition(PlannedActivityQueryDto dto)
        {
            try
            {
                var predicateResult = ApplyFilter(dto);

                var rtn = new QueryResultDto<ServicePlanPaGridDto>(new GenerateRenderForGrid<ServicePlanPaGridDto>(_columnManager))
                {
                    
                };
                var query = await GetQuery(predicateResult);

                if (dto.VerticalName?.Any() == true)
                {
                    var verticalNames = dto.VerticalName;

                    // null case (when "yes" is included)
                    var nullVerticals = query;
                    if (verticalNames.Contains("yes"))
                        nullVerticals = query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));

                    var otherVerticals = query.Where(x =>
                        x.VerticalFilterDto != null &&
                        x.VerticalFilterDto.Any(v => verticalNames.Where(t => t != "yes").Contains(v.Key.ToString()))
                    );

                    // combine both
                    if (!(verticalNames.Contains("yes")))
                        query = otherVerticals;
                    else if (verticalNames.Contains("yes") && verticalNames.Count > 1)
                        query = nullVerticals.Union(otherVerticals);
                    else query = nullVerticals;

                }
                query = query.ApplyOrdering(dto, GetColumnsMap());
                query = query.ApplyPaging(dto);
                var data = _mapper.Map<IEnumerable<ServicePlanPaGridDto>>(query);

                rtn.TotalItems = query.Count();
                rtn.Items = data.ToArray();
                

                return rtn;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
        private static ExpressionStarter<Serviceplan> ApplyFilter(PlannedActivityQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Serviceplan>(true);
            var predicateInner = PredicateBuilder.New<Serviceplan>(true);


            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Serviceplan>();
                foreach (var item in buildFilterDto.OpCo)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ServiceName != null && buildFilterDto.ServiceName.Any())
            {
                predicateInner = PredicateBuilder.New<Serviceplan>();
                foreach (var item in buildFilterDto.ServiceName)
                    predicateInner.Or(x => x.Servicemasterid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignComponentFamilyName != null && buildFilterDto.DesignComponentFamilyName.Any())
            {
                predicateInner = PredicateBuilder.New<Serviceplan>();
                foreach (var item in buildFilterDto.DesignComponentFamilyName)
                    predicateInner.Or(x => x.Serviceplandcfmappings.Any(x => x.Dcfid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedActivity != null && buildFilterDto.PlannedActivity.Any())
            {
                predicateInner = PredicateBuilder.New<Serviceplan>();
                foreach (long item in buildFilterDto.PlannedActivity)
                {
                    _ = predicateInner.Or(x => x.Plannedactivities.Any(s => s.Plannedactivityid == item));
                }

                _ = predicateResult.And(predicateInner);
            }
            //if (buildFilterDto != null && buildFilterDto.Creationuser.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Serviceplan>();
            //    foreach (var item in buildFilterDto.Creationuser)
            //        predicateInner.Or(x => x.Creationuser == item);
            //    predicateResult.And(predicateInner);
            //}

            //if (buildFilterDto.Modificationuser != null && buildFilterDto.Modificationuser.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Serviceplan>();
            //    foreach (var item in buildFilterDto.Modificationuser)
            //        predicateInner.Or(x => x.Modificationuser == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.Creationdate != null)
            //{
            //    predicateInner = PredicateBuilder.New<Serviceplan>();
            //    if (buildFilterDto.Creationdate.StartDate != null)
            //        predicateInner.And(x => x.Creationdate >= buildFilterDto.Creationdate.StartDate);
            //    if (buildFilterDto.Creationdate.EndDate != null)
            //        predicateInner.And(x => x.Creationdate <= buildFilterDto.Creationdate.EndDate);
            //    predicateResult.And(predicateInner);
            //}


            return predicateResult;
        }

        private async Task<IQueryable<ServicePlan>> GetQuery(ExpressionStarter<Serviceplan> predicateResult)
        {
            var query = _repositoryWrapper.ServicePlanRepository.FindByConditionWithDelete(predicateResult)
                .Include(x => x.Servicemaster).Include(x => x.Opco)
                 .Include(x => x.Serviceplandcfmappings)
                .Include(x => x.Serviceplandcfmappings).ThenInclude(x => x.Dcf).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                                                        .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                .Include(x => x.Serviceplandcfmappings).ThenInclude(x => x.Dcf).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                                                        .ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Deliverystatus);


            var asyncdata = await query.AsNoTracking().ToListAsync();
            var data = asyncdata.AsEnumerable().Select(x => ServicePlanMapper.Get(x, true)).ToList();
            foreach (var item in data)
            {
                item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.DesignContactList,
                       0)?.Select(t => new FilterValueDtoKeyValueList
                       {
                           Key = Convert.ToInt16(t.Value),
                           Value = t.Text
                       })?.Distinct()?.ToList();
            }

            return data.AsQueryable();

        }

        private Dictionary<string, Expression<Func<ServicePlan, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ServicePlan, object>>[]>
            {
                ["servicePlanId"] = new Expression<Func<ServicePlan, object>>[] { p => p.Serviceplanid },
                ["modificationDate"] = new Expression<Func<ServicePlan, object>>[] { p => p.ModificationDate },

            };
        }


        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, PlannedActivityQueryDto request,bool isAdmin)
        {
            var predicateResult = ApplyFilter(request);

            var query = await GetQuery(predicateResult);

            if (request.VerticalName != null && request.VerticalName.Any() == true && !request.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Any(v => request.VerticalName.Contains(v.Key.ToString())));
            }
            else if (request.VerticalName?.Any() == true && request.VerticalName.Count() == 1 && request.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));
            }
            else if (request.VerticalName?.Any() == true && request.VerticalName.Count() > 1 && request.VerticalName.Contains("yes"))
            {
                var nullVerticals = request.VerticalName.Contains("yes") ?
                    query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0)) : null;
                var verticalFilter = query
                                    .Where(x => x.VerticalFilterDto != null &&
                                    x.VerticalFilterDto.Any(c => request.VerticalName.Where(t => t != "yes").Contains(c.Key.ToString())));
                query = nullVerticals?.Any() == true && verticalFilter?.Any() == true ?
                    nullVerticals.Concat(verticalFilter) : nullVerticals?.Any() == true && verticalFilter
                    ?.Any() == false ? nullVerticals
                    : nullVerticals?.Any() == false && verticalFilter?.Any() == true ? verticalFilter : null;
            }

            var rtn = propertyName switch
            {

                "verticalName" => query.ToList().Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                         .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                         { Text = t.Value, Value = t.Key.ToString() }))?.Distinct()?.ToList()
                          .Concat(query.ToList().Where(x=>(x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0))
                            .Select(x => new FilterValueDto
                            {
                                Text = "---",
                                Value = "yes",
                            })).Distinct().ToList(),
                "serivePlanId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Serviceplanid.ToString(), Value = p.Serviceplanid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Serviceplanid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Serviceplanid.ToString(), Value = p.Serviceplanid.ToString() }).Distinct()
                   .ToList(),
                "opCo" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Opco.OpCoDescription.ToString(), Value = p.Opcoid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Opcoid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Opco.OpCoDescription.ToString(), Value = p.Serviceplanid.ToString() }).Distinct()
                   .ToList(),
                "serviceName" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.Servicemaster.Description.ToString(), Value = p.Servicemasterid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Servicemasterid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.Servicemaster.Description.ToString(), Value = p.Servicemasterid.ToString() }).Distinct()
                   .ToList(),
                // "serivePlanId" => string.IsNullOrEmpty(propertyFilter)
                //? query.Select(p => new FilterValueDto
                //{ Text = p.Serviceplandcfmappings..ToString(), Value = p.Serviceplanid.ToString() }).Distinct().ToList()
                //: query
                //    .Where(x => x.Serviceplanid.ToString().Contains(propertyFilter)).Select(p =>
                //        new FilterValueDto { Text = p.Serviceplanid.ToString(), Value = p.Serviceplanid.ToString() }).Distinct()
                //    .ToList(),
                "plannedActivity" => string.IsNullOrEmpty(propertyFilter)
                     ? query.ToList().SelectMany(q => q.Plannedactivity.Select(x => new FilterValueDto
                     {
                         Value = x.PlannedActivityId.ToString(),
                         Text = x.GetPlannedAction(_repositoryWrapper)
                     }).ToList()).Distinct().ToList()
                     : query.ToList()
                         .Where(x =>
                             x.Plannedactivity.Any(s => s.PlannedImplementationYear.ToString().Contains(propertyFilter) || s.ActivityStatus.ActivityStatusDescription.Contains(propertyFilter) || s.PlanningActivityStatus.PlanningActivityStatusDescription.Contains(propertyFilter))).ToList()
                         .SelectMany(q => q.Plannedactivity.Select(x => new FilterValueDto
                         {
                             Value = x.PlannedActivityId.ToString(),
                             Text = x.GetPlannedAction(_repositoryWrapper)
                         }).ToList()).Distinct().ToList(),

                _ => new List<FilterValueDto>(),
            };

            if (!isAdmin && (request.VerticalName != null && request.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                rtn = rtn.Where(x => request.VerticalName.Contains(x.Value.ToString())).ToList();
            }
            return rtn;

        }
        #endregion

        #region  // Create and Edit page
        public async Task<ServicePlanCreateDto> GetCreatePage(List<short> _opcoList)
        {
            var dto = new ServicePlanCreateDto();
            try
            {
                var paDetails = await _plannedActivityManager.GetCreatePage(_opcoList, new List<int>(), 0, 0, true);
                var serviceResource = await _repositoryWrapper.ServiceMasterRepository.FindAll().ToListAsync();

                paDetails.PlannedActivityResource = paDetails.PlannedActivityResource.Where(x =>x.Value.RuleLinkedDc == (int)PlannedActivityResourceEnum.Service_Planned).ToDictionary(x =>(short)x.Key, y => y.Value);
                paDetails.PlannedActivityResourceId = paDetails.PlannedActivityResource.Select(x => x.Key).FirstOrDefault();
                dto.PlannedActivityCreateDto = paDetails;
                dto.ServiceMasterResource = serviceResource.ToDictionary(x => x.Servicemasterid, y => y.Description);

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return dto;
            }

        }
        public async Task<ServicePlanUpdateDto> GetUpdatedPage(long id, List<short> _opcoList)
        {
            var dto = new ServicePlanUpdateDto();
            try
            {
                var serviceMasterResource = await _repositoryWrapper.ServiceMasterRepository.FindAll().ToListAsync();
                var updatePadetails = await _plannedActivityManager.GetUpdatePage(id, _opcoList, new List<int>(), 0, 0, true);
                var createPaDetails = await _plannedActivityManager.GetCreatePage(_opcoList, new List<int>(), 0, 0, true);


                var servicePlanDetails = await GetServicePlanDetails(id);
                var servicePlanDcfDetails = await GetServicePlanDcfDetails(id);

                dto.PlannedActivityUpdateDto = updatePadetails;
                dto.PlannedActivityCreateDto = createPaDetails;
                dto.ServicePlanDetails = servicePlanDetails;
                dto.ServiceMasterResource = serviceMasterResource.ToDictionary(x => x.Servicemasterid, y => y.Description);
                dto.DcfIdList = servicePlanDcfDetails.Select(x => x.DCFId).ToList();
                dto.ServicePlanDcfDetails = servicePlanDcfDetails;
                dto.ServicePlanId = (int)id;
                dto.PlannedActivityId = updatePadetails != null ? updatePadetails.PlannedActivityId : 0;

                return dto;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return dto;
            }

        }

        public async Task<ServicePlanGridDto> GetServicePlanDetails(long serviceplanId)
        {
            var result = new ServicePlanGridDto();
            try
            {
                
                var Entity = await _repositoryWrapper.ServicePlanRepository.FindByCondition(x => x.Serviceplanid == serviceplanId).FirstOrDefaultAsync();
                if (Entity != null)
                {
                    result.ServicePlanId = Entity.Serviceplanid;
                    result.ServiceMasterId = Entity.Servicemasterid;
                    result.OpCoId = Entity.Opcoid;
                }

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<List<ServicePlanGridDto>> GetServicePlanDcfDetails(long serviceplanId)
        {
            var result = new List<ServicePlanGridDto>();
            try
            {
                var Entities = await _repositoryWrapper.ServicePlanDcfMappingRepository.FindByCondition(x => x.Serviceplanid == serviceplanId).ToListAsync();

                result = Entities.Select(x =>
                {
                    var grid = new ServicePlanGridDto();
                    grid.ServicePlanId = x.Serviceplanid.Value;
                    grid.Status = x.Status.ToString();
                    grid.DCFId = x.Dcfid;
                    grid.ServicePlanDcfMappingId = x.Serviceplandcfmappingid;
                    grid.DcfStatus = ConstantValueFilter.daMigrationStatusCode;


                    return grid;
                }).ToList();

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        #endregion

        #region // Add adn Update 
        public async Task<ResultDto> Add(ServicePlanCreateDto dto)
        {
            try
            {
                var createServicePlan = await CreateOrUpdateServicePlan(dto.ServicePlanDetails);
                if (createServicePlan.Warning != true)
                {
                    if (dto.PlannedActivityCreateDto != null)
                    {
                        var getEmptybag = await _repositoryWrapper.BuildBagRepository.FindByCondition(f => f.Bagdescription.ToLower().Trim() == ConstantValueFilter.dummyBagName.ToLower().Trim()).FirstOrDefaultAsync();
                        dto.PlannedActivityCreateDto.PlannedActivityResource = null;
                        dto.PlannedActivityCreateDto.BuildBagId = getEmptybag.Buildbagid;
                        dto.PlannedActivityCreateDto.ServicePlanid = (int)createServicePlan.Data;

                        var addingPA = await _plannedActivityManager.Add(dto.PlannedActivityCreateDto, true);

                        if (addingPA.Warning == true && addingPA.Info == ResultMessages.EntryUpdateExists)
                        {
                            return new ResultDto
                            {
                                Info = ResultMessages.EntryUpdateExists,
                                Warning = true,

                            };
                        }
                        else if (addingPA.Warning == true && addingPA.Info != ResultMessages.EntryUpdateExists)
                        {
                            return new ResultDto
                            {
                                Info = ResultMessages.EntryNotAdd,
                                Warning = true,

                            };
                        }
                        else
                        {
                            var paId = (long)addingPA.Data;
                            await _commonManager.CreateOrUpdateBptreport(paId, 0, 0);
                        }
                    }

                    foreach (var data in dto.DcfIdList)
                    {
                        var model = new Serviceplandcfmappings
                        {
                            Serviceplanid = (int)createServicePlan.Data,
                            Dcfid = data,
                            Status = (int)ServicePlanEnum.Planned,
                        };
                        _repositoryWrapper.ServicePlanDcfMappingRepository.Create(model);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryAddSuccess,
                    };
                }
                return new ResultDto
                {
                    Info = createServicePlan.Info,
                    Warning = createServicePlan.Warning,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto
                {
                    Info = ResultMessages.EntryNotAdd,
                    Warning = true,

                };
            }
        }

        public async Task<ResultDto> Update(ServicePlanUpdateDto dto)
        {
            try
            {
                var updateServicePlan = await CreateOrUpdateServicePlan(dto.ServicePlanDetails);
                if (updateServicePlan.Warning != true)
                {
                    if (dto.PlannedActivityCreateDto != null)
                    {
                        var getEmptybag = await _repositoryWrapper.BuildBagRepository.FindByCondition(f => f.Bagdescription.ToLower().Trim() == ConstantValueFilter.dummyBagName.ToLower().Trim()).FirstOrDefaultAsync();
                        dto.PlannedActivityCreateDto.PlannedActivityResource = null;
                        dto.PlannedActivityCreateDto.BuildBagId = getEmptybag.Buildbagid;
                        dto.PlannedActivityCreateDto.ServicePlanid = (int)updateServicePlan.Data;

                        var addingPA = await _plannedActivityManager.Add(dto.PlannedActivityCreateDto, true);

                        if (addingPA.Warning == true && addingPA.Info == ResultMessages.EntryUpdateExists)
                        {
                            return new ResultDto
                            {
                                Info = ResultMessages.EntryUpdateExists,
                                Warning = true,

                            };
                        }
                        else if (addingPA.Warning == true && addingPA.Info != ResultMessages.EntryUpdateExists)
                        {
                            return new ResultDto
                            {
                                Info = ResultMessages.EntryNotAdd,
                                Warning = true,

                            };
                        }
                        else
                        {
                            var paId = (long)addingPA.Data;
                            await _commonManager.CreateOrUpdateBptreport(paId, 0, 0);
                        }
                    }

                    if (dto.PlannedActivityUpdateDto != null)
                    {
                        dto.PlannedActivityUpdateDto.PlannedActivityResource = null;
                        var updatePa = await _plannedActivityManager.Update(dto.PlannedActivityUpdateDto, true,dto.ServicePlanDetails.ServicePlanId);
                        var paId = (long)updatePa.Data;
                        if (updatePa.Warning == true)
                        {
                            return new ResultDto { Info = updatePa.Info, Warning = true, };
                        }                        
                    }

                    if (dto.ServicePlanDcfDetails != null && dto.ServicePlanDcfDetails.Count > 0)
                    {
                        var getAllServicePlanDcfIds = await _repositoryWrapper.ServicePlanDcfMappingRepository.FindByCondition(x => x.Serviceplanid == (int)updateServicePlan.Data).Select(x => x.Serviceplandcfmappingid).ToListAsync();

                        foreach (var serivePlanDcfId in getAllServicePlanDcfIds)
                        {
                            var servicePlanDcfEntity = await _repositoryWrapper.ServicePlanDcfMappingRepository.FindByCondition(x => x.Serviceplandcfmappingid == serivePlanDcfId).FirstOrDefaultAsync();

                            if (dto.ServicePlanDcfDetails.Select(x => x.ServicePlanDcfMappingId).Contains(serivePlanDcfId))
                            {
                                if (servicePlanDcfEntity != null)
                                {
                                    servicePlanDcfEntity.Status = Convert.ToInt16(dto.ServicePlanDcfDetails.Where(f => f.ServicePlanDcfMappingId == serivePlanDcfId).Select(x => x.Status).FirstOrDefault());
                                }
                                _repositoryWrapper.ServicePlanDcfMappingRepository.Update(servicePlanDcfEntity);
                            }
                            else
                            {
                                _repositoryWrapper.ServicePlanDcfMappingRepository.DeleteDeep(servicePlanDcfEntity);
                            }
                        }
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                    }
                    if (dto.DcfIdList != null && dto.DcfIdList.Count > 0)
                    {
                        foreach (var data in dto.DcfIdList)
                        {
                            var checkExistEntry = await _repositoryWrapper.ServicePlanDcfMappingRepository.FindByCondition(f => f.Dcfid == data && f.Serviceplanid == (int)updateServicePlan.Data).FirstOrDefaultAsync();

                            if (checkExistEntry == null)
                            {
                                var model = new Serviceplandcfmappings
                                {
                                    Serviceplanid = dto.ServicePlanDetails.ServicePlanId,
                                    Dcfid = data,
                                    Status = (int)ServicePlanEnum.Planned,
                                };
                                _repositoryWrapper.ServicePlanDcfMappingRepository.Create(model);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();
                            }
                        }
                    }

                    return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
                }
                return new ResultDto { Info = updateServicePlan.Info, Warning = true };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto { Info = ResultMessages.EntryNotUpdate, Warning = true, };
            }

        }


        public async Task<ResultDto> CreateOrUpdateServicePlan(ServicePlanGridDto dto)
        {
            try 
            {               
                
                if (dto.ServicePlanId == 0)
                {
                    var Entity = await _repositoryWrapper.ServicePlanRepository.FindByCondition(x => x.Opcoid == dto.OpCoId && x.Servicemasterid == dto.ServiceMasterId)
                    .FirstOrDefaultAsync();

                    if (Entity != null)
                    {
                        return new ResultDto
                        {
                            Info = ResultMessages.EntryAlreadyExists,
                            Warning = true,
                        };
                    }

                    var model = ServicePlanMapper.Set( new ServicePlan
                    {
                        Servicemasterid = dto.ServiceMasterId,
                        Opcoid = dto.OpCoId,
                        Serviceplanid = dto.ServicePlanId,
                    });

                    _repositoryWrapper.ServicePlanRepository.Create(model);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryAddSuccess,
                        Data = model.Serviceplanid,
                    };
                }
                else
                {
                    var Entity = await _repositoryWrapper.ServicePlanRepository.FindByCondition(x => x.Serviceplanid != dto.ServicePlanId && x.Opcoid == dto.OpCoId && x.Servicemasterid == dto.ServiceMasterId)
                   .FirstOrDefaultAsync();

                    if (Entity != null)
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryUpdateExists,
                            Data = Entity.Serviceplanid
                        };
                    }

                    var model = ServicePlanMapper.Set( new ServicePlan
                    {
                        Serviceplanid = dto.ServicePlanId,
                        Servicemasterid = dto.ServiceMasterId,
                        Opcoid = dto.OpCoId,
                        Deleted = false,

                    });

                    _repositoryWrapper.ServicePlanRepository.Update(model);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateSuccess,
                        Data = model.Serviceplanid
                    };
                }
            
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        #endregion

        #region // Delete
        public async Task<ResultDto> DeleteDeep(long id)
        {
            try
            {
                var servicEntity = await _repositoryWrapper.ServicePlanRepository
                .FindByCondition(x => x.Serviceplanid == id)
                .FirstOrDefaultAsync();

                var deletePa = await _plannedActivityManager.DeleteDeep(id,true);
                var servicePlanDcfEntities = await _repositoryWrapper.ServicePlanDcfMappingRepository.FindByCondition(x => x.Serviceplanid == id).ToListAsync();

                if (servicEntity != null && deletePa.Warning)
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryDeleteNotExists,
                        Warning = true,
                    };
                }
                else
                {
                    foreach(var entity in servicePlanDcfEntities)
                    {
                        _repositoryWrapper.ServicePlanDcfMappingRepository.DeleteDeep(entity);
                    }
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                    _repositoryWrapper.ServicePlanRepository.DeleteDeep(servicEntity);
                   await _repositoryWrapper.SaveAsync();
                   await _repositoryWrapper.ClearTracker();

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryDeleteSuccess,
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotDeleted,
                    Warning = true,
                };
            }
        }
        #endregion

        #region // Service Plan PA Migration
        public async Task<ResultDto> ApplySettingUpdateRulesForServicePlanPA(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, short? plannedActivityTypeFor,
           List<ServicePlanGridDto> servicePlanPaDcfDetails = null)
        {
            try
            {               
                if (plannedActivityTypeFor == (short)PlannedActivityResourceEnum.Service_Planned)
                {

                    var plannedActivity = await _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid, true, false).FirstOrDefaultAsync();
                        
                    plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                    plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;


                    if (settingNew.Ruleelementcount != (int)ArchivingRuleEnum.ArchivePlannedActivityAndParentEntry && settingNew.Ruleelementcount != (int)ArchivingRuleEnum.ArchivePlannedActivityOnly)
                    {
                        if(servicePlanPaDcfDetails != null && servicePlanPaDcfDetails.Count > 0)
                        {
                            foreach (var servicePlanDcfPa in servicePlanPaDcfDetails)
                            {
                                var updateSerivePlanEntity = await _repositoryWrapper.ServicePlanDcfMappingRepository.FindByCondition(x => x.Serviceplandcfmappingid == servicePlanDcfPa.ServicePlanDcfMappingId).FirstOrDefaultAsync();

                                if (updateSerivePlanEntity != null)
                                {
                                    //updateSerivePlanEntity.Serviceplanid = servicePlanDcfPa.ServicePlanId;
                                   // updateSerivePlanEntity.Servicemasterid = servicePlanDcfPa.ServiceMasterId;
                                    //updateSerivePlanEntity.Designcomponentfamilyid = servicePlanDcfPa.DCFId;
                                    updateSerivePlanEntity.Status = Convert.ToInt16(servicePlanDcfPa.Status);
                                    _repositoryWrapper.ServicePlanDcfMappingRepository.Update(updateSerivePlanEntity);
                                }
                            }
                        }
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                        _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                    else if ((settingNew.Ruleelementcount == (int)ArchivingRuleEnum.ArchivePlannedActivityOnly))
                    {
                        Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();
                        if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Service_Planned)
                        {
                            bool setArchivalStatus = false;
                            if (servicePlanPaDcfDetails != null && servicePlanPaDcfDetails.Any())
                            {
                                foreach (var servicePlanDcf in servicePlanPaDcfDetails)
                                {
                                    var updateServicePlanDcfPaEntities = _repositoryWrapper.ServicePlanDcfMappingRepository
                                                            .FindByCondition(x => x.Serviceplandcfmappingid == servicePlanDcf.ServicePlanDcfMappingId)
                                                            .FirstOrDefault();

                                    if (updateServicePlanDcfPaEntities != null)
                                    {
                                        updateServicePlanDcfPaEntities.Status = Convert.ToInt16(servicePlanDcf.Status);
                                        _repositoryWrapper.ServicePlanDcfMappingRepository.Update(updateServicePlanDcfPaEntities);
                                    }
                                }
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();

                                var servicePlanIds = servicePlanPaDcfDetails.Select(da => da.ServicePlanId).ToList();

                                var servicePlanDcfEntity = await _repositoryWrapper.ServicePlanDcfMappingRepository
                                                                .FindByCondition(x => servicePlanIds.Contains(x.Serviceplanid.Value))
                                                                .ToListAsync();

                                setArchivalStatus = servicePlanDcfEntity.All(f => f.Status == 3)? true : false;

                                if (setArchivalStatus)
                                {
                                    plannedActivity.Archived = true;
                                    plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                                    plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;
                                    await _commonManager.setArchiveStatusForBpt(plannedActivity);
                                }
                                else
                                {
                                    return new ResultDto
                                    {
                                        Info = $"All DCF Status should be Completed before Rollout",
                                        Data = plannedActivity,
                                        Warning = true,
                                    };
                                }
                            }
                        }
                        else
                        {
                            plannedActivity.Archived = true;
                            plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                            plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;

                            await _commonManager.setArchiveStatusForBpt(plannedActivity);
                        }
                        _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                        //Project Plan Archival
                    }

                    //Below update is needed since we update plannedactivty the user should able to see the DA entry at top of the page
                    //_repositoryWrapper.DesignAspectRepository.Update(designAspectEntity);
                    //await _repositoryWrapper.SaveAsync();

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateSuccess,
                        Data = plannedActivity
                    };
                }
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }

        }
        #endregion

    }
}
