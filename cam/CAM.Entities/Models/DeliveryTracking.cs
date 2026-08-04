using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Models
{
    [Table("DELIVERYTRACKINGS")]
    public class DeliveryTracking : AuditableEntity
    {
        public int Id { get; set; }
        public long? PlannedActivityId { get; set; }
        public string MS1EventType { get; set; }
        public DateTime? MS1BaseLineDate { get; set; }
        public DateTime? MS1LatestPlanningDate { get; set; }
        public int? MS1Status { get; set; }
        public string MS2EventType { get; set; }
        public DateTime? MS2BaseLineDate { get; set; }
        public DateTime? MS2LatestPlanningDate { get; set; }
        public int? MS2Status { get; set; }
        public string MS3EventType { get; set; }
        public DateTime? MS3BaseLineDate { get; set; }
        public DateTime? MS3LatestPlanningDate { get; set; }
        public int? MS3Status { get; set; }
        public int? MS4EventType { get; set; }
        public DateTime? MS4BaseLineDate { get; set; }
        public DateTime? MS4LatestPlanningDate { get; set; }
        public int? MS4Status { get; set; }
        public DateTime? PPMImportDate { get; set; }
        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public int CreationUser { get; set; }
        public DateTime CreationDate { get; set; }
        public int ModificationUser { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletionDate { get; set; }

        public virtual Aspnetusers CreationUserNavigation { get; set; }
        public virtual Aspnetusers ModificationUserNavigation { get; set; }
        public virtual PlannedActivity PlannedActivity { get; set; }

        public long? LcmEngineeringId { get; set; }
        public List<int?> LcmEngineeringSubdomainSpoc {  get; set; }
        public long? AssetId { get; set; }
        public List<int?> AssetSubdomainSpoc { get; set; }
        public List<FilterValueDtoKeyValueList> VerticalFilterDto { get; set; }
        public long? DesignAspectId { get; set; }
        public List<int?> DesignContactDto { get; set; }
        public long? ServiceInfoId { get; set; }
        public short? OpcoId { get; set; }  
        public string OpcoDescription { get; set; }
        public string VerticalName { get; set; }
    }
}
