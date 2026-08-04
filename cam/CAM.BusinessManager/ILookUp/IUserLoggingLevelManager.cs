using CAM.DataTransferObjects;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CAM.BusinessManager.LookUp.UserLoggingLevelManager;

namespace CAM.BusinessManager.ILookUp
{
    public interface IUserLoggingLevelManager
    {
        Task<ResultDto> AddOrEdit(UserLoggingLevelDto dto);
        UsersLoggingLevelsDtoGrid GetCreatePage();
        Task<string> GetCurrentLogLevel();

        long GetPageSize();

        Task<UserPagePrefrenceDetails> GetUserPrefrenceDetails(long userId);
        Task<List<AuthRoleDetailDto>> GetRoleDetails(long userId);
    }
}
