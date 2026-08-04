using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    public class SecurityManager : AuditableEntity
    {
        public int Id {  get; set; }
        public string Description { get; set; }
    }
}
