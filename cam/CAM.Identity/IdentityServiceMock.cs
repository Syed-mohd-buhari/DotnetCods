using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.Identity
{
    public class IdentityServiceMock : IIdentityService
    {
        public Task<string> GetUserNameAsync(int userId) => new Task<string>(() => "TestUser");
        public async Task<List<string>> GetUserRuleAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> GetUserIdByEmailAsync(string email)
        {
            throw new System.NotImplementedException();
        }


        public Task<(bool Result, int UserId)> CreateUserAsync(string userName, string password)
        {
            return new Task<(bool Result, int UserId)>(() => (true, 1));
        }

        public Task<bool> DeleteUserAsync(int userId)
        {
            return new Task<bool>(() => true);
        }

        public Task<List<IdentityUserClaim<int>>> GetUserClaim(int userId, string claimType)
        {
            return new Task<List<IdentityUserClaim<int>>>(() => new List<IdentityUserClaim<int>>() { new IdentityUserClaim<int> { Id = 1, UserId = 1, ClaimType = "opco", ClaimValue = "1" } });
        }
        public Task<IList<ApplicationUser>> GetUsersFromClaim(string claimType, int claimValue)
        {
            return new Task<IList<ApplicationUser>>(() => new List<ApplicationUser>() { new ApplicationUser() { }  });
        }
    }
}