using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    public class LCMDeploymentStatus: AuditableEntity
    {
        public LCMDeploymentStatus()
        {
            Lcmengineerings = new List<LcmEngineering>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }
        //public int CreationUser { get; set; }
        //public DateTime CreationDate { get; set; }
        //public int ModificationUser { get; set; }
        //public DateTime ModificationDate { get; set; }
        //public bool? Deleted { get; set; }
        //public DateTime? DeletionDate { get; set; }
        public string Description { get; set; }


        [InverseProperty(nameof(LcmEngineering.LCMDeploymentStatus))]
        public List<LcmEngineering> Lcmengineerings { get; set; }
    }
}
