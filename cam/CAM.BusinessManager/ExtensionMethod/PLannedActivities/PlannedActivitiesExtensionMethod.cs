using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.GenericReportDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using CAM.Repository;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using IdentityModel;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account.Manage;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static CAM.BusinessManager.Rules.LCMEngineeringRulesExtension;

namespace CAM.BusinessManager.ExtensionMethod.PLannedActivities
{
    static class PlannedActivitiesExtensionMethod
    {


        public static PlannedActivity SelectPlanndeActivities(this IQueryable<Entities.Models.LcmEngineering> source, string value)
        {
            var t = source
                 .Select(c => c.PlannedActivities.Where(FuncPura(value))
                 .OrderBy(x => x.PlannedCompletion).FirstOrDefault());
            return t.FirstOrDefault();
        }
        public static PlannedActivity SelectPlanndeActivities(this Entities.Models.LcmEngineering source, string value)
        {
            var t = source.PlannedActivities
                .Where(FuncPura(value))
                    .OrderBy(x => x.PlannedCompletion).FirstOrDefault();
            return t;
        }

        public static Plannedactivities GetPlannedActivityFilteredDB(this IEnumerable<Plannedactivities> plannedActivities, string value, bool? checkRagStatus = false)
        {
            return plannedActivities.Where(FuncPuraDB(value, checkRagStatus))
                .DefaultIfEmpty(new Plannedactivities()
                {
                    Responsibilityphase = new Responsibilityphases(),
                    Deliverystatus = new Deliverystatuses(),
                    Plannedactivityresource = new Plannedactivityresources(),
                })
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault();
        }


        public static PlannedActivity GetPlannedActivityFiltered(this IEnumerable<PlannedActivity> plannedActivities, string value)
        {
            return plannedActivities
                .Where(FuncPura(value))
                .DefaultIfEmpty(new PlannedActivity()
                {
                    ResponsibilityPhase = new ResponsibilityPhase(),
                    DeliveryStatus = new DeliveryStatus(),
                    PlannedActivityResource = new Entities.Models.Lookup.PlannedActivityResource(),
                })
                .OrderBy(x => x.PlannedCompletion).FirstOrDefault();
        }
        public static IEnumerable<PlannedActivity> GetPlannedActivityFilteredEnumerable(this IEnumerable<PlannedActivity> plannedActivities, string value)
        {
            return plannedActivities
                .Where(FuncPura(value))
                .DefaultIfEmpty(new PlannedActivity()
                {
                    ResponsibilityPhase = new ResponsibilityPhase(),
                    DeliveryStatus = new DeliveryStatus(),
                    PlannedActivityResource = new Entities.Models.Lookup.PlannedActivityResource(),
                })
                .OrderBy(x => x.PlannedCompletion);
        }

