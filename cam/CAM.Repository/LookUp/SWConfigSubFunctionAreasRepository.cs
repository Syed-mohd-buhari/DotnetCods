using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class SWConfigSubFunctionAreasRepository : RepositoryBase<Swconfigsubfunctionareas>, ISWConfigSubFunctionAreasRepository
    {
        public SWConfigSubFunctionAreasRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
