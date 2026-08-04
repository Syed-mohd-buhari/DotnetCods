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
using OracleModels.DBModels; 
using CAM.Entities.Mappers.Entity;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Infrastucture.QueryResult;
using CAM.DataTransferObjects.Entita.PlannedActivityTypes;
using CAM.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CAM.BusinessManager.Entity
{
    public class PlannedActivityTypesManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private ICurrentUserService _currentUserService;
        private GridCustomColumnManager _manager;
        public PlannedActivityTypesManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _currentUserService = currentUserService;
        }

        #region UIMemberFunction
        private static ExpressionStarter<Plannedactivitytypes> ApplyFilter(PlannedActivityTypesQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Plannedactivitytypes>();
            var predicateInner = PredicateBuilder.New<Plannedactivitytypes>();

            if (buildFilterDto.PlannedActivityTypesId != null && buildFilterDto.PlannedActivityTypesId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.PlannedActivityTypesId)
                    predicateInner.Or(x => x.Plannedactivitytypesid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedActivityTypeDescription != null && buildFilterDto.PlannedActivityTypeDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.PlannedActivityTypeDescription)
                    predicateInner.Or(x => x.Plannedactivitytypedescription == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LinkedDcRule != null && buildFilterDto.LinkedDcRule.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.LinkedDcRule)
                    predicateInner.Or(x => x.Linkeddcrule == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HwOem != null && buildFilterDto.HwOem.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.HwOem)
                    predicateInner.Or(x => x.Hwoem == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HwPlatform != null && buildFilterDto.HwPlatform.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.HwPlatform)
                    predicateInner.Or(x => x.Hwplatform == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HwSolution != null && buildFilterDto.HwSolution.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.HwSolution)
                    predicateInner.Or(x => x.Hwsolution == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SwProductname != null && buildFilterDto.SwProductname.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.SwProductname)
                    predicateInner.Or(x => x.Swproductname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SwVersion != null && buildFilterDto.SwVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.SwVersion)
                    predicateInner.Or(x => x.Swversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SwOem != null && buildFilterDto.SwOem.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.SwOem)
                    predicateInner.Or(x => x.Swoem == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubNetworkService != null && buildFilterDto.SubNetworkService.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.SubNetworkService)
                    predicateInner.Or(x => x.Subnetworkservice == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SubNetworkService != null && buildFilterDto.SubNetworkService.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.SubNetworkService)
                    predicateInner.Or(x => x.Subnetworkservice == item);
                predicateResult.And(predicateInner);
            }
            
            if (buildFilterDto.CreationUser != null && buildFilterDto.CreationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.CreationUser)
                    predicateInner.Or(x => x.CreationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ModificationUser != null && buildFilterDto.ModificationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.ModificationUser)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CreationDate != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                if (buildFilterDto.CreationDate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= buildFilterDto.CreationDate.StartDate);
                if (buildFilterDto.CreationDate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= buildFilterDto.CreationDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ModificationDate != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                if (buildFilterDto.ModificationDate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= buildFilterDto.ModificationDate.StartDate);
                if (buildFilterDto.ModificationDate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= buildFilterDto.ModificationDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ForLcm != null && buildFilterDto.ForLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.ForLcm)
                    predicateInner.Or(x => x.Forlcm == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ForAsset != null && buildFilterDto.ForAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.ForAsset)
                    predicateInner.Or(x => x.Forasset == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ForDesignAspect != null && buildFilterDto.ForDesignAspect.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.ForDesignAspect)
                    predicateInner.Or(x => x.Fordesignaspect == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ForServicePlan != null && buildFilterDto.ForServicePlan.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitytypes>();
                foreach (var item in buildFilterDto.ForServicePlan)
                    predicateInner.Or(x => x.Forservice.ToString() == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private Dictionary<string, Expression<Func<PlannedActivityTypes, object>>[]> GetColumnsMap()
        {
            Dictionary<string, Expression<Func<PlannedActivityTypes, object>>[]> gridDto = new Dictionary<string, Expression<Func<PlannedActivityTypes, object>>[]>
            {

                ["plannedActivityTypeId"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.PlannedActivityTypesId },
                ["plannedActivityTypeDescription"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.PlannedActivityTypeDescription },
                ["hwOem"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.HwOem },
                ["hwPlatform"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.HwPlatform },
                ["hwSolution"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.HwSolution },
                ["swProductname"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.SwProductname },
                ["swVersion"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.SwVersion },
                ["swOem"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.SwOem },
                ["subNetworkService"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.SubNetworkService },
                ["linkedDcRule"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.LinkedDcRule },
                ["modificationDate"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.ModificationDate },
                ["modificationUser"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationDate"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.CreationDate },
                ["creationUser"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.CreationUserEntity.Email },
                ["deleted"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.Deleted } ,
                ["forLcm"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.ForLcm },
                ["forAsset"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.ForAsset },
                ["forDesignAspect"] = new Expression<Func<PlannedActivityTypes, object>>[] { p => p.ForDesignAspect },
            };

            return gridDto;
        }
        public QueryResultDto<PlannedActivityTypesGridDto> GetPlannedActivity
            (PlannedActivityTypesQueryDto plannedRuledto)
        {
            var predicateResult = ApplyFilter(plannedRuledto);

            var query = FindByCondition(predicateResult, plannedRuledto.Deleted ?? false)
                 .ApplyOrdering(plannedRuledto, GetColumnsMap());

            var rtn = new QueryResultDto<PlannedActivityTypesGridDto>
              (new GenerateRenderForGrid<PlannedActivityTypesGridDto>(_manager))
            {
                TotalItems = query.Count()
            };

            query = query.ApplyPaging(plannedRuledto);
            var data = query.ToList();

            IEnumerable<PlannedActivityTypesGridDto> _plannedActivityResult;

            _plannedActivityResult = _mapper.Map<IEnumerable<PlannedActivityTypesGridDto>>(data);

            rtn.Items = _plannedActivityResult.ToArray();
            return rtn;


        }

        private IQueryable<PlannedActivityTypes> FindByCondition(ExpressionStarter<Plannedactivitytypes> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
              ? _repositoryWrapper.PlannedActivityTypesRepository
              .FindByCondition(predicateResult, includeDeleted)
              .Include(x => x.CreationuserNavigation)
              .Include(x => x.ModificationuserNavigation)

             : _repositoryWrapper.PlannedActivityTypesRepository.FindAll()
                  .Include(x => x.CreationuserNavigation)
              .Include(x => x.ModificationuserNavigation);


            return query.AsEnumerable().Select(x => PlannedActivityTypesMapper.GetPlannedActivityTypesRules(x)).AsQueryable();



        }
        public List<PlannedActivityTypesDropDownDto> GetPlannedDropDownList([FromBody] PlannedActivityTypesDropDownDto dropdowndto)
        {
  
            var plannedTypeQuery = _repositoryWrapper.PlannedActivityTypesRepository
             .FindByCondition(x => x.Deleted.Value == false
             ).Select(dto => new
             {
                 PlannedActivityTypesId = (short)dto.Plannedactivitytypesid,
                 PlannedActivityTypeDescription = dto.Plannedactivitytypedescription,
                 ForLcm = (bool) dto.Forlcm,
                 ForAsset = (bool) dto.Forasset,
                 ForDesignAspect = (bool) dto.Fordesignaspect
             });

            List<PlannedActivityTypesDropDownDto> typedropdownlist = new List<PlannedActivityTypesDropDownDto>();

            if (dropdowndto != null && dropdowndto.PagewiseId != null)
            {

                // LcmEngineering = 1, AddAsset = 2,   EditAsset = 3,  DesignAspect = 4

                if (dropdowndto.PagewiseId == 1)
                    typedropdownlist = plannedTypeQuery.Where(x => x.ForLcm == true).Select(dto => new PlannedActivityTypesDropDownDto()
                    {
                        PlannedActivityTypesId = (short)dto.PlannedActivityTypesId,
                        PlannedActivityTypeDescription = dto.PlannedActivityTypeDescription

                    }).ToList<PlannedActivityTypesDropDownDto>();

                else if (dropdowndto.PagewiseId == 2)

                    typedropdownlist = plannedTypeQuery.Where(x => x.ForAsset == true).Select(dto => new PlannedActivityTypesDropDownDto()
                    {
                        PlannedActivityTypesId = (short)dto.PlannedActivityTypesId,
                        PlannedActivityTypeDescription = dto.PlannedActivityTypeDescription

                    }).ToList<PlannedActivityTypesDropDownDto>();

                else if (dropdowndto.PagewiseId == 3)

                    typedropdownlist = plannedTypeQuery.Where(x => x.ForDesignAspect == true).Select(dto => new PlannedActivityTypesDropDownDto()
                    {
                        PlannedActivityTypesId = (short)dto.PlannedActivityTypesId,
                        PlannedActivityTypeDescription = dto.PlannedActivityTypeDescription

                    }).ToList<PlannedActivityTypesDropDownDto>();
                else
                {
                    long maxPaTypeId = _repositoryWrapper.PlannedActivityTypesRepository.FindAll().Max(x => x.Plannedactivitytypesid);
                    typedropdownlist = plannedTypeQuery.Where(x => x.PlannedActivityTypesId >= 0 &&
                        x.PlannedActivityTypesId <= maxPaTypeId
                        ).Select(dto => new PlannedActivityTypesDropDownDto()
                        {
                            PlannedActivityTypesId = (short)dto.PlannedActivityTypesId,
                            PlannedActivityTypeDescription = dto.PlannedActivityTypeDescription

                        }).ToList<PlannedActivityTypesDropDownDto>();
                }

            }
             

            return typedropdownlist;

           

        }
        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, PlannedActivityTypesQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = FindByCondition(predicateResult, false);

            var rtn = propertyName switch
            {
                "plannedactivitytypesid" => string.IsNullOrEmpty(propertyFilter)
   ? query.Select(p => new FilterValueDto
   { Text = p.PlannedActivityTypesId.ToString(), Value = p.PlannedActivityTypesId.ToString() }).Distinct().ToList()
   : query
       .Where(x => x.PlannedActivityTypesId.ToString().Contains(propertyFilter)).Select(p =>
           new FilterValueDto { Text = p.PlannedActivityTypesId.ToString(), Value = p.PlannedActivityTypesId.ToString() }).Distinct()
       .ToList(),

                "hwOem" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.HwOem == false ? "NO" : "YES", Value = p.HwOem.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.HwOem.Value ? "YES" : "NO").Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.HwOem == false ? "NO" : "YES", Value = p.HwOem.ToString() }).Distinct()
                        .ToList(),


                "hwSolution" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.HwSolution == false ? "NO" : "YES", Value = p.HwSolution.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.HwSolution.Value ? "YES" : "NO").Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.HwSolution == false ? "NO" : "YES", Value = p.HwSolution.ToString() }).Distinct()
                        .ToList(),


                "hwPlatform" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.HwPlatform == false ? "NO" : "YES", Value = p.HwPlatform.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.HwPlatform.Value ? "YES" : "NO").Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.HwPlatform == false ? "NO" : "YES", Value = p.HwPlatform.ToString() }).Distinct()
                        .ToList(),
                "swOem" =>
          string.IsNullOrEmpty(propertyFilter)
          ? query
              .Select(p => new FilterValueDto { Text = p.SwOem == false ? "NO" : "YES", Value = p.SwOem.ToString() }).Distinct().ToList()
          : query
              .Where(p => (p.SwOem.Value ? "YES" : "NO").Contains(propertyFilter))
              .Select(p => new FilterValueDto { Text = p.SwOem == false ? "NO" : "YES", Value = p.SwOem.ToString() }).Distinct()
              .ToList(),


                "swProductname" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.SwProductname == false ? "NO" : "YES", Value = p.SwProductname.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.SwProductname.Value ? "YES" : "NO").Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.SwProductname == false ? "NO" : "YES", Value = p.SwProductname.ToString() }).Distinct()
                        .ToList(),


                "swVersion" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.SwVersion == false ? "NO" : "YES", Value = p.SwVersion.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.SwVersion.Value ? "YES" : "NO").Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.SwVersion == false ? "NO" : "YES", Value = p.SwVersion.ToString() }).Distinct()
                        .ToList(),

                "deleted" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.Deleted == false ? "NO" : "YES", Value = p.Deleted.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.Deleted ? "YES" : "NO").Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Deleted == false ? "NO" : "YES", Value = p.Deleted.ToString() }).Distinct()
                        .ToList(),

                "subNetworkService" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.SubNetworkService == false ? "NO" : "YES", Value = p.SubNetworkService.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.SubNetworkService.Value ? "YES" : "NO").Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.SubNetworkService == false ? "NO" : "YES", Value = p.SubNetworkService.ToString() }).Distinct()
                        .ToList(),
                "plannedActivityTypeDescription" =>
                                string.IsNullOrEmpty(propertyFilter)
                                ? query
                                    .Select(p => new FilterValueDto { Text = p.PlannedActivityTypeDescription.ToString(), Value = p.PlannedActivityTypeDescription.ToString() }).Distinct().ToList()
                                : query
                                    .Where(p => (p.PlannedActivityTypeDescription.ToString()).Contains(propertyFilter))
                                    .Select(p => new FilterValueDto { Text = p.PlannedActivityTypeDescription.ToString(), Value = p.PlannedActivityTypeDescription.ToString() }).Distinct()
                                    .ToList(),
                "linkedDcRule" =>
                           string.IsNullOrEmpty(propertyFilter)
                           ? query
                               .Select(p => new FilterValueDto { Text = p.LinkedDcRule == false ? "NO" : "YES", Value = p.LinkedDcRule.ToString() }).Distinct().ToList()
                           : query
                               .Where(p => (p.LinkedDcRule.Value ? "YES" : "NO").Contains(propertyFilter))
                               .Select(p => new FilterValueDto { Text = p.LinkedDcRule == false ? "NO" : "YES", Value = p.LinkedDcRule.ToString() }).Distinct()
                               .ToList(),
               
                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.CreationUser.ToString(), Value = p.CreationUser.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.CreationUser.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.CreationUser.ToString(), Value = p.CreationUser.ToString() }).Distinct().ToList(),



                "creationDate" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.CreationDate.ToString(), Value = p.CreationDate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.CreationDate.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.CreationDate.ToString(), Value = p.CreationDate.ToString() }).Distinct().ToList(),


                "modificationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationUser.ToString(), Value = p.ModificationUser.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUser.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUser.ToString(), Value = p.ModificationUser.ToString() }).Distinct().ToList(),

             
                "modificationDate" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.ModificationDate.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList(),

                "forLcm" =>
                                    string.IsNullOrEmpty(propertyFilter)
                                    ? query
                                        .Select(p => new FilterValueDto { Text = p.ForLcm == false ? "NO" : "YES", Value = p.ForLcm.ToString() }).Distinct().ToList()
                                    : query
                                        .Where(p => (p.ForLcm.Value ? "YES" : "NO").Contains(propertyFilter))
                                        .Select(p => new FilterValueDto { Text = p.ForLcm == false ? "NO" : "YES", Value = p.ForLcm.ToString() }).Distinct()
                                        .ToList(),
                "forAsset" =>
                                    string.IsNullOrEmpty(propertyFilter)
                                    ? query
                                        .Select(p => new FilterValueDto { Text = p.ForAsset == false ? "NO" : "YES", Value = p.ForAsset.ToString() }).Distinct().ToList()
                                    : query
                                        .Where(p => (p.ForAsset.Value ? "YES" : "NO").Contains(propertyFilter))
                                        .Select(p => new FilterValueDto { Text = p.ForAsset == false ? "NO" : "YES", Value = p.ForAsset.ToString() }).Distinct()
                                        .ToList(),
                "forDesignAspect" =>
                                    string.IsNullOrEmpty(propertyFilter)
                                    ? query
                                        .Select(p => new FilterValueDto { Text = p.ForDesignAspect == false ? "NO" : "YES", Value = p.ForDesignAspect.ToString() }).Distinct().ToList()
                                    : query
                                        .Where(p => (p.ForDesignAspect.Value ? "YES" : "NO").Contains(propertyFilter))
                                        .Select(p => new FilterValueDto { Text = p.ForDesignAspect == false ? "NO" : "YES", Value = p.ForDesignAspect.ToString() }).Distinct()
                                        .ToList(),

                "isServicePlan" =>
                                string.IsNullOrEmpty(propertyFilter)
                                    ? query
                                        .Select(p => new FilterValueDto
                                        {
                                            Text = p.Forservice.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                                            Value = p.Forservice.ToString()
                                        }).Distinct().ToList()
                                    : query
                                        .Where(p => p.Forservice.ToString().Contains(propertyFilter))
                                        .Select(p => new FilterValueDto
                                        {
                                            Text = p.Forservice.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                                            Value = p.Forservice.ToString()
                                        }).Distinct().ToList(),

                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        #endregion




        #region //CRUD


        public async Task<ResultDto> DeleteRule([FromBody] PlannedActivityTypesCreateOrUpdateDto dto)
        {
            if (dto == null) return new ResultDto
            {
                Info = ResultMessages.EntryLcmIdParameterIssue
            };

            var entity = await _repositoryWrapper.PlannedActivityTypesRepository
                    .FindByConditionWithDelete(x => x.Plannedactivitytypesid == dto.PlannedActivityTypesId).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.Deleted = !(entity.Deleted);
                _repositoryWrapper.PlannedActivityTypesRepository.Update(entity);

            }
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess
            };
        }
        public async Task<ResultDto> EnableDisableLinkedDcRule([FromBody] PlannedActivityTypesCreateOrUpdateDto dto)
        {

            if (dto == null) return new ResultDto
            {
                Info = ResultMessages.EntryLcmIdParameterIssue
            };

            var entity = await _repositoryWrapper.PlannedActivityTypesRepository
                    .FindByConditionWithDelete(x => x.Plannedactivitytypesid == dto.PlannedActivityTypesId).FirstOrDefaultAsync();
            if (entity != null)
            {

                entity.Linkeddcrule = dto.LinkedDcRule;

                _repositoryWrapper.PlannedActivityTypesRepository.Update(entity);
                await _repositoryWrapper.SaveAsync();
            }

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,

            };
        }
        public async Task<ResultDto> Add(PlannedActivityTypesCreateOrUpdateDto dto)
        {
            try
            {

                PlannedActivityTypes entity = new PlannedActivityTypes()
                {
                    PlannedActivityTypeDescription = dto.PlannedActivityTypeDescription,
                    HwOem = dto.HwOem,
                    HwPlatform = dto.HwPlatform,
                    HwSolution = dto.HwSolution,
                    SwOem = dto.SwOem,
                    SwProductname = dto.SwProductname,
                    SwVersion = dto.SwVersion,
                    SubNetworkService = dto.SubNetworkService,
                    LinkedDcRule = dto.LinkedDcRule,
                    Deleted = true ,
                    ForLcm = dto.ForLcm,
                    ForAsset= dto.ForAsset,
                    ForDesignAspect = dto.ForDesignAspect,
                    Forservice = dto.ForServicePlan,

                };

                _repositoryWrapper.PlannedActivityTypesRepository.
                    Create(PlannedActivityTypesMapper.SetPlannedActivityTypesRules(entity));

                await _repositoryWrapper.SaveAsync();


                return new ResultDto { Info = ResultMessages.EntryAddSuccess };
            }
            catch
            {
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        public async Task<ResultDto> Update(PlannedActivityTypesCreateOrUpdateDto dto)
        {
            try
            {

                var entity = await _repositoryWrapper.PlannedActivityTypesRepository.
                    FindByConditionWithDelete(x => x.Plannedactivitytypesid == dto.PlannedActivityTypesId).FirstOrDefaultAsync();
                if (entity != null)
                {
                    entity.Plannedactivitytypedescription = dto.PlannedActivityTypeDescription;
                    entity.Hwoem = dto.HwOem;
                    entity.Hwplatform = dto.HwPlatform;
                    entity.Hwsolution = dto.HwSolution;
                    entity.Swversion = dto.SwVersion;
                    entity.Swproductname = dto.SwProductname;
                    entity.Swoem = dto.SwOem;
                    entity.Subnetworkservice = dto.SubNetworkService;
                    entity.Linkeddcrule = dto.LinkedDcRule;
                    entity.Forlcm = dto.ForLcm;
                    entity.Forasset = dto.ForAsset;
                    entity.Fordesignaspect = dto.ForDesignAspect;
                    entity.Forservice = dto.ForServicePlan;

                }
                _repositoryWrapper.PlannedActivityTypesRepository.Update(entity);


                await _repositoryWrapper.SaveAsync();


                return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
            }
            catch
            {
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        #endregion


    }
}
