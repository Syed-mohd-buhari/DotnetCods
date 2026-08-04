using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models.Cross
{
    public class DesignAspectNetworkFunction : AuditableEntity
    {
        public int Id { get; set; }

        public long DesignAspectId { get; set; }

        public int NetworkFunctionId { get; set; }

        public virtual NetworkFunction NetworkFunction { get; set; }
        public virtual DesignAspect DesignAspect { get; set; }
    }
}
