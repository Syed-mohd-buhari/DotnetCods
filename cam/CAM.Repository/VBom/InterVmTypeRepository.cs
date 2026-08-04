using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.VBom
{
    public class InterVmTypeRepository : RepositoryBase<Intervmtype>, IInterVmTypeRepository
    {
        ModelContext _context;
        public InterVmTypeRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
