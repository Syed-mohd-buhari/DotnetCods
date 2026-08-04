using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.GenericReportDto;
using CAM.Entities.Models;
using CAM.Enum;
using DocumentFormat.OpenXml.Presentation;
using IdentityServer4.Extensions;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Rules
{
    public enum LCMEngineeringRulesSoftware
    {
        //LCMSoftware = 0,
        //LCMHardware = 1,
        LCMSoftwareOutput = 0,
        LCMSoftwareWarranty = 1,
        LCMSoftwareEndofWarrantyDate = 2,
        LCMSoftwareSupportType = 3,
        LCMSoftwareSupportProvider = 4,
        LCMSoftwareEndOfSupportContract = 5,
        LCMSoftwareEndOfSupportContractOutputLcm = 6,
        LCMSoftwareLcmStatusOps = 7,
        LCMSoftwareLcmStatusEng = 8,
        LCMSoftwareLcmStatus = 9,

    }
    public enum LCMEngineeringRulesHardware
    {

        LCMHardwareOutput = 0,
        LCMHardwareSupportType = 1,
        LCMHardwareSupportProvider = 2,
        LCMHardwareEndOfSupportContractDate = 3,
        LCMHardwareEndOfSupportContractOutputLcm = 4,
        LCMHardwareLcmStatusOps = 5,
        LCMHardwareLcmStatusEng = 6,
        LCMHardwareLcmStatus = 7,
    }
    public static class LCMEngineeringRulesExtension
    {
        public struct Outputs
        {
            public const string yesFullExtendedMaintenance = "Yes (Full extended maintenance)";
            public const string yesCoveredByWarranty = "Yes (Full extended maintenance)";
            public const string warrantyOutput = "Warranty";
            public const string extended = "Extended";
            public const string bestEffort = "Best Effort";
            public const string yes = "Yes";
            public const string standard = "Standard";
            public const string yesPartialExtendedMaintenance = "No";
            public const string fullTpmContract = "Yes (Full extended maintenance)";
            public const string partialTpmContract = "TPM contract";
            public const string entityOutputToLcmHardware = "No";
            public const string noOpenSource = "No (Open Source)";
            public const string noVfInternalSupport = "Yes (VF internal support)";
            public const string pendingOMRenewal = "Pending O&M Renewal";
            public const string noPendingOMRenewal = "No (Pending O&M Renewal)";
            public const string openSourceSw = "Open Source SW";
            public const string itCanBeSupportedInternally = "It can be supported internally";
            public const string noNoContractDueToVfDecision = "No";
            public const string none = "None";
            public const string no = "No";
            public const string vodafoneDecisionCloseToDecommissioningRiskToCostRatioAccepted = "Vodafone decision (close to decommissioning/Risk to Cost ratio accepted)";
            public const string other = "Other";
            public const string infoMissing = "OPS info missing";
            public const string expired = "Expired";
            public const string onSupport = "On support";
            public const string expiration = "On expiration";
            public const string opsMissing = "OPS Missing";
            public const string engInfoMissing = "ENG info missing";
            public const string opsEngInfoMissing = "OPS & ENG info missing";
            public const string sparePartsAvailability = "Spare Parts Availability";
            public const string wrongCombination = "Wrong combination";
            public const string vodafone = "Vodafone";
            public const string sparesStock = "Spares Stock";
            public const string NoSparepartAvailiability = "No spare parts availiability, no support";
            public const string OnExpiration = "On expiration";
            public const string OnsupportOnExpiration = "On support/On Expiration";
            public const string ProjectStartedBudgetNotNecessary = "Planned, to be started (BDG not necessary)";
            public const string ProjectStartedBudgetInLRP = "Planned, (BDG in LRP)";
            public const string ProjectStartedBudgetInDB = "Planned, (BDG in DB)";
            public const string ProjectOngoingBudgetInDB = "Planned, (BDG in DB)";
            public const string ProjectOngoingBudgetNotNecessary = "Planned, Ongoing (BDG not necessary)";
            public const string RequestedButRefused = "Not Planned, (BDG requested but refused)";
            public const string ProjectCompleted = "Project Completed (remember to update also OOS column)";
            public const string RequestedButOngoing = "Not Planned, (BDG requested but approval ongoing)";
            public const string NotRequested = "Not Planned, (BDG not requested)";
            public const string BusinessDepend = "Not Planned, Business dependencies";
            public const string NetworkDepend = "Not Planned, Network dependencies";
            public const string ActionCompleted = "Historical back-up (remediation action completed)";
            public const string AssetPlannedToBeInserted = "Asset planned to be inserted in the network";
            public const string DeAssetManagedByDELocal = "DE asset (managed by DE local tool)";
            public const string InScope = "In scope";
            public const string NoReplacement = "Asset in planned dismission, no replacement";
            public const string Empty = "";
            public const string Error = "Error";
            public const string Planned = "Planned";
            public const string Compliant = "Compliant";
            public const string NonCompliant = "Non-Compliant";


        }

        //public async static Task SetRule(this LcmEngineering entity, IRepositoryWrapper _repositoryWrapper, LCMEngineeringRules rule)
        //{
        //    switch (rule)
        //    {
        //        case LCMEngineeringRules.LCMSoftware: { await SetLCMSoftware(entity, _repositoryWrapper); break; }
        //        case LCMEngineeringRules.LCMHardware: { await SetLCMHardware(entity, _repositoryWrapper); break; }
        //        default: throw new NotImplementedException();
        //    }
        //}

        //Quando la regola non ha bisogno di connettersi al database
        public async static Task SetRule(this Lcmengineering entity, LCMEngineeringRulesSoftware rule)
        {
            switch (rule)
            {
                default: throw new NotImplementedException();
            }
        }

        public async static Task<T> GetRuleSoftware<T>(this Lcmengineering entity, IRepositoryWrapper _repositoryWrapper, LCMEngineeringRulesSoftware rule, bool isLcmDBExport = false)

        {
            T returnValue;
            switch (rule)
            {
                case LCMEngineeringRulesSoftware.LCMSoftwareOutput:
                    returnValue = ConvertValue<T>(await GetOutputLcmSoftware(entity, _repositoryWrapper, isLcmDBExport));
                    break;
                case LCMEngineeringRulesSoftware.LCMSoftwareWarranty:
                    returnValue = ConvertValue<T>(await GetWarrantyLcmSoftware(entity, _repositoryWrapper));
                    break;
                case LCMEngineeringRulesSoftware.LCMSoftwareEndofWarrantyDate:
                    returnValue = ConvertValue<T>(await GetEndOfWarrantyDateLcmSoftware(entity, _repositoryWrapper));
                    break;
                case LCMEngineeringRulesSoftware.LCMSoftwareSupportType:
                    returnValue = ConvertValue<T>(await GetSupportTypeLcmSoftware(entity, _repositoryWrapper));
                    break;
                case LCMEngineeringRulesSoftware.LCMSoftwareSupportProvider:
                    returnValue = ConvertValue<T>(await GetSupportProviderLcmSoftware(entity, _repositoryWrapper));
                    break;

                case LCMEngineeringRulesSoftware.LCMSoftwareEndOfSupportContract:
                    returnValue = ConvertValue<T>(await GetEndOfSupportContractLcmSoftware(entity, _repositoryWrapper));

                    break;
                case LCMEngineeringRulesSoftware.LCMSoftwareEndOfSupportContractOutputLcm:
                    returnValue = ConvertValue<T>(await GetEndOfSupportContractLcmSoftwareReport(entity, _repositoryWrapper));
                    break;
                case LCMEngineeringRulesSoftware.LCMSoftwareLcmStatus:
                    returnValue = ConvertValue<T>(await GetNewLcmStatusSoftware(entity, _repositoryWrapper));

                    break;
                case LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusOps:
                    returnValue = ConvertValue<T>(GetLcmStatusOpsSoftware(entity, _repositoryWrapper));

                    break;
                case LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusEng:
                    returnValue = ConvertValue<T>(await GetLcmStatusEngSoftware(entity, _repositoryWrapper));

                    break;

                default: throw new NotImplementedException();
            }
            return returnValue;
        }

        public static T ConvertValue<T>(object? value)
        {
            if (value == null) return default(T);
            var property = value?.GetType();
            Type t = Nullable.GetUnderlyingType(property) ?? property;
            return (T)((value == null) ? null : Convert.ChangeType(value, t));
        }


        public async static Task<T> GetRuleHardware<T>(this Lcmengineering entity, IRepositoryWrapper _repositoryWrapper, LCMEngineeringRulesHardware rule)
        {
            T returnValue;
            switch (rule)
            {
                case LCMEngineeringRulesHardware.LCMHardwareOutput:
                    returnValue = ConvertValue<T>(await GetOutputLcmHardware(entity, _repositoryWrapper));
                    break;

                case LCMEngineeringRulesHardware.LCMHardwareSupportType:
                    returnValue = ConvertValue<T>(await GetSupportTypeLcmHardware(entity, _repositoryWrapper));
                    break;
                case LCMEngineeringRulesHardware.LCMHardwareSupportProvider:
                    returnValue = ConvertValue<T>(await GetSupportProviderLcmHardware(entity, _repositoryWrapper));
                    break;
                case LCMEngineeringRulesHardware.LCMHardwareEndOfSupportContractDate:
                    returnValue = ConvertValue<T>(await GetEndOfSupportContractDateLcmHardware(entity, _repositoryWrapper));

                    break;
                case LCMEngineeringRulesHardware.LCMHardwareEndOfSupportContractOutputLcm:
                    returnValue = ConvertValue<T>(await GetEndOfSupportContractDateLcmHardwareOutput(entity, _repositoryWrapper));

                    break;
                case LCMEngineeringRulesHardware.LCMHardwareLcmStatusOps:
                    returnValue = ConvertValue<T>(GetLCMStatusOPSHardware(entity, _repositoryWrapper));

                    break;
                case LCMEngineeringRulesHardware.LCMHardwareLcmStatusEng:
                    returnValue = ConvertValue<T>(await GetLcmStatusEngHardware(entity, _repositoryWrapper));

                    break;
                case LCMEngineeringRulesHardware.LCMHardwareLcmStatus:
                    returnValue = ConvertValue<T>(await GetNewLcmStatusHardware(entity, _repositoryWrapper));

                    break;
                default: throw new NotImplementedException();
            }
            return returnValue;
        }

        private static async Task<DateTime?> GetEndOfSupportContractDateLcmHardwareOutput(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var selectedIsHardwareSupported = entity.Hardwaresupportedid.HasValue ? (await _repositoryWrapper.SupportedResource
                .FindByCondition(x => x.Id == entity.Hardwaresupportedid).FirstOrDefaultAsync()).Rule : 0;

            switch ((LcmEnum.SupportedResourceEnums)selectedIsHardwareSupported)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return entity.Hardwareendofsupportcontract;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                case LcmEnum.SupportedResourceEnums.AsThirdParty:
                    return entity.Hardwareendofsupportcontract;
                case LcmEnum.SupportedResourceEnums.AsNone:
                    return (DateTime?)null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task<DateTime?> GetEndOfSupportContractDateLcmHardware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            //var hw = (await _repositoryWrapper.DesignComponent
            //        .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid).Include(x => x.Systemtype)
            //        .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
            //        .ThenInclude(s => s.Orgeqpmanufacturer).SingleAsync())
            //    .Systemtype.Systemtypesmajorhardwarebuilds.SingleOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware;

            var selectedIsHardwareSupported = entity.Hardwaresupportedid.HasValue ? (await _repositoryWrapper.SupportedResource
                .FindByCondition(x => x.Id == entity.Hardwaresupportedid).FirstOrDefaultAsync()).Rule : 0;
            switch ((LcmEnum.SupportedResourceEnums)selectedIsHardwareSupported)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return (DateTime?)null;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                case LcmEnum.SupportedResourceEnums.AsThirdParty:
                    return entity.Hardwareendofsupportcontract;
                case LcmEnum.SupportedResourceEnums.AsNone:
                    return (DateTime?)null;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task<string> GetSupportProviderLcmHardware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var hw = entity.Designcomponentid == 0 ? null : (await _repositoryWrapper.DesignComponent
                               .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                               .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                               .ThenInclude(x => x.Majorhardware)
                               .ThenInclude(s => s.Orgeqpmanufacturer).SingleAsync())
                           .Systemtype.Systemtypesmajorhardwarebuilds.SingleOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware;

            var selectedIsHardwareSupported = entity.Hardwaresupportedid.HasValue ? (await _repositoryWrapper.SupportedResource
                .FindByCondition(x => x.Id == entity.Hardwaresupportedid).SingleAsync()).Rule : 0;
            switch ((LcmEnum.SupportedResourceEnums)selectedIsHardwareSupported)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return Outputs.no;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                    if (hw?.Endofsupport == null)
                    {
                        return hw?.Orgeqpmanufacturer?.Originalequipmentmanufacturer;
                    }
                    else
                    {
                        return hw?.Orgeqpmanufacturer?.Originalequipmentmanufacturer;
                    }
                //break;
                case LcmEnum.SupportedResourceEnums.AsThirdParty:
                    return entity.Hardwaresupportprovider;
                case LcmEnum.SupportedResourceEnums.AsNone:
                    return Outputs.vodafone;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task<string> GetSupportTypeLcmHardware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var hw = entity.Designcomponentid == 0 ? null : (await _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                    .Include(x => x.Systemtype)
                    .ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware)
                    .ThenInclude(s => s.Orgeqpmanufacturer).SingleAsync())
                .Systemtype.Systemtypesmajorhardwarebuilds.SingleOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware;

            var selectedIsHardwareSupported = entity.Hardwaresupportedid.HasValue ? (await _repositoryWrapper.SupportedResource
                .FindByCondition(x => x.Id == entity.Hardwaresupportedid).FirstOrDefaultAsync()).Rule : 0;
            switch ((LcmEnum.SupportedResourceEnums)selectedIsHardwareSupported)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return Outputs.no;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                    if (hw?.Endofsupport == null)
                    {
                        return Outputs.standard;
                    }

                    if (entity.Hardwareendofsupportcontract < hw?.Endofsupport) return Outputs.standard;
                    if (entity.Hardwareendofsupportcontract >= hw?.Endofsupport) return Outputs.extended;
                    break;
                case LcmEnum.SupportedResourceEnums.AsThirdParty:
                    return entity.Sparesprovisioned ? Outputs.standard : Outputs.bestEffort;
                case LcmEnum.SupportedResourceEnums.AsNone:
                    if (entity.Renewalinprogress)
                    {
                        return Outputs.noPendingOMRenewal;
                    }
                    if (entity.Sparesprovisioned)
                    {
                        return Outputs.sparesStock;
                    }

                    var text = "";
                    if (entity.Reasoncheckboxresourcelcmengineeringhardware != null)
                        foreach (var checkbox in entity.Reasoncheckboxresourcelcmengineeringhardware)
                        {
                            if (checkbox != null)
                            {
                                text = _repositoryWrapper.ReasonCheckboxResource.FindByCondition(x => x.Id == checkbox.Reasoncheckboxresourceid).Single().Description;

                            }
                        }

                    return string.IsNullOrEmpty(text.ToString()) ? Outputs.no : $"No({text})";

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Outputs.no;
        }

        private static async Task<string> GetOutputLcmHardware(Lcmengineering entity,
            IRepositoryWrapper _repositoryWrapper)
        {
            try
            {
                var selectedIsHardwareSupported = entity.Hardwaresupportedid.HasValue ? (await _repositoryWrapper.SupportedResource
            .FindByCondition(x => x.Id == entity.Hardwaresupportedid).SingleAsync())?.Rule : 0;

                var dataMajorHardwareBuildsEOS = entity.Designcomponentid == 0 ? null : (await _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid).Include(x => x.Systemtype)
                    .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                    .SelectMany(x => x.Systemtype.Systemtypesmajorhardwarebuilds).Where(x => x.Deleted == false && x.Ismain)
                    .FirstOrDefaultAsync())?.Majorhardware?.Endofsupport;

                switch ((LcmEnum.SupportedResourceEnums)selectedIsHardwareSupported)
                {
                    case LcmEnum.SupportedResourceEnums.NoRule:
                        return Outputs.Empty;
                    case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:

                        if (!dataMajorHardwareBuildsEOS.HasValue && entity.Hardwareendofsupportcontract.HasValue)
                        {
                            return entity.Fullorpartialsupporthwid switch
                            {
                                1 => Outputs.yesFullExtendedMaintenance,
                                2 => Outputs.yesPartialExtendedMaintenance,
                                _ => Outputs.yesPartialExtendedMaintenance,
                            };
                        }
                        if (dataMajorHardwareBuildsEOS.HasValue && entity.Hardwareendofsupportcontract < dataMajorHardwareBuildsEOS)
                        {
                            return Outputs.yesCoveredByWarranty;
                        }
                        else
                        {
                            return entity.Fullorpartialsupporthwid switch
                            {
                                1 => Outputs.yesFullExtendedMaintenance,
                                2 => Outputs.yesPartialExtendedMaintenance,
                                _ => Outputs.yesPartialExtendedMaintenance,
                            };
                        }
                    //if (entity.Hardwareendofsupportcontract >= dataMajorHardwareBuildsEOS) return Outputs.yesFullExtendedMaintenance;
                    //break;

                    case LcmEnum.SupportedResourceEnums.AsThirdParty:
                        return entity.Sparesprovisioned ? Outputs.fullTpmContract : Outputs.partialTpmContract;
                    case LcmEnum.SupportedResourceEnums.AsNone:
                        if (entity.Renewalinprogress)
                        {
                            return Outputs.entityOutputToLcmHardware;
                        }
                        else
                        {
                            if (entity.Sparesprovisioned)
                            {
                                return Outputs.noVfInternalSupport;

                            }
                            else
                            {
                                return entity.Hardwaresupporttype switch
                                {
                                    "No(Hardware Refresh Late)" => Outputs.no,
                                    "No(Best Effort Contract Only)" => Outputs.no,
                                    "No(Other)" => Outputs.NoSparepartAvailiability,
                                    _ => Outputs.Empty,
                                };
                                //return Outputs.no;
                                //StringBuilder ss = new StringBuilder();
                                //if (entity.CheckboxResourceLcmEngineeringHardwares != null)
                                //    foreach (var checkbox in entity.CheckboxResourceLcmEngineeringHardwares)
                                //    {
                                //        if (checkbox != null)
                                //        {
                                //            var reason = _repositoryWrapper.ReasonCheckboxResource.FindByCondition(x => x.Id == checkbox.ReasonCheckboxResourceId).Single().Description;
                                //            ss.AppendJoin(',', reason);
                                //        }
                                //    }

                                //return string.IsNullOrEmpty(ss.ToString()) ? Outputs.no : $"No({ss.ToString()})";

                            }
                        }
                    // case LcmEnum.SupportedResourceEnums.AsNone:
                    default:
                        return Outputs.Empty;

                }
            }
            catch (Exception e)
            {
                return Outputs.Empty;
            }
            //throw new ArgumentOutOfRangeException();


            //return Outputs.Empty;
        }


        private static async Task<DateTime?> GetEndOfSupportContractLcmSoftwareReport(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var selectedIsSoftwareSupported = entity.Softwaresupportedid.HasValue
                ? (await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == entity.Softwaresupportedid).FirstOrDefaultAsync()).Rule
                : 0;
            //var dataMajorSoftwareBuildsEOS = (await _repositoryWrapper.DesignComponent
            //        .FindByCondition(x => x.DesignComponentId == entity.DesignComponentId).Include(x => x.SystemType).ThenInclude(x => x.MajorSoftwareBuilds).ThenInclude(x => x.OriginalEquipmentManufacturer).SingleAsync())
            //    .SystemType.MajorSoftwareBuilds.EndOfsupport;
            if (entity.Warranty) return entity.Softwareendofwarrantydate;

            switch ((LcmEnum.SupportedResourceEnums)selectedIsSoftwareSupported)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return entity.Softwareendofwarrantydate;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                case LcmEnum.SupportedResourceEnums.AsThirdParty:
                    return entity.Softwareendofsupportcontract;
                case LcmEnum.SupportedResourceEnums.AsNone:
                    return (DateTime?)null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task<DateTime?> GetEndOfSupportContractLcmSoftware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var selectedIsSoftwareSupported = entity.Softwaresupportedid.HasValue
                 ? (await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == entity.Softwaresupportedid).FirstOrDefaultAsync()).Rule
                 : 0;
            //var dataMajorSoftwareBuildsEOS = (await _repositoryWrapper.DesignComponent
            //        .FindByCondition(x => x.DesignComponentId == entity.DesignComponentId).Include(x => x.SystemType).ThenInclude(x => x.MajorSoftwareBuilds).ThenInclude(x => x.OriginalEquipmentManufacturer).SingleAsync())
            //    .SystemType.MajorSoftwareBuilds.EndOfsupport;
            if (entity.Warranty) return null;

            switch ((LcmEnum.SupportedResourceEnums)selectedIsSoftwareSupported)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return (DateTime?)null;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                case LcmEnum.SupportedResourceEnums.AsThirdParty:
                    return entity.Softwareendofsupportcontract;
                case LcmEnum.SupportedResourceEnums.AsNone:
                    return (DateTime?)null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task<string> GetSupportProviderLcmSoftware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var selectedIsSoftwareSupported = entity.Softwaresupportedid.HasValue
                          ? (await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == entity.Softwaresupportedid)
                          .FirstOrDefaultAsync()).Rule
                          : 0;

            var softwareOem = entity.Designcomponentid == 0 ? "" : (await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                                  .Include(x => x.Systemtype)
                                  .ThenInclude(x => x.Majorsoftwarebuilds)
                                  .ThenInclude(x => x.Orgeqpmanufacturer).FirstOrDefaultAsync())?.Systemtype
                              ?.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer ?? "";

            if (entity.Warranty) return softwareOem;
            switch ((LcmEnum.SupportedResourceEnums)selectedIsSoftwareSupported)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return entity.Softwaresupportprovider;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                    return softwareOem;
                case LcmEnum.SupportedResourceEnums.AsThirdParty:
                    return entity.Softwaresupportprovider;//todo se è null? secondo me none
                case LcmEnum.SupportedResourceEnums.AsNone:
                    return Outputs.none;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task<string> GetSupportTypeLcmSoftware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var selectedIsSoftwareSupported = entity.Softwaresupportedid.HasValue
                ? (await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == entity.Softwaresupportedid).SingleAsync()).Rule
                : 0;
            var dataMajorSoftwareBuildsEOS = entity.Designcomponentid == 0 ? null : (await _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid).Include(x => x.Systemtype)
                    .ThenInclude(x => x.Majorsoftwarebuilds)
                    .ThenInclude(x => x.Orgeqpmanufacturer).FirstOrDefaultAsync())
                .Systemtype.Majorsoftwarebuilds.Endofsupport;

            if (entity.Warranty) return Outputs.warrantyOutput;

            switch ((LcmEnum.SupportedResourceEnums)selectedIsSoftwareSupported)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return Outputs.no;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                    if (!dataMajorSoftwareBuildsEOS.HasValue) return Outputs.standard;
                    if (dataMajorSoftwareBuildsEOS.HasValue && entity.Softwareendofsupportcontract < dataMajorSoftwareBuildsEOS)
                    {
                        return Outputs.standard;
                    }

                    else
                    {
                        return entity.Fullorpartialsupportid switch
                        {
                            1 => Outputs.extended,
                            2 => Outputs.bestEffort,
                            _ => Outputs.bestEffort,
                        };
                    }
                case LcmEnum.SupportedResourceEnums.AsThirdParty:
                    return entity.Fullorpartialsupportid switch
                    {
                        1 => Outputs.standard,
                        2 => Outputs.bestEffort,
                        _ => Outputs.bestEffort,
                    };
                case LcmEnum.SupportedResourceEnums.AsNone:
                    List<string> ss = new List<string>();
                    if (entity.Reasoncheckboxresourcelcmengineeringsoftware != null)
                        foreach (var checkbox in entity.Reasoncheckboxresourcelcmengineeringsoftware)
                        {
                            if (checkbox != null)
                            {
                                var reason = _repositoryWrapper.ReasonCheckboxResource.FindByCondition(x => x.Id == checkbox.Reasoncheckboxresourceid).Single().Description;
                                ss.Add(reason);
                            }
                        }
                    return ss.Count > 0 ? $"No ({String.Join(',', ss)})" : Outputs.no;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task<DateTime?> GetEndOfWarrantyDateLcmSoftware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var selectedIsSoftwareSupported = entity.Softwaresupportedid.HasValue
                ? (await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == entity.Softwaresupportedid).FirstOrDefaultAsync()).Rule
                : 0;
            if (entity.Warranty) return entity.Softwareendofwarrantydate;

            switch ((LcmEnum.SupportedResourceEnums)selectedIsSoftwareSupported)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return entity.Softwareendofwarrantydate;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                case LcmEnum.SupportedResourceEnums.AsThirdParty:
                case LcmEnum.SupportedResourceEnums.AsNone:
                    return (DateTime?)null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task<bool> GetWarrantyLcmSoftware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var selectedIsSoftwareSupported = entity.Softwaresupportedid.HasValue
                ? (await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == entity.Softwaresupportedid).FirstOrDefaultAsync()).Rule
                : 0;
            if (entity.Warranty) return true;

            switch ((LcmEnum.SupportedResourceEnums)selectedIsSoftwareSupported)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return true;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                case LcmEnum.SupportedResourceEnums.AsThirdParty:
                case LcmEnum.SupportedResourceEnums.AsNone:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async static Task<T> GetRule<T>(this LcmEngineering entity, LCMEngineeringRulesSoftware rule)
        {
            T returnValue;
            switch (rule)
            {
                default: throw new NotImplementedException();
            }
            // return returnValue;
        }

        #region Implementazione delle regole
        private async static Task SetLCMSoftware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var selectedIsSoftwareSupported = entity.Softwaresupportedid.HasValue
                                                ? (await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == entity.Softwaresupportedid).SingleAsync()).Rule
                                                : 0;

            var dataMajorSoftwareBuildsEOS = entity.Designcomponentid == 0 ? null : (await _repositoryWrapper.DesignComponent
                     .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                     .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                     .ThenInclude(x => x.Orgeqpmanufacturer).FirstOrDefaultAsync())
                 .Systemtype.Majorsoftwarebuilds.Endofsupport;


            #region Software

            entity.Outputtolcmsoftware = await GetOutputLcmSoftware(entity, _repositoryWrapper);


            if (entity.Warranty)
            {
                entity.Softwaresupporttype = Outputs.warrantyOutput;
                entity.Softwaresupportprovider = entity.Designcomponentid == 0 ? null :
                    (await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                        .Include(x => x.Systemtype)
                        .ThenInclude(x => x.Majorsoftwarebuilds)
                        .ThenInclude(x => x.Orgeqpmanufacturer).SingleAsync())?.Systemtype
                    ?.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer ?? "";
                entity.Softwareendofsupportcontract = null;
            }
            else
            {
                switch ((LcmEnum.SupportedResourceEnums)selectedIsSoftwareSupported)
                {
                    case LcmEnum.SupportedResourceEnums.NoRule:
                        break;
                    case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:
                        if (entity.Softwareendofsupportcontract < dataMajorSoftwareBuildsEOS)
                        {
                            //entity.Warranty = true;
                            entity.Softwareendofwarrantydate = entity.Softwareendofwarrantydate;
                            entity.Softwaresupporttype = Outputs.standard;
                            entity.Softwaresupportprovider = entity.Designcomponentid == 0 ? null :
                                (await _repositoryWrapper.DesignComponent
                                    .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                                    .Include(x => x.Systemtype)
                                    .ThenInclude(x => x.Majorsoftwarebuilds)
                                    .ThenInclude(x => x.Orgeqpmanufacturer)
                                    .SingleAsync())?.Systemtype?.Majorsoftwarebuilds.Orgeqpmanufacturer
                                    .Originalequipmentmanufacturer ?? "";
                            entity.Softwareendofsupportcontract = entity.Softwareendofsupportcontract;
                        }
                        else
                        {
                            entity.Softwareendofwarrantydate = null;
                            entity.Softwaresupportprovider = entity.Designcomponentid == 0 ? null :
                                (await _repositoryWrapper.DesignComponent
                                    .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                                    .Include(x => x.Systemtype)
                                    .ThenInclude(x => x.Majorsoftwarebuilds)
                                    .ThenInclude(x => x.Orgeqpmanufacturer).SingleAsync())?.Systemtype?.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer ?? "";

                            entity.Softwaresupporttype = entity.Fullorpartialsupportid switch
                            {
                                1 => Outputs.extended,
                                2 => Outputs.bestEffort,
                                _ => entity.Softwaresupporttype
                            };

                        }
                        break;
                    case LcmEnum.SupportedResourceEnums.AsThirdParty:

                        //entity.Warranty = false;
                        entity.Softwareendofwarrantydate = null;

                        entity.Softwaresupportprovider = entity.Softwaresupportprovider;
                        entity.Softwareendofsupportcontract = entity.Softwareendofsupportcontract;

                        entity.Softwaresupporttype = entity.Fullorpartialsupportid switch
                        {
                            1 => Outputs.standard,
                            2 => Outputs.bestEffort,
                            _ => entity.Softwaresupporttype
                        };


                        break;
                    case LcmEnum.SupportedResourceEnums.AsNone:

                        List<string> ss = new List<string>();
                        if (entity.Reasoncheckboxresourcelcmengineeringsoftware != null)
                            foreach (var checkbox in entity.Reasoncheckboxresourcelcmengineeringsoftware)
                            {
                                if (checkbox != null)
                                {
                                    var reason = _repositoryWrapper.ReasonCheckboxResource.FindByCondition(x => x.Id == checkbox.Reasoncheckboxresourceid).Single().Description;
                                    ss.Append(reason);
                                }

                            }

                        //entity.Warranty = false;
                        entity.Softwareendofwarrantydate = null;
                        entity.Softwaresupportprovider = Outputs.none;
                        entity.Softwareendofsupportcontract = null;

                        if (ss.Count > 0)
                        {
                            entity.Softwaresupporttype = $"No ({String.Join(',', ss)})";
                        }
                        else
                        {
                            entity.Softwaresupporttype = Outputs.no;
                        }

                        break;
                }
            }

            //calcolo LCMStatusOPSSoftware

            entity.Lcmstatusopssoftware = GetLcmStatusOpsSoftware(entity, _repositoryWrapper);
            entity.Lcmstatusengsoftware = await GetLcmStatusEngSoftware(entity, _repositoryWrapper);
            //calcolo OutputToLCMSoftware alias LCM Status Software
            entity.Lcmstatussoftware = await GetNewLcmStatusSoftware(entity, _repositoryWrapper);

            #endregion
        }


        private static async Task<string> GetLcmStatusEngSoftware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var rtn = "";
            //calcolo LcmStatusEngSoftware
            var sw = entity.Designcomponentid == 0 ? null : (
                    await _repositoryWrapper.DesignComponent
                        .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                        .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                        .ThenInclude(x => x.Orgeqpmanufacturer)
                        .FirstOrDefaultAsync()
                )
                ?.Systemtype?.Majorsoftwarebuilds;

            var dataEom = sw?.Endofmaintenance;

            if (dataEom == null && sw?.Eomstatus == (short)EOMEnum.NotAnnounced)
            {
                rtn = Outputs.onSupport;
                return rtn;
            }
            if (dataEom != null)
            {
                if (dataEom > DateTime.Now.AddYears(1))
                {
                    rtn = Outputs.onSupport;
                }
                else if (dataEom > DateTime.Now && dataEom <= DateTime.Now.AddYears(1))
                {
                    rtn = Outputs.OnExpiration;
                }
                else if (dataEom < DateTime.Now && dataEom > DateTime.MinValue)
                {
                    rtn = Outputs.expired;
                }
                else
                {
                    rtn = (dataEom == null ? Outputs.engInfoMissing : "");
                }
            }
            else
            {
                rtn = Outputs.engInfoMissing;
            }

            return rtn;
        }


        private static string GetLcmStatusOpsSoftware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var outputtolcmsoftware = GetOutputLcmSoftware(entity, _repositoryWrapper).Result;
            if (string.IsNullOrEmpty(outputtolcmsoftware))
            {
                return Outputs.infoMissing;
            }
            else
            {
                if (outputtolcmsoftware == Outputs.partialTpmContract ||
                    outputtolcmsoftware == Outputs.no ||
                    outputtolcmsoftware == Outputs.noNoContractDueToVfDecision ||
                    outputtolcmsoftware == Outputs.entityOutputToLcmHardware ||
                    outputtolcmsoftware == Outputs.yesPartialExtendedMaintenance ||
                    outputtolcmsoftware == Outputs.NoSparepartAvailiability)
                {
                    return Outputs.expired;
                }
                else if (
                    outputtolcmsoftware == Outputs.yesCoveredByWarranty ||
                    outputtolcmsoftware == Outputs.yesFullExtendedMaintenance ||
                    outputtolcmsoftware == Outputs.fullTpmContract ||
                    outputtolcmsoftware == Outputs.noVfInternalSupport ||
                    outputtolcmsoftware == Outputs.noOpenSource)
                {
                    return Outputs.onSupport;
                }
            }
            return Outputs.infoMissing;
        }

        private async static Task SetLCMHardware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            #region Hardware

            var hw = entity.Designcomponentid == 0 ? null : (await _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                .Include(x => x.Systemtype)
                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware)
                .ThenInclude(s => s.Orgeqpmanufacturer).SingleAsync())
            .Systemtype.Systemtypesmajorhardwarebuilds.SingleOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware;

            var selectedIsHardwareSupported = entity.Hardwaresupportedid.HasValue ? (await _repositoryWrapper.SupportedResource
               .FindByCondition(x => x.Id == entity.Hardwaresupportedid).SingleAsync()).Rule : 0;

            switch (selectedIsHardwareSupported)
            {
                case 0:
                    entity.Outputtolcmhardware = Outputs.no;
                    break;
                case 1:
                    entity.Hardwaresupportprovider = hw?.Orgeqpmanufacturer?.Originalequipmentmanufacturer;
                    entity.Hardwareendofsupportcontract = entity.Hardwareendofsupportcontract;
                    entity.Hardwaresupporttype = hw?.Endofsupport > DateTime.Now ? Outputs.standard : Outputs.extended;
                    entity.Outputtolcmhardware = hw?.Endofsupport > entity.Hardwareendofsupportcontract ? Outputs.yes : Outputs.yesFullExtendedMaintenance;
                    break;
                case 2:
                    entity.Hardwaresupportprovider = entity.Hardwaresupportprovider;
                    entity.Hardwareendofsupportcontract = entity.Hardwareendofsupportcontract;
                    entity.Hardwaresupporttype = entity.Sparesprovisioned ? Outputs.standard : Outputs.bestEffort;
                    entity.Outputtolcmhardware = entity.Sparesprovisioned ? Outputs.fullTpmContract : Outputs.partialTpmContract;
                    break;
                case 3:
                    StringBuilder ss = new StringBuilder();
                    if (entity.Reasoncheckboxresourcelcmengineeringhardware != null)
                        foreach (var checkbox in entity.Reasoncheckboxresourcelcmengineeringhardware)
                        {
                            if (checkbox != null)
                            {
                                var reason = _repositoryWrapper.ReasonCheckboxResource.FindByCondition(x => x.Id == checkbox.Reasoncheckboxresourceid).Single().Description;
                                ss.AppendJoin(',', reason);
                            }
                        }
                    entity.Hardwareendofsupportcontract = null;
                    if (entity.Renewalinprogress)
                    {
                        entity.Hardwaresupporttype = string.IsNullOrEmpty(ss.ToString()) ? Outputs.no : $"No({ss.ToString()})";
                        entity.Hardwaresupportprovider = Outputs.vodafone;
                        entity.Outputtolcmhardware = Outputs.entityOutputToLcmHardware;
                    }
                    else
                    {
                        if (entity.Sparesprovisioned)
                        {
                            entity.Hardwaresupporttype = Outputs.sparesStock;
                            entity.Hardwaresupportprovider = Outputs.vodafone;
                            entity.Outputtolcmhardware = Outputs.sparePartsAvailability;
                        }
                        else
                        {
                            if (ss.ToString().ToUpper().Contains("Other no".ToUpper()))
                            {
                                entity.Hardwaresupportprovider = Outputs.none;
                            }
                            else
                            {
                                entity.Hardwaresupportprovider = Outputs.vodafone;
                            }
                            entity.Hardwaresupporttype = string.IsNullOrEmpty(ss.ToString()) ? Outputs.no : $"No({ss.ToString()})";
                            //mettere sempre no
                            entity.Outputtolcmhardware = string.IsNullOrEmpty(ss.ToString()) ? Outputs.no : $"No({ss.ToString()})";
                        }
                    }
                    break;
            }





            entity.Lcmstatusopshardware = GetLCMStatusOPSHardware(entity, _repositoryWrapper);




            entity.Lcmstatusenghardware = await GetLcmStatusEngHardware(entity, _repositoryWrapper);

            //calcolo  LCM Status Hardware
            entity.Lcmstatushardware = await GetNewLcmStatusHardware(entity, _repositoryWrapper);
            #endregion
        }

        private static async Task<string> GetNewLcmStatusHardware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var futureDate = DateTime.Today.AddYears(1);
            var outputtolcmhardware = GetOutputLcmHardware(entity, _repositoryWrapper).Result;
            // July 05 2024 - Runtime Error - Ticket 759 - Release 3.19.8 Demo comments
            var hw = entity.Designcomponentid == 0 ? null : (await _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware).ThenInclude(s => s.Orgeqpmanufacturer).FirstOrDefaultAsync())
                ?.Systemtype.Systemtypesmajorhardwarebuilds.SingleOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware;

            if (string.IsNullOrEmpty(outputtolcmhardware))
            {
                if (hw?.Endofmaintenance != null)
                {
                    return Outputs.opsMissing;
                }
                else
                {
                    if (hw?.Eomstatus == (short)EOMEnum.NotAnnounced)
                        return Outputs.opsMissing;

                    return Outputs.opsEngInfoMissing;
                }
            }
            else
            {
                if (hw?.Endofmaintenance == null && hw?.Eomstatus == (short)EOMEnum.NotSpecified)
                {
                    return Outputs.engInfoMissing;
                }
                else if (
                        ((hw?.Endofmaintenance == null && hw?.Eomstatus == (short)EOMEnum.NotAnnounced)
                      || (hw?.Endofmaintenance != null && hw?.Endofmaintenance > futureDate))
                  && (outputtolcmhardware == Outputs.yesCoveredByWarranty
                      || outputtolcmhardware == Outputs.noVfInternalSupport
                      || outputtolcmhardware == Outputs.fullTpmContract
                      || outputtolcmhardware == Outputs.yesFullExtendedMaintenance)
                      )
                {
                    return Outputs.onSupport;
                }
                else if ((hw?.Endofmaintenance != null && hw?.Endofmaintenance < DateTime.Now) &&
                  (outputtolcmhardware == Outputs.noVfInternalSupport
                    || outputtolcmhardware == Outputs.fullTpmContract
                    || outputtolcmhardware == Outputs.yesFullExtendedMaintenance)
                  )
                {
                    return Outputs.OnExpiration;
                }
                else if (
                    (hw?.Endofmaintenance != null && hw?.Endofmaintenance >= DateTime.Today && hw?.Endofmaintenance < futureDate)
                  && (outputtolcmhardware == Outputs.yesCoveredByWarranty
                      || outputtolcmhardware == Outputs.noVfInternalSupport
                      || outputtolcmhardware == Outputs.fullTpmContract
                      || outputtolcmhardware == Outputs.yesFullExtendedMaintenance)
                      )
                {
                    return Outputs.OnExpiration;
                }
                else if (
                    ((hw?.Endofmaintenance == null && hw?.Eomstatus == (short)EOMEnum.NotAnnounced)
                    || hw?.Endofmaintenance != null)
                    &&
                    outputtolcmhardware == Outputs.noOpenSource)
                {
                    return Outputs.onSupport;
                }
                else if (
                    ((hw?.Endofmaintenance == null && hw?.Eomstatus == (short)EOMEnum.NotAnnounced)
                    || hw?.Endofmaintenance != null)
                    &&
                        (outputtolcmhardware == Outputs.no ||
                        outputtolcmhardware == Outputs.noNoContractDueToVfDecision ||
                       outputtolcmhardware == Outputs.entityOutputToLcmHardware ||
                       outputtolcmhardware == Outputs.NoSparepartAvailiability)
                       )
                {
                    return Outputs.expired;
                }
                else if (
                     ((hw?.Endofmaintenance == null && hw?.Eomstatus == (short)EOMEnum.NotAnnounced)
                    || hw?.Endofmaintenance != null)
                    &&
                    (
                         outputtolcmhardware == Outputs.yesPartialExtendedMaintenance ||
                         outputtolcmhardware == Outputs.partialTpmContract)
                      )
                {
                    return Outputs.expired;
                }
                else { return Outputs.wrongCombination; }
            }

        }

        private static async Task<string> GetNewLcmStatusSoftware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var futureDate = DateTime.Today.AddYears(1);
            var outputtolcmsoftware = GetOutputLcmSoftware(entity, _repositoryWrapper).Result;
            var sw = entity.Designcomponentid == 0 ? null : (await _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                    .ThenInclude(x => x.Orgeqpmanufacturer).FirstOrDefaultAsync())
                ?.Systemtype.Majorsoftwarebuilds;

            if (string.IsNullOrEmpty(outputtolcmsoftware))
            {
                if (sw?.Endofmaintenance != null)
                {
                    return Outputs.opsMissing;
                }
                else
                {
                    if (sw?.Eomstatus == (short)EOMEnum.NotAnnounced)
                        return Outputs.opsMissing;

                    return Outputs.opsEngInfoMissing;
                }
            }
            else
            {
                if (sw?.Endofmaintenance == null && sw?.Eomstatus == (short)EOMEnum.NotSpecified)
                {
                    return Outputs.engInfoMissing;
                }
                else if (
                        ((sw?.Endofmaintenance == null && sw?.Eomstatus == (short)EOMEnum.NotAnnounced)
                      || (sw?.Endofmaintenance != null && sw?.Endofmaintenance > futureDate))
                  && (outputtolcmsoftware == Outputs.yesCoveredByWarranty
                      || outputtolcmsoftware == Outputs.noVfInternalSupport
                      || outputtolcmsoftware == Outputs.fullTpmContract
                      || outputtolcmsoftware == Outputs.yesFullExtendedMaintenance)
                      )
                {
                    return Outputs.onSupport;
                }
                else if ((sw?.Endofmaintenance != null && sw?.Endofmaintenance < DateTime.Now) &&
                    (outputtolcmsoftware == Outputs.noVfInternalSupport
                      || outputtolcmsoftware == Outputs.fullTpmContract
                      || outputtolcmsoftware == Outputs.yesFullExtendedMaintenance)
                    )
                {
                    return Outputs.OnExpiration;
                }
                else if (
                    (sw?.Endofmaintenance != null && sw?.Endofmaintenance >= DateTime.Today && sw?.Endofmaintenance < futureDate)
                  && (outputtolcmsoftware == Outputs.yesCoveredByWarranty
                      || outputtolcmsoftware == Outputs.noVfInternalSupport
                      || outputtolcmsoftware == Outputs.fullTpmContract
                      || outputtolcmsoftware == Outputs.yesFullExtendedMaintenance)
                      )
                {
                    return Outputs.OnExpiration;
                }
                else if (
                    ((sw?.Endofmaintenance == null && sw?.Eomstatus == (short)EOMEnum.NotAnnounced)
                    || sw?.Endofmaintenance != null)
                    &&
                    outputtolcmsoftware == Outputs.noOpenSource)
                {
                    return Outputs.onSupport;
                }
                else if (
                    ((sw?.Endofmaintenance == null && sw?.Eomstatus == (short)EOMEnum.NotAnnounced)
                    || sw?.Endofmaintenance != null)
                    &&
                        (outputtolcmsoftware == Outputs.no ||
                        outputtolcmsoftware == Outputs.noNoContractDueToVfDecision ||
                       outputtolcmsoftware == Outputs.entityOutputToLcmHardware ||
                       outputtolcmsoftware == Outputs.NoSparepartAvailiability)
                       )
                {
                    return Outputs.expired;
                }
                else if (
                     ((sw?.Endofmaintenance == null && sw?.Eomstatus == (short)EOMEnum.NotAnnounced)
                    || sw?.Endofmaintenance != null)
                    &&
                    (
                         outputtolcmsoftware == Outputs.yesPartialExtendedMaintenance ||
                         outputtolcmsoftware == Outputs.partialTpmContract)
                      )
                {
                    return Outputs.expired;
                }
                else { return Outputs.wrongCombination; }
            }

        }

        private static async Task<string> GetLcmStatusEngHardware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var rtn = "";
            var hw = entity.Designcomponentid == 0 ? null : (await _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid).Include(x => x.Systemtype)
                    .ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware).ThenInclude(s => s.Orgeqpmanufacturer).FirstOrDefaultAsync())
                ?.Systemtype.Systemtypesmajorhardwarebuilds.SingleOrDefault(x => x.Ismain && !x.Deleted.Value)?
                .Majorhardware;

            var dataEom = hw?.Endofmaintenance;

            if (dataEom == null && hw?.Eomstatus == (short)EOMEnum.NotAnnounced)
            {
                rtn = Outputs.onSupport;
                return rtn;
            }

            if (dataEom != null)
            {
                if (dataEom > DateTime.Now.AddYears(1))
                {
                    rtn = Outputs.onSupport;
                }
                else if (dataEom > DateTime.Now && dataEom <= DateTime.Now.AddYears(1))
                {
                    rtn = Outputs.OnExpiration;
                }
                else if (dataEom < DateTime.Now && dataEom > DateTime.MinValue)
                {
                    rtn = Outputs.expired;
                }
                else
                {
                    rtn = (dataEom == null ? Outputs.engInfoMissing : "");
                }
            }
            else
            {
                rtn = Outputs.engInfoMissing;
            }

            return rtn;
        }

        private static string GetLCMStatusOPSHardware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper)
        {
            var outputtolcmhardware = GetOutputLcmHardware(entity, _repositoryWrapper).Result;
            if (string.IsNullOrEmpty(outputtolcmhardware))
            {
                return Outputs.infoMissing;
            }
            else
            {
                if (outputtolcmhardware == Outputs.partialTpmContract ||
                    outputtolcmhardware == Outputs.no ||
                    outputtolcmhardware == Outputs.noNoContractDueToVfDecision ||
                    outputtolcmhardware == Outputs.entityOutputToLcmHardware ||
                    outputtolcmhardware == Outputs.yesPartialExtendedMaintenance ||
                    outputtolcmhardware == Outputs.NoSparepartAvailiability)
                {
                    return Outputs.expired;
                }
                else if (outputtolcmhardware == Outputs.yesCoveredByWarranty ||
                    outputtolcmhardware == Outputs.yesFullExtendedMaintenance ||
                    outputtolcmhardware == Outputs.fullTpmContract ||
                    outputtolcmhardware == Outputs.noVfInternalSupport ||
                    outputtolcmhardware == Outputs.noOpenSource)
                {
                    return Outputs.onSupport;
                }
            }
            return Outputs.infoMissing;
        }

        #endregion
        //Todo Verificare i testi.
        private static async Task<string> GetOutputLcmSoftware(Lcmengineering entity, IRepositoryWrapper _repositoryWrapper, bool isLcmDBExport = false)
        {

            var rule = entity.Softwaresupportedid.HasValue
                ? (await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == entity.Softwaresupportedid).SingleAsync()).Rule
                : 0;

            var dataMajorSoftwareBuildsEOS = entity.Designcomponentid == 0 ? null : (await _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                    .Include(x => x.Systemtype)
                    .ThenInclude(x => x.Majorsoftwarebuilds)
                    .ThenInclude(x => x.Orgeqpmanufacturer).FirstOrDefaultAsync())
                ?.Systemtype.Majorsoftwarebuilds.Endofsupport;


            if (entity.Warranty) return Outputs.yesCoveredByWarranty;
            switch ((LcmEnum.SupportedResourceEnums)rule)
            {
                case LcmEnum.SupportedResourceEnums.NoRule:
                    return Outputs.Empty;
                //break;
                case LcmEnum.SupportedResourceEnums.AsEquipmentManufacturer:

                    if (!dataMajorSoftwareBuildsEOS.HasValue && entity.Softwareendofsupportcontract.HasValue)
                    {
                        return entity.Fullorpartialsupportid switch
                        {
                            1 => Outputs.yesFullExtendedMaintenance,
                            2 => Outputs.yesPartialExtendedMaintenance,
                            _ => Outputs.yesPartialExtendedMaintenance,
                        };
                    }
                    if (entity.Softwareendofsupportcontract < dataMajorSoftwareBuildsEOS)
                    {

                        return Outputs.yesFullExtendedMaintenance;
                    }
                    else
                    {
                        return entity.Fullorpartialsupportid switch
                        {
                            1 => Outputs.yesFullExtendedMaintenance,
                            2 => Outputs.yesPartialExtendedMaintenance,
                            _ => Outputs.yesPartialExtendedMaintenance,
                        };
                    }
                //break;
                case LcmEnum.SupportedResourceEnums.AsThirdParty:

                    return entity.Fullorpartialsupportid switch
                    {
                        1 => Outputs.fullTpmContract,
                        2 => Outputs.partialTpmContract,
                        _ => Outputs.partialTpmContract
                    };
                //break;
                case LcmEnum.SupportedResourceEnums.AsNone:
                    {
                        if (isLcmDBExport)
                        {
                            return entity.Outputtolcmsoftware;
                        }
                        else
                        {
                            List<string> ss = new List<string>();
                            if (entity.Reasoncheckboxresourcelcmengineeringsoftware != null)
                                foreach (var checkbox in entity.Reasoncheckboxresourcelcmengineeringsoftware)
                                {
                                    if (checkbox != null)
                                    {
                                        var reason = _repositoryWrapper.ReasonCheckboxResource.FindByCondition(x => x.Id == checkbox.Reasoncheckboxresourceid).Single().Description;
                                        ss.Add(reason);
                                    }
                                }

                            if (ss.Count > 0)
                            {
                                //Se la selezione NON è multipla
                                return ss[0] switch
                                {
                                    Outputs.pendingOMRenewal => Outputs.entityOutputToLcmHardware,
                                    Outputs.openSourceSw => Outputs.noOpenSource,
                                    Outputs.itCanBeSupportedInternally => Outputs.noVfInternalSupport,
                                    Outputs.vodafoneDecisionCloseToDecommissioningRiskToCostRatioAccepted => Outputs.noNoContractDueToVfDecision,
                                    Outputs.other => Outputs.no,
                                    _ => Outputs.Empty
                                };
                            }

                            ////Se la selezione è multipla va gestita anche rispetto alle descrizione delle costanti
                            //entity.OutputToLCMSoftware = $"No (string.Join(',',
                            //    ss.Select(x =>
                            //                    x switch
                            //                    {
                            //                        Outputs.pendingOMRenewal => Outputs.entityOutputToLcmHardware,
                            //                        Outputs.openSourceSw => Outputs.noOpenSource,
                            //                        Outputs.itCanBeSupportedInternally => Outputs.noVfInternalSupport,
                            //                        Outputs.vodafoneDecisionCloseToDecommissioningRiskToCostRatioAccepted => Outputs.noNoContractDueToVfDecision,
                            //                        Outputs.other => Outputs.no,
                            //                        _ => entity.OutputToLCMSoftware
                            //                    }
                            //              )
                            //    );
                        }

                        return Outputs.Empty;
                    }
            }
            return Outputs.Empty;
        }

        public static async Task<string> GetAssetOutOfScop(this Lcmengineering entity, IRepositoryWrapper _repositoryWrapper, string projectStatus, int noOfNodes, Plannedactivities plannedActivity)
        {
            if (entity != null)
            {
                var opco = _repositoryWrapper.OpCo.FindByCondition(x => x.Opcoid == entity.Opcoid).FirstOrDefault();
                var assets = entity.Designcomponentid == 0 ? null : _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == entity.Opcoid && x.Designcomponentid == entity.Designcomponentid)
                               .Include(x => x.Deploymentstatus)
                               .Include(x => x.Plannedactivities).ThenInclude(x => x.Plannedactivityresource);
                if (assets != null)
                {
                    var decommissioning = !assets.Any(x => x.Deploymentstatus.Deploymentstatus.ToLower().Replace(" ", "") != "Decommissioning");

                    var deliveryStatus = _repositoryWrapper.DeliveryStatus.FindByCondition(x => plannedActivity != null && x.Deliverystatusid == plannedActivity.Deliverystatusid).FirstOrDefault();
                    if (projectStatus == Outputs.ProjectCompleted &&
                        noOfNodes == 0 &&
                       deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") == "Rollout Complete".ToLower().Replace(" ", ""))
                    {
                        return Outputs.ActionCompleted;
                    }
                    else if (projectStatus != Outputs.ProjectCompleted &&
                        opco?.Opco.ToUpper() == "DE" &&
                        noOfNodes == 0 &&
                       deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != "Rollout Complete".ToLower().Replace(" ", ""))
                    {
                        return Outputs.AssetPlannedToBeInserted;
                    }
                    else if (projectStatus != Outputs.ProjectCompleted &&
                        opco?.Opco.ToUpper() == "DE")
                    {
                        return Outputs.DeAssetManagedByDELocal;
                    }
                    else if (projectStatus != Outputs.ProjectCompleted &&
                        opco?.Opco.ToUpper() != "DE" &&
                         noOfNodes > 0 &&
                        assets.Any(x => x.Deploymentstatus.Deploymentstatus.ToLower().Replace(" ", "") == "In-Service".ToLower().Replace(" ", ""))
                        )
                    {
                        return Outputs.InScope;
                    }
                    else if (projectStatus != Outputs.ProjectCompleted &&
                        opco?.Opco.ToUpper() != "DE" &&
                         noOfNodes > 0 &&
                        decommissioning
                        )
                    {
                        return Outputs.NoReplacement;
                    }
                }

            }

            return "";
        }

        //LCM R8 Phase 1  
        public static string GetEngKpi2(string lcmStatusEng, string OpsMainttenanceContract)
        {
            if (lcmStatusEng == Outputs.onSupport || lcmStatusEng == Outputs.OnExpiration ||
                 (lcmStatusEng == Outputs.expired && (OpsMainttenanceContract == Outputs.yesFullExtendedMaintenance || OpsMainttenanceContract == Outputs.fullTpmContract)))
            {
                return Outputs.onSupport;
            }
            else
            {
                return Outputs.expired;
            }
        }
        public static string GetRiskCluster(int? vodafoneId, IRepositoryWrapper repository)
        {
            var riskClusterId = repository.RiskClusterVodafoneNamesRepository.FindByCondition(x => x.Vodafonenameid == vodafoneId).FirstOrDefault()?.Riskclusterid;

            return (riskClusterId != null) ? repository.RiskClusterRepository.FindByCondition(x => x.Riskclusterid == riskClusterId).FirstOrDefault()?.Description : string.Empty;

        }
        public static string GetExpLCMstatusatendofFY24(string lcmStatus, string OpsMainttenanceContract, DateTime? OpsMainttenanceContractEndDate, DateTime? projectEndDate, string projectStatus)
        {
            var yearFromToday = DateTime.Now.AddYears(1);
            var date = new DateTime(yearFromToday.Year, 3, 31);

            if (OpsMainttenanceContract == Outputs.noVfInternalSupport ||
                lcmStatus == Outputs.onSupport ||
                (lcmStatus == Outputs.OnExpiration && OpsMainttenanceContractEndDate >= date) ||
                (lcmStatus == Outputs.OnExpiration && OpsMainttenanceContractEndDate < date && projectStatus == Outputs.Planned && projectEndDate <= date) ||
                (lcmStatus == Outputs.expired && projectStatus == Outputs.Planned && projectEndDate <= date))
            {
                return Outputs.onSupport;
            }
            else if ((lcmStatus == Outputs.OnExpiration && OpsMainttenanceContractEndDate < date && projectStatus == Outputs.Planned && projectEndDate > date) ||
                     (lcmStatus == Outputs.OnExpiration && OpsMainttenanceContractEndDate < date && projectStatus != Outputs.Planned) ||
                     (lcmStatus == Outputs.expired && projectStatus != Outputs.Planned))
            {
                return Outputs.expired;
            }
            else
            {
                return Outputs.Error;
            }
        }


        public static string GetIdentificationAction(string plannedActivityType)
        {
            if (plannedActivityType != null)
            {
                if (plannedActivityType.ToLower().Replace(" ", "") == "hardwareupgrade" ||
                        plannedActivityType.ToLower().Replace(" ", "") == "softwareupgrade")
                {
                    return "Yes";
                }
                else if (plannedActivityType.ToLower().Replace(" ", "").Contains("modernize") ||
                            plannedActivityType.ToLower().Replace(" ", "").Contains("replace"))
                {
                    return "In-Preparation";
                }
            }

            return "No";
        }
        public static string GetIdentificationActionForLcmExport(Plannedactivities paentity, DateTime? EndofMaintenance, IRepositoryWrapper repositoryWrapper)
        {
            string IdentifiedAction = string.Empty;
            var yearFromToday = DateTime.Now.AddYears(1);
            var endOfFinancialyear = new DateTime(yearFromToday.Year, 3, 31);
            DateTime targetDate = new DateTime(yearFromToday.Year, 6, 1);
            var noPaResources = "";

            if (paentity != null && paentity?.Planningactivitystatus == null)
            {
                noPaResources = repositoryWrapper.PlanningActivityStatus.FindByCondition(x => x.Planningactivitystatusid == paentity.Planningactivitystatusid
                                           && x.Planningactivitystatus.Replace(" ", "").ToLower() == ConstantValueFilter.paPlanningActivityStatus).FirstOrDefault()?.Planningactivitystatus;
            }
            else
            {
                noPaResources = paentity?.Planningactivitystatus.Planningactivitystatus.Trim().Replace(" ", "").ToLower() == ConstantValueFilter.paPlanningActivityStatus ?
                    paentity?.Planningactivitystatus.Planningactivitystatus : string.Empty;
            }

            if (!String.IsNullOrEmpty(noPaResources))
                return "No";

            if (paentity != null && paentity?.Planningactivitystatus?.Planningactivitystatus.ToLower() == "rejected")
                return "No";

            if ((EndofMaintenance != null) && ((DateTime)EndofMaintenance <= targetDate))
            {
                if (paentity != null)
                {
                    if (paentity.Plannedcompletion <= endOfFinancialyear)
                    {
                        IdentifiedAction = "Yes";
                    }
                    else
                    {
                        IdentifiedAction = "No";
                    }
                }
                else
                {
                    IdentifiedAction = "In Preparation";
                }
            }
            else
            {
                IdentifiedAction = "";
            }

            return IdentifiedAction;
        }


        public static string GetBundleBudget(string budgetTrackingId)
        {
            string bundleBudget = null;
            if (budgetTrackingId != null)
            {
                if (budgetTrackingId.StartsWith("1-"))
                {
                    bundleBudget = "Yes";
                }
                else if (budgetTrackingId.StartsWith("0-"))
                {
                    bundleBudget = "No";
                }
            }

            return bundleBudget;
        }

        public static string GetAssetStatus(string deploymentStatus)
        {
            string assetStatus = null;
            if (deploymentStatus != null)
            {
                if (deploymentStatus.ToLower().Replace(" ", "") == "planned")
                {
                    assetStatus = "New";
                }
                else if (deploymentStatus.ToLower().Replace(" ", "") == "in-service")
                {
                    assetStatus = "Upgraded";
                }
                else if (deploymentStatus.ToLower().Replace(" ", "") == "decommissioning")
                {
                    assetStatus = "Dismissed";
                }
            }
            return assetStatus;
        }

        public static string GetAssetStatusBasedOnOriginalHwAndSw(string LcmSpreadSheet)
        {
            string assetStatus = string.Empty;
            if (!string.IsNullOrEmpty(LcmSpreadSheet))
            {
                assetStatus = "New";
            }
            return assetStatus;
        }


        public static string GetWbsCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var wbsCode = code.Split("-");
                return (wbsCode != null && wbsCode.Count() > 1 ? wbsCode.Skip(1).Aggregate((a, b) => a + "-" + b) : null);
            }
            return null;
        }

        public static string GetNewOpsRiskEvaluationValue(string incidentClassValue, string occurenceProbabilityValue)
        {

            if (string.IsNullOrEmpty(incidentClassValue) || string.IsNullOrEmpty(occurenceProbabilityValue))
                return "";
            else
            {
                string mergeValue = (incidentClassValue + occurenceProbabilityValue).ToLower();

                Dictionary<string, string> newOpsRiskEvaluationDic = new Dictionary<string, string>
            {
            {"p0low", "High"},{"p0medium", "High"},{"p0high", "High"}
            ,{"p1low", "Medium"},{"p1medium", "Medium"},{"p1high", "High"}
            ,{"p2low", "Medium"},{"p2medium", "Medium"},{"p2high", "Medium"}
            ,{"p3low", "Low"},{"p3medium", "Medium"},{"p3high", "Medium"}
            ,{"p4low", "Low"},{"p4medium", "Low"},{"p4high", "Low"}
            ,{"p5low", "Low"},{"p5medium", "Low"},{"p5high", "Low"}
            ,{"p6low", "Low"},{"p6medium", "Low"},{"p6high", "Low"}

            };
                try
                {
                    return newOpsRiskEvaluationDic[mergeValue];
                }
                catch
                {
                    return "";
                }

            }
        }

        public static string GetSecurityRiskOverAllValue(string riskEffective, string riskPotential)
        {
            if (!string.IsNullOrEmpty(riskEffective))
                return riskEffective;
            else if (!string.IsNullOrEmpty(riskPotential))
                return riskPotential;
            else return "";
        }

        public static string GetExposedEdgeValue(bool? isExposeEdge)
        {
            if (isExposeEdge == true)
            {
                return ConstantValueFilter.yes;
            }
            else
            {
                return ConstantValueFilter.No;
            }
            ;
        }


        public static string GetRiskLevelValue(int vodafoneId, IRepositoryWrapper repository)
        {
            var riskClusterId = repository.RiskClusterVodafoneNamesRepository.FindByCondition(x => x.Vodafonenameid == vodafoneId).FirstOrDefault()?.Riskclusterid;

            var risklevel = (riskClusterId != null) ? repository.RiskClusterRepository.FindByCondition(x => x.Riskclusterid == riskClusterId).FirstOrDefault()?.Risklevel : string.Empty;

            return risklevel;
        }

        #region get Asset Deployment Status 
        public static IDictionary<short, string> GetAssetDeploymentStatusBasedOnParameter(string assetValue,
      IRepositoryWrapper _repositoryWrapper)
        {
            var assetDeploymentStatusDic = _repositoryWrapper.DeploymentStatus
                .FindByCondition(x => x.Deploymentstatus.ToLower().Replace(" ", "") == assetValue.ToLower())
                 .ToDictionary(x => (short)x.Deploymentstatusid,
                        x => x.Deploymentstatus);

            return assetDeploymentStatusDic;
        }
        #endregion

        #region // Dev 719 Regulatory Fields Changes

        public static Dictionary<string, bool> GetRegulatoryFieldsValue(bool? Ispecn = false, bool? Ispecs = false, bool? Isscf = false, bool? Isnof = false)
        {
            var reslut = new Dictionary<string, bool>();

            if (Ispecn != null && Ispecn.Value)
            {
                reslut.Add("PECN", Ispecn.Value);
            }
            if (Ispecs != null && Ispecs.Value)
            {
                reslut.Add("PECS", Ispecs.Value);
            }
            if (Isscf != null && Isscf.Value)
            {
                reslut.Add("SCF", Isscf.Value);
            }
            if (Isnof != null && Isnof.Value)
            {
                reslut.Add("NOF", Isnof.Value);
            }

            return reslut;
        }
        #endregion

        public static string GetIdentificationActionForTsr(Plannedactivities paentity, DateTime? EndofMaintenance)
        {
            string IdentifiedAction = string.Empty;
            var yearFromToday = DateTime.Now.AddYears(1);
            var endOfFinancialyear = new DateTime(yearFromToday.Year, 3, 31);
            DateTime targetDate = new DateTime(yearFromToday.Year, 6, 1);

            if (paentity == null)
                return "No";

            if (paentity != null)
            {
                IdentifiedAction = "Yes";
            }
            else
            {
                if (EndofMaintenance == null || (DateTime)EndofMaintenance <= targetDate)
                    IdentifiedAction = "In Preparation";
            }

            return IdentifiedAction;
        }

        public static string GetAssetStatus(DateTime? Eofsdate, int FinancialYear)
        {
            if(Eofsdate == null)
            {
                return Outputs.Empty;
            }
            var output = string.Empty;
            DateTime date = new DateTime(FinancialYear + 1, 3, 31);
            DateTime previousYear = new DateTime(FinancialYear, 3, 31);

            if (Eofsdate > date)
            {
                return Outputs.onSupport;
            }
            else if ((previousYear < Eofsdate) && (Eofsdate <= date))
            {
                return Outputs.OnExpiration;
            }
            else if (Eofsdate <= previousYear)
            {
                return Outputs.expired;
            }
            else
            {
                return Outputs.Empty;
            }

        }
        public static string GetEoslKpiFrozen(DateTime? Eofsdate, int FinancialYear, DateTime? plannedEnddate,string assetStatus, DateTime? eoxDate,bool isLcmatGlance = false)
        {
            DateTime date = new DateTime(FinancialYear + 1, 3, 31);
            DateTime previousYear = new DateTime(FinancialYear, 3, 31);
            if (Eofsdate == null)
                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.Empty;

            if (Eofsdate > date && assetStatus == Outputs.onSupport)
            {
                return isLcmatGlance ? ConstantValueFilter.Green : Outputs.Compliant;
            }
            else if (((previousYear < Eofsdate) && (Eofsdate <= date)) && (assetStatus == Outputs.OnExpiration))
            {
                return isLcmatGlance ? ConstantValueFilter.Green : Outputs.Compliant;
            }
            else if (Eofsdate <= previousYear && assetStatus == Outputs.expired)
            {
                if(plannedEnddate != null)
                {
                    if (eoxDate != null)
                    {
                        if (eoxDate > date)
                        {
                            return isLcmatGlance ? ConstantValueFilter.Green : Outputs.Compliant;
                        }
                        else
                        {
                            return isLcmatGlance ? ConstantValueFilter.Red : Outputs.NonCompliant;
                        }
                    }
                    else
                    {
                        return isLcmatGlance? ConstantValueFilter.Red: Outputs.NonCompliant;
                    }
                }
                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.NonCompliant;
            }
            else
            {
                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.Empty;
            }

        }
        public static string GetEoslKpiForeCast(DateTime? Eofsdate, int FinancialYear, DateTime? plannedEnddate, string assetStatus,DateTime? eoxDate, bool isLcmatGlance = false)
        {
            var output = string.Empty;
            DateTime date = new DateTime(FinancialYear + 1, 3, 31);
            DateTime previousYear = new DateTime(FinancialYear, 3, 31);
            if (Eofsdate == null)
                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.Empty;
            if (Eofsdate > date && assetStatus == Outputs.onSupport)
            {
                return isLcmatGlance? ConstantValueFilter.Green: Outputs.Compliant;
            }
            else if (((previousYear < Eofsdate) && (Eofsdate <= date)) && (assetStatus == Outputs.OnExpiration))
            {
                return isLcmatGlance ? ConstantValueFilter.Green : Outputs.Compliant;
            }
            else if (Eofsdate <= previousYear && assetStatus == Outputs.expired)
            {
                if (plannedEnddate != null)
                {
                    if (plannedEnddate.Value <= date)
                    {
                        return isLcmatGlance ? ConstantValueFilter.Green : Outputs.Compliant;
                    }
                    else if (plannedEnddate.Value > date)
                    {
                        if (eoxDate != null)
                        {
                            if (eoxDate > date)
                            {
                                return isLcmatGlance ? ConstantValueFilter.Green : Outputs.Compliant;
                            }
                            else
                            {
                                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.NonCompliant;
                            }
                        }
                        else
                        {
                            return isLcmatGlance ? ConstantValueFilter.Red : Outputs.NonCompliant;
                        }
                    }
                    else
                    {
                        return isLcmatGlance ? ConstantValueFilter.Red : Outputs.NonCompliant;
                    }
                }
                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.NonCompliant;
            }
            else
            {
                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.Empty;
            }

        }
        public static string GetEoslKpiTarget(DateTime? Eofsdate, int FinancialYear, DateTime? plannedEnddate, string assetStatus, DateTime? eoxDate, bool isLcmatGlance = false)
        {
            var output = string.Empty;
            DateTime date = new DateTime(FinancialYear + 1, 3, 31);
            DateTime previousYear = new DateTime(FinancialYear, 3, 31);
            if (Eofsdate == null)
                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.Empty;
            if (Eofsdate > date && assetStatus == Outputs.onSupport)
            {
                return isLcmatGlance ? ConstantValueFilter.Green : Outputs.Compliant;
            }
            else if (((previousYear < Eofsdate) && (Eofsdate <= date)) && (assetStatus == Outputs.OnExpiration))
            {
                return isLcmatGlance ? ConstantValueFilter.Green : Outputs.Compliant;
            }
            else if (Eofsdate <= previousYear && assetStatus == Outputs.expired)
            {
                if (plannedEnddate != null)
                {
                    if (plannedEnddate.Value <= date)
                    {
                        return isLcmatGlance ? ConstantValueFilter.Green : Outputs.Compliant;
                    }
                    else if (plannedEnddate.Value > date)
                    {
                        if (eoxDate != null) 
                        {
                            if (eoxDate > date)
                            { 
                                return isLcmatGlance ? ConstantValueFilter.Green : Outputs.Compliant;
                            }
                            else
                            {
                                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.NonCompliant;
                            }
                        }
                        else
                        {
                            return isLcmatGlance ? ConstantValueFilter.Red : Outputs.NonCompliant;
                        }
                    }
                    else
                    {
                        return isLcmatGlance ? ConstantValueFilter.Red : Outputs.NonCompliant;
                    }
                }
                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.NonCompliant;
            }
            else
            {
                return isLcmatGlance ? ConstantValueFilter.Red : Outputs.Empty;
            }

        }
    }
}
