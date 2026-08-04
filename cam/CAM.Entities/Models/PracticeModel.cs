using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models
{
    [Table("Practice")]
    public class PracticeModel : AuditableEntity
    {
        public int PracticeId { get; set; }
        public string PracticeDescription { get; set; }
        public int? PracticeEmailId { get; set; }

        public virtual ApplicationUser PracticeEmail { get; set; }

        public virtual ICollection<OrganisationModel> OrganisationEnitity { get; set; }
    }
}