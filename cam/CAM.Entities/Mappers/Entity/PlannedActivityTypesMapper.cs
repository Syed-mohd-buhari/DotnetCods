using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Mappers.Entity
{
    public static class PlannedActivityTypesMapper
    {
        public static Models.PlannedActivityTypes GetPlannedActivityTypesRules(Plannedactivitytypes model)
        {
            if (model == null)
                return null;
            return new Models.PlannedActivityTypes()
            {
                PlannedActivityTypesId = model.Plannedactivitytypesid,
                PlannedActivityTypeDescription = model.Plannedactivitytypedescription,
                HwOem = model.Hwoem,
                HwSolution = model.Hwsolution,
                HwPlatform = model.Hwplatform,
                SwOem = model.Swoem,
                SwVersion = model.Swversion,
                SwProductname = model.Swproductname,
                SubNetworkService = model.Subnetworkservice,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                DeletionDate = model.Deletiondate,
                LinkedDcRule = model.Linkeddcrule,
                Deleted = model.Deleted.Value,
                ForLcm = model.Forlcm,
                ForAsset = model.Forasset,
                ForDesignAspect = model.Fordesignaspect,
                Forservice = model.Forservice,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)

            };
        }
        public static Plannedactivitytypes SetPlannedActivityTypesRules(PlannedActivityTypes model)
        {
            if (model == null)
                return null;
            return new Plannedactivitytypes()
            {
                Plannedactivitytypesid = model.PlannedActivityTypesId,
                Plannedactivitytypedescription = model.PlannedActivityTypeDescription,
                Hwoem = model.HwOem,
                Hwsolution = model.HwSolution,
                Hwplatform = model.HwPlatform,
                Swoem = model.SwOem,
                Swproductname = model.SwProductname,
                Swversion = model.SwVersion,
                Subnetworkservice = model.SubNetworkService,
                Linkeddcrule = model.LinkedDcRule,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Forlcm = model.ForLcm,
                Forasset = model.ForAsset,
                Fordesignaspect = model.ForDesignAspect,
                Forservice = model.Forservice,
                
            };
        }
    }
}
