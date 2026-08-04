using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cross
{
    public static class ReasonCheckboxResourceLcmEngineeringSoftwareMapper
    {
        public static ReasonCheckboxResourceLcmEngineeringSoftware Get(Reasoncheckboxresourcelcmengineeringsoftware model)
        {
            if (model == null)
                return null;
            return new ReasonCheckboxResourceLcmEngineeringSoftware()
            {
                Id = model.Id,
                ReasonCheckboxResourceId = model.Reasoncheckboxresourceid,
                LcmEngineeringId = model.Lcmengineeringid,
                CheckboxResource = ReasonCheckboxResourceMapper.GetReasonCheckboxResourceMapper(model.Reasoncheckboxresource)


            };
        }
        public static Reasoncheckboxresourcelcmengineeringsoftware Set(ReasonCheckboxResourceLcmEngineeringSoftware model)
        {
            return new Reasoncheckboxresourcelcmengineeringsoftware()
            {
                Id = model.Id,
                Reasoncheckboxresourceid = model.ReasonCheckboxResourceId,
                Lcmengineeringid = model.LcmEngineeringId,
            };
        }
    }
}
