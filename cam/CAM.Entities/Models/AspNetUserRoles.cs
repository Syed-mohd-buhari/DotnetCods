using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using System;

namespace CAM.Entities.Models
{
    public partial class AspNetUserRoles :AuditableEntity
    {
        public decimal AspNetUserRoleId { get; set; }
        public int Userid { get; set; }
        public int Roleid { get; set; }
       // public int? Verticalresponsibleid { get; set; }
       // public short? Opcoid { get; set; }
        public int? Creationuser { get; set; }
        public DateTime? Creationdate { get; set; }
        public int? Modificationuser { get; set; }
        public DateTime? Modificationdate { get; set; }
        //public virtual OpCo Opco { get; set; }
        //public virtual VerticalResponsible VerticalResponsible { get; set; }
        public virtual AspNetRoles Role { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
