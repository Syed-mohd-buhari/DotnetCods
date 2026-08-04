using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Enum;
using OracleModels.DBModels;
namespace CAM.Entities.Mappers.Entity
{
    public static class LCMEngineeringMapper
    {
        public static LcmEngineering GetLcmEngineeringMapper(Lcmengineering model, bool Include = true, PatBuildConstructionEnum? filter = null)
        {
            if (model == null)
            {
                return null;
            }

            LcmEngineering result = new()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                DesignComponentId = model.Designcomponentid,
                LCMDeploymentStatusId = model.Lcmdeploymentstatusid,
                FullorPartialSupportId = model.Fullorpartialsupportid,
                FullorPartialSupportHWId = model.Fullorpartialsupporthwid,
                HardwareEndOfSupportContract = model.Hardwareendofsupportcontract,
                HardwareSheetIndex = model.Hardwaresheetindex,
                HardwareSupportedId = model.Hardwaresupportedid,
                HardwareSupportProvider = model.Hardwaresupportprovider,
                HardwareSupportType = model.Hardwaresupporttype,
                LcmengineeringId = model.Lcmengineeringid,
                LcmStatusEngHardware = model.Lcmstatusenghardware,
                LcmStatusEngSoftware = model.Lcmstatusengsoftware,
                LcmStatusHardware = model.Lcmstatushardware,
                LCMStatusOPSHardware = model.Lcmstatusopshardware,
                LCMStatusOPSSoftware = model.Lcmstatusopssoftware,
                LcmStatusSoftware = model.Lcmstatussoftware,
                NumberOfNodes = model.Numberofnodes,
                NumberOfNodesInLab = model.Numberofnodesinlab,
                OnHardware = model.Onhardware,
                OnSoftware = model.Onsoftware,
                OpCoId = model.Opcoid,
                OutputToLCMHardware = model.Outputtolcmhardware,
                OutputToLCMSoftware = model.Outputtolcmsoftware,
                ProductImportanceId = model.Productimportanceid,
                RenewalInProgress = model.Renewalinprogress,
                SoftwareEndOfSupportContract = model.Softwareendofsupportcontract,
                SoftwareEndOfWarrantyDate = model.Softwareendofwarrantydate,
                SoftwareSheetIndex = model.Softwaresheetindex,
                SoftwareSupportedId = model.Softwaresupportedid,
                SoftwareSupportProvider = model.Softwaresupportprovider,
                SparesProvisioned = model.Sparesprovisioned,
                SoftwareSupportType = model.Softwaresupporttype,
                VendorEndOfMaintenanceDateHardware = model.Vendorendmntdatehw,
                VendorEndOfMaintenanceDateSoftware = model.Vendorendmntedatesw,
                Warranty = model.Warranty,
                DesignComponent = Include ? DesignComponentMapper.GetDesignComponentMapper(model.Designcomponent) : null,
                LCMDeploymentStatus = Include ? LCMDeploymentStatusMapper.GetLCMDeploymentStatusMapper(model.Lcmdeploymentstatus) : null,
                DesignComponentFamilyId = model.Designcomponentfamily == null ? (model.Designcomponent != null ? model.Designcomponent.Designcomponentfamilyid : model.Designcomponentfamilyid) : model.Designcomponentfamilyid,//model.Designcomponent != null ? model.Designcomponent.Designcomponentfamilyid : model.Designcomponentfamilyid,
                FullOrPartialResourceRel = FullOrPartialResourceMapper.GetFullOrPartialResourceMapper(model.Fullorpartialsupport),
                HardwareSupported = SupportedResourceMapper.GetSupportedResourceMapper(model.Hardwaresupported),
                OpCo = OpCoMapper.GetOpCoMapper(model.Opco),
                ProductImportanceRel = ProductImportanceMapper.GetProductImportanceMapper(model.Productimportance),
                SoftwareSupported = SupportedResourceMapper.GetSupportedResourceMapper(model.Softwaresupported),
                ElementCount = model.Elementcount,
                Order = filter != null ? SetLcmOrders(model, filter) : 1,
                Archived = model.Archived,
                VodafoneName = (model.Designcomponent != null && model.Designcomponent.Subnetworkboundary != null && model.Designcomponent.Subnetworkboundary.Vodafonename != null) ? model.Designcomponent.Subnetworkboundary.Vodafonename.Description : "",
                ResourceKey = model.Resourcekey,
                PreviousResourceKey = model.Previousresourcekey,
                IsExtendedSupportOfferedByVendor = model.Isextendedsupportofferedbyvendor,
                HwIsExtendedSupportOfferedByVendor = model.Hwisextendedsupportofferedbyvendor,
                Isreleasedetailunknown = model.Isreleasedetailunknown,
                Designcomponentfamily = Include ? DesignComponentFamilyMapper.Get(model.Designcomponentfamily) : null,
                BuildBagId = model.Buildbagid,
                BuildBag = Include ? BuildBagMapper.GetBuildBag(model.Buildbag) : null,
            };

