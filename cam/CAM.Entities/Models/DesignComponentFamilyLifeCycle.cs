using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Models
{
    [Table("Dcflifecycle")]
    public class DesignComponentFamilyLifeCycle : AuditableEntity
    {
        public long DcfLifeCycleId { get; set; }
        public long DcfId { get; set; }
        public long? ResourceTypesId { get; set; }
        public string ResourceKey { get; set; }
        public string PreviousResourceKey { get; set; }
        public short? OpCoId { get; set; }
        public long? DcId { get; set; }
        public string EventName { get; set; }
        public long? EventId { get; set; }
        public string Notes { get; set; }
        public new bool? Deleted { get; set; }
        public new DateTime? DeletionDate { get; set; }
        public long? NodeIndex { get; set; }
        public string CurrentDetails { get; set; }

        public string OpcoDescription { get; set; }
        public string DcfDescription { get; set; }

        public string? DcDescription { get; set; }
        public int? CategoryType { get; set; }
        public string PlannedDetails { get; set; }

        public string BagName { get; set; }
        
    }
}
