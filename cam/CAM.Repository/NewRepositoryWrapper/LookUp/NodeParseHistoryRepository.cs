using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class NodeParseHistoryRepository : RepositoryBaseNew<Nodeparsehistory>, INodeParseHistoryRepository
    {
        public NodeParseHistoryRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {

        }
    }
}
