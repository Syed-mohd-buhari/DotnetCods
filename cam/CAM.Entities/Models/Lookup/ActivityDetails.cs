using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("ActivityDetails")]
    public partial class ActivityDetails : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short ActivityDetailsId { get; set; }
        [Column("ActivityDetails")]
        public string ActivityDetailsDescription { get; set; }
        [Column("ForVirtualized")]
        public bool ForVirtualized { get; set; }
    }
}