using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public partial class DaMigrationStatusMapper
    {
        public static DaMigrationStatus GetDaMigrationStatus(Damigrationstatus model)
        {
            if (model == null)
                return null;
            var result = new DaMigrationStatus()
            {
                DaMigrationStatuId = model.Damigrationstatuid,
                OpcoId = model.Opcoid,
                LocationId = model.Locationid,
                StatusId = model.Statusid,
                LocationName = model.Location != null ? model.Location.Location : string.Empty,
                OpcoName = model.Opco != null ? model.Opco.Opco : string.Empty,
                 PlannedActivityId = model.Plannedactivityid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };

            return result;
        }

        public static Damigrationstatus SetDaMigrationStatus(DaMigrationStatus model)
        {
            if (model == null)
                return null;
            var result = new Damigrationstatus()
            {
                Damigrationstatuid = model.DaMigrationStatuId,
                Opcoid = model.OpcoId,
                Locationid = model.LocationId,
                Statusid = model.StatusId,
            };
            return result;
        }
    }
}
