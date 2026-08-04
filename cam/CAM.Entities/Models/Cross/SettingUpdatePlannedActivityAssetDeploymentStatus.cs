using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Entities.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models.Cross
{
    public class SettingUpdatePlannedActivityAssetDeploymentStatus : AuditableEntity
    {
        public short Id { get; set; }
        public short SettingUpdatePlannedActivityId { get; set; }
        public short AssetDeploymentStatusId { get; set; }
        public virtual SettingsUpdatePlannedActivity Settingupdateplannedactivity { get; set; }
        public virtual DeploymentStatus AssetDeploymentStatus { get; set; }
    }
}
