using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.RBAC
{
    public class TeamMemberRepository : RepositoryBaseNew<Teammembers>, ITeamMemberRepository
    {
        ModelContextNew _context;
        public TeamMemberRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
