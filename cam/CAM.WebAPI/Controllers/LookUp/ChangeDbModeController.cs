using CAM.Contracts;
using CAM.Repository.Helpers;
using CAM.WebAPI.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]


    public class ChangeDbModeController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        public IConfiguration Configuration;
        public ConnectionStrings ConnectionStrings { get; set; }
        public IHttpContextAccessor contextAccessor;
        public ChangeDbModeController(
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor,
            ICurrentUserService currentUserService,
            IOptionsSnapshot<ConnectionStrings> ConnectionStrings = null,
            IConfiguration Configuration = null) : base(logger, contextAccessor)
        {
            if (ConnectionStrings != null) this.ConnectionStrings = ConnectionStrings.Value;
            this.Configuration = Configuration;
            _currentUserService = currentUserService;
            this.contextAccessor = contextAccessor;
        }

        [HttpPost]
        public async void Post([FromBody] ChangeDbModeObject mode)
        {
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value?.ToLower() : null;
            try
            {
                if (GlobalDbMode.DbMode.ContainsKey(email))
                    GlobalDbMode.DbMode[email] = mode.mode;
                else
                    GlobalDbMode.DbMode.Add(email, mode.mode);

            }
            catch (Exception ex)
            {
                _logger.LogError("Change Db Mode Exception: " + ex);
            }
        }

    }
}
