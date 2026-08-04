using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.ComponentSoftware
{
    public class ComponentSoftwareBuildsDesignContactRepository : RepositoryBaseNew<Componentsoftwarebuildsdesigncontacts>, IComponentSoftwareBuildsDesignContactRepository
    {
        public ComponentSoftwareBuildsDesignContactRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            
        }
    }
}
