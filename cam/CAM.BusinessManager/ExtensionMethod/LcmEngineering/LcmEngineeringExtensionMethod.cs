using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.Common;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Enum;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.ExtensionMethod.LcmEngineering
{
    public static class LcmEngineeringExtensionMethod
    {
        private static readonly DateTime fronzenDate = new DateTime(DateTime.Now.Year, 6, 1);
        private static readonly DateTime targetDate = new DateTime(DateTime.Now.Year + 1, 6, 1);
        public static string toHardwareSheetIndex(this Designcomponents dc, long id, string opco)
        {
            var name = $"{id.ToString("D4")}HW_{opco}{/*dc.Systemtype.Verticalresponsible?.Verticalresponsible*/""}" +
                             $"{/*dc.Systemtype.Subdomainresponsible?.Subdomainresponsible*/ ""}_" +
                             $"{dc.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer}_" +
                             $"{dc.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Hardwaretype}";
            return name;
        }
        public static string toSoftwareSheetIndex(this Designcomponents dc, long id, string opco)
        {
            var name = $"{id.ToString("D4")}SW_{opco}{/*dc.Systemtype.Verticalresponsible?.Verticalresponsible*/ ""}" +
                             $"{/*dc.Systemtype.Subdomainresponsible?.Subdomainresponsible*/ ""}_" +
                             $"{dc.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer}_" +
                             $"{dc.Systemtype.Majorsoftwarebuilds.Softwareversion}";
            return name;
        }
        public static int CountNetworkElementReleated(this Entities.Models.LcmEngineering entity, bool isLab,
             IRepositoryWrapper _repositoryWrapper)
        {
            if (isLab)
            {
                var nodesNum = entity.ElementCount ? _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                   .Count(s =>
                    s.Lcmengineeringid == entity.LcmengineeringId && s.Environment.Environment.ToLower() != "production"
                     && !ConstantValueFilter.excludeDeployementStatus.Contains(s.Deploymentstatus.Deploymentstatus.Replace(" ", "").ToLower())) : entity.NumberOfNodesInLab;
                return nodesNum;
            }
            else
            {
                var nodeNumProd = entity.ElementCount ? _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                            .Count(s =>
                    s.Lcmengineeringid == entity.LcmengineeringId && s.Environment.Environment.ToLower() == "production"
                     && !ConstantValueFilter.excludeDeployementStatus.Contains(s.Deploymentstatus.Deploymentstatus.Replace(" ", "").ToLower())) : entity.NumberOfNodes;
                return nodeNumProd;
            }

        }

        public static int CountNetworkElementReleated(this Lcmengineering entity, bool isLab,
           IRepositoryWrapper _repositoryWrapper)
        {
            try
            {
                if (isLab)
                {
                    return entity.Elementcount ? _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                        .Count(s =>
                        s.Lcmengineeringid == entity.Lcmengineeringid && s.Environment.Environment.ToLower() != "production"
                         && !ConstantValueFilter.excludeDeployementStatus.Contains(s.Deploymentstatus.Deploymentstatus.Replace(" ", "").ToLower())) : entity.Numberofnodesinlab;
                }
                else
                {

                    return entity.Elementcount ? _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                        .Count(s =>
                        s.Lcmengineeringid == entity.Lcmengineeringid && s.Environment.Environment.ToLower() == "production"
                         && !ConstantValueFilter.excludeDeployementStatus.Contains(s.Deploymentstatus.Deploymentstatus.Replace(" ", "").ToLower())) : entity.Numberofnodes;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        // April 18 #435 Ticket
        public static async Task<int> CountAggregatedBasedNetworkElementReleated(this Lcmengineering entity, bool isLab,
                  IRepositoryWrapper _repositoryWrapper)
        {

            if (isLab)
            {
                return await Task.Run(() => entity.Elementcount ? _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                    .Count(s =>
                    s.Lcmengineeringid == entity.Lcmengineeringid && s.Environment.Environment.ToLower() != "production"
                     && !ConstantValueFilter.excludeDeployementStatus.Contains(s.Deploymentstatus.Deploymentstatus.Replace(" ", "").ToLower())) : entity.Numberofnodesinlab);
            }
            else
            {
                string[] includeDeployementStatus = { "in-service", "in commissioning", "decommissioning" };

                var lcmDeploymentPlannedStatusId = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Description.ToLower() == "planned").FirstOrDefault().Id;

                if (lcmDeploymentPlannedStatusId == entity.Lcmdeploymentstatusid)
                {
                    return 0;
                }
                else
                {

                    var deployementStatusIdList = _repositoryWrapper.DeploymentStatus
                        .FindByCondition(x => includeDeployementStatus.Contains(
                            x.Deploymentstatus.ToLower())).Select(x => x.Deploymentstatusid).ToList();


                    return await Task.Run(() => entity.Elementcount ? _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                        .Count(s =>
                        s.Lcmengineeringid == entity.Lcmengineeringid &&
                        s.Environment.Environment.ToLower() == "production"
                        && !ConstantValueFilter.excludeDeployementStatus.Contains(s.Deploymentstatus.Deploymentstatus.Replace(" ", "").ToLower())
                        && (deployementStatusIdList.Contains(s.Deploymentstatusid))
                         ) : entity.Numberofnodes);
                }


            }

        }
        public static int CountNetworkElementReleated(this LcmEngineeringDtoUpdate entity, bool isLab,
      IRepositoryWrapper _repositoryWrapper)
        {
            if (isLab)
            {
                return entity.ElementCount ? _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                    .Count(s =>
                    s.Lcmengineeringid == entity.LcmEngineeringId && s.Environment.Environment.ToLower() != "production"
                     && !ConstantValueFilter.excludeDeployementStatus.Contains(s.Deploymentstatus.Deploymentstatus.Replace(" ", "").ToLower())) : entity.NumberOfNodesInLab.Value;
            }
            else
            {
                return entity.ElementCount ? _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                    .Count(s =>
                    s.Lcmengineeringid == entity.LcmEngineeringId && s.Environment.Environment.ToLower() == "production"
                     && !ConstantValueFilter.excludeDeployementStatus.Contains(s.Deploymentstatus.Deploymentstatus.Replace(" ", "").ToLower())) : entity.NumberOfNodes.Value;
            }
        }
        public static int CountNodes(this Lcmengineering entity, bool isLab,
   IRepositoryWrapper _repositoryWrapper)
        {
            if (isLab)
            {
                return entity.Elementcount ? _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                    .Count(s =>
                    s.Lcmengineeringid == entity.Lcmengineeringid && s.Opcoid == entity.Opcoid && s.Environment.Environment.ToLower() != "production"
                     && !ConstantValueFilter.excludeDeployementStatus.Contains(s.Deploymentstatus.Deploymentstatus.Replace(" ", "").ToLower())) : entity.Numberofnodesinlab;
            }
            else
            {
                return entity.Elementcount ? _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                    .Count(s =>
                    s.Lcmengineeringid == entity.Lcmengineeringid && s.Environment.Environment.ToLower() == "production"
                     && !ConstantValueFilter.excludeDeployementStatus.Contains(s.Deploymentstatus.Deploymentstatus.Replace(" ", "").ToLower())) : entity.Numberofnodes;
            }
        }
        public static string toDescription(this Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            string retVal = "";
            if (entity != null)
            {
                if (entity.Opco != null)
                {
                    retVal += entity.Opco.Opco;
                }
                if (entity.Designcomponent != null)
                {
                    if (retVal != "") retVal += " - ";
                    retVal += entity.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper);

                }
                if (entity.Onhardware)
                {
                    if (retVal != "") retVal += " - ";
                    retVal += "ON HARDWARE";
                }
                if (entity.Onsoftware)
                {
                    if (retVal != "") retVal += " - ";
                    retVal += "ON SOFTWARE";
                }

            }
            return retVal;

        }

        public static string toOperationaContractDescription(this Lcmengineering entity, bool forLcm, bool forDesignAspect)
        {
            if (entity.Lcmoperationalcontracts == null)
            {
                return null;
            }
            var entities = entity.Lcmoperationalcontracts.ToList();
            if (entities.Count <= 0)
            {
                return null;
            }
            return string.Join(" | ", entities.Select(x => x.Operationalcontract?.Description).Where(x => x != null).ToArray());
        }

        #region Ticket 465 - LCM export: RAG status field - filter is not working fine
        public static string ConvertToDeliveryStatusEnum(int? status)
        {
            var result = "";
            switch (status)
            {
                case 1:
                    result = "On Track";
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

        public static IEnumerable<FilterValueDto> aggregatedRagStatusFilterRecord(IEnumerable<Lcmengineering> lcm, List<Lcmengineering> singleLcm, int reportType, string? filterSearchValue = "")
        {

            FilterValueDto ragDropDownFilterValueDto = new FilterValueDto();  // -- Return RagStatus DropDown Record
            FilterValueDto ragStatusBasedLcmIdFilterValueDto = new FilterValueDto();  //- Return list fo LcmId to filter based on RagStatus

            // singleLcm = lcm.Where(X => X.Lcmengineeringid == 1755).ToList();
            if (singleLcm != null && singleLcm.Count() > 0)
                lcm = singleLcm;  // Get RagStatus value for Grid row by row record

            string reportTypeValue = (reportType == 1) ? "HARDWARE" : "SOFTWARE";

            return lcm.Select(x =>
            {
                var paRecord = x.PlannedactivitiesLcmengineering.GetPlannedActivityFilteredDB(reportTypeValue) != null ?
                x.PlannedactivitiesLcmengineering.GetPlannedActivityFilteredDB(reportTypeValue)?.Deliverytrackings : null;

                ragDropDownFilterValueDto = new FilterValueDto();
                ragStatusBasedLcmIdFilterValueDto = new FilterValueDto();

                string unAssignedRagStatusValue = "---";

                var hardWare = (reportType == 1) ?
                (x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.SingleOrDefault
                (m => m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid && m.Deleted == false)?.Majorhardware)
                : null;

                var software = (reportType == 2) ?
                x.Designcomponent?.Systemtype?.Majorsoftwarebuilds
                : null;


                DateTime? VendorEndOfMaintenanceDate = (reportType == 1) ? hardWare?.Endofmaintenance : software?.Endofmaintenance;

                EOMEnum EOMStatus = (reportType == 1) ? (hardWare != null ? (EOMEnum)hardWare.Eomstatus : EOMEnum.NotSpecified)
                : (software != null ? (EOMEnum)software.Eomstatus : EOMEnum.NotSpecified);

                #region Filter Text
                ragDropDownFilterValueDto.Text = (String.IsNullOrEmpty(filterSearchValue)) ?
          (

            (x.Archived == true) ?
                LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(3) :
                 ((reportType == 1) ? (hardWare != null) : (software != null)) ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate)
            (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?
            unAssignedRagStatusValue :

              //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null)

              (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null) ?
              (string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) ? unAssignedRagStatusValue
              : LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
            (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate) ?
             (string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) ? unAssignedRagStatusValue
             : LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

            : unAssignedRagStatusValue
            : unAssignedRagStatusValue)
            : unAssignedRagStatusValue;

                #endregion Filter Text

                #region filterValue - Value

                ragDropDownFilterValueDto.Value = ragDropDownFilterValueDto.Text;
                //     ragDropDownFilterValueDto.Value = (String.IsNullOrEmpty(filterSearchValue)) ? (

                // (x.Archived == true) ?
                //     LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(3) :
                //       ((reportType == 1) ? (hardWare != null) : (software != null)) ?

                // //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
                // (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?
                // unAssignedRagStatusValue :

                // //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate|| x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?

                // (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null) ?
                // (string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) ? unAssignedRagStatusValue :
                //   LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

                //:
                // //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
                // (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate) ?
                //   (string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) ? unAssignedRagStatusValue :
                //     LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

                // : unAssignedRagStatusValue
                // : unAssignedRagStatusValue)
                // : unAssignedRagStatusValue;

                #endregion filterValue - Value



                #region fetch LcmEngineeringId

                ragStatusBasedLcmIdFilterValueDto.Value = (!String.IsNullOrEmpty(filterSearchValue)) ?
      (
                #region Completed Status
           (filterSearchValue.ToString() == "Completed") ?
            (x.Archived == true) ?
                 x.Lcmengineeringid.ToString() :
                  ((reportType == 1) ? (hardWare != null) : (software != null)) ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
            (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?
            "0" :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?
            (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null) ?
             ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "Completed")) ? x.Lcmengineeringid.ToString() : "0" :

             //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
             (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate) ?
             ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "Completed")) ? x.Lcmengineeringid.ToString() : "0"
           : "0"
            : "0"
                #endregion Completed Status
        :
                #region On Track 
          (filterSearchValue.ToString() == "On Track") ?
            (x.Archived == true) ?
                 "0" :
                  (x.Designcomponent?.Systemtype?.Majorsoftwarebuilds != null) ?

           // (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
           (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?

            "0" :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null && (paRecord.FirstOrDefault()?.Ms2status != null)) ?

             ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == filterSearchValue)
              ) ?
             x.Lcmengineeringid.ToString() : "0"
           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
              ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
               (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == filterSearchValue)
              ) ?
               x.Lcmengineeringid.ToString() : "0"
           : "0"
            : "0"
                #endregion On Track && On Track Status
        :
                #region Delayed  Status
          (filterSearchValue.ToString() == "Delayed") ?
            (x.Archived == true) ?
                 "0" :
                  (x.Designcomponent?.Systemtype?.Majorsoftwarebuilds != null) ?

           // (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
           (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?

            "0" :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null && (paRecord.FirstOrDefault()?.Ms2status != null)) ?

             ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
             (
             LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == filterSearchValue)) ?
             x.Lcmengineeringid.ToString() : "0"
           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
              ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
              (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == filterSearchValue)) ?
               x.Lcmengineeringid.ToString() : "0"
           : "0"
            : "0"
                #endregion On Track && On Track Status
        :
                #region --- Records
         (filterSearchValue.ToString() == unAssignedRagStatusValue) ?
           (x.Archived == true) ?
                "0" :
                  ((reportType == 1) ? (hardWare != null) : (software != null)) ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
            (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?
             x.Lcmengineeringid.ToString() :

             //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?

             (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null) ?
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "") ?
             x.Lcmengineeringid.ToString() : "0"
           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
            (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate) ?
            (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "") ?
             x.Lcmengineeringid.ToString() : "0"
           : x.Lcmengineeringid.ToString()
            : x.Lcmengineeringid.ToString()

           : "0"
                #endregion --- Records
        ) : "";

                ragStatusBasedLcmIdFilterValueDto.Text = ragStatusBasedLcmIdFilterValueDto.Value;
                #endregion

                return (!String.IsNullOrEmpty(filterSearchValue)) ? ragStatusBasedLcmIdFilterValueDto : ragDropDownFilterValueDto;

            }
            );

            //return null;

        }

        public static IEnumerable<FilterValueDto> disAggregatedRagStatusFilterRecord(IEnumerable<Networkelementsasplanned> lcm, List<Networkelementsasplanned> singleLcm, int reportType, string? filterSearchValue = "")
        {

            FilterValueDto ragDropDownFilterValueDto = new FilterValueDto();  // -- Return RagStatus DropDown Record
            FilterValueDto ragStatusBasedLcmIdFilterValueDto = new FilterValueDto();  //- Return list fo LcmId to filter based on RagStatus

            string reportTypeValue = (reportType == 1) ? "HARDWARE" : "SOFTWARE";

            if (singleLcm != null && singleLcm.Count() > 0)
                lcm = singleLcm;  // Get RagStatus value for Grid row by row record


            return lcm.Select(x =>
            {
                var paRecord = x.Lcmengineering.PlannedactivitiesLcmengineering.GetPlannedActivityFilteredDB(reportTypeValue) != null ?
                x.Lcmengineering.PlannedactivitiesLcmengineering.GetPlannedActivityFilteredDB(reportTypeValue)?.Deliverytrackings : null;

                ragDropDownFilterValueDto = new FilterValueDto();
                ragStatusBasedLcmIdFilterValueDto = new FilterValueDto();

                string unAssignedRagStatusValue = "---";

                var hardWare = (reportType == 1) ? x.Designcomponent.Systemtype?.Systemtypesmajorhardwarebuilds?.SingleOrDefault
                (m => m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid && m.Deleted == false)?.Majorhardware
                 : null;

                var software = (reportType == 2) ?
                x.Designcomponent?.Systemtype?.Majorsoftwarebuilds
                : null;

                DateTime? VendorEndOfMaintenanceDate = (reportType == 1) ? hardWare?.Endofmaintenance : software?.Endofmaintenance;

                EOMEnum EOMStatus = (reportType == 1) ? (hardWare != null ? (EOMEnum)hardWare.Eomstatus
                      : EOMEnum.NotSpecified) : (software != null ? (EOMEnum)software.Eomstatus
                      : EOMEnum.NotSpecified);

                #region Filter Text
                ragDropDownFilterValueDto.Text = (String.IsNullOrEmpty(filterSearchValue)) ?
          (

            (x.Lcmengineering.Archived == true) ?
                LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(3) :
                  ((reportType == 1) ? (hardWare != null) : (software != null)) ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate)
            (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?
            unAssignedRagStatusValue :

              //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null)

              (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null) ?
              (string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) ? unAssignedRagStatusValue
              : LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
            (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate) ?
             (string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) ? unAssignedRagStatusValue
             : LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

            : unAssignedRagStatusValue
            : unAssignedRagStatusValue)
            : unAssignedRagStatusValue;

                #endregion Filter Text

                #region filterValue - Value
                ragDropDownFilterValueDto.Value = ragDropDownFilterValueDto.Text;
                //     ragDropDownFilterValueDto.Value = (String.IsNullOrEmpty(filterSearchValue)) ? (

                // (x.Lcmengineering.Archived == true) ?
                //     LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(3) :
                //       ((reportType == 1) ? (hardWare != null) : (software != null)) ?

                // //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
                // (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?
                // unAssignedRagStatusValue :

                // //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate|| x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?

                // (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null) ?
                // (string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) ? unAssignedRagStatusValue :
                //    LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

                //:
                // //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
                // (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate) ?
                //   (string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) ? unAssignedRagStatusValue :
                //     LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

                // : unAssignedRagStatusValue
                // : unAssignedRagStatusValue)
                // : unAssignedRagStatusValue;

                #endregion filterValue - Value



                #region fetch LcmEngineeringId

                ragStatusBasedLcmIdFilterValueDto.Value = (!String.IsNullOrEmpty(filterSearchValue)) ?
      (
                #region Completed Status
            //(filterSearchValue.ToString() == "Completed") ?
            (filterSearchValue.ToString() == "Completed") ?
            (x.Lcmengineering.Archived == true) ?
                 x.Lcmengineeringid.ToString() :
                  ((reportType == 1) ? (hardWare != null) : (software != null)) ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
            (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?
            "0" :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?
            (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null) ?
             ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "Completed")) ? x.Lcmengineeringid.ToString() : "0" :

             //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
             (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate) ?
             ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "Completed")) ? x.Lcmengineeringid.ToString() : "0"
           : "0"
            : "0"
                #endregion Completed Status
        :
                #region On Track  Status
           //(filterSearchValue.ToString() == "On Track" ) ?
           (filterSearchValue.ToString() == "On Track") ?
            (x.Lcmengineering.Archived == true) ?
                 "0" :
                  (x.Designcomponent?.Systemtype?.Majorsoftwarebuilds != null) ?

           // (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
           (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?

            "0" :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null && (paRecord.FirstOrDefault()?.Ms2status != null)) ?

             ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == filterSearchValue)) ?
             x.Lcmengineeringid.ToString() : "0"
           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
              ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
              (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == filterSearchValue)) ?
               x.Lcmengineeringid.ToString() : "0"
           : "0"
            : "0"
                #endregion "On Track Status
        :
                #region Delayed Status
           //(filterSearchValue.ToString() == "Delayed") ?
           (filterSearchValue.ToString() == "Delayed") ?
            (x.Lcmengineering.Archived == true) ?
                 "0" :
                  (x.Designcomponent?.Systemtype?.Majorsoftwarebuilds != null) ?

           // (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
           (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?

            "0" :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null && (paRecord.FirstOrDefault()?.Ms2status != null)) ?

             ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == filterSearchValue)) ?
             x.Lcmengineeringid.ToString() : "0"
           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
              ((!string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status))) &&
              (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == filterSearchValue)) ?
               x.Lcmengineeringid.ToString() : "0"
           : "0"
            : "0"
                #endregion Delayed Status
        :
                #region --- Records
         (filterSearchValue.ToString() == unAssignedRagStatusValue) ?
           (x.Lcmengineering.Archived == true) ?
                "0" :
                 ((reportType == 1) ? (hardWare != null) : (software != null)) ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
            (EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate) ?
             x.Lcmengineeringid.ToString() :

             //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?

             (VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null) ?
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "") ?
             x.Lcmengineeringid.ToString() : "0"
           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
            (fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate) ?
            (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "") ?
             x.Lcmengineeringid.ToString() : "0"
           : x.Lcmengineeringid.ToString()
            : x.Lcmengineeringid.ToString()

           : "0"
                #endregion --- Records
        ) : "";

                ragStatusBasedLcmIdFilterValueDto.Text = ragStatusBasedLcmIdFilterValueDto.Value;
                #endregion

                return (!String.IsNullOrEmpty(filterSearchValue)) ? ragStatusBasedLcmIdFilterValueDto : ragDropDownFilterValueDto;

            }
            );
 

        }
        #endregion

        #region Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
        public static void DeleteDCForLcmMovedToArchive(long dcid, IRepositoryWrapper _repositoryWrapper)
        {
            var deleteDcRecord = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == dcid).FirstOrDefault();
            if (deleteDcRecord != null)
            {
                _repositoryWrapper.DesignComponent.Delete(deleteDcRecord);
                _repositoryWrapper.SaveAsync();
            }

        }

        #endregion

        #region Ticket 837 - UpdatePlannedActivityManager - code improvements
        public static Plannedactivities PlannedActivityExists(long plannedActivityId, long? plannedDC, short? OpcoId, long dc, short? plannedActivityResourceID, IRepositoryWrapper _repositoryWrapper)
        {
            //Ticket 658 Dev - #622 - Release details unknown - Duplicate PA's
            return plannedActivityId == 0
                ? _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityresourceid == plannedActivityResourceID
                && x.Opcoid == OpcoId && x.Lcmengineering.Designcomponentid == dc && x.Designcomponentid == plannedDC && x.Archived != true)
                .Include(x => x.Lcmengineering).FirstOrDefault()

                : _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid != plannedActivityId
                && x.Plannedactivityresourceid == plannedActivityResourceID
                && x.Opcoid == OpcoId && x.Lcmengineering.Designcomponentid == dc && x.Designcomponentid == plannedDC && x.Archived != true).Include(x => x.Lcmengineering).FirstOrDefault();

        }
        #endregion        

        public static string GetSCF(this Lcmancillarydata entity)
        {
            if (entity == null)
                return ConstantValueFilter.No;
            if((entity.Ispecn != null && entity.Ispecn != false) || (entity.Isscf != null && entity.Isscf != false) || (entity.Isnof != null && entity.Isnof != false) || (entity.Ispecs != null && entity.Ispecs != false))
            {
                return ConstantValueFilter.Yes;
            }
            return ConstantValueFilter.No;
        }

    }

}
