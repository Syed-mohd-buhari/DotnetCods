using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;

namespace CAM.WebAPI.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IIdentityService _identityService;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {

            var email = httpContextAccessor.HttpContext?.User?.FindFirstValue("Email") ?? "";
            try
            {
                if (email != null && email != string.Empty)
                {

                    EMail = email;
                    UserId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirstValue("UserId"));
                    var rules = httpContextAccessor.HttpContext?.User?.FindAll("Role").Select(p => p.Value).ToList();
                    Rule = rules;
                    adminRoleId = 1;
                    lcmDeploymentStatus = new List<string>() { "planned", "in-service", "in commissioning" };

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Not authorize");
            }
        }
        public string EMail { get; }
        public int UserId { get; }
        public List<string> Rule { get; }

        public int adminRoleId { get; }
        public List<string> lcmDeploymentStatus { get; set; }

        public bool UserInRole(string role, bool throwException = true)
        {
            if (Rule != null)
                return Rule.Contains(role) ? true : (throwException) ? throw new AuthenticationException("Not authorize") : false;
            else
                return false;


        }
        public bool UserInRole(bool throwException = true, params string[] roles)
        {
            bool returnValue = false;
            foreach (string role in roles)
            {
                returnValue |= Rule.Contains(role);
            }

            if (throwException && !returnValue)
            {
                throw new AuthenticationException("Not authorize");
            }

            return returnValue;
        }
        //public List<short> GetOpCos()
        //{
        //    return _userManager.GetUserClaim(this.UserId, "opco");

        //}

    }
}
