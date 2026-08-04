using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
namespace CAM.Entities.Mappers.Cross
{
    public static class PlannedActivityResourceDriverMapper
    {
        public static PlannedActivityResourceDriver Get(Plannedactivityresourcedriver model)
        {

            if (model == null)
                return null;
            return new PlannedActivityResourceDriver()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                PlannedActivityResourceId = model.Plannedactivityresourceid,
               // PlannedActivityResource = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(model.Plannedactivityresource),
                DriverId = model.Driverid,
                ForLcm = model.Forlcm,
                PlannedActivityResourceDriverId = model.Planactivityresdriverid,
                Driver = DriverMapper.GetDriverMapper(model.Driver),
                ForDesignAspect = model.Fordesignaspect,
                ForAddAsset = model.Foraddasset,
                ForEditAsset = model.Foreditasset
            };
        }

        public static Plannedactivityresourcedriver Set(PlannedActivityResourceDriver model)
        {
            return new Plannedactivityresourcedriver()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Plannedactivityresourceid = model.PlannedActivityResourceId,
                Driverid = model.DriverId,
                Forlcm = model.ForLcm,
                Planactivityresdriverid = model.PlannedActivityResourceDriverId,
                Fordesignaspect = model.ForDesignAspect,
                Foraddasset = model.ForAddAsset,
                Foreditasset = model.ForEditAsset

            };
        }
    }
}