            if (model.Reasoncheckboxresourcelcmengineeringhardware != null)
            {
                foreach (Reasoncheckboxresourcelcmengineeringhardware item in model.Reasoncheckboxresourcelcmengineeringhardware)
                {
                    result.CheckboxResourceLcmEngineeringHardwares.Add(ReasonCheckboxResourceLcmEngineeringHardwareMapper.Get(item));
                }
            }
            if (model.Reasoncheckboxresourcelcmengineeringsoftware != null)
            {
                foreach (Reasoncheckboxresourcelcmengineeringsoftware item in model.Reasoncheckboxresourcelcmengineeringsoftware)
                {
                    result.CheckboxResourceLcmEngineeringSoftwares.Add(ReasonCheckboxResourceLcmEngineeringSoftwareMapper.Get(item));
                }
            }
            if (model.Lcmengineeringeduspoc != null)
            {
                foreach (Lcmengineeringeduspoc item in model.Lcmengineeringeduspoc)
                {
                    result.LcmEngineeringEduSpoc.Add(LcmEngineeringEduSpocMapper.Get(item));
                }
            }
            if (model.Lcmengineeringsubdomainspoc != null)
            {
                foreach (Lcmengineeringsubdomainspoc item in model.Lcmengineeringsubdomainspoc)
                {
                    result.LcmEngineeringSubDomainSpoc.Add(LcmEngineeringSubDomainSpocMapper.Get(item));
                }
            }
            if (model.Lcmoperationalcontracts != null)
            {
                foreach (Lcmoperationalcontracts item in model.Lcmoperationalcontracts)
                {
                    result.LCMOperationContracts.Add(LCMOperationalContractsMapper.Get(item));
                }
            }
            return result;
        }
        public static Lcmengineering SetLcmEngineeringMapper(LcmEngineering model)
        {
            if (model == null)
            {
                return null;
            }

            Lcmengineering result = new()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Designcomponentid = model.DesignComponentId,
                Lcmdeploymentstatusid = model.LCMDeploymentStatusId,
                Fullorpartialsupportid = model.FullorPartialSupportId,
                Fullorpartialsupporthwid = model.FullorPartialSupportHWId,
                Hardwareendofsupportcontract = model.HardwareEndOfSupportContract,
                Hardwaresheetindex = model.HardwareSheetIndex,
                Hardwaresupportedid = model.HardwareSupportedId,
                Hardwaresupportprovider = model.HardwareSupportProvider,
                Hardwaresupporttype = model.HardwareSupportType,
                Lcmengineeringid = model.LcmengineeringId,
                Lcmstatusenghardware = model.LcmStatusEngHardware,
                Lcmstatusengsoftware = model.LcmStatusEngSoftware,
                Lcmstatushardware = model.LcmStatusHardware,
                Lcmstatusopshardware = model.LCMStatusOPSHardware,
                Lcmstatusopssoftware = model.LCMStatusOPSSoftware,
                Lcmstatussoftware = model.LcmStatusSoftware,
                Numberofnodes = model.NumberOfNodes,
                Numberofnodesinlab = model.NumberOfNodesInLab,
                Onhardware = model.OnHardware,
                Onsoftware = model.OnSoftware,
                Opcoid = model.OpCoId,
                Outputtolcmhardware = model.OutputToLCMHardware,
                Outputtolcmsoftware = model.OutputToLCMSoftware,
                Productimportanceid = model.ProductImportanceId,
                Renewalinprogress = model.RenewalInProgress,
                Softwareendofsupportcontract = model.SoftwareEndOfSupportContract,
                Softwareendofwarrantydate = model.SoftwareEndOfWarrantyDate,
                Softwaresheetindex = model.SoftwareSheetIndex,
                Softwaresupportedid = model.SoftwareSupportedId,
                Softwaresupportprovider = model.SoftwareSupportProvider,
                Sparesprovisioned = model.SparesProvisioned,
                Softwaresupporttype = model.SoftwareSupportType,
                Vendorendmntdatehw = model.VendorEndOfMaintenanceDateHardware,
                Vendorendmntedatesw = model.VendorEndOfMaintenanceDateSoftware,
                Warranty = model.Warranty,
                Designcomponent = DesignComponentMapper.SetDesignComponentMapper(model.DesignComponent),
                Lcmdeploymentstatus = LCMDeploymentStatusMapper.SetLCMDeploymentStatusMapper(model.LCMDeploymentStatus),
                Fullorpartialsupport = FullOrPartialResourceMapper.SetfullorpartialresourceMapper(model.FullOrPartialResourceRel),
                Hardwaresupported = SupportedResourceMapper.SetSupportedResourceMapper(model.HardwareSupported),
                Opco = OpCoMapper.SetOpCoMapper(model.OpCo),
                Productimportance = ProductImportanceMapper.SetProductImportanceMapper(model.ProductImportanceRel),
                Softwaresupported = SupportedResourceMapper.SetSupportedResourceMapper(model.SoftwareSupported),
                Elementcount = model.ElementCount,
                Archived = model.Archived,
                Isextendedsupportofferedbyvendor = model.IsExtendedSupportOfferedByVendor,
                Resourcekey = model.ResourceKey,
                Previousresourcekey = model.PreviousResourceKey,
                Hwisextendedsupportofferedbyvendor = model.HwIsExtendedSupportOfferedByVendor,
                Isreleasedetailunknown = model.Isreleasedetailunknown,
                Designcomponentfamilyid = model.DesignComponentFamilyId,
                Designcomponentfamily = DesignComponentFamilyMapper.Set(model.Designcomponentfamily),
                Buildbagid = model.BuildBagId
            };

            if (model.PlannedActivities != null)
            {
                foreach (PlannedActivity item in model.PlannedActivities)
                {
                    result.PlannedactivitiesLcmengineering.Add(PlannedActivityMapper.Set(item));
                }
            }
            if (model.CheckboxResourceLcmEngineeringHardwares != null)
            {
                foreach (Models.Cross.ReasonCheckboxResourceLcmEngineeringHardware item in model.CheckboxResourceLcmEngineeringHardwares)
                {
                    result.Reasoncheckboxresourcelcmengineeringhardware.Add(ReasonCheckboxResourceLcmEngineeringHardwareMapper.Set(item));
                }
            }
            if (model.CheckboxResourceLcmEngineeringSoftwares != null)
            {
                foreach (Models.Cross.ReasonCheckboxResourceLcmEngineeringSoftware item in model.CheckboxResourceLcmEngineeringSoftwares)
                {
                    result.Reasoncheckboxresourcelcmengineeringsoftware.Add(ReasonCheckboxResourceLcmEngineeringSoftwareMapper.Set(item));
                }
            }
            if (model.LcmEngineeringEduSpoc != null)
            {
                foreach (Models.Cross.LcmEngineeringEduSpoc item in model.LcmEngineeringEduSpoc)
                {
                    result.Lcmengineeringeduspoc.Add(LcmEngineeringEduSpocMapper.Set(item));
                }
            }
            if (model.LcmEngineeringSubDomainSpoc != null)
            {
                foreach (Models.Cross.LcmEngineeringSubDomainSpoc item in model.LcmEngineeringSubDomainSpoc)
                {
                    result.Lcmengineeringsubdomainspoc.Add(LcmEngineeringSubDomainSpocMapper.Set(item));
                }
            }
            if (model.LCMOperationContracts != null)
            {
                foreach (Models.Cross.LcmOperationalContracts item in model.LCMOperationContracts)
                {
                    result.Lcmoperationalcontracts.Add(LCMOperationalContractsMapper.Set(item));
                }
            }
            return result;
        }

        private static int SetLcmOrders(Lcmengineering model, PatBuildConstructionEnum? filter)
        {
            int order = filter == PatBuildConstructionEnum.EricssonVirtualizedBundles ?
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Isvmware == true &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "bundle" ? 1
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "ericsson" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "vnfm" ? 2
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "ericsson" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") != "vnfm" ? 3
                         : 4

                         : filter == PatBuildConstructionEnum.HuwaeiVirtualizedBundles ?
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Isvmware == true &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "bundle" ? 1
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "huawei" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "vnfm" ? 2
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "huawei" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") != "vnfm" ? 3
                         : 4

                         : filter == PatBuildConstructionEnum.NokiaVirtualizedBundles ?

                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Isvmware == true &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "bundle" ? 1
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "nokia" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") == "vnfm" ? 2
                         :
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Replace(" ", "") == "nokia" &&
                         model.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description.ToLower().Replace(" ", "") != "vnfm" ? 3
                         : 4
                         :
                         1;

            return order;
        }

        public static LcmEngineering GetLcmEngineeringDeployStatusMapper(Lcmengineering model)
        {
            if (model == null)
            {
                return null;
            }

            LcmEngineering result = new()
            {
                LCMDeploymentStatusId = model.Lcmdeploymentstatusid,
                LcmengineeringId = model.Lcmengineeringid,
                LCMDeploymentStatusValue = model.Lcmdeploymentstatus.Description,
                OpCoId = model.Opcoid
            };

            return result;
        }
    }
}
