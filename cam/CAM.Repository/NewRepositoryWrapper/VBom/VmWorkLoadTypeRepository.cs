using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.VBom
{
    internal class VmWorkLoadTypeRepository : RepositoryBaseNew<Vmworkloadtype>, IVmWorkLoadTypeRepository
    {
        ModelContextNew _context;
        public VmWorkLoadTypeRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}