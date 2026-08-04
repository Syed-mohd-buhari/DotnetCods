using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models.Cross
{
    public class MajorSoftwareBuildFamilyNetworkFunction : AuditableEntity
    {
        public long Id { get; set; }

        public long MajorSoftwareBuildId { get; set; }
        public int NetworkFunctionId { get; set; }

        public virtual MajorSoftwareBuild MajorSoftwareBuild { get; set; }
        public virtual NetworkFunction NetworkFunction { get; set; }
    }
}
