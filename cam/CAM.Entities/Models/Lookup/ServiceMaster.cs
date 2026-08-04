using CAM.Entities.Models.Base;
using System;

namespace CAM.Entities.Models.Lookup
{
    public partial class ServiceMaster : AuditableEntity
    {
        public int Servicemasterid { get; set; }
        public string Description { get; set; }      

    }
}
