using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Models.Lookup
{
    [Table("Majorhwbuildsdesigncontacts")]
    public partial class MajorHwBuidlsDesignContact : AuditableEntity
    {
        public long MajorHwBuidlsDesignContactId { get; set; }
        public int DesignContactId { get; set; }
        public long MajorHardwareBuildsId { get; set; }

        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual MajorHardwareBuild MajorHardwareBuilds { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ApplicationUser DesignContact { get; set; }

    }
}