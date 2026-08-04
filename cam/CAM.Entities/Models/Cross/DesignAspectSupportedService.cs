using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models.Cross
{
    public class DesignAspectSupportedService : AuditableEntity
    {
        public int Id { get; set; }

        public long DesignAspectId { get; set; }

        public int ServiceId { get; set; }

        public virtual SupportedService SupportedService { get; set; }
        public virtual DesignAspect DesignAspect { get; set; }
    }
}
