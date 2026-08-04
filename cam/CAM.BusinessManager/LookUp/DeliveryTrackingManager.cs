using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.DeliveryTracking;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;

namespace CAM.BusinessManager.LookUp
{
    public class DeliveryTrackingManager : GridBaseAsync<DeliveryTracking, DeliveryTrackingDtoGrid, DeliveryTrackingQueryDto, Deliverytrackings>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private CommonManager _commonManager;

        public DeliveryTrackingManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _commonManager = commonManager;
        }
  

        public override ExpressionStarter<Deliverytrackings> ApplyFilterForOracleModel(DeliveryTrackingQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Deliverytrackings>();
            var predicateInner = PredicateBuilder.New<Deliverytrackings>();
            predicateInner.Or(x => x.Plannedactivity.Archived == false);
            predicateResult.And(predicateInner);

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }
            // add ppmid for new requirment
            if (request.PpmID != null && request.PpmID.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.PpmID)
                    predicateInner.Or(x => x.Plannedactivity.Deliveryprojectid == item);
                predicateResult.And(predicateInner);
            }

            if (request.PPMImportDate != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.PPMImportDate.StartDate != null)
                    predicateInner.And(x => x.Ppmimportdate.Value.Date >= request.PPMImportDate.StartDate);
                if (request.PPMImportDate.EndDate != null)
                    predicateInner.And(x => x.Ppmimportdate.Value.Date <= request.PPMImportDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.MS1LatestPlanningDate != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.MS1LatestPlanningDate.StartDate != null)
                    predicateInner.And(x => x.Ms1latestplanningdate.Value.Date >= request.MS1LatestPlanningDate.StartDate);
                if (request.MS1LatestPlanningDate.EndDate != null)
                    predicateInner.And(x => x.Ms1latestplanningdate.Value.Date <= request.MS1LatestPlanningDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.MS1EventType != null && request.MS1EventType.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.MS1EventType)
                    predicateInner.Or(x => x.Ms1eventtype == item);
                predicateResult.And(predicateInner);
            }


            if (request.MS1Status != null && request.MS1Status.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.MS1Status)
                    predicateInner.Or(x => x.Ms1status == item);
                predicateResult.And(predicateInner);
            }

            if (request.MS1BaseLineDate != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.MS1BaseLineDate.StartDate != null)
                    predicateInner.And(x => x.Ms1baselinedate.Value.Date >= request.MS1BaseLineDate.StartDate);
                if (request.MS1BaseLineDate.EndDate != null)
                    predicateInner.And(x => x.Ms1baselinedate.Value.Date <= request.MS1BaseLineDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.MS2LatestPlanningDate != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.MS2LatestPlanningDate.StartDate != null)
                    predicateInner.And(x => x.Ms2latestplanningdate.Value.Date >= request.MS2LatestPlanningDate.StartDate);
                if (request.MS2LatestPlanningDate.EndDate != null)
                    predicateInner.And(x => x.Ms2latestplanningdate.Value.Date <= request.MS2LatestPlanningDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.MS2EventType != null && request.MS2EventType.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.MS2EventType)
                    predicateInner.Or(x => x.Ms2eventtype == item);
                predicateResult.And(predicateInner);
            }


            if (request.MS2Status != null && request.MS2Status.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.MS2Status)
                    predicateInner.Or(x => x.Ms2status == item);
                predicateResult.And(predicateInner);
            }

            if (request.MS2BaseLineDate != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.MS2BaseLineDate.StartDate != null)
                    predicateInner.And(x => x.Ms2baselinedate.Value.Date >= request.MS2BaseLineDate.StartDate);
                if (request.MS2BaseLineDate.EndDate != null)
                    predicateInner.And(x => x.Ms2baselinedate.Value.Date <= request.MS2BaseLineDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.MS3LatestPlanningDate != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.MS3LatestPlanningDate.StartDate != null)
                    predicateInner.And(x => x.Ms3latestplanningdate.Value.Date >= request.MS3LatestPlanningDate.StartDate);
                if (request.MS3LatestPlanningDate.EndDate != null)
                    predicateInner.And(x => x.Ms3latestplanningdate.Value.Date <= request.MS3LatestPlanningDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.MS3EventType != null && request.MS3EventType.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.MS3EventType)
                    predicateInner.Or(x => x.Ms3eventtype == item);
                predicateResult.And(predicateInner);
            }


            if (request.MS3Status != null && request.MS3Status.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.MS3Status)
                    predicateInner.Or(x => x.Ms3status == item);
                predicateResult.And(predicateInner);
            }

            if (request.MS3BaseLineDate != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.MS3BaseLineDate.StartDate != null)
                    predicateInner.And(x => x.Ms3baselinedate.Value.Date >= request.MS3BaseLineDate.StartDate);
                if (request.MS3BaseLineDate.EndDate != null)
                    predicateInner.And(x => x.Ms3baselinedate.Value.Date <= request.MS3BaseLineDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.MS4LatestPlanningDate != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.MS4LatestPlanningDate.StartDate != null)
                    predicateInner.And(x => x.Ms4latestplanningdate.Value.Date >= request.MS4LatestPlanningDate.StartDate);
                if (request.MS4LatestPlanningDate.EndDate != null)
                    predicateInner.And(x => x.Ms2latestplanningdate.Value.Date <= request.MS4LatestPlanningDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.MS4EventType != null && request.MS4EventType.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.MS4EventType)
                    predicateInner.Or(x => x.Ms4eventtype == item);
                predicateResult.And(predicateInner);
            }


            if (request.MS4Status != null && request.MS4Status.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.MS4Status)
                    predicateInner.Or(x => x.Ms4status == item);
                predicateResult.And(predicateInner);
            }

            if (request.MS4BaseLineDate != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.MS4BaseLineDate.StartDate != null)
                    predicateInner.And(x => x.Ms4baselinedate.Value.Date >= request.MS4BaseLineDate.StartDate);
                if (request.MS2BaseLineDate.EndDate != null)
                    predicateInner.And(x => x.Ms4baselinedate.Value.Date <= request.MS4BaseLineDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Activity != null && request.Activity.Any())
            {

                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                //long dcId;
                //short activityTypeId;
                foreach (var item in request.Activity)
                {
                    predicateInner.Or(x => x.Id == item);
                    //var activityTypeIdHasValue = short.TryParse(item.Split("-")[0], out activityTypeId);
                    //var dcIdHasValue = long.TryParse(item.Split("-")[1], out dcId);
                    //if( activityTypeIdHasValue && dcIdHasValue )
                    //{
                    //    predicateInner.Or(x => x.Plannedactivity.Plannedactivityresourceid == activityTypeId && 
                    //                            x.Plannedactivity.Designcomponentid == dcId);
                    //}
                }
                predicateResult.And(predicateInner);
            }

            if (request.PlannedActivityId != null && request.PlannedActivityId.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.PlannedActivityId)
                    predicateInner.Or(x => x.Plannedactivityid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Notes1 != null && request.Notes1.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.Notes1)
                    predicateInner.Or(x => x.Notes1 == item);
                predicateResult.And(predicateInner);
            }

            if (request.Notes2 != null && request.Notes2.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.Notes2)
                    predicateInner.Or(x => x.Notes2 == item);
                predicateResult.And(predicateInner);
            }

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Plannedactivity.Plannedactivityresource.Plannedactivityresource == item);
                predicateResult.And(predicateInner);
            }

            if (request.ActivityIndex != null && request.ActivityIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.ActivityIndex)
                    predicateInner.Or(x => x.Plannedactivityid == long.Parse(item.Substring(2)));
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Deliverytrackings>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<DeliveryTrackingDtoGrid> CastObjectToDto(IQueryable<DeliveryTracking> request)
        {
            return  request.Select(dto => new DeliveryTrackingDtoGrid()
            {
                Id = (short)dto.Id,
                Notes1 = dto.Notes1,
                Notes2 = dto.Notes2,
                Ms1BaseLineDate = dto.MS1BaseLineDate,
                Ms1EventType = dto.MS1EventType,
                Ms1LatestPlanningDate = dto.MS1LatestPlanningDate,
                Ms1Status = dto.MS1Status != null ? ConvertToDeliveryStatusEnum(dto.MS1Status) :string.Empty,
                Ms2BaseLineDate = dto.MS2BaseLineDate,
                Ms2EventType = dto.MS2EventType,
                Ms2LatestPlanningDate = dto.MS2LatestPlanningDate,
                Ms2Status = dto.MS2Status != null ? ConvertToDeliveryStatusEnum(dto.MS2Status) : string.Empty,
                Ms3BaseLineDate = dto.MS3BaseLineDate,
                Ms3EventType = dto.MS3EventType,
                Ms3LatestPlanningDate = dto.MS3LatestPlanningDate,
                Ms3Status = dto.MS3Status != null ? ConvertToDeliveryStatusEnum(dto.MS3Status) : string.Empty,
                Ms4BaseLineDate = dto.MS4BaseLineDate,
                Ms4EventType = dto.MS4EventType,
                Ms4LatestPlanningDate = dto.MS4LatestPlanningDate,
                Ms4Status = dto.MS4Status != null ? ConvertToDeliveryStatusEnum(dto.MS4Status) : string.Empty,
                // condition added in order to remove hyphen in case of no dc available # Based on the new requirement we are displaying the opco,Dc,and PA type.
                Activity = PlannedActivityMapper.Set(dto.PlannedActivity).GetActvityString(_repositoryWrapper),
                PlannedActivityId = dto.PlannedActivityId,
                PpmImportDate = dto.PPMImportDate,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture),
                Description = dto.PlannedActivity.PlannedActivityResource.PlannedActivityResourceDescription,
                ActivityIndex = $"AI{dto.PlannedActivityId:000000}",
                PpmID = dto.PlannedActivity.DeliveryProjectId,
                Opco = dto.OpcoDescription,
                OpcoId = (short)dto.OpcoId,
                VerticalName=dto.VerticalFilterDto!=null && dto.VerticalFilterDto.Count>0 ? 
                            string.Join(",",dto.VerticalFilterDto.Select(x=>x.Value).ToList()??new List<string>()):string.Empty,
            }).ToList();
        }

        private string ConvertToDeliveryStatusEnum(int? status)
        {
            var result = "";
            switch (status)
            {               
                   
                case 1:
                    result = "On track";
                    break;
                case 2:
                    result = "Delayed";
                    break;
                case 3:
                    result = "Completed";
                    break;
                default:
                    result = "";
                    break;

            }
            return result;
        }

        public override Dictionary<string, Expression<Func<DeliveryTracking, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<DeliveryTracking, object>>[]>
            {
                ["id"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.Id },
                ["plannedActivityId"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.PlannedActivityId },
                ["ms1EvenType"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS1EventType },
                ["ms1Status"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS1Status },
                ["ms1BaselineDate"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS1BaseLineDate },
                ["ms1LatestPlanningDate"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS1LatestPlanningDate },
                ["ms2EvenType"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS2EventType },
                ["ms2Status"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS2Status },
                ["ms2BaselineDate"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS2BaseLineDate },
                ["ms2LatestPlanningDate"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS2LatestPlanningDate },
                ["ms3EvenType"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS3EventType },
                ["ms3Status"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS3Status },
                ["ms3BaselineDate"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS3BaseLineDate },
                ["ms3LatestPlanningDate"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS3LatestPlanningDate },
                ["ms4EvenType"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS4EventType },
                ["ms4Status"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS4Status },
                ["ms4BaselineDate"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS4BaseLineDate },
                ["ms4LatestPlanningDate"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.MS4LatestPlanningDate },
                ["notes1"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.Notes1 },
                ["notes2"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.Notes2 },
                ["ppmImportDate"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.PPMImportDate },
                ["lastModifiedBy"] = new Expression<Func<DeliveryTracking, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<DeliveryTracking> request, string propertyName, string propertyFilter)
        {
            IEnumerable<FilterValueDto> result = propertyName switch
            {
                "id" =>  await Task.Run(()=>request.Select(x => new FilterValueDto(x.Id.ToString()))),
                "plannedActivityId" =>  request.Select(x => new FilterValueDto(x.PlannedActivityId)),
                "ppmID" => request.Select(x => new FilterValueDto
                {
                    Text = x.PlannedActivity.DeliveryProjectId,
                    Value = x.PlannedActivity.DeliveryProjectId
                }),
                "activity" => request.Select(x => new FilterValueDto
                {
                    Text = PlannedActivityMapper.Set(x.PlannedActivity).GetActvityString(_repositoryWrapper),
                    Value = x.Id.ToString()
                }),
                "ms1EventType" => request.Select(x => new FilterValueDto(x.MS1EventType)),
                "ms1Status" =>  request.Select(x => new FilterValueDto
                {
                    Text =x.MS1Status != null?ConvertToDeliveryStatusEnum(x.MS1Status):"" , Value = x.MS1Status.ToString()
                }),
                "ms1BaseLineDate" =>  request.Select(x => new FilterValueDto(x.MS1BaseLineDate)),
                "ms1LatestPlanningDate" =>  request.Select(x => new FilterValueDto(x.MS1LatestPlanningDate)),
                "ms2EventType" =>  request.Select(x => new FilterValueDto(x.MS2EventType)),
                "ms2Status" =>  request.Select(x => new FilterValueDto
                {
                    Text = x.MS2Status != null ? ConvertToDeliveryStatusEnum(x.MS2Status) : "",
                    Value = x.MS2Status.ToString()
                }),
                "ms2BaseLineDate" =>  request.Select(x => new FilterValueDto(x.MS2BaseLineDate)),
                 "ms2LatestPlanningDate" =>  request.Select(x => new FilterValueDto(x.MS2LatestPlanningDate)),
                "ms3EventType" =>  request.Select(x => new FilterValueDto(x.MS3EventType)),
                "ms3Status" =>  request.Select(x => new FilterValueDto
                {
                    Text = x.MS3Status != null ? ConvertToDeliveryStatusEnum(x.MS3Status) : "",
                    Value = x.MS3Status.ToString()
                }),
                "ms3BaseLineDate" =>  request.Select(x => new FilterValueDto(x.MS3BaseLineDate)),
                "ms3LatestPlanningDate" =>  request.Select(x => new FilterValueDto(x.MS3LatestPlanningDate)),
                "ms4EventType" =>  request.Select(x => new FilterValueDto(x.MS4EventType)),
                "ms4Status" =>  request.Select(x => new FilterValueDto
                {
                    Text = x.MS4Status != null ? ConvertToDeliveryStatusEnum(x.MS4Status) : "",
                    Value = x.MS4Status.ToString()
                }),
                "ms4BaseLineDate" =>  request.Select(x => new FilterValueDto(x.MS4BaseLineDate)),
                "ms4LatestPlanningDate" =>  request.Select(x => new FilterValueDto(x.MS4LatestPlanningDate)),
                "notes1" =>  request.Select(x => new FilterValueDto(x.Notes1)),
                "notes2" =>  request.Select(x => new FilterValueDto(x.Notes2)),
                "lastModifiedBy" => request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "description" => request.Select(x => new FilterValueDto(x.PlannedActivity.PlannedActivityResource.PlannedActivityResourceDescription)),
                "activityIndex" => request.Select(x => new FilterValueDto($"AI{x.PlannedActivityId:000000}")),
                "opco" =>request.Select(x=>new FilterValueDto { Value=x.OpcoId.ToString(),Text=x.OpcoDescription}),
                "verticalName" => request.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count > 0)
                                                  .SelectMany(x=>x.VerticalFilterDto.Select(y=>new FilterValueDto { Text=y.Value,Value=y.Key.ToString()})).Distinct().ToList()
                                                  .Concat(
                                     (request.AsEnumerable().Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count == 0))
                                     .Select(x =>
                                             _commonManager.AddBlankFilterValue()
                                     )
                                 )
                                 .Distinct().ToList(),
                
                _ => new List<FilterValueDto>()

            };

            return result;
        }

        public override IQueryable<DeliveryTracking> PrepareQuery(DeliveryTrackingQueryDto request, ExpressionStarter<DeliveryTracking> predicateResult , ExpressionStarter<Deliverytrackings> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.DeliveryTrackingRepository.FindAll();

            var includedQueryEntities_withoutFilter = query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Plannedactivityresource)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponentfamily)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Opco)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily);

            if((request.OpCo!=null && request.OpCo.Count>0) && (request.VerticalName != null && request.VerticalName.Count > 0))
            {
                var includedQueryEntities= includedQueryEntities_withoutFilter
                    .Include(x => x.Plannedactivity).ThenInclude(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Networkelementasplanned).ThenInclude(x => x.Networkelementasplannedsubdomainspoc)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                                                             .ThenInclude(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                                                             .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware.Majorhwbuildsdesigncontacts)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Serviceplan).ThenInclude(x => x.Serviceplandcfmappings).ThenInclude(x => x.Dcf).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                .Include(x => x.Plannedactivity).ThenInclude(x => x.Serviceplan).ThenInclude(x => x.Serviceplandcfmappings).ThenInclude(x => x.Dcf).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)

                .AsEnumerable().Select(p => DeliveryTrackingMapper.GetDeliveryTrackingMapper(p)).AsQueryable();

                var entity = includedQueryEntities.ToList();
            foreach(var item in entity)
                {
                if (item.AssetId != null && item.AssetSubdomainSpoc !=null && item.AssetSubdomainSpoc.Count>0)
                    {
                        item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.AssetSubdomainSpoc.ToList(), 0
                           )?.Select(t => new FilterValueDtoKeyValueList
                           {
                               Key = Convert.ToInt16(t.Value),
                               Value = t.Text
                           })?.Distinct()?.ToList();
                    }
                    else if (item.LcmEngineeringId != null && item.LcmEngineeringSubdomainSpoc != null && item.LcmEngineeringSubdomainSpoc.Count > 0)
                    {
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.LcmEngineeringSubdomainSpoc.ToList(),0
                           )?.Select(t => new FilterValueDtoKeyValueList
                           {
                               Key = Convert.ToInt16(t.Value),
                               Value = t.Text
                           })?.Distinct()?.ToList();
                    }
                else if (item.DesignAspectId != null && item.DesignContactDto!=null && item.DesignContactDto.Count>0)
                    {
                        item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.DesignContactDto,
                           0)?.Select(t => new FilterValueDtoKeyValueList
                           {
                               Key = Convert.ToInt16(t.Value),
                               Value = t.Text
                           })?.Distinct()?.ToList();
                    }
                    else if (item.ServiceInfoId != null && item.DesignContactDto != null && item.DesignContactDto.Count > 0)
                    {
                        item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.DesignContactDto,
                           0)?.Select(t => new FilterValueDtoKeyValueList
                           {
                               Key = Convert.ToInt16(t.Value),
                               Value = t.Text
                           })?.Distinct()?.ToList();
                    }
                }

                #region Opco and vertical filtering for login user

                var entityToFilter = entity.AsQueryable();

                if (request.OpCo?.Any() == true)
                {
                    entityToFilter = entityToFilter.Where(x => x.OpcoId != null && request.OpCo.Contains((short)x.OpcoId));
                }
                if (request.VerticalName?.Any() == true && !request.VerticalName.Contains("yes"))
                {
                    entityToFilter = entityToFilter.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Any(c => x.VerticalFilterDto != null && request.VerticalName.Contains(c.Key.ToString())));
                }
                else if (request.VerticalName?.Any() == true && request.VerticalName.Count() == 1 && request.VerticalName.Contains("yes"))
                {
                    entityToFilter = entityToFilter.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));
                }
                else if (request.VerticalName?.Any() == true && request.VerticalName.Count() > 1 && request.VerticalName.Contains("yes"))
                {
                    var nullVerticals = request.VerticalName.Contains("yes") ?
                        entityToFilter.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0)) : null;
                    var verticalFilter = entityToFilter
                                        .Where(x => x.VerticalFilterDto != null &&
                                        x.VerticalFilterDto.Any(c => request.VerticalName.Where(t => t != "yes").Contains(c.Key.ToString())));
                    entityToFilter = nullVerticals?.Any() == true && verticalFilter?.Any() == true ?
                        nullVerticals.Concat(verticalFilter) : nullVerticals?.Any() == true && verticalFilter
                        ?.Any() == false ? nullVerticals
                        : nullVerticals?.Any() == false && verticalFilter?.Any() == true ? verticalFilter : null;
                }

                #endregion
                return entityToFilter;
            }
            else
            {
                var entityResult=includedQueryEntities_withoutFilter.AsEnumerable().Select(p => DeliveryTrackingMapper.GetDeliveryTrackingMapper(p)).AsQueryable();
                return entityResult;
            }



        }

        public async Task<ResultDto> Add(DeliveryTrackingDtoCreate dto)
        {
            var paHasDeliveryTracking = _repositoryWrapper.DeliveryTrackingRepository
                                    .FindByCondition(x => x.Plannedactivityid == dto.PlannedActivityId)
                                    .Any();
            if (!paHasDeliveryTracking)
            {

                DeliveryTracking entity = new DeliveryTracking()
                {
                    Id = dto.Id,
                    Notes1 = dto.Notes1,
                    Notes2 = dto.Notes2,
                    MS1BaseLineDate = dto.Ms1BaseLineDate,
                    MS1EventType = dto.Ms1EventType,
                    MS1LatestPlanningDate = dto.Ms1LatestPlanningDate,
                    MS1Status = dto.Ms1status,
                    MS2BaseLineDate = dto.Ms2BaseLineDate,
                    MS2EventType = dto.Ms2EventType,
                    MS2LatestPlanningDate = dto.Ms2LatestPlanningDate,
                    MS2Status = dto.Ms2status,
                    MS3BaseLineDate = dto.Ms3BaseLineDate,
                    MS3EventType = dto.Ms3EventType,
                    MS3LatestPlanningDate = dto.Ms3LatestPlanningDate,
                    MS3Status = dto.Ms3status,
                    MS4BaseLineDate = dto.Ms4BaseLineDate,
                    MS4EventType = dto.Ms4EventType,
                    MS4LatestPlanningDate = dto.Ms4LatestPlanningDate,
                    MS4Status = dto.Ms4status,
                    PlannedActivityId = dto.PlannedActivityId,
                    PPMImportDate = dto.PpmImportDate
                };
                if(entity != null)
                {
                    _repositoryWrapper.DeliveryTrackingRepository.Create(DeliveryTrackingMapper.SetDeliveryTrackingMapper(entity));
                    await _repositoryWrapper.SaveAsync();
                }
                else return new ResultDto { Info = ResultMessages.EntryNotAdd };

            }
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(DeliveryTrackingDtoUpdate dto)
        {
            DeliveryTracking entity = new DeliveryTracking()
            {
                Id = dto.Id,
                Notes1 = dto.Notes1,
                Notes2 = dto.Notes2,
                MS1BaseLineDate = dto.Ms1BaseLineDate,
                MS1EventType = dto.Ms1EventType,
                MS1LatestPlanningDate = dto.Ms1LatestPlanningDate,
                MS1Status = dto.Ms1status,
                MS2BaseLineDate = dto.Ms2BaseLineDate,
                MS2EventType = dto.Ms2EventType,
                MS2LatestPlanningDate = dto.Ms2LatestPlanningDate,
                MS2Status = dto.Ms2status,
                MS3BaseLineDate = dto.Ms3BaseLineDate,
                MS3EventType = dto.Ms3EventType,
                MS3LatestPlanningDate = dto.Ms3LatestPlanningDate,
                MS3Status = dto.Ms3status,
                MS4BaseLineDate = dto.Ms4BaseLineDate,
                MS4EventType = dto.Ms4EventType,
                MS4LatestPlanningDate = dto.Ms4LatestPlanningDate,
                MS4Status = dto.Ms4status,
                PlannedActivityId = dto.PlannedActivityId,
                PPMImportDate = dto.PpmImportDate,
                };
            if(entity != null )
            {
                _repositoryWrapper.DeliveryTrackingRepository.Update(DeliveryTrackingMapper.SetDeliveryTrackingMapper(entity));
                await _repositoryWrapper.SaveAsync();
                return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
            }
            else
                return new ResultDto { Info = ResultMessages.EntryNotUpdate };

        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.DeliveryTrackingRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.DeliveryTrackingRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {

            var entities = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Deliverytrackings.FirstOrDefault().Id == id)
                 .Include(x => x.Plannedactivityresource)
                  .Include(x => x.Designcomponent)
                .ToList();

            var unremovableItems = new List<string>();

            foreach (var item in entities)
            {
                unremovableItems.Add(string.Join("-", item.Plannedactivityid.ToString(), item.Plannedactivityresource.Plannedactivityresource,
                               item.Designcomponent?.toDesignComponentNameLcm(_repositoryWrapper)));

            }

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (unremovableItems.Count > 0)
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = unremovableItems.ToArray() });

            var entity = await _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Id == id)
                                .SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Delivery Tracking",
                        RecordName = entity.Id.ToString(),
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();

        }


        public DeliveryTrackingDtoCreate GetCreatePage()
        {
            var dto = new DeliveryTrackingDtoCreate();
            return dto;
        }

        public DeliveryTrackingDtoUpdate GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Id == id)
                                            .Include(x => x.ModificationuserNavigation)
                                            .Include(m => m.CreationuserNavigation)
                                            .Include(x => x.Plannedactivity).ThenInclude(x => x.Plannedactivityresource)
                                            .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
                                            .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                                            .Single();

            var entity = DeliveryTrackingMapper.GetDeliveryTrackingMapper(model);
            var dto = new DeliveryTrackingDtoUpdate()
            {
                Id = (short)entity.Id,
                Notes1 = entity.Notes1,
                Notes2 = entity.Notes2,
                Ms1BaseLineDate = entity.MS1BaseLineDate,
                Ms1EventType = entity.MS1EventType,
                Ms1LatestPlanningDate = entity.MS1LatestPlanningDate,
                Ms1status = entity.MS1Status,
                Ms2BaseLineDate = entity.MS2BaseLineDate,
                Ms2EventType = entity.MS2EventType,
                Ms2LatestPlanningDate = entity.MS2LatestPlanningDate,
                Ms2status = entity.MS2Status,
                Ms3BaseLineDate = entity.MS3BaseLineDate,
                Ms3EventType = entity.MS3EventType,
                Ms3LatestPlanningDate = entity.MS3LatestPlanningDate,
                Ms3status = entity.MS3Status,
                Ms4BaseLineDate = entity.MS4BaseLineDate,
                Ms4EventType = entity.MS4EventType,
                Ms4LatestPlanningDate = entity.MS4LatestPlanningDate,
                Ms4status = entity.MS4Status,
                PlannedActivityId = entity.PlannedActivityId,
                PpmImportDate = entity.PPMImportDate,
                Activity = string.Join("-", entity.PlannedActivity.PlannedActivityResource.PlannedActivityResourceDescription,
                                entity.PlannedActivity.DesignComponent.toDesignComponentNameLcm(_repositoryWrapper)),
                PpmID = entity.PlannedActivity.DeliveryProjectId,
                MS1EventTypeResource = new Dictionary<int, string>(),
                MS1StatusResource = new Dictionary<int, string>(),
                MS2EventTypeResource = new Dictionary<int, string>(),
                MS2StatusResource = new Dictionary<int, string>(),
                MS3EventTypeResource = new Dictionary<int, string>(),
                MS3StatusResource = new Dictionary<int, string>(),
                MS4EventTypeResource = new Dictionary<int, string>(),
                MS4StatusResource = new Dictionary<int, string>(),
                MsStatusResource = new Dictionary<int, string>()
                {
                    {
                        1 ,"On track"                       

                    },
                    {
                        2 ,"Delayed"

                    },
                    {
                        3 ,"Completed"

                    }
                },               
            };
            return dto;
        }

        #region Delivery Tracking

        public IQueryable<Deliverytrackings> GetDeliveryTrackingBulkQuery()
        {
            var deliveryTrackingEntities = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => 
            (x.Plannedactivity.Deliveryprojectid == null ) ||
            (x.Plannedactivity.Deliveryprojectid != null  && (x.Ms1baselinedate == null ) )
           )
              .Include(x => x.Plannedactivity).ThenInclude(x => x.Lcmengineering)
          .AsQueryable();

            return deliveryTrackingEntities;
        }
        public async Task<ResultDto> DeliveryTrackingDataRefresh()
        {
            var deliverytrackingsCreationEnity = new List<Deliverytrackings>();
            var deliverytrackingsUpdateEnity = new List<Deliverytrackings>();

            try
            {
                var deliveryTrackingRelatedData =  GetDeliveryTrackingBulkQuery()?.ToList();

                if (deliveryTrackingRelatedData != null)
                {
                    var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                    var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;

                    
                    foreach (var result in deliveryTrackingRelatedData)// .Where(x => x.Plannedactivityid == 3854 || x.Id == 744))
                    {
                        var deliveryTrackingEntity = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Id == result.Id).FirstOrDefault();
                        var mileStoneStatus = await _commonManager.CalculateMSDuration(result.Plannedactivity, _repositoryWrapper);
                        if (mileStoneStatus.isSuccess)
                        {
                            int assetCount = 0;
                            if (result.Plannedactivity.Lcmengineering != null && result.Plannedactivity?.Lcmengineering?.Elementcount == true)
                            {
                                
                               var assets = _repositoryWrapper.NetworkElementAsPlanned
                                     .FindByCondition(x => x.Opcoid == result.Plannedactivity.Lcmengineering.Opcoid &&
                                     x.Designcomponentid == result.Plannedactivity.Lcmengineering.Designcomponentid
                                     && x.Deploymentstatusid == getAssetInserviceID && x.Environmentid == getProductionEnvrionmentId)
                                     .ToList();
                                assetCount = Convert.ToInt16( assets?.Count());
                            }

                            var deliveryTracking = _commonManager.MappingDeliveryTracingForUpdate(mileStoneStatus, deliveryTrackingEntity,result?.Plannedactivity.Plannedcompletion, assetCount);
                            deliverytrackingsUpdateEnity.Add(deliveryTracking);
                        }


                    }
                }

                if (deliverytrackingsUpdateEnity != null && deliverytrackingsUpdateEnity.Count > 0)
                {
                    _repositoryWrapper.DeliveryTrackingRepository.BulkUpdate(deliverytrackingsUpdateEnity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }


                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = string.Join(",", deliverytrackingsUpdateEnity.Select(x => x.Id).ToList())
                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryNotUpdate,
                    Data = ex.StackTrace
                };

            }
        }


        #endregion   
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, DeliveryTrackingQueryDto request, bool isAdmin = false)
        {
            request.PageSize = 0;
            request.Page = 1;
            var predicateResult = ApplyFilterForOracleModel(request);

            var query = PrepareQuery(request, null, predicateResult);

            var data = (await GetFilterValueList(query, propertyName, propertyFilter)).Distinct().ToList();

            if (!isAdmin && (request.VerticalName != null && request.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                data = data.Where(x => request.VerticalName.Contains(x.Value.ToString())).ToList();
            }
            return data;
        }
    }
}
