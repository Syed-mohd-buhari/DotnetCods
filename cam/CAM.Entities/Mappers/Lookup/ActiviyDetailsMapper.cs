using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class ActiviyDetailsMapper
    {
        public static ActivityDetails GetActivityDetailsMapper (Activitydetails activitydetails)
        {
            if (activitydetails == null)
                return null;
            return new ActivityDetails()
            {
                ActivityDetailsId = activitydetails.Activitydetailsid,
                ActivityDetailsDescription =activitydetails.Description,
                CreationDate  =activitydetails.Creationdate,
                CreationUser = activitydetails.Creationuser,
                ModificationDate =activitydetails.Modificationdate,
                ModificationUser =activitydetails.Modificationuser,
                Deleted =activitydetails.Deleted.Value,
                DeletionDate =activitydetails.Deletiondate,
                ForVirtualized =activitydetails.Forvirtualized,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(activitydetails.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(activitydetails.ModificationuserNavigation),
            };
        }
        public static Activitydetails SetActivityDetailsMapper(ActivityDetails activitydetails)
        {
            return new Activitydetails()
            {
                Activitydetailsid = activitydetails.ActivityDetailsId,
                Description = activitydetails.ActivityDetailsDescription,
                Creationdate = activitydetails.CreationDate,
                Creationuser = activitydetails.CreationUser,
                Modificationdate = activitydetails.ModificationDate,
                Modificationuser = activitydetails.ModificationUser,  
                Deleted = activitydetails.Deleted,
                Deletiondate = activitydetails.DeletionDate,
                Forvirtualized = activitydetails.ForVirtualized,
       
            };
        }
    }
}
