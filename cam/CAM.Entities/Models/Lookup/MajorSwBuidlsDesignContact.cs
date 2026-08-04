using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Models.Lookup
{
    [Table("Majorswbuildsdesigncontacts")]
    public partial class MajorSwBuidlsDesignContact : AuditableEntity
    {
        [Key]
        public long MajorSwBuidlsDesignContactId { get; set; }
        public int DesignContactId { get; set; }
        public long MajorsoftwarebuildsId { get; set; }

        public virtual ApplicationUser CreationuserNavigation { get; set; }
        [ForeignKey(nameof(MajorsoftwarebuildsId))]
        public virtual MajorSoftwareBuild MajorsoftwareBuilds { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }        
        public virtual ApplicationUser DesignContact { get; set; }

    }
}