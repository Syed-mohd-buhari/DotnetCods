using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class NodeParseHistoryRepository : RepositoryBase<Nodeparsehistory>, INodeParseHistoryRepository
    {
        public NodeParseHistoryRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
