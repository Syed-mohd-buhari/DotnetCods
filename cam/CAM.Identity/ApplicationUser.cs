using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
namespace CAM.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<int>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<int>> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationOrgandVertical> ApplicationOrgAndVerticals { get; set; }
        public virtual ICollection<ApplicationUserOpco> Opcos { get; set; }
        public virtual ApplicationSubDomainRes ApplicationSubDomainRes { get; set; }
        public bool? Active { get; set; }
        public int? Creationuser { get; set; }
        public DateTime? Creationdate { get; set; }
        public int? Modificationuser { get; set; }
        public DateTime? Modificationdate { get; set; }
        public bool? Issubdomainspoc { get; set; }
        public bool? Iseduspoc { get; set; }
        public bool? Isdesigncontact { get; set; }
        public int? Subdomainresponsibleid { get; set; }
        public string ModificationUserEmail { get; set; }
    }
}
