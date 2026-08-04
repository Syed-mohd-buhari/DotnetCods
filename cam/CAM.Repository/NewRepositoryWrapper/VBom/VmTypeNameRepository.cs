using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper .VBom
{
    public class VmTypeNameRepository : RepositoryBaseNew<Vmtypename>, IVmTypeNameRepository
    {
        ModelContextNew _context;
        public VmTypeNameRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
