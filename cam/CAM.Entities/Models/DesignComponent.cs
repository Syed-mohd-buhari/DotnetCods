using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models
{
    [Table("DesignComponents")]
    public partial class DesignComponent : AuditableEntity
    {
        public DesignComponent()
        {
            Lcmengineerings = new List<LcmEngineering>();
            PlannedActivities = new List<PlannedActivity>();
            Networkelementsasplanned = new List<NetworkElementAsPlanned>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DesignComponentId { get; set; }
        public string ProductName { get; set; }
        //[Required]
        //[StringLength(255)]
        //public string ServiceApplication { get; set; }

        public long? DesignComponentFamilyId { get; set; }

        //[Required]
        public long SubNetworkBoundaryId { get; set; }
        public bool GdprRelevant { get; set; }

        public long SystemTypeId { get; set; }
      
        [ForeignKey(nameof(SystemTypeId))]
        [InverseProperty("DesignComponents")]
        public virtual SystemType SystemType { get; set; }
        [InverseProperty(nameof(LcmEngineering.DesignComponent))]
        public List<LcmEngineering> Lcmengineerings { get; set; }    
        
        [InverseProperty(nameof(PlannedActivity.DesignComponent))]
        public List<PlannedActivity> PlannedActivities{ get; set; }

        [InverseProperty(nameof(NetworkElementAsPlanned.DesignComponent))]
        public List<NetworkElementAsPlanned> Networkelementsasplanned { get; set; }


        public virtual SubNetworkBoundary SubNetworkBoundary { get; set; }

        [ForeignKey(nameof(DesignComponentFamilyId))]
        [InverseProperty(nameof(Models.DesignComponentFamily.DesignComponents))]
        public virtual DesignComponentFamily DesignComponentFamily { get; set; }


        public bool? VisibleFlag { get; set; }
    }
}