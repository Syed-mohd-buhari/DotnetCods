using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.VBom
{
    public class VmWorkLoadTypeRepository : RepositoryBase<Vmworkloadtype>, IVmWorkLoadTypeRepository
    {
        ModelContext _context;
        public VmWorkLoadTypeRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}