using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using System.Collections.Generic;
using System.Security.Policy;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
    public class PlannedActivityDtoUpdate : PlannedActivityDtoCreate
    {
        public long PlannedActivityId { get; set; }

        public long? OriginalLcmEngineeringId { get; set; }
        public string PlannedActivityName { get; set; }

        public DaAssetMigrationAddUpdateDto PlaftformMigrationDcfResources { get; set; }
        public bool IsRefactorDC { get; set; } = false;

    }
}