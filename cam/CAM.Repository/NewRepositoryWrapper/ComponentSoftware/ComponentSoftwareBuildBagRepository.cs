using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.ComponentSoftware
{
    public class ComponentSoftwareBuildBagRepository : RepositoryBaseNew<Componentsoftwarebuildbags>, IComponentSoftwareBuildBagRepository
    {
        
        public ComponentSoftwareBuildBagRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
             
        }
 
       
    }
} 