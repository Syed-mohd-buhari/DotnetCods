using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class OpCoMapper
    {
        public static Models.Lookup.OpCo GetOpCoMapper(Opcos OpCo)
        {
            if (OpCo == null)
                return null;
            return new Models.Lookup.OpCo()
            {
                OpCoId= OpCo.Opcoid,
                OpCoDescription = OpCo.Opco,
                CreationDate = OpCo.Creationdate,
                CreationUser = OpCo.Creationuser,
                ModificationDate = OpCo.Modificationdate,
                ModificationUser = OpCo.Modificationuser,
                Deleted = OpCo.Deleted.Value,
                DeletionDate = OpCo.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(OpCo.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(OpCo.ModificationuserNavigation),
               
            };
        }

        public static Models.Lookup.OpCo GetOpCoMapperForBPT(Opcos OpCo)
        {
            if (OpCo == null)
                return null;
            return new Models.Lookup.OpCo()
            {
                OpCoId = OpCo.Opcoid,
                OpCoDescription = OpCo.Opco                 
            };
        }
        public static Opcos SetOpCoMapper(Models.Lookup.OpCo OpCo)
        {
            if (OpCo == null)
                return null;
            return new Opcos()
            {

                Opcoid = OpCo.OpCoId,
                Opco = OpCo.OpCoDescription,
                Creationdate = OpCo.CreationDate,
                Creationuser = OpCo.CreationUser,
                Modificationdate = OpCo.ModificationDate,
                Modificationuser = OpCo.ModificationUser,
                Deleted = OpCo.Deleted,
                Deletiondate = OpCo.DeletionDate,
            };
        }

        public static ApplicationOpco GetApplicationOpco(Opcos OpCo)
        {
            if (OpCo == null)
                return null;
            return new ApplicationOpco()
            {
                OpCoId = OpCo.Opcoid,
                OpCoName = OpCo.Opco,
               

            };
        }
    }
}
