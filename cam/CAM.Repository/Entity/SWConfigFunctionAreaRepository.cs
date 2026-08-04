using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class SWConfigFunctionAreaRepository : RepositoryBase<Swconfigfunctionareas>, ISWConfigFunctionAreaRepository
    {
        public SWConfigFunctionAreaRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
