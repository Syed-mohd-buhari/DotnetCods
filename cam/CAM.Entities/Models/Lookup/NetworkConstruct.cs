using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("NetworkConstructs")]
    public  class NetworkConstruct : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short NetworkConstructsId { get; set; }
        [Required]
        [Column("NetworkConstruct")]
        public string NetworkConstructDescription { get; set; }

       
    }
}
