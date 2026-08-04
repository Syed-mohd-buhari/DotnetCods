using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public class ServicePlanMapper
    {
        public static ServicePlan Get(Serviceplan model, bool include = false)
        {
            if (model == null)
                return null;
            var result = new ServicePlan()
            {
                Serviceplanid = model.Serviceplanid,
                Servicemasterid = model.Servicemasterid,
                Plannedactivityid = model.Serviceplanid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation,include),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation,include),   
                Servicemaster = ServiceMasterMapper.Get(model.Servicemaster, include),
                Opco = OpCoMapper.GetOpCoMapper(model.Opco),

            };
            if (model.Plannedactivities != null)
            {
                foreach (Plannedactivities item in model.Plannedactivities)
                {
                    result.Plannedactivity.Add(PlannedActivityMapper.Get(item,false));
                }
            }

            if (model.Serviceplandcfmappings != null && model.Serviceplandcfmappings.Any() == true)
            {
                result.DesignContactList = model.Serviceplandcfmappings?.SelectMany(x => x.Dcf.Designcomponents?
                                           .SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
                                           .Where(x => x.Deleted == false)
                                           .Select(m => (int?)m.Designcontactid)))?.
                                           Distinct().ToList();

                var majorHardwareDesignContact = model.Serviceplandcfmappings?.SelectMany(x => x.Dcf.Designcomponents?
                                                .SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                                .SelectMany(m => m.Majorhardware?.Majorhwbuildsdesigncontacts.
                                                Where(x => x.Deleted == false)
                                                .Select(n => (int?)n.Designcontactid))?.
                                                Distinct().ToList()))?.ToList();

                if (result.DesignContactList?.Any() == true && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactList = result.DesignContactList.Union(majorHardwareDesignContact).ToList();
                else if (result.DesignContactList?.Any() == false && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactList = majorHardwareDesignContact;
            }
            return result;
        }

        public static Serviceplan Set(ServicePlan model)
        {
            return new Serviceplan()
            {

                Serviceplanid = model.Serviceplanid,
                Servicemasterid = model.Servicemasterid,
                Opcoid = model.Opcoid,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,

            };
        }
    }
}
