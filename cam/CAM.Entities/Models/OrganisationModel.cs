using CAM.Entities.Models.Base;
using CAM.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models
{
    public class OrganisationModel : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrganisationId { get; set; }
        public int MainOrganisationId { get; set; }
        public int PracticeId { get; set; }
        public int? VerticalResponsibleId { get; set; }
        public string VerticalResponsibleName { get; set; }
        public virtual ApplicationUser ContactNavigation { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual MainOrganisation MainOrganisation { get; set; }
        public virtual PracticeModel Practice { get; set; } 
        public virtual VerticalResponsible VerticalResponsible { get; set; }
    }
}
