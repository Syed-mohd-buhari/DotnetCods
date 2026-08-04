using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.ComponentSoftware
{
    public class BuildBagRepository : RepositoryBaseNew<Buildbags>, IBuildBagRepository
    {
        ModelContextNew _context;
        public BuildBagRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
 
       
    }
}
