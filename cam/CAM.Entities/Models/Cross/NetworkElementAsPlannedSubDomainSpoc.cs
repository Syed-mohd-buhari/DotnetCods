using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Models.Cross
{
    [Table("NetworkElementAsPlannedSubDomainSpoc")]
    public partial class NetworkElementAsPlannedSubDomainSpoc : AuditableEntity
    {
        [Key]
        public long NetworkElementAsPlannedSubDomainSpocId { get; set; }

        public long NetworkElementAsPlannedId { get; set; }

        public int? Subdomainspocid { get; set; }


        [ForeignKey(nameof(NetworkElementAsPlannedId))]
        public virtual NetworkElementAsPlanned NetworkElementAsPlanned { get; set; }

        public virtual ApplicationUser Subdomainspoc { get; set; }
    }
}

