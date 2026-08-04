using System.Collections.Generic;

namespace CAM.Identity
{
    public class ApplicationOpco
    {
        public short? OpCoId { get; set; }
        public string OpCoName { get; set; }
        public virtual ICollection<ApplicationUser> UserRole { get; set; }
       
    }
}
