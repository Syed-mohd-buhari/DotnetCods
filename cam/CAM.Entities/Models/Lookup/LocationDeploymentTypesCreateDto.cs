using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    public class LocationDeploymentTypes :AuditableEntity
    {
        public long Id { get; set; }
        public short LocationId { get; set; }
        public short DeploymentTypeId { get; set; }
        public Location Location { get; set; }
        public DeploymentType DeploymentType { get; set; }
    }
}
