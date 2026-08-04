using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class SWConfigFunctionAreaRepository : RepositoryBaseNew<Swconfigfunctionareas> ,ISWConfigFunctionAreaRepository
    {
        public SWConfigFunctionAreaRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
