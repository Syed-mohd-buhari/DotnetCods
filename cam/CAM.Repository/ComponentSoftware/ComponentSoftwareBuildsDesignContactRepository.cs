using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.ComponentSoftware
{
    public class ComponentSoftwareBuildsDesignContactRepository : RepositoryBase<Componentsoftwarebuildsdesigncontacts>, IComponentSoftwareBuildsDesignContactRepository
    {
        public ComponentSoftwareBuildsDesignContactRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            
        }
    }
}
