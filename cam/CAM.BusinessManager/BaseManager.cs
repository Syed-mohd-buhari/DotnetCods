using CAM.Contracts.RepositoryContracts.Base;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CAM.BusinessManager
{
    public abstract class BaseManager
    {
        public BaseManager(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, out IRepositoryWrapper repositoryWrapper)
        {
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
        }
    }
}
