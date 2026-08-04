using CAM.Contracts.RepositoryContracts.CBOM;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.CBom
{
    public class FunctionStandardNameRepository : RepositoryBase<Functionstandardname>, IFunctionStandardNameRepository
    {
        ModelContext _context;
        public FunctionStandardNameRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
