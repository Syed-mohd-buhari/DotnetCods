using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public class ServicePlanDcfMappingMapper
    {
        public static ServicePlanDcfMapping Get(Serviceplandcfmappings model, bool include = false)
        {
            if (model == null)
                return null;
            var result = new ServicePlanDcfMapping()
            {
                Serviceplanid = model.Serviceplanid,
                Dcfid = model.Dcfid,
                Status = model.Status,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation, include),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation, include),
                Dcf = DesignComponentFamilyMapper.Get(model.Dcf, include),
                Serviceplan = ServicePlanMapper.Get(model.Serviceplan, include),
            };
            return result;
        }

        public static Serviceplandcfmappings Set(ServicePlanDcfMapping model)
        {
            return new Serviceplandcfmappings()
            {

                Serviceplanid = model.Serviceplanid,
                Dcfid = model.Dcfid,
                Status= model.Status,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser

            };
        }
    }
}
