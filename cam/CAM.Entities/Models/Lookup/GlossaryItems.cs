using CAM.Entities.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.Lookup
{
    [Table("GlossaryItems")]
    public class GlossaryItems : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Glossaryitemsid")]
        public long GlossaryItemsId { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public bool? Istsrfield { get; set; }

    }
}
