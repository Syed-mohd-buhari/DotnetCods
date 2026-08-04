using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class NetworkElementAsIsMapper
    {
        public static NetworkElementAsIs Get(Networkelementsasis model)
        {
            if (model == null)
                return null;
            var rtn= new NetworkElementAsIs()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                
                DataAcquisitionDate = model.Dataacquisitiondate,
                DataAcquisitionMethod = model.Dataacquisitionmethod,
                ElementDeploymentName = model.Elementdeploymentname,
                ElementManager = model.Elementmanager,
                ElementManagerExportFileFormat = model.Elementmanagerexportfileformat,
                HardwareAcquisition = model.Hardwareacquisition,
                ManualOverride = model.Manualoverride,
                NetworkElementAsPlannedId = model.Networkelementasplannedid,
                NodeType = model.Nodetype,
                LocationId = model.Locationid,
                PatchDetails = model.Patchdetails,
                NetworkElementAsIsId = model.Networkelementasisid,
                SoftwareInstallDate = model.Softwareinstalldate,
                OpCoId = model.Opcoid,
                OriginalEquipmentManufacturerId = model.Orgeqpmanufacturerid,
                SoftwareProductionDate = model.Softwareproductiondate,
                SoftwareProductNumber = model.Softwareproductnumber,
                SystemTypeId =model.Systemtypeid,
                NetworkElementAsPlanned = NetworkElementAsPlannedMapper.Get(model.Networkelementasplanned),
                OriginalEquipmentManufacturer = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(model.Orgeqpmanufacturer),
                SystemType = SystemTypeMapper.GetSystemTypeMapper(model.Systemtype),
                Location = LocationMapper.GetLocationMapper(model.Location),
                OpCo = OpCoMapper.GetOpCoMapper(model.Opco),                
                HardwareInstallDate=model.Hardwareinstalldate,
                PlatformType = model.Platformtype,
                HardwareType = model.Hardwaretype,
                SoftwareReleaseInformation = model.Softwarereleaseinformation

            };

            if(model.Networkelementasplanned?.Networkelementasplannedsubdomainspoc!=null && model.Networkelementasplanned?.Networkelementasplannedsubdomainspoc.Count>0)
                rtn.NetworkElementAsPlannedSubdomainSpoc= model.Networkelementasplanned?.Networkelementasplannedsubdomainspoc.Select(x=>x.Subdomainspocid).ToList();
            return rtn;
        }

        public static Networkelementsasis Set(NetworkElementAsIs model)
        {
            if (model == null)
                return null;
            return new Networkelementsasis()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,

                Dataacquisitiondate = model.DataAcquisitionDate,
                Dataacquisitionmethod = model.DataAcquisitionMethod,
                Elementdeploymentname = model.ElementDeploymentName,
                Elementmanager = model.ElementManager,
                Elementmanagerexportfileformat = model.ElementManagerExportFileFormat,
                Hardwareacquisition = model.HardwareAcquisition,
                Manualoverride = model.ManualOverride,
                Networkelementasplannedid = model.NetworkElementAsPlannedId,
                Nodetype = model.NodeType,
                Locationid = model.LocationId,
                Patchdetails = model.PatchDetails,
                Networkelementasisid = model.NetworkElementAsIsId,
                Softwareinstalldate = model.SoftwareInstallDate,
                Opcoid = model.OpCoId,
                Orgeqpmanufacturerid = model.OriginalEquipmentManufacturerId,
                Softwareproductiondate = model.SoftwareProductionDate,
                Softwareproductnumber = model.SoftwareProductNumber,
                Systemtypeid = model.SystemTypeId,
                Hardwareinstalldate = model.HardwareInstallDate,
                Platformtype = model.PlatformType,
                Hardwaretype = model.HardwareType,
                Softwarereleaseinformation = model.SoftwareReleaseInformation,
            };
        }
    }
}
