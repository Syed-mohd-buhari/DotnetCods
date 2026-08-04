using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("Program")]
    public partial class ProgramEntity : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProgramId { get; set; }
        
        
        public string ProgramDescription { get; set; }

    }
}
