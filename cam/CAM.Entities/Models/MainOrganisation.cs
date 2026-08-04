using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Models
{
    [Table("Mainorganisation")]
    public class MainOrganisation : AuditableEntity
    {

        public int MainorganisationId { get; set; }
        public string MainorganisationDescription { get; set; }

        public virtual ICollection<OrganisationModel> OrganisationEnitity { get; set; }
    }
}