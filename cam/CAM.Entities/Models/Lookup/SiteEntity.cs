using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    [Table("Sites")]
    public partial class SiteEntity : AuditableEntity
    {
        public int SiteId { get; set; }
        public short LocationId { get; set; }
        public string SiteCode { get; set; }
        public string SiteCategory { get; set; }
        public string Region { get; set; }

        public virtual Location Location { get; set; }
    }
}
