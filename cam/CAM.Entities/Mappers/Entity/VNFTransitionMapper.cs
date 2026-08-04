using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Entity
{
    public static class VNFTransitionMapper
    {
        public static VNFTransition  Get(Vnftransitions model)
        {
            if (model == null)
                return null;
            return new VNFTransition()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                VNFTransitionId = model.Vnftransitionid,
                CurrentRelease = model.Currentrelease,
                EquipmentStatusId = model.Equipmentstatusid,
                NFVIBundleIDId = model.Nfvibundleidid,
                ElementName = model.Elementname,
                Location = model.Location,
                NFVISiteDesignation = model.Nfvisitedesignation,
                OpCoId = model.Opcoid,
                PlannedRelease =model.Plannedrelease,
                Spare1Json = model.Spare1json,
                VNFDesignComponentId = model.Vnfdesigncomponentid,
                VNFType =model.Vnftype,
                
                EquipmentStatus = EquipmentStatusMapper.GetEquipmentStatusMapper(model.Equipmentstatus),
                NFVIBundleID = NFVIBundleIDMapper.Get(model.Nfvibundleid),
                OpCo = OpCoMapper.GetOpCoMapper(model.Opco),
                VNFDesignComponent = VNFDesignComponentMapper.GetVNFDesignComponentMapper(model.Vnfdesigncomponent),
                

            };
        }
        public static Vnftransitions Set(VNFTransition model)
        {
            return new Vnftransitions()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Vnftransitionid = model.VNFTransitionId,
                Currentrelease = model.CurrentRelease,
                Equipmentstatusid = model.EquipmentStatusId,
                Nfvibundleidid = model.NFVIBundleIDId,
                Elementname = model.ElementName,
                Location = model.Location,
                Nfvisitedesignation = model.NFVISiteDesignation,
                Opcoid = model.OpCoId,
                Plannedrelease = model.PlannedRelease,
                Spare1json = model.Spare1Json,
                Vnfdesigncomponentid = model.VNFDesignComponentId,
                Vnftype = model.VNFType,
            };
        }
    }
}
