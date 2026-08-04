using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class PlannedActivityCategoryMapper
    {
        public static PlannedActivityCategory Get (Plannedactivitycategory model,bool include= true)
        {
            if (model == null)
                return null;
            var result = new PlannedActivityCategory()
            {
                Plannedactivitycategoryid = model.Plannedactivitycategoryid,
                Categorydescription =model.Categorydescription,
                CreationDate  =model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate =model.Modificationdate,
                ModificationUser =model.Modificationuser,
                Deleted =model.Deleted.Value,
                DeletionDate =model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };
            if (include && model.Plannedactivities != null)
            {
                foreach (var item in model.Plannedactivities)
                {
                    result.Plannedactivities.Add(PlannedActivityMapper.Get(item,false));
                }
            }
            return result;
        }
        public static Plannedactivitycategory Set(PlannedActivityCategory model)
        {
            return new Plannedactivitycategory()
            {
                Plannedactivitycategoryid = model.Plannedactivitycategoryid,
                Categorydescription = model.Categorydescription,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,  
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
               
       
            };
        }
    }
}
