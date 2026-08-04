using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;


namespace CAM.Entities.Mappers.Entity
{
    public static class NfviSoftwareCompatibilityMapper
    {
        public static NfviSoftwareCompatibility GetNfviSoftwareCompatibilityMapper(Nfvisoftwarecompatibility model)
        {
            if (model == null)
                return null;
            return new NfviSoftwareCompatibility()
            {
                NfviSoftwareCompatibilityId = model.Nfvisoftwarecompatibilityid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                MinimumSupportedVersion = model.Minimumsupportedversion,
                PlaftFormId = model.Plaftformid,
                ProductId = model.Productid,
                VendorId = model.Vendorid,
                Vendor = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(model.Vendor),
                ProductName = ProductNameMapper.GetProductNameMapper(model.Product),
                MajorSoftwareVmwarePlaftForm = MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(model.Plaftform),

            };
        }
        public static Nfvisoftwarecompatibility SetNfviSoftwareCompatibilityMapper(NfviSoftwareCompatibility model)
        {
            return new Nfvisoftwarecompatibility()
            {
                Nfvisoftwarecompatibilityid = model.NfviSoftwareCompatibilityId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Minimumsupportedversion = model.MinimumSupportedVersion,
                Plaftformid = model.PlaftFormId,
                Productid = model.ProductId,
                Vendorid = model.VendorId

            };
        }
    }
}
