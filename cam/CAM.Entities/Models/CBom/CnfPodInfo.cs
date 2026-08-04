using CAM.Entities.Models.Base;
using CAM.Identity;
using OracleModels.DBModels;
using System.Collections.Generic;

namespace CAM.Entities.Models.CBom
{
    public class CnfPodInfo : AuditableEntity
    {
        public CnfPodInfo()
        {
            CnfCapacity = new HashSet<CnfCapacity>();
        }

        public long CnfPodInfoId { get; set; }
        public long CnfClusterInfoId { get; set; }
        public long PodTypeInfoId { get; set; }
        public long FunctionStandardId { get; set; }
        public long PriorityId { get; set; }
         public long? PodroleDescriptionId { get; set; }   
        public bool? DaemonSetPod { get; set; }
        public string IntraPodRules { get; set; }
        public string InterPodRules { get; set; }
        public bool? IsEnhancedHa { get; set; }
        public string PodTypeQos { get; set; }
        public string IsPersistanceStorageFlag { get; set; }
        public bool? IsProdHpaEnable { get; set; }       
        public virtual CnfClusterInfo Cnfclusterinfo { get; set; }

        public long? PodRoleDescriptionId { get; set; }

        public string PodTypeInfoName { get; set; }

        public string PodroleDescription { get; set; }
        public string PriorityName { get; set; }
        public string FunctionStandardName { get; set; }
        public virtual Functionstandardname Functionstandard { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual PodTypeInfo PodRoleDescription { get; set; }
        public virtual PodTypeInfo PodTypeInfo { get; set; }
        public virtual CnfPriority Priority { get; set; }
        public virtual ICollection<CnfCapacity> CnfCapacity { get; set; }
         
    }
}
