using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Contracts
{
    public interface ICurrentUserService
    {
        string EMail { get; }
        int UserId { get; }
        List<string> Rule { get; }
        int adminRoleId { get; }

        public List<string> lcmDeploymentStatus { get; set; }
        public bool UserInRole(string role, bool throwException = true);
        public bool UserInRole(bool throwException = true, params string[] roles);
    }
}
