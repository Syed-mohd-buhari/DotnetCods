using CAM.Entities.Models.Base;
namespace CAM.Entities.Models
{
    public class ServicePlanDcfMapping : AuditableEntity
    {
        public int Serviceplandcfmappingid { get; set; }
        public int? Serviceplanid { get; set; }
        public long? Dcfid { get; set; }
        public short? Status { get; set; }

        public virtual DesignComponentFamily Dcf { get; set; }
        public virtual ServicePlan Serviceplan { get; set; }
    }
}
