using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Models
{
    [Table("Problemcategory")]
    public class ProblemCategory : AuditableEntity
    {

        public long ProblemCategoryId { get; set; }
        public string ProblemCategoryDescription { get; set; }

        public virtual ICollection<SystemVerificationProblems> SystemVerificationProblems { get; set; }

    }
}