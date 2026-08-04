using CAM.Entities.Models.Base;
using CAM.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.Engine
{
    public partial class GridCustomColumn : AuditableEntity
    {
        
        public decimal GridCustomColumnId { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required] 
        public string ClassName { get; set; }
        [Required]
        public string JsonGridCustomizationData { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        public int? Preferencedate { get; set; }


        public int? MessagingDate { get; set; } 

    }
}