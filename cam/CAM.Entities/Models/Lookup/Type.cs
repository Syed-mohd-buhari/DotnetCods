using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("Types")]
    public partial class Type : AuditableEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]

        public int ClassId { get; set; }

        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }
    }
}