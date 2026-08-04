using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cross
{
    public static class ReasonCheckboxResourceLcmEngineeringHardwareMapper
    {
        public static ReasonCheckboxResourceLcmEngineeringHardware Get(Reasoncheckboxresourcelcmengineeringhardware model)
        {
            if (model == null)
                return null;
            return new ReasonCheckboxResourceLcmEngineeringHardware()
            {
               Id = model.Id,
               ReasonCheckboxResourceId = model.Reasoncheckboxresourceid,
               LcmEngineeringId = model.Lcmengineeringid,
               CheckboxResource = ReasonCheckboxResourceMapper.GetReasonCheckboxResourceMapper(model.Reasoncheckboxresource)
               
            };
        }
        public static Reasoncheckboxresourcelcmengineeringhardware Set(ReasonCheckboxResourceLcmEngineeringHardware model)
        {
            if (model == null)
                return null;
            return new Reasoncheckboxresourcelcmengineeringhardware()
            {
                Id = model.Id,
                Reasoncheckboxresourceid = model.ReasonCheckboxResourceId,
                Lcmengineeringid = model.LcmEngineeringId,
            };
        }
    }
}
