using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Repository;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.NewRepositoryWrapper.LookUp
{
    public class SWConfigSubFunctionAreasRepository : RepositoryBaseNew<Swconfigsubfunctionareas>, ISWConfigSubFunctionAreasRepository
    {
        public SWConfigSubFunctionAreasRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {

        }
    }
}
