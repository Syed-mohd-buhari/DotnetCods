using CAM.BusinessManager;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Infrastucture.QueryResult;
using CAM.Exports;
using System.Linq;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.LookUp.PlannedActivityResourceDto;
using CAM.Entities.Models.Settings;
using CAM.Enum;

namespace CAM.WebAPI.Controllers
{

    public partial class PlannedActivityController : CamControllerBase
    {

        [HttpPost(template: "GetLinkedDesignComponent")]
        public async Task<ResultDto> GetLinkedDesignComponent(long id, int rule)
        {
            return await _plannedActivityManager.GetLinkedDesignComponent(id, rule);
        }
        [HttpPost(template: "GetActivityDetails")]
        public async Task<ResultDto> GetActivityDetails(long id, int rule)
        {

            return new ResultDto()
            {
                Data = await _plannedActivityManager.GetActivityDetailsFromLcmRule(id, rule)
            };

        }

        [HttpGet(template: "GetPlannedActivityListForMigration")]
        public async Task<ResultDto> GetPlannedActivityListForMigration(long dcId,short opcoId)
        {

            return await _plannedActivityManager.GetPlannedActivityListForMigration(dcId, opcoId);

        }
        
        [HttpGet(template: "GetPlannedActivityListForUpdatePlannedActivityStatus")]
        public async Task<ResultDto> GetPlannedActivityListForUpdatePlannedActivityStatus(long dcId, short opcoId, short plannedActivityTypeId, short? plannedActivityTypeFor = null)
        {

            return await _updatePlannedActivityManager.GetPlannedActivityListForUpdatePlannedActivityStatus(dcId, opcoId, plannedActivityTypeId, plannedActivityTypeFor);

        }

        [HttpGet(template: "GetPlannedActivityForLink{id}")]
        public async Task<ResultDto<PlannedActivityForLinkDto>> GetPlannedActivityForLink(long id)
        {

           return await _plannedActivityManager.GetPlannedActivityForLink(id);

        }

        [HttpGet(template: "GetLcmEngineeringPlannedActivity")]
        public async Task<ResultDto<Dictionary<long, PlannedActivityToConnectData>>> GetLcmEngineeringPlannedActivity(long designComponentId, long opCoId) {
            return await _plannedActivityManager.GetLcmEngineeringPlannedActivity(designComponentId, opCoId);
        }

        [HttpGet(template: "GetOpCoList")]
        public async Task<ResultDto> GetOpCoList()
        {

            return await _plannedActivityManager.GetOpCoList(_opcoList);

        }

        [HttpGet(template: "GetDesignComponentList{opCoId}")]
        public async Task<ResultDto> GetDesignComponentList(short opCoId)
        {

            return await _plannedActivityManager.GetDesignComponentList(opCoId,_verticalList);

        }

        [HttpPost(template: "ActivityStatusLogics")]
        public async Task<ResultDto<List<short>>> ActivityStatusLogics(int? deliveryId, int? budgetAvId, int? responsibilityPhase, bool? localApproval)
        {
            return new ResultDto<List<short>>()
            {
                Data = await _plannedActivityManager.ActivityStatusLogics(responsibilityPhase, deliveryId, budgetAvId, localApproval)
            };
        }

        //[HttpPut(template: "PlannedActivityMigrations")]
        //public async Task<ResultDto> PlannedActivityMigrations([FromBody] PlannedActivityMigrationsDto data)
        //{
        //        return await _plannedActivityManager.PlannedActivityMigrations(data);
        //}

        [HttpGet(template: "GetCreateUpdatePlannedActivityStatus")]
        public async Task<UpdatePlannedActivityStatusDto> GetCreateUpdatePlannedActivityStatus()
        {

            return await _updatePlannedActivityManager.GetCreateUpdatePlannedActivityStatus(_opcoList);

        }


        [HttpGet(template: "GetSettingUpdatePlannedActivityResource")]
        public async Task<ResultDto> GetSettingUpdatePlannedActivityResource(short plannedActivityResourceId, short? plannedActivityTypeFor = null)
        {
            return await _updatePlannedActivityManager.GetSettingUpdatePlannedActivityResource(plannedActivityResourceId, plannedActivityTypeFor);
        }

        [HttpGet(template: "GetUpdatePlannedActivityStatus")]
        public async Task<ResultDto<UpdatePlannedActivityStatusDto>> GetUpdatePlannedActivityStatus(long id, PlannedActivityTypeForEnum plannedActivityTypeFor)
        {

            return await _updatePlannedActivityManager.GetUpdatePlannedActivityStatus(id, plannedActivityTypeFor);
        }

        [HttpPut(template: "SaveUpdatePlannedActivityStatus")]
        public async Task<ResultDto> SaveUpdatePlannedActivityStatus([FromBody] UpdatePlannedActivityStatusDto data)
        {

            return await _updatePlannedActivityManager.SaveUpdatePlannedActivityStatus(data);

        }

        [HttpGet(template: "GetPlannedActivityTypeswithDcIdAndOpcoId")]
        public async Task<ResultDto> GetPlannedActivityTypeswithDcIdAndOpcoId(long dcId, short opcoId)
        {

            return await _updatePlannedActivityManager.GetPlannedActivityTypeswithDcIdAndOpcoId(dcId, opcoId);

        }

        [HttpGet(template: "CheckPlannedActivityTypefor")]
        public async Task<ResultDto> CheckPlannedActivityTypefor(long plannedActivityTypeId, long designComponentId, long opCoId)
        {

            return await _updatePlannedActivityManager.CheckPlannedActivityTypefor(plannedActivityTypeId, designComponentId, opCoId);

        }


    }
}

