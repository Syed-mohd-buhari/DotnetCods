using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;
namespace CAM.Entities.Models.Lookup
{
    public partial class OperationalContract : AuditableEntity
    {
        public short Id { get; set; }

        public string Description { get; set; }
    }
}
