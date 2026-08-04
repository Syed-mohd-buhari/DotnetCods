using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;
using System;
using Environment = CAM.Entities.Models.Lookup.Environment;

namespace CAM.Entities.Models
{
    public class DaAssetMigration : AuditableEntity
    {
        public long DaAssetMigrationId { get; set; }
        public long PlannedActivityId { get; set; }

        public long? NetworkElementAsPlannedId { get; set; }
        public long  CurrentDesignComponenetId { get; set; }
        public string OldAssetName { get; set; }
        public string OldEnvironment { get; set; }
        public string OldDeploymentType { get; set; }
        public string OldDeploymentStatus { get; set; }


        public string NewelEmentName { get; set; }
        public long? TargetDesignComponenetId { get; set; }
        public short? NewEnvironmentId { get; set; }
        public short? NewDeploymentStatusId { get; set; }
        

        public string? NewEnvironment { get; set; }
        public string? NewDeploymentStatus { get; set; }
 

        public short OpcoId { get; set; }
        public string NewOpco { get; set; }
        public string? NewLocation { get; set; }
        public short? NewLocationId { get; set; }


        public DateTime? RfoDate { get; set; }
        public DateTime? RfsDate { get; set; }
        public DateTime? MigrationCompletionDate { get; set; }
        public string TrafficNodePercentage { get; set; }

        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual Location Location { get; set; }

        public virtual OpCo Opco { get; set; }
        public string OpcoDescription { get; set; }
        public string EnvironmentDescription { get; set; }
        public long EnvironmentId { get; set; }
        public virtual PlannedActivity PlannedActivity { get; set; } 
        public virtual DeploymentStatus DeploymentStatus { get; set; } 
        public virtual Environment Environment { get; set; }
       
        public virtual NetworkElementAsPlanned NetworkElementAsPlanned { get; set; }
       
        public virtual DesignComponent Targetdesigncomponenet { get; set; }
        public DateTime? HwPoRaisedDate { get; set; }
        public DateTime? HwPoArrivedDate { get; set; }
        public DateTime? BomSubmittedDate { get; set; }
        public DateTime? RfaDate { get; set; }

        public string ProductName { get; set; }
        public string Platform { get; set; }

        public short? PlatformId { get; set; }
        public decimal? ProductNameId { get; set; }

        public virtual Platform Platforms { get; set; }
        public virtual ProductName ProductNames { get; set; }

        public bool? IsDecommissioned { get; set; }
        public DateTime? Vecdate { get; set; }
        public DateTime? Startofappintegration { get; set; }
        public DateTime? Migrationstart { get; set; }
    }
}