        public static string GetPlannedActivityBudgetEstimated(this IEnumerable<PlannedActivity> plannedActivities, string value)
        {
            var data = plannedActivities
                .Where(FuncPura(value))
                ?.OrderBy(x => x.PlannedCompletion)?.FirstOrDefault();
            if (data != null)
            {
                return $"{data.BudgetValue.ToString()} {data.Currency}";
            }
            return string.Empty;
        }
        public static string GetPlannedActivityBudgetEstimated(this IEnumerable<Plannedactivities> plannedActivities, string value)
        {
            var data = plannedActivities
                .Where(FuncPuraDB(value))
                ?.OrderBy(x => x.Plannedcompletion)?.FirstOrDefault();
            if (data != null)
            {
                return $"{data.Budgetvalue.ToString()} {data.Currency}";
            }
            return string.Empty;
        }
        public static string GetPlannedActivityBudgetEstimatedForTsr(this IEnumerable<Plannedactivities> plannedActivities)
        {
            var data = plannedActivities?.OrderBy(x => x.Plannedcompletion)?.FirstOrDefault();
            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.Budgetvalue.ToString()))
                {
                    return $"{data.Currency}{data.Budgetvalue.ToString()}";
                }
                else
                {
                    return "£0.0";
                }
            }
            return string.Empty;
        }

        public static string GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(this IEnumerable<Plannedactivities> plannedActivities, string value)
        {
            var data = plannedActivities
                .Where(FuncPuraDB(value))
                ?.OrderBy(x => x.Plannedcompletion)?.FirstOrDefault();
            if (data != null)
            {
                return $"{data.Deliveryprojectname}";
            }
            return string.Empty;
        }


        public static Func<PlannedActivity, bool> FuncPura(string filtroHwSw)
        {
            if (filtroHwSw.ToLower() == "software")
                return a =>
                    //a.PlannedCompletion >= DateTime.Now.Date &&
                    a.PlannedActivityResourceId != null && a.PlannedActivityResource.Exportable && a.Deleted == false && a.PlannedActivityResource.LcmSoftware && a.LcmEngineering.NumberOfNodes > 0;
            else
                return a =>
                    //a.PlannedCompletion >= DateTime.Now.Date &&
                    a.PlannedActivityResourceId != null && a.PlannedActivityResource.Exportable && a.Deleted == false && a.PlannedActivityResource.LcmHardware && a.LcmEngineering.NumberOfNodes > 0;
        }
        public static Func<Plannedactivities, bool> FuncPuraDB(string filtroHwSw, bool? checkRagStatusForLcmExport = false)
        {
            if (filtroHwSw.ToLower() == "software")
                return a =>
                      //a.PlannedCompletion >= DateTime.Now.Date &&
                      a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable
                      && ((bool)checkRagStatusForLcmExport ?
                      a.Deleted == false : a.Deleted == false && a.Archived == false)
                      && a.Plannedactivityresource.Lcmsoftware;
            else
                return a =>
                    //a.PlannedCompletion >= DateTime.Now.Date &&
                    a.Plannedactivityresourceid != null
                    && a.Plannedactivityresource.Exportable && ((bool)checkRagStatusForLcmExport ?
                      a.Deleted == false : a.Deleted == false && a.Archived == false)
                    && a.Plannedactivityresource.Lcmhardware;
        }

        public static Expression<Func<Plannedactivities, bool>> FiltroPlannedPerWhere(string filtroHwSw)
        {
            if (filtroHwSw.ToLower() == "software")
                return a =>
                a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Archived == false && a.Plannedactivityresource.Lcmsoftware;
            else
                return a =>
                    a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Archived == false && a.Plannedactivityresource.Lcmhardware;
        }



        public static Expression<Func<PlannedActivity, bool>> FiltroPlannedPerWhereModel(string filtroHwSw)
        {
            if (filtroHwSw.ToLower() == "software")
                return a =>
                a.PlannedActivityResourceId != null && a.PlannedActivityResource.Exportable && a.Deleted == false && a.PlannedActivityResource.LcmSoftware && a.LcmEngineering.NumberOfNodes > 0;
            else
                return a =>
                    a.PlannedActivityResourceId != null && a.PlannedActivityResource.Exportable && a.Deleted == false && a.PlannedActivityResource.LcmHardware && a.LcmEngineering.NumberOfNodes > 0;
        }

        public static string toLinkedPlannedActivityName(this PlannedActivity x)
        {
            return
                $"{x.PlannedImplementationYear} | {x.PlannedActivityResource?.PlannedActivityResourceDescription} | {StripHTML(!string.IsNullOrEmpty(x.ActivityDetails) ? x.ActivityDetails : "")} | {x.ActivityStatus?.ActivityStatusDescription} | {x.DeliveryStatus?.DeliveryStatusDescription}";
        }
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        public static string toOriginalDesignComponentDescriptionToArchivedPAs(this Plannedactivities entity, IRepositoryWrapper _repositoryWrapper)
        {
            Entities.Models.DesignComponent designComponent = null;
            if (entity.Lcmengineeringid != null)
            {
                designComponent = DesignComponentMapper.GetDesignComponentMapper(
                    _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Lcmengineeringid == entity.Lcmengineeringid)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .FirstOrDefault()?.Designcomponent);


            }

            else if (entity.Networkelementasplannedid != null)
            {
                designComponent = DesignComponentMapper.GetDesignComponentMapper(_repositoryWrapper.NetworkElementAsPlanned
                    .FindByCondition(x => x.Networkelementasplannedid == entity.Networkelementasplannedid)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                    .ThenInclude(x => x.Platform)
                    .FirstOrDefault()?.Designcomponent);
            }
            if (designComponent == null) return null;
            return DesignComponentMapper.SetDesignComponentMapper(designComponent).toDesignComponentNameLcm(_repositoryWrapper);
        }
        public static string toOriginalDesignComponentDescription(this Entities.Models.PlannedActivity entity, IRepositoryWrapper _repositoryWrapper)
        {
            Entities.Models.DesignComponent designComponent = null;
            if (entity.LcmEngineeringId != null)
            {
                designComponent = DesignComponentMapper.GetDesignComponentMapper(
                    _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Lcmengineeringid == entity.LcmEngineeringId)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .FirstOrDefault()?.Designcomponent);


            }

            else if (entity.NetworkElementAsPlannedId != null)
            {
                designComponent = DesignComponentMapper.GetDesignComponentMapper(_repositoryWrapper.NetworkElementAsPlanned
                    .FindByCondition(x => x.Networkelementasplannedid == entity.NetworkElementAsPlannedId)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                    .ThenInclude(x => x.Platform)
                    .FirstOrDefault()?.Designcomponent);
            }
            if (designComponent == null) return null;
            return DesignComponentMapper.SetDesignComponentMapper(designComponent).toDesignComponentNameLcm(_repositoryWrapper);
        }

        public static string toOriginalDesignComponentFamilyDescriptionFromArchivedPAs(this Plannedactivities entity, IRepositoryWrapper _repositoryWrapper)
        {
            Entities.Models.DesignComponentFamily designComponentFamily = null;
            if (entity.Lcmengineeringid != null)
            {
                designComponentFamily = DesignComponentFamilyMapper.Get(
                    _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Lcmengineeringid == entity.Lcmengineeringid)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Designcomponent).ThenInclude(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary)
                    .FirstOrDefault()?.Designcomponent.Designcomponentfamily);


            }

            else if (entity.Networkelementasplannedid != null)
            {
                designComponentFamily = DesignComponentFamilyMapper.Get(_repositoryWrapper.NetworkElementAsPlanned
                    .FindByCondition(x => x.Networkelementasplannedid == entity.Networkelementasplannedid)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Designcomponent).ThenInclude(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary)
                    .FirstOrDefault()?.Designcomponent.Designcomponentfamily);
            }
            else if (entity.Designaspectid != null)
            {
                designComponentFamily = DesignComponentFamilyMapper.Get(_repositoryWrapper.DesignAspectRepository
                    .FindByCondition(x => x.Id == entity.Designaspectid)
                    .Include(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary)
                    .FirstOrDefault()?.Designcomponentfamily);
            }
            if (designComponentFamily == null) return null;
            return designComponentFamily.DCFName(_repositoryWrapper);
        }
        public static string toOriginalDesignComponentFamilyDescription(this Entities.Models.PlannedActivity entity, IRepositoryWrapper _repositoryWrapper)
        {
            Entities.Models.DesignComponentFamily designComponentFamily = null;
            //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
            if (entity.LcmEngineeringId != null)
            {
                designComponentFamily = DesignComponentFamilyMapper.Get(
                    _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Lcmengineeringid == entity.LcmEngineeringId && x.Designcomponent.Deleted == false)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Designcomponent).ThenInclude(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary)
                    .FirstOrDefault()?.Designcomponent.Designcomponentfamily);


            }

            else if (entity.NetworkElementAsPlannedId != null)
            {
                designComponentFamily = DesignComponentFamilyMapper.Get(_repositoryWrapper.NetworkElementAsPlanned
                    .FindByCondition(x => x.Networkelementasplannedid == entity.NetworkElementAsPlannedId && x.Designcomponent.Deleted == false)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Designcomponent).ThenInclude(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary)
                    .FirstOrDefault()?.Designcomponent.Designcomponentfamily);
            }
            else if (entity.DesignAspectId != null)
            {
                designComponentFamily = DesignComponentFamilyMapper.Get(_repositoryWrapper.DesignAspectRepository
                    .FindByCondition(x => x.Id == entity.DesignAspectId && x.Designcomponentfamily.Deleted == false)
                    .Include(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary)
                    .FirstOrDefault()?.Designcomponentfamily);
            }
            if (designComponentFamily == null) return null;
            return designComponentFamily.DCFName(_repositoryWrapper);
        }

        public static Plannedactivities GetPlannedActivityProjectStatus(this Plannedactivities entity, IRepositoryWrapper _repositoryWrapper)
        {

            if (entity != null)
            {
                var activityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Activitystatusid == entity.Activitystatusid).SingleOrDefault();//_repositoryWrapper.ActivityStatus.FindByCondition(x => x.Activitystatusid == entity.Activitystatusid).FirstOrDefault();
                var projectstatusActivityStatus = (activityStatus)?.Projectstatuscombinationrule;

                var budgetAvailability = _repositoryWrapper.BudgetAvailability.FindByCondition(x => x.Budgetavailabilityid == entity.Budgetavailabilityid).SingleOrDefault();//_repositoryWrapper.BudgetAvailability.FindByCondition(x => x.Budgetavailabilityid == entity.Budgetavailabilityid).FirstOrDefault();
                var projectstatusBudgetAvailability = (budgetAvailability)?.Projectstatuscombinationrule;

                var deliveryStatus = _repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatusid == entity.Deliverystatusid).SingleOrDefault();//_repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatusid == entity.Deliverystatusid).FirstOrDefault();

                var planningStatus = _repositoryWrapper.PlanningActivityStatus.FindByCondition(x => x.Planningactivitystatusid == entity.Planningactivitystatusid).SingleOrDefault();//_repositoryWrapper.PlanningActivityStatus.FindByCondition(x => x.Planningactivitystatusid == entity.Planningactivitystatusid).FirstOrDefault();

                if (entity.Plannedimplementationyear > DateTime.Now.Year &&
                        activityStatus?.Activitystatus.ToLower().Replace(" ", "") == "In Planning".ToLower().Replace(" ", "") &&
                        deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != "Rollout Complete".ToLower().Replace(" ", "") &&
                        planningStatus?.Planningactivitystatus.ToLower().Replace(" ", "") == "PROPOSED".ToLower().Replace(" ", "") &&
                        budgetAvailability?.Description.ToLower().Replace(" ", "") == "No".ToLower().Replace(" ", ""))// old code
                {
                    entity.Projectstatus = Outputs.ProjectStartedBudgetNotNecessary;
                }
                else if (entity.Plannedimplementationyear >= DateTime.Now.Year &&
                   activityStatus?.Activitystatus.ToLower().Replace(" ", "") == "In Mobilisation".ToLower().Replace(" ", "") &&
                   deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != "Rollout Complete".ToLower().Replace(" ", "") &&
                   budgetAvailability?.Description.ToLower().Replace(" ", "") == "Yes".ToLower().Replace(" ", ""))// old code
                {
                    entity.Projectstatus = Outputs.ProjectStartedBudgetInDB;
                }
                else if (
                    (activityStatus?.Activitystatus.ToLower().Replace(" ", "") == "In Delivery Engineering".ToLower().Replace(" ", "")
                    || activityStatus?.Activitystatus.ToLower().Replace(" ", "") == "IN DELIVERY OPERATIONS".ToLower().Replace(" ", "")) &&
                  deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != "Rollout Complete".ToLower().Replace(" ", "") &&
                    planningStatus?.Planningactivitystatus.ToLower().Replace(" ", "") == "CONFIRMED".ToLower().Replace(" ", "") &&
                  budgetAvailability?.Description.ToLower().Replace(" ", "") == "Yes".ToLower().Replace(" ", ""))// old code
                {
                    entity.Projectstatus = Outputs.ProjectOngoingBudgetInDB;
                }
                else if (
                   (activityStatus?.Activitystatus.ToLower().Replace(" ", "") == "In Delivery Engineering".ToLower().Replace(" ", "")
                   || activityStatus?.Activitystatus.ToLower().Replace(" ", "") == "IN DELIVERY OPERATIONS".ToLower().Replace(" ", "")) &&
                 deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != "Rollout Complete".ToLower().Replace(" ", "") &&
                   planningStatus?.Planningactivitystatus.ToLower().Replace(" ", "") == "CONFIRMED".ToLower().Replace(" ", "") &&
                 budgetAvailability?.Description.ToLower().Replace(" ", "") == "No".ToLower().Replace(" ", ""))// old code
                {
                    entity.Projectstatus = Outputs.ProjectOngoingBudgetNotNecessary;
                }
                else if (
                   activityStatus?.Activitystatus.ToLower().Replace(" ", "") == "In Planning".ToLower().Replace(" ", "") &&
                 deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != "Rollout Complete".ToLower().Replace(" ", "") &&
                   planningStatus?.Planningactivitystatus.ToLower().Replace(" ", "") == "rejected" &&
                 budgetAvailability?.Description.ToLower().Replace(" ", "") == "No".ToLower().Replace(" ", ""))// old code
                {
                    entity.Projectstatus = Outputs.RequestedButRefused;
                }
                else if (
                   activityStatus?.Activitystatus.ToLower().Replace(" ", "") == "COMPLETE".ToLower().Replace(" ", "") &&
                 deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") == "Rollout Complete".ToLower().Replace(" ", ""))
                {
                    entity.Projectstatus = Outputs.ProjectCompleted;
                }
                else
                {
                    // Set Default Value April 16 2024
                    entity.Projectstatus = Outputs.NotRequested;
                }
            }

            return entity;
        }

        public static string toPlannedDesignComponentDescription(this Entities.Models.PlannedActivity entity, IRepositoryWrapper _repositoryWrapper)
        {
            Entities.Models.DesignComponent designComponent = null;
            var decommission_StatusId = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == entity.PlannedActivityResourceId && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.Decommission_Service_Node).FirstOrDefault();

            designComponent = DesignComponentMapper.GetDesignComponentMapper(_repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == entity.DesignComponentId, true)
                 .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary).FirstOrDefault());

            //return designComponent == null
            //    ? null
            //    : $"{DesignComponentMapper.SetDesignComponentMapper(designComponent).toDesignComponentNameLcm(_repositoryWrapper)}"
            //      + (decommission_StatusId != null ? $" {ConstantValueFilter.toBeDecommissioned}" : "");

            if (designComponent == null)
            {
                return null;
            }
            else if (decommission_StatusId != null)
            {
                return $"{ConstantValueFilter.toBeDecommissioned} {DesignComponentMapper.SetDesignComponentMapper(designComponent).toDesignComponentNameLcm(_repositoryWrapper)}";
            }
            else
            {
                return $"{DesignComponentMapper.SetDesignComponentMapper(designComponent).toDesignComponentNameLcm(_repositoryWrapper)}";
            }

        }

        public static string toPlannedDesignComponentDescription(this ICollection<Plannedactivities> entity, IRepositoryWrapper _repositoryWrapper)
        {
            Entities.Models.DesignComponent designComponent = null;

            var decommission_StatusId = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == entity.Select(y => y.Plannedactivityresourceid).FirstOrDefault() && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.Decommission_Service_Node).FirstOrDefault();


            designComponent = DesignComponentMapper.GetDesignComponentMapper(_repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == entity.Select(d => d.Designcomponentid).FirstOrDefault(), true)
                 .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary).FirstOrDefault());

            //return designComponent == null
            //    ? null
            //    : $"{DesignComponentMapper.SetDesignComponentMapper(designComponent).toDesignComponentNameLcm(_repositoryWrapper)}"
            //      + (decommission_StatusId != null ? $" {ConstantValueFilter.toBeDecommissioned}" : "");

            if (designComponent == null)
            {
                return null;

            }
            else if (decommission_StatusId != null)
            {
                return $"{ConstantValueFilter.toBeDecommissioned} {DesignComponentMapper.SetDesignComponentMapper(designComponent).toDesignComponentNameLcm(_repositoryWrapper)}";
            }
            else
            {
                return $"{DesignComponentMapper.SetDesignComponentMapper(designComponent).toDesignComponentNameLcm(_repositoryWrapper)}";
            }
        }

        public static PlannedActivityTypeForEnum? GetPlannedActivityTypeFor(this PlannedActivity entity)
        {
            if (entity.LcmEngineeringId != null)
            {
                return PlannedActivityTypeForEnum.LcmEngineering;
            }
            else if (entity.DesignAspectId != null)
            {
                return PlannedActivityTypeForEnum.DesignAspect;
            }
            else if (entity.NetworkElementAsPlannedId != null && entity.ForAddAsset == true)
            {
                return PlannedActivityTypeForEnum.AddAsset;
            }
            else if (entity.NetworkElementAsPlannedId != null && entity.ForEditAsset == true)
            {
                return PlannedActivityTypeForEnum.EditAsset;
            }

            return null;
        }

        public static string GetPlannedImplementationyear(this PlannedActivity entity)
        {
            int year = entity.PlannedImplementationYear;
            DateTime starYear = new DateTime(year, 4, 1);

            string financialYear = $"FY {(starYear.AddYears(1)).ToString("yy")}: 'Apr {starYear.ToString("yy")} - Mar {(starYear.AddYears(1)).ToString("yy")}'";

            return financialYear;

        }
        public static string GetPlannedAction(this PlannedActivity entity, IRepositoryWrapper _repositoryWrapper)
        {
            string plannedAction = string.Empty;
            try
            {
                var paEntity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == entity.PlannedActivityId)
                    .Include(x => x.Damigrationstatus)
                    .Include(x => x.Daassetmigration).ThenInclude(x => x.Deploymentstatus).FirstOrDefault();

                var pAResources = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == entity.PlannedActivityResourceId
                                       && (x.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity ||
                                       x.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity_For_DA)).FirstOrDefault();
                if (pAResources != null)
                {
                    return "No Planned Activity";
                }
                var PlannedActivityResourceEntity = _repositoryWrapper.PlannedActivityResourceRepository
                    .FindByCondition(x => x.Plannedactivityresourceid == entity.PlannedActivityResourceId).Select(x => new
                    { x.Plannedactivityresource, x.Plannedactivityresourceid,x.Rulelinkeddc}).FirstOrDefault();


                var PlannedActivityResource = PlannedActivityResourceEntity?.Plannedactivityresource ?? string.Empty  ;
                

                DateTime pACompletionDate = (DateTime)entity.PlannedCompletion;
                string month = pACompletionDate.ToString("MMM");
                if ((paEntity.Damigrationstatus != null && paEntity.Damigrationstatus.Count > 1) && 
                    (PlannedActivityResourceEntity?.Rulelinkeddc == (int)PlannedActivityResourceEnum.Infra_Readiness) )
                {
                    var plannedStatusCount = paEntity.Damigrationstatus.Count(x => x.Statusid == 1);
                    var inProgressStatusCount = paEntity.Damigrationstatus.Count(x => x.Statusid == 2);
                    var completeStatusCount = paEntity.Damigrationstatus.Count(x => x.Statusid == 3);

                    plannedAction = $"{month}  -  {pACompletionDate.ToString("yyyy")}  |  {PlannedActivityResource}  |" +
                        $"  {entity?.DeliveryStatus?.DeliveryStatusDescription} | Planned : {plannedStatusCount} | InProgress : {inProgressStatusCount} | Completed : {completeStatusCount}";
                }
                else if ( (paEntity.Daassetmigration != null && paEntity.Daassetmigration.Count > 1)
                    &&
                    (PlannedActivityResourceEntity?.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration))
                {
                    
                    var assetDeploymentStatusWiseCountDetails = GetDaAssetDepoymentStatusCount(null,_repositoryWrapper, paEntity.Daassetmigration.ToList());

                  

                    plannedAction = $"{month}  -  {pACompletionDate.ToString("yyyy")}  |  {PlannedActivityResource}  |" +
                        $"  {entity?.DeliveryStatus?.DeliveryStatusDescription} {assetDeploymentStatusWiseCountDetails}";
                }
                else
                {
                    plannedAction = $"{month + " - " + pACompletionDate.ToString("yyyy") + " | " + PlannedActivityResource + " | " + entity?.DeliveryStatus?.DeliveryStatusDescription}";
                }
                return plannedAction;
            }
            catch (Exception ex)
            {
                return plannedAction;
            }
        }
public static string GetDaAssetDepoymentStatusCount(this Plannedactivities entity, IRepositoryWrapper _repositoryWrapper, List<Daassetmigration> daAssetEntities)
        {

            var daAssetDeploymentStatus = daAssetEntities?.Select(x => new
            {
                x.Deploymentstatusid,
                x?.Deploymentstatus?.Deploymentstatus
            })?.GroupBy(x => x.Deploymentstatus)?.ToList();
            var assetDeploymentStatusWiseCountDetails = "";

            foreach (var item in daAssetDeploymentStatus)
            {
                assetDeploymentStatusWiseCountDetails += "| " + (item.Key ?? ConstantValueFilter.StatusNotAssigned) + " : " + item.Count();
            }
            return assetDeploymentStatusWiseCountDetails;
        }
        public static string GetPlannedAction(this Plannedactivities entity, IRepositoryWrapper _repositoryWrapper)
        {
            try
            {
                var pAResources = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == entity.Plannedactivityresourceid
                       && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity).FirstOrDefault();
                if (pAResources != null)
                {
                    return "No Planned Activity";
                }

                string PlannedActivityResource = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == entity.Plannedactivityresourceid).Select(x => x.Plannedactivityresource).FirstOrDefault();
                DateTime pACompletionDate = (DateTime)entity.Plannedcompletion;
                string month = pACompletionDate.ToString("MMM");
                string plannedAction = $"{month + " - " + pACompletionDate.ToString("yyyy") + " | " + PlannedActivityResource + " | " + entity?.Deliverystatus?.Deliverystatus}";
                return plannedAction;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetPlannedAction(this ICollection<Plannedactivities> plannedActivities, IRepositoryWrapper _repositoryWrapper)
        {
            try
            {
                var pAResources = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == plannedActivities.Select(x => x.Plannedactivityresourceid).FirstOrDefault()
                       && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity).FirstOrDefault();
                if (pAResources != null)
                {
                    return "No Planned Activity";
                }
                string PlannedActivityResource = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == plannedActivities.Select(x => x.Plannedactivityresourceid).FirstOrDefault()).Select(x => x.Plannedactivityresource).FirstOrDefault();
                DateTime pACompletionDate = (DateTime)plannedActivities.Select(x => x.Plannedcompletion).FirstOrDefault();
                string month = pACompletionDate.ToString("MMM");
                string plannedAction = $"{month + " - " + pACompletionDate.ToString("yyyy") + " | " + PlannedActivityResource + " | " + plannedActivities.Select(x => x.Deliverystatus?.Deliverystatus).FirstOrDefault()}";
                return plannedAction;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetAncillaryDataForNoPa(this ICollection<PlannedActivity> entity, IRepositoryWrapper _repositoryWrapper, bool isReason = true)
        {
            string ancillaryDataForNoPa = string.Empty;
            try
            {

                var pAResources = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == entity.Select(x => x.PlannedActivityResourceId).FirstOrDefault()
                       && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity).FirstOrDefault();
                var lcmAncillaryData = _repositoryWrapper.LcmAncillaryData.FindByCondition(x => x.Lcmengineeringid == entity.Select(x => x.LcmEngineeringId).FirstOrDefault()).FirstOrDefault();
                if (pAResources != null && lcmAncillaryData != null)
                {
                    if (isReason)
                    {
                        ancillaryDataForNoPa = lcmAncillaryData.Reasonfornoplan;
                    }
                    else
                    {
                        ancillaryDataForNoPa = lcmAncillaryData.Commentonprojectstatus;
                    }
                }
                return ancillaryDataForNoPa;
            }
            catch (Exception ex)
            {
                return ancillaryDataForNoPa;
            }
        }

        public static string GetAncillaryDataForNoPa(this Plannedactivities entity, IRepositoryWrapper _repositoryWrapper, bool isReason = true)
        {
            string ancillaryDataForNoPa = string.Empty;
            try
            {

                var pAResources = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == entity.Plannedactivityresourceid
                       && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity).FirstOrDefault();
                var lcmAncillaryData = _repositoryWrapper.LcmAncillaryData.FindByCondition(x => x.Lcmengineeringid == entity.Lcmengineeringid).FirstOrDefault();
                if (pAResources != null && lcmAncillaryData != null)
                {
                    if (isReason)
                    {
                        ancillaryDataForNoPa = lcmAncillaryData.Reasonfornoplan;
                    }
                    else
                    {
                        ancillaryDataForNoPa = lcmAncillaryData.Commentonprojectstatus;
                    }
                }
                return ancillaryDataForNoPa;
            }
            catch (Exception ex)
            {
                return ancillaryDataForNoPa;
            }
        }
        public static string GetActvityString(this Plannedactivities plannedactivities, IRepositoryWrapper _repositoryWrapper)
        {
            string activityString = string.Empty;
            if (plannedactivities == null)
            {
                return activityString;
            }
            else
            {
                 var paWithIncludeTables = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedactivities.Plannedactivityid)
                        .Include(x => x.Opco)
                        .Include(x => x.Plannedactivityresource)
                        .Include(x => x.Designcomponent)
                        .Include(x => x.Designcomponentfamily)
                        .Include(x => x.Serviceplan).ThenInclude(y => y.Servicemaster) 
                        .FirstOrDefault();
                if(paWithIncludeTables != null)
                    {
                        string project = paWithIncludeTables?.Deliveryprojectname?
                                                .Split('-')
                                                .FirstOrDefault() ?? string.Empty;
                   
                        if (paWithIncludeTables.Plannedactivityresource != null && paWithIncludeTables.Isserviceplan!=null && paWithIncludeTables.Isserviceplan == true)
                        {
                            string serviceMasterDescription = 
                            paWithIncludeTables.Serviceplan.Servicemaster.Description ??
                                                              string.Empty ;
                            activityString = string.Join("-", paWithIncludeTables.Opco?.Opco, project, serviceMasterDescription);
                        }
                        else if (paWithIncludeTables.Plannedactivityresource != null && paWithIncludeTables.Plannedactivityresource?.Fordesignaspect == true)
                        {
                            var designComponentFamilyName = paWithIncludeTables.Designcomponentfamilyid != null ? 
                            paWithIncludeTables.Designcomponentfamily.DCFName(_repositoryWrapper) : string.Empty;

                            activityString = paWithIncludeTables.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Project_Plan ?
                                string.Join("-", paWithIncludeTables.Opco?.Opco, project, paWithIncludeTables.Projectdescription)
                                : string.Join("-", paWithIncludeTables.Opco?.Opco, designComponentFamilyName, paWithIncludeTables.Plannedactivityresource?.Plannedactivityresource);
                        }
                        else if (paWithIncludeTables.Plannedactivityresource != null)
                        {
                            var designComponentName = paWithIncludeTables.Designcomponentid != null ? paWithIncludeTables.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper) : string.Empty;
                            activityString = string.Join("-", paWithIncludeTables.Opco?.Opco, designComponentName, paWithIncludeTables.Plannedactivityresource?.Plannedactivityresource);
                        }
                        else
                        {
                            return activityString;
                        }
                    }
                    return activityString;
                }

        }

    }
}
