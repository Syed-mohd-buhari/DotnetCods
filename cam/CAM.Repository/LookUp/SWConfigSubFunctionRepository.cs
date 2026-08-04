using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class SWConfigSubFunctionRepository : RepositoryBase<Swconfigsubfunction>, ISWConfigSubFunctionRepository
    {
        public SWConfigSubFunctionRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
