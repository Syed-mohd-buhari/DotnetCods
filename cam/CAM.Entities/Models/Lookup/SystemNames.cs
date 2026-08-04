using CAM.Entities.Models.Base;
using CAM.Identity;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    [Table("Systemnames")]
    public partial class SystemNames : AuditableEntity
    {
        public long SystemNameId { get; set; }
        public string SystemNameDescription { get; set; }

        public virtual ICollection<NetworkElementAsPlanned> NetworkElementsAsPlanned { get; set; }


    }
}
