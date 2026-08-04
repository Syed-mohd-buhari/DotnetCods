using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Entities.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models.Cross
{
    public class SettingUpdatePlannedActivityLcmDeploymentStatus:AuditableEntity
    {
        public short Id { get; set; }
        public short SettingUpdatePlannedActivityId { get; set; }
        public short LcmDeploymentStatusId { get; set; }
        public virtual SettingsUpdatePlannedActivity Settingupdateplannedactivity { get; set; }
        public virtual LCMDeploymentStatus LcmDeploymentStatus { get; set; }
    }
}
