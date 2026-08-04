using CAM.Contracts.RepositoryContracts.VBom;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.VBom
{
    internal class IntraVmTypeRepository : RepositoryBaseNew<Intravmtype>, IIntraVmTypeRepository
    {
        ModelContextNew _context;
        public IntraVmTypeRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }


    }
}