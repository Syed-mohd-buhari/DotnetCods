using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.Identity
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(int userId);
        Task<List<string>> GetUserRuleAsync(int userId);

        Task<int> GetUserIdByEmailAsync(string email);

        Task<(bool Result, int UserId)> CreateUserAsync(string userName, string password);

        Task<bool> DeleteUserAsync(int userId);
        Task<List<IdentityUserClaim<int>>> GetUserClaim(int userId, string claimType);
        Task<IList<ApplicationUser>> GetUsersFromClaim(string claimType, int claimValue);
    }
}
