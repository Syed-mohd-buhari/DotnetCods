using CAM.DataTransferObjects.LookUp;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.DaAsssetMigration
{
    public class DaAssetMigrationAddUpdateDto
    {
        public DaAssetMigrationAddUpdateDto()
        {
            EnvironmentReosurce = new Dictionary<int, string>();
          
            daAssetMigrationDtoGrid = new List<DaAssetMigrationDtoGrid>();
        }
        public List<DaAssetMigrationDtoGrid> daAssetMigrationDtoGrid { get; set; }

        public IDictionary<int, string> EnvironmentReosurce { get; set; }
        public IDictionary<short, DeploymentStatusDto> DeploymentStatusReosurce { get; set; }
       
        public IDictionary<short, string> LocationReosurce { get; set; }
         
        public List<KeyValuePair<long, string>> TargetDesignComponentResource { get; set; }

        public List<ExistingAssetDto> ExistingAssetResource { get; set; }
    }

    public class ExistingAssetDto
    {
        public string  value { get; set; }
        public long key { get; set; }

//public string Environment { get; set; }
        public long EnvironmentId { get; set; }

      //  public string Location { get; set; }
        public long LocationId { get; set; }

//public string DeploymentStatus { get; set; }
        public long DeploymentStatusId { get; set; }

    }
    public class PlatformPlannedDcfDto
    {
        public PlatformPlannedDcfDto()
        {
            dcfResource = new Dictionary<long,string>();

            compareValue = string.Empty;
        }

        public Dictionary<long, string> dcfResource { get; set; }
        public string compareValue { get; set; }
    }
}
