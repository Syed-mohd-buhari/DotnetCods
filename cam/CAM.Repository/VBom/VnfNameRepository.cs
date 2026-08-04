using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.VBom
{
    public class VnfNameRepository : RepositoryBase<Vnfname>, IVnfNameRepository
    {
        ModelContext _context;
        public VnfNameRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
