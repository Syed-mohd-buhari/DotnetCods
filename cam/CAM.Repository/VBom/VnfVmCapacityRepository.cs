using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.VBom
{
    public class VnfVmCapacityRepository : RepositoryBase<Vnfvmcapacity>, IVnfVmCapacityRepository
    {
        ModelContext _context;
        public VnfVmCapacityRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
