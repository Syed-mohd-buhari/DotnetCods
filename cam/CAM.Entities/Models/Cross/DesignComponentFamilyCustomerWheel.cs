using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models.Cross
{
    public class DesignComponentFamilyCustomerWheel : AuditableEntity
    {
        public long Id { get; set; }

        public long DesignComponentFamilyId { get; set; }

        public int CustomerWheelId { get; set; }

        public virtual CustomerWheel CustomerWheel { get; set; }
        public virtual DesignComponentFamily DesignComponentFamily { get; set; }
    }
}
