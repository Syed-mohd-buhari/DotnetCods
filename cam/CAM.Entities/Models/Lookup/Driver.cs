using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("Drivers")]
    public partial class Driver : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short DriverId { get; set; }
        [Column("Driver")]
        
        public string DriverDescription { get; set; }
        public string BptDriverDetails { get; set; }

    }
}
