using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class FeedBackLoopAuditRepository : RepositoryBaseNew<Feedbackloopaudits> ,IFeedBackLoopAuditRepository
    {
        public FeedBackLoopAuditRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
