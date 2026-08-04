using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.VBom
{
    public class VnfNameRepository : RepositoryBaseNew<Vnfname>, IVnfNameRepository
    {
        ModelContextNew _context;
        public VnfNameRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
