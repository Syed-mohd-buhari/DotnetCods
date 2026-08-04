using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.ComponentSoftware
{
    public class ComponentSoftwareBuildRepository : RepositoryBaseNew<Componentsoftwarebuilds>, IComponentSoftwareBuildRepository
    {
        ModelContextNew _context;
        public ComponentSoftwareBuildRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        } 
        
    }
}

