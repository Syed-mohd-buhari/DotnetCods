using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.ComponentSoftware
{
    public class ComponentSoftwareBuildBagRepository : RepositoryBase<Componentsoftwarebuildbags>, IComponentSoftwareBuildBagRepository
    {
        ModelContext _context;
        public ComponentSoftwareBuildBagRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
 
       
    }
}
