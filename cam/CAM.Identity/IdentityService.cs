using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CAM.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> GetUserNameAsync(int userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
            return user.UserName;
        }
        public async Task<List<string>> GetUserRuleAsync(int userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
            return (await _userManager.GetRolesAsync(user)).ToList();
        }


        public async Task<int> GetUserIdByEmailAsync(string email)
        {
            email = email.ToUpper();
            var user = await _userManager.Users.SingleAsync(u => u.NormalizedEmail == email);
            return user.Id;
        }

        public async Task<(bool Result, int UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };
            var result = await _userManager.CreateAsync(user, password);
            return (result.Succeeded, user.Id);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                return await DeleteUserAsync(user);
            }
            return true;
        }

        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<List<IdentityUserClaim<int>>> GetUserClaim(int userId, string claimType)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
            var claims = (await _userManager.GetClaimsAsync(user)).Where(x => x.Type.ToLower() == claimType.ToLower());

            return claims.Select(x =>
            {
                var res = new IdentityUserClaim<int>();
                res.InitializeFromClaim(x);
                return res;
            }).ToList();
        }
        public async Task<IList<ApplicationUser>> GetUsersFromClaim(string claimType, int claimValue)
        {
            //var users = await _userManager.GetUsersForClaimAsync(new Claim(claimType, claimValue.ToString()));
            var users = await _userManager.Users
                .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .Include(x => x.Claims)
                .Where(x => x.Claims.Any(xx => xx.ClaimType == claimType && xx.ClaimValue == claimValue.ToString())).ToListAsync();
            
            return users;

            //.GetClaimsAsync(user)).Where(x => x.Type.ToLower() == claimType.ToLower());

            //return claims.Select(x =>
            //{
            //    var res = new IdentityUserClaim<int>();
            //    res.InitializeFromClaim(x);
            //    return res;
            //}).ToList();
        }
    }
}
