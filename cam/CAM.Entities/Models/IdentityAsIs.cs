using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models
{
    [Table("Identitiesasis")]
    public partial class IdentityAsIs : AuditableEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Value { get; set; }

        [StringLength(255)]
        public string ResourceKey { get; set; }

        [StringLength(255)]
        public string PreviousResourceKey { get; set; }

        public int? ClassId { get; set; }

        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public int? TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public Type Type { get; set; }

        [Required]
        public long AssetId { get; set; }
        public string InterfaceType { get; set; }

        public string InterfaceName { get; set; }

        [ForeignKey(nameof(AssetId))]
        public NetworkElementAsPlanned Asset { get; set; }

        public Dictionary<short, string> VerticalFilterDto { get; set; }

    }
}