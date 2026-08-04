using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.VBom
{
    public class VmTypeNameRepository : RepositoryBase<Vmtypename>, IVmTypeNameRepository
    {
        ModelContext _context;
        public VmTypeNameRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
