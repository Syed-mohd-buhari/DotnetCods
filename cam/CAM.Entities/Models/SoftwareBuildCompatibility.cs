using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Models
{
    [Table("SoftwareBuildCompatibility")]
    public class SoftwareBuildCompatibility : AuditableEntity
    {
        public long SoftwareBuildCompatibilityId { get; set; }
        public long MajorSoftwareBuildId { get; set; }
        public long BundleMajorSoftwareBuildId { get; set; }
        public short BundleType { get; set; }
        //public int Creationuser { get; set; }
        //public DateTime Creationdate { get; set; }
       // public int Modificationuser { get; set; }
       // public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Majorsoftwarebuilds BundleMajorSoftwareBuildNavigation { get; set; }
        
       // public virtual Majorsoftwarebuilds MajorSoftwareBuild { get; set; }
    }
}
