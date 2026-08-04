using OracleModels.DBModels;
using System;
using System.Collections.Generic;

namespace CAM.Entities.Models
{
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            Aspnetroleclaims = new HashSet<Aspnetroleclaims>();
            Aspnetuserroles = new HashSet<AspNetUserRoles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Normalizedname { get; set; }
        public string Concurrencystamp { get; set; }
        public string Description { get; set; }
        public string Abstractiontaborder { get; set; }
        public int? PortalRoleId { get; set; }
        public virtual ICollection<Aspnetroleclaims> Aspnetroleclaims { get; set; }
        public virtual ICollection<AspNetUserRoles> Aspnetuserroles { get; set; }
    }
}
