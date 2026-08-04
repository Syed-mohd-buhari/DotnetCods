using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
namespace CAM.Entities.Mappers.Lookup
{
    public static class BundleUpgradeInitiativeMapper
    {
        public static BundleUpgradeInitiative GetBundleUpgradeInitiativeMapper(Bundleupgradeinitiatives BundleUpgradeInitiative)
        {
            if (BundleUpgradeInitiative == null)
                return null;
            return new BundleUpgradeInitiative()
            {
                BundleUpgradeInitiativeId = BundleUpgradeInitiative.Bundleupgradeinitiativeid,
                OEMCertifiedRelease = BundleUpgradeInitiative.Oemcertifiedrelease,
                CreationDate = BundleUpgradeInitiative.Creationdate,
                CreationUser = BundleUpgradeInitiative.Creationuser,
                ModificationDate = BundleUpgradeInitiative.Modificationdate,
                ModificationUser = BundleUpgradeInitiative.Modificationuser,
                Deleted = BundleUpgradeInitiative.Deleted.Value,
                DeletionDate = BundleUpgradeInitiative.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(BundleUpgradeInitiative.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(BundleUpgradeInitiative.ModificationuserNavigation),
                OriginalEquipmentManufacturerId = BundleUpgradeInitiative.Orgeqpmanufacturerid,
                Remarks = BundleUpgradeInitiative.Remarks,
                Spare1Json = BundleUpgradeInitiative.Spare1json,
                VerticalOwner = BundleUpgradeInitiative.Verticalowner,
                VNFType = BundleUpgradeInitiative.Vnftype,
                OriginalEquipmentManufacturer = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(BundleUpgradeInitiative.Orgeqpmanufacturer),
                
            };
        }
        public static Bundleupgradeinitiatives SetBundleUpgradeInitiativeMapper(BundleUpgradeInitiative BundleUpgradeInitiative)
        {
            return new Bundleupgradeinitiatives()
            {
                Bundleupgradeinitiativeid = BundleUpgradeInitiative.BundleUpgradeInitiativeId,
                Oemcertifiedrelease = BundleUpgradeInitiative.OEMCertifiedRelease,
                Creationdate = BundleUpgradeInitiative.CreationDate,
                Creationuser = BundleUpgradeInitiative.CreationUser,
                Modificationdate = BundleUpgradeInitiative.ModificationDate,
                Modificationuser = BundleUpgradeInitiative.ModificationUser,
                Deleted = BundleUpgradeInitiative.Deleted,
                Deletiondate = BundleUpgradeInitiative.DeletionDate,
                Orgeqpmanufacturerid  = BundleUpgradeInitiative.OriginalEquipmentManufacturerId,
                Remarks = BundleUpgradeInitiative.Remarks,
                Spare1json = BundleUpgradeInitiative.Spare1Json,
                Verticalowner = BundleUpgradeInitiative.VerticalOwner,
                Vnftype = BundleUpgradeInitiative.VNFType,
            };
        }
    }
}
