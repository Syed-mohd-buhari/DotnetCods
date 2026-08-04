using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.RBAC
{
    public class TeamMemberRepository : RepositoryBase<Teammembers>, ITeamMemberRepository
    {
        ModelContext _context;
        public TeamMemberRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
