using CAM.Entities.Models.Base;
using CAM.Identity;
using System;
using System.Collections.Generic;

namespace CAM.Entities.Models
{
    public partial class ComponentSoftwareBuildsDesignContact : AuditableEntity
    {
        public long ComponentSoftwarebuildsDesignContactId { get; set; }
        public int DesignContactId { get; set; }
        public long ComponentSoftwareBuildId { get; set; }
      
        public virtual ComponentSoftwareBuild ComponentSoftwareBuilds { get; set; } 
        public virtual ApplicationUser DesignContact { get; set; } 
    }
}
