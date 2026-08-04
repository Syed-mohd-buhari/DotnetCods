using AutoMapper;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.Enum;
using CAM.Repository;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CAM.BusinessManager.Entity
{
    public class AuthorizedRoleManager : BaseManager
    {
        private readonly ModelContext _modelContext;
        private readonly IRepositoryWrapper _repositoryWrapper;
        List<string> userRoleList = new List<string> { "admin", "temsfunctionaladmin" };
        List<string> userKpiRoleList = new List<string> { "admin", "temsfunctionaladmin" , "kpiadministrator","kpieditor" };
        List<string> userKpiAdminRoleList = new List<string> { "admin", "temsfunctionaladmin", "kpiadministrator"};
        List<string> managerList = new List<string> {"manager"};
        private readonly CommonDapperRepository _commonDapperRepository;
        private readonly string dapperDatabaseMode = "normal";
        public AuthorizedRoleManager(IEnumerable<IRepositoryWrapper> wrappers,
            IMapper mapper, IRepositoryWrapper repositoryWrapper,
           IHttpContextAccessor contextAccessor, ILoggerManager logger, CommonDapperRepository commonDapperRepository
          ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _commonDapperRepository = commonDapperRepository;
            _repositoryWrapper = repositoryWrapper;
            dapperDatabaseMode = GlobalDbMode.DbMode.ContainsKey(CurrentLogLevelConfig._UserName) ? GlobalDbMode.DbMode[CurrentLogLevelConfig._UserName] : "normal";
        }

        public List<AspNetUserROVGridDto> GetUserRoleOpcoList(long userId)
        {
            return new List<AspNetUserROVGridDto>();
            //RolebasedAccessCommented
            //return null;
            //List<AspNetUserROVGridDto> resultUserRole = new List<AspNetUserROVGridDto>();


            //resultUserRole = _repositoryWrapper.UserRoleRepository.
            // FindByCondition(x => x.Userid == userId && x.Deleted == false  ).

            //   Select(x => new
            //AspNetUserROVGridDto
            //   {
            //       Role = x.Roleid.ToString(),
            //       OpCo = x.Opcoid.ToString(),
            //       VerticalResponsible = x.Verticalresponsibleid.ToString(),
            //   }).ToList();



        }

        public List<int> GetUsersListBasedOnVerticalId(List<int> verticalIds , bool isAdmin)
        {
            var users = new List<int>();
            var userlist = _repositoryWrapper.UserRepository.FindAll().Include(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical);

            if (isAdmin)
            {
                users = userlist.Select(x => x.Id).Distinct().ToList();
            }
            else
            {
                users = userlist.Where(x => (verticalIds != null && verticalIds.Count > 0) &&  x.AspnetuserverticalsUser.Any(f => verticalIds.Contains(f.Organisation.Vertical.Verticalresponsibleid) == true) && x.Deleted == false)
                        .Select(x => x.Id)
                        .Distinct()
                        .ToList();
            }

            return users;
        }
        public async Task<AspNetUserRoleRBODto> GetUserRoleDetails(long userId)
        {
            var result = new AspNetUserRoleRBODto();

            var UserData = await _repositoryWrapper.UserRepository
                .FindByCondition(x => x.Id == userId && x.Deleted == false)
                .AsNoTracking().Include(x => x.Aspnetuserroles).Include(x => x.AspnetuseropcosUser)
                .Include(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).FirstOrDefaultAsync();
            if(UserData != null && UserData.Aspnetuserroles != null && UserData.Aspnetuserroles.Count > 0 && UserData.Aspnetuserroles.Any(x => x.Roleid == 1 ))
            {

                result = new AspNetUserRoleRBODto
                {
                    UserRoleId = new List<int>() {1},
                };

            }
            else
            {
                result = new AspNetUserRoleRBODto
                {
                    UserRoleId = UserData.Aspnetuserroles != null && UserData.Aspnetuserroles.Count > 0 ? UserData.Aspnetuserroles.Select(x => x.Roleid).ToList() : new List<int>(),
                    OpcoDetails = UserData.AspnetuseropcosUser != null && UserData.AspnetuseropcosUser.Count > 0 ?
                    UserData.AspnetuseropcosUser.Select(x => (short)x.Opcoid).ToList() : new List<short>(),
                    VerticalDetails = UserData.AspnetuserverticalsUser != null && UserData.AspnetuserverticalsUser.Count > 0 ?
                    UserData.AspnetuserverticalsUser.Select(x => (int)x.Organisation.Verticalid).ToList() : new List<int>()
                };

            }
            return result;
        }

        #region // SW Planned Activity Rule
        public Task<IEnumerable<short>> GetSwPlannedActivityRulesAsync()
        {
            return _commonDapperRepository.QueryAsync<short>(
                dapperDatabaseMode,
                @"
            SELECT Plannedactivityresourceid
            FROM plannedactivityresources
            WHERE rulelinkeddc IN :RuleLinkedDc",
                new
                {
                    RuleLinkedDc = PlannedActivityResourceEnum
                        .SwArchitectureUpgrade_SwMajorRelease
                });
        }
        #endregion

        public async Task<AspNetUserRoleRBODto> GetUserRoleDetailsUsingDapper(long userId,bool isNewPortalLogin = false)
        {

            //var swOwnerId = await _commonDapperRepository.QueryFirstOrDefaultAsync<long>(
            //            dapperDatabaseMode,
            //            @"SELECT Id
            //                  FROM aspnetroles where name ='SW Product Owner'
            //                  AND Deleted = 0", null
            //             );


            var user = await _commonDapperRepository.QueryFirstOrDefaultAsync<string>(
                dapperDatabaseMode,
                @"SELECT Id
                  FROM AspNetUsers
                  WHERE Id = :userId
                  AND Deleted = 0",
                new { userId });

            var rolesRecords = (await _commonDapperRepository.QueryAsync<RoleDto>(
                dapperDatabaseMode,
                @"SELECT u.RoleId ,r.Name as RoleName , r.portalroleid as PortalRoleId
                     FROM AspNetUserRoles u
                     join aspnetroles r on r.id = u.ROLEID 
                     WHERE UserId = :userId",
                            new { userId })).ToList();

            var opcos = (await _commonDapperRepository.QueryAsync<short>(
                dapperDatabaseMode,
                @"SELECT CAST(OpcoId AS NUMBER(5))
                  FROM AspNetUserOpcos
                  WHERE UserId = :userId",
                            new { userId })).ToList();

            var verticals = (await _commonDapperRepository.QueryAsync<int>(
                dapperDatabaseMode,
                @"SELECT DISTINCT O.VerticalId
                  FROM AspNetUserVerticals UV
                  INNER JOIN Organisation O
                  ON UV.OrganisationId = O.OrganisationId
                  WHERE UV.UserId = :userId",
                            new { userId })).ToList();
 
            if (user == null)
            {
                return new AspNetUserRoleRBODto();
            }
            else  if (!isNewPortalLogin && rolesRecords.Any(x => x.RoleId == 1))
            {
                return new AspNetUserRoleRBODto
                {
                    UserRoleId = new List<int> { 1 }
                };
            }
 

            return new AspNetUserRoleRBODto
            {
                UserRoleId = rolesRecords.Select(x => (int) x.RoleId).ToList(),
                OpcoDetails = opcos,
                VerticalDetails = verticals,
                RoleRecords = rolesRecords
            };
        }
        #region // Checking Org Spocs

        public async Task<OrganisatioinSpocsDto> GetOrganisationSpocsDetails(long userId)
        {
            var spocs = new OrganisatioinSpocsDto();
            try
            {
                var orgEntity = await _repositoryWrapper.UserRepository.FindByCondition(x => x.Id == userId).FirstOrDefaultAsync();

                if(orgEntity != null)
                {
                    spocs.IsEduSpoc = orgEntity.Iseduspoc;
                    spocs.IsSubdomainSpoc = orgEntity.Issubdomainspoc;
                    spocs.IsUserExistInOrg = true;
                }

                return spocs;
            }
            catch
            {
                return spocs;
            }

        }

        public AspNetUserRoleRBODto GetUserRelevantOpcoVerticalDetails(long userId)
        {
            var result = new AspNetUserRoleRBODto();
            var userRoleData = _repositoryWrapper.UserRepository.FindByCondition(x => x.Id == userId && x.Deleted == false)
                                                                      .Include(x => x.Aspnetuserroles).ThenInclude(x => x.Role)
                                                                      .Include(x => x.AspnetuseropcosUser).ThenInclude(x=>x.Opco)
                                                                      .Include(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation)
                                                                      .Include(x => x.Subdomainresponsible)
                                                                      .FirstOrDefault();

            if (userRoleData != null &&
               (userRoleData.Aspnetuserroles != null && userRoleData.Aspnetuserroles.Any(x => userRoleList.Contains(x.Role.Name.Replace(" ", "").ToLower()))
               ))
            {
                result.UserRoleId = new List<int> { 1 };
                result.IsAdmin = true;
                result.IsKpiAdmin = true;
                result.IsKpiUser= true;
            }
            else
            {
                if (userRoleData != null &&
              (userRoleData.Aspnetuserroles != null && userRoleData.Aspnetuserroles.Any(x => managerList.Contains(x.Role.Name.Replace(" ", "").ToLower()))))
                {
                    result.IsManager = true;                  
                }
                if (userRoleData != null &&
              (userRoleData.Aspnetuserroles != null && userRoleData.Aspnetuserroles.Any(x => userKpiRoleList.Contains(x.Role.Name.Replace(" ", "").ToLower()))))
                {
                    result.IsKpiUser = true;
                }
                if (userRoleData != null &&
             (userRoleData.Aspnetuserroles != null && userRoleData.Aspnetuserroles.Any(x => userKpiAdminRoleList.Contains(x.Role.Name.Replace(" ", "").ToLower()))))
                {
                    result.IsKpiAdmin = true;
                }
                var UserOpcoData = userRoleData?.AspnetuseropcosUser != null && userRoleData?.AspnetuseropcosUser.Any() == true ?
                                   userRoleData?.AspnetuseropcosUser.Where(x => x.Isrestrictedopco == false).Select(x =>new { Opcoid=(short)x.Opcoid, Opco=x.Opco.Opco}).Distinct().ToList() : null;

                if (UserOpcoData != null && UserOpcoData.Any() == true)
                {
                    result.OpcoDetails = UserOpcoData.Select(x => x.Opcoid).ToList();
                    result.OpcoDescription = UserOpcoData.Select(x => x.Opco).ToList();
                }

                var UserVerticalDta = userRoleData?.AspnetuserverticalsUser != null && userRoleData?.AspnetuserverticalsUser.Any() == true ?
                                      userRoleData?.AspnetuserverticalsUser.Select(x => (int)x.Organisation.Verticalid).Distinct().ToList() : null;

                if (UserVerticalDta != null && UserVerticalDta.Any() == true)
                    result.VerticalDetails = UserVerticalDta;

                result.SubdomainDetails = userRoleData?.Subdomainresponsibleid;
            }
            return result;
        }
        #endregion
    }
}
