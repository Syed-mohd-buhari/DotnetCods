using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Models
{
    [Table("Severity")]
    public class SeverityEntity : AuditableEntity
    {
       
        public int SeverityId { get; set; }
        public string SeverityDescription { get; set; }


        public virtual ICollection<SystemVerificationProblems> SystemVerificationProblems { get; set; }
    }
}