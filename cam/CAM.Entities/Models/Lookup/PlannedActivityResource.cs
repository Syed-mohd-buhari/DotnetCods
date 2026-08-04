using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Settings;

namespace CAM.Entities.Models.Lookup
{
    [Table("PlannedActivityResources")]
    public partial class PlannedActivityResource : AuditableEntity
    {
        public PlannedActivityResource()
        {
            PlannedActivities = new List<PlannedActivity>();
            PlannedActivityResourcePlanningRisk = new List<PlannedActivityResourcePlanningRisk>();
            PlannedActivityResourceDriver = new List<PlannedActivityResourceDriver>();
            PlannedActivityResourceBenefit = new List<PlannedActivityResourceBenefit>();
            SettingsUpdatePlannedActivity = new List<SettingsUpdatePlannedActivity>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short PlannedActivityResourceId { get; set; }
        [Required]
        [Column("PlannedActivityResource")]
        [StringLength(150)]
        public string PlannedActivityResourceDescription { get; set; }
        
        public string LcmLabelSoftware { get; set; }
        public string LcmLabelHardware { get; set; }

        public string AddAssetLabelSoftware { get; set; }
        public string AddAssetLabelHardware { get; set; }

        public string ActivityDetailsAddAsset { get; set; }

        public string EditAssetLabelSoftware { get; set; }
        public string EditAssetLabelHardware { get; set; }

        public string ActivityDetailsEditAsset { get; set; }

        public string ActivityDetailsLcm { get; set; }

        public string ActivityDetailsForVirtualizedAddAsset { get; set; }

        public int? RuleAddAsset { get; set; }
        public string DriverTextAddAsset { get; set; }
        public string BenefitTextAddAsset { get; set; }
        public bool? ForCreateAddAsset { get; set; }
        public bool? ForEditAddAsset { get; set; }

        public bool? PlannedDesignComponentRequiredAddAsset { get; set; }
        public int? RuleActicvityDetailsAddAsset { get; set; }

        public string ActivityDetailsForVirtualizedEditAsset { get; set; }

        public int? RuleEditAsset { get; set; }
        public string DriverTextEditAsset { get; set; }
        public string BenefitTextEditAsset { get; set; }
        public bool? ForCreateEditAsset { get; set; }
        public bool? ForEditEditAsset { get; set; }

        public bool? PlannedDesignComponentRequiredEditAsset { get; set; }
        public int? RuleActicvityDetailsEditAsset { get; set; }

        public bool Exportable { get; set; }
        public bool ForLcm { get; set; }

        public bool LcmHardware { get; set; }
        public bool LcmSoftware { get; set; }

        public bool? AddAssetHardware { get; set; }
        public bool? AddAssetSoftware { get; set; }

        public bool? EditAssetHardware { get; set; }
        public bool? EditAssetSoftware { get; set; }

        public int RuleActicvityDetails { get; set; }
        public int RuleLinkedDc { get; set; }
        
        public string JsonForm { get; set; }

        public bool? OnBareMetalAddAsset { get; set; }
        public bool? OnVirtualizedAddAsset { get; set; }

        public bool? OnBareMetalEditAsset { get; set; }
        public bool? OnVirtualizedEditAsset { get; set; }


        public string DesignAspectLabelSoftware { get; set; }
        public string DesignAspectLabelHardware { get; set; }
        public bool ForDesignAspect { get; set; }
        public string ActivityDetailsDesignAspect { get; set; }

        public bool DesignAspectSoftware { get; set; }
        public bool DesignAspectHardware { get; set; }

        public int? RuleDesignAspect { get; set; }
        public bool? ForAddAsset { get; set; }
        public bool? ForEditAsset { get; set; }
        public bool? ForServicePlan {  get; set; }

        public bool DesignAspectExportable { get; set; }
        public virtual List<PlannedActivity> PlannedActivities { get; set; }
        public virtual List<PlannedActivityResourcePlanningRisk> PlannedActivityResourcePlanningRisk { get; set; }

        public virtual List<PlannedActivityResourceBenefit> PlannedActivityResourceBenefit { get; set; }
        public virtual List<PlannedActivityResourceDriver> PlannedActivityResourceDriver { get; set; }
        public virtual List<SettingsUpdatePlannedActivity> SettingsUpdatePlannedActivity { get; set; }
        public virtual PlannedActivityTypes RuleLinkedDcNavigation { get; set; }
    }
}