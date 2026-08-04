using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class FeedBackLoopAuditRepository : RepositoryBase<Feedbackloopaudits>, IFeedBackLoopAuditRepository
    {
        public FeedBackLoopAuditRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
