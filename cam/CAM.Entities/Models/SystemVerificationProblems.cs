using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Models
{
    [Table("Systemverificationproblems")]
    public class SystemVerificationProblems : AuditableEntity
    {
        public long SystemVerificationProblemId { get; set; }
        public string ProblemId { get; set; }
        public short? OpcoId { get; set; }
        public long? SystemTypeId { get; set; }
        public short? EnvironmentId { get; set; }
        public DateTime? DateFound { get; set; }
        public long? ProblemCategoryId { get; set; }
        public string ProblemDescription { get; set; }
        public string MaintenanceReference { get; set; }
        public int? Severity { get; set; }
        public string StatusUrl { get; set; }
        public string Mitigation { get; set; }
        public string SolutionDescription { get; set; }
        public string PatchReference { get; set; }
        public string ProductUpgradeReference { get; set; }
        public string SuppleMental { get; set; }
        public string VendorCsr { get; set; }
        public string SubNetwork { get; set; }
        public string TestReport { get; set; }
        public string StandardNir { get; set; }
        public string EricssonSecReport { get; set; }
        public string SwAndStEntries { get; set; }
        public string PenTestingReport { get; set; }

        public virtual CAM.Entities.Models.Lookup.Environment Environment { get; set; }
        public virtual OpCo OpCo { get; set; }
        public virtual ProblemCategory ProblemCategory { get; set; }
        public virtual SeverityEntity SeverityEntity { get; set; }
        public virtual SystemType Systemtype { get; set; }
    }
}