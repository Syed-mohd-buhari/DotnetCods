using CAM.Contracts;
using CAM.Entities.Models;
using CAM.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Authentication;

namespace CAM.WebAPI.Identity
{
    public class CurrentUserServiceMock : ICurrentUserService
    {
        public CurrentUserServiceMock(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            //try
            //{
            //    var email = "davide.camerlingo@bcsoft.net";
            //    EMail = email;
            //    UserId = identityService.GetUserIdByEmailAsync(email).Result;
            //    Rule = new List<string>() { "Admin", "KPI Editor" };
            //}
            try
            {
                var email = "davide.camerlingo@bcsoft.net";
                UserId = 1; //identityService.GetUserIdByEmailAsync(email).Result;
                Rule = new List<string>() { "Admin" };
                adminRoleId = 1;
                lcmDeploymentStatus = new List<string>() { "planned", "in-service" }; 
            }
            catch (Exception ex)
            {
                throw new Exception("Not authorize");
            }
        }
        public List<string> lcmDeploymentStatus { get; set; }
        public string EMail { get; }
        public int UserId { get; }
        public List<string> Rule { get; }
        public int adminRoleId { get; }
        public List<short> GetOpCos()
        {
            return new List<short>() { 1, 2, 3 };
        }

        public bool UserInRole(string role, bool throwException = true)
        {
            return Rule.Contains(role) ? true : (throwException) ? throw new AuthenticationException("Not authorize") : false;
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
       
        public void SetValues(UserInfoModel model)
        {
            throw new NotImplementedException();
        }
    }
}