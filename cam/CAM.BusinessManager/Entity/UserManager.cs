//using CAM.Contracts.DBModels;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using CAM.Repository.NewRepositoryWrapper;
using Microsoft.AspNetCore.Http;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CAM.BusinessManager.Entity
{
    public class UserManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly ModelContextNew _contextNew;

        public UserManager(IEnumerable<IRepositoryWrapper> wrappers, IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper, ModelContextNew modelContextNew) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _contextNew = modelContextNew;
        }
        public int GetUserIdByMail(string mail)
        {
            string mode = "normal";
            GlobalDbMode.DbMode.TryGetValue(mail.ToLower(), out mode);
            if (mode == "training")
            {
                return new RepositoryWrapperNew(_contextNew).UserRepository.FindByCondition(p => p.Email.ToLower().Trim() == mail.ToLower().Trim()).Select(x => x.Id).FirstOrDefault();

            }
            else
            {
              return _repositoryWrapper.UserRepository.FindByCondition(p => p.Email.ToLower().Trim() == mail.ToLower().Trim()).Select(x => x.Id).FirstOrDefault();
            }

        }
        public UserInfoModel GetUserInfo(string email)
        {
            string mode = "normal";
            GlobalDbMode.DbMode.TryGetValue(email.ToLower(), out mode);
            if (mode == "training")
            {
                return GetUserInforTraining(email);
            }
            else
            {
                return GetUserInforNormal(email);

            }

        }

        public UserInfoModel GetUserInfo(int userId)
        {
            var user = _repositoryWrapper.UserRepository.FindByCondition(p => p.Id == userId).FirstOrDefault();
            if (user != null)
            {
                var userRoles = _repositoryWrapper.UserRoleRepository.FindByCondition(p => p.Userid == user.Id).ToList();
                if (userRoles != null)
                {
                    var roles = _repositoryWrapper.RoleRepository.FindByCondition(k => userRoles.Select(p => p.Roleid).ToList().Contains(k.Id)).ToList();
                    if (roles != null)
                    {
                        var data = new UserInfoModel() { IsExist = true, ErrorMessage = null, Email = user.Email, RoleId = roles.Select(p => p.Id).ToList(), RolesName = roles.Select(p => p.Name).ToList(), UserID = user.Id };
                        return data;
                    }
                    else
                    {
                        return new UserInfoModel() { IsExist = false, ErrorMessage = "This user not assigned to any roles on TEMS Application , Please contact the administrator" };
                    }
                }
            }
            else
            {
                return new UserInfoModel() { IsExist = false, ErrorMessage = "This user not exist on TEMS Application , Please contact the administrator" };
            }
            return null;
        }

        public List<short> GetUserClaim(int userId, string claimType)
        {
            var user = _repositoryWrapper.UserRepository.FindByCondition(u => u.Id == userId).FirstOrDefault();
            if (user != null)
            {
                var claims = _repositoryWrapper.ClaimsRepository.FindByCondition(x => x.Userid == user.Id && x.Claimtype == claimType.ToLower()).ToList();
                return claims.Any() ? claims.Select(x => short.Parse(x.Claimvalue)).ToList() : null;
            }
            return null;
        }


        public List<Aspnetusers> GetKPIAdminForOpCo(int opCoId)
        {
            var userCLaims = _repositoryWrapper.ClaimsRepository.FindByCondition(p => p.Claimtype == "opco" && p.Claimvalue == opCoId.ToString()).Select(p => p.Userid).ToList();
            List<Aspnetusers> users = _repositoryWrapper.UserRepository.FindByCondition(p => userCLaims.Contains(p.Id)).Include(p => p.Aspnetuserroles).ToList();
            if (users != null && users.Count > 0)
            {

                return users.Where(x => x.Aspnetuserroles.Any(xx => xx.Role.Normalizedname.ToLower() == "kpi administrator")).ToList();
            }
            return new List<Aspnetusers>();
        }


        private UserInfoModel GetUserInforNormal(string email)
        {
            var user = _repositoryWrapper.UserRepository.FindByCondition(p => p.Email.ToLower().Trim() == email.ToLower().Trim() || p.Normalizedemail.ToLower().Trim() == email.ToLower().Trim()).FirstOrDefault();
            if (user != null)
            {
                var userRoles = _repositoryWrapper.UserRoleRepository.FindByCondition(p => p.Userid == user.Id).ToList();
                if (userRoles != null)
                {
                    var roles = _repositoryWrapper.RoleRepository.FindByCondition(k => userRoles.Select(p => p.Roleid).ToList().Contains(k.Id)).ToList();
                    if (roles != null)
                    {
                        var data = new UserInfoModel() { IsExist = true, ErrorMessage = null, Email = user.Email.ToLower(), RoleId = roles.Select(p => p.Id).ToList(), RolesName = roles.Select(p => p.Name).ToList(), UserID = user.Id };
                        return data;
                    }
                    else
                    {
                        return new UserInfoModel() { IsExist = false, ErrorMessage = "This user not assigned to any roles on TEMS Application , Please contact the administrator" };
                    }
                }
            }
            else
            {
                return new UserInfoModel() { IsExist = false, ErrorMessage = "This user not exist on TEMS Application , Please contact the administrator" };
            }
            return null;
        }


        private UserInfoModel GetUserInforTraining(string email)
        {
            var user = new RepositoryWrapperNew(_contextNew).UserRepository.FindByCondition(p => p.Email.ToLower().Trim() == email.ToLower().Trim() || p.Normalizedemail.ToLower().Trim() == email.ToLower().Trim()).FirstOrDefault();
            if (user != null)
            {
                var userRoles = new RepositoryWrapperNew(_contextNew).UserRoleRepository.FindByCondition(p => p.Userid == user.Id).ToList();
                if (userRoles != null)
                {
                    var roles = new RepositoryWrapperNew(_contextNew).RoleRepository.FindByCondition(k => userRoles.Select(p => p.Roleid).ToList().Contains(k.Id)).ToList();
                    if (roles != null)
                    {
                        var data = new UserInfoModel() { IsExist = true, ErrorMessage = null, Email = user.Email.ToLower(), RoleId = roles.Select(p => p.Id).ToList(), RolesName = roles.Select(p => p.Name).ToList(), UserID = user.Id };
                        return data;
                    }
                    else
                    {
                        return new UserInfoModel() { IsExist = false, ErrorMessage = "This user not assigned to any roles on TEMS Application , Please contact the administrator" };
                    }
                }
            }
            else
            {
                return new UserInfoModel() { IsExist = false, ErrorMessage = "This user not exist on TEMS Application , Please contact the administrator" };
            }
            return null;
        }
    }
}
