using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CAM.Entities.Models
{
    [Table("VolteKPIWorklog")]
    public class VolteKPIWorklog : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("VolteKPIWorklogId")]
        public long VolteKPIWorklogId { get; set; }

        //Chiave VolteKPI
        public long VolteKPIId { get; set; }
        //Chiave Naturale
        public short OpCoId { get; set; }
        public short Month { get; set; }
        public short Year { get; set; }
        public int VolteKPIType { get; set; }

        public decimal? TargetMonthlyValueOld { get; set; }
        public decimal? TargetMonthlyValueProposed { get; set; }
        public decimal? TargetMonthlyValueNew { get; set; }

        public decimal? EoyTargetOld { get; set; }
        public decimal? EoyTargetNew { get; set; }

        public decimal? ActualMonthlyValueOld { get; set; }
        public decimal? ActualMonthlyValueNew { get; set; }
        public decimal? ActualNumberOfRegisteredOld { get; set; }
        public decimal? ActualNumberOfRegisteredNew { get; set; }
        public decimal? ActualNumberOfProvisionedOld { get; set; }
        public decimal? ActualNumberOfProvisionedNew { get; set; }
        public string Comments { get; set; }
        public bool? Approved { get; set; }
        public bool IsStored { get; set; }

        [ForeignKey(nameof(OpCoId))]
        [InverseProperty("VolteKPIWorklog")]
        public virtual OpCo OpCo { get; set; }
    }
}
