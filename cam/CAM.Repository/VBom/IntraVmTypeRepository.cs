using CAM.Contracts.RepositoryContracts.VBom;
using CAM.Entities.Models.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.VBom
{
    public class IntraVmTypeRepository : RepositoryBase<Intravmtype>, IIntraVmTypeRepository
    {
        ModelContext _context;
        public IntraVmTypeRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
