using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.VBom
{
    public class InterVmTypeRepository : RepositoryBaseNew<Intervmtype>, IInterVmTypeRepository
    {
        ModelContextNew _context;
        public InterVmTypeRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}
