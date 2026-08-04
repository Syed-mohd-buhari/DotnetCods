using CAM.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.Base
{
    public abstract class AuditableEntity
    {
        public int CreationUser { get; set; }

        [ForeignKey(nameof(CreationUser))]
        public ApplicationUser CreationUserEntity { get; set; }

        public DateTime CreationDate { get; set; }

        public int ModificationUser { get; set; }

        [ForeignKey(nameof(ModificationUser))]
        public ApplicationUser ModificationUserEntity { get; set; }

        public DateTime ModificationDate { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletionDate { get; set; }


    }
}
