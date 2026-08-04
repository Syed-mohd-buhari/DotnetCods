using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.ComponentSoftware
{
    public class BuildBagRepository : RepositoryBase<Buildbags>, IBuildBagRepository
    {
        ModelContext _context;
        public BuildBagRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
 
       
    }
}
