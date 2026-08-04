using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.CBom
{
    public class FunctionStandardNameRepository : RepositoryBaseNew<Functionstandardname>, IFunctionStandardNameRepository
    {
        ModelContextNew _context;
        public FunctionStandardNameRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
