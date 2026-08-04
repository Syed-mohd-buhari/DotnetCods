using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Repository;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.NewRepositoryWrapper.LookUp
{
    public class SWConfigSubFunctionRepository : RepositoryBaseNew<Swconfigsubfunction>, ISWConfigSubFunctionRepository
    {
        public SWConfigSubFunctionRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {

        }
    }
}
