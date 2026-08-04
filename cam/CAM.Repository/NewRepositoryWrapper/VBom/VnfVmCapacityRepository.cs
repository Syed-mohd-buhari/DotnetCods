using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.VBom
{
    public class VnfVmCapacityRepository : RepositoryBaseNew<Vnfvmcapacity>, IVnfVmCapacityRepository
    {
        ModelContextNew _context;
        public VnfVmCapacityRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
