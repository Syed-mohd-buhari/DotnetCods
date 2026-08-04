using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.ComponentSoftware
{
    public class ComponentManufacturersRepository : RepositoryBaseNew<Componentmanufacturers>, IComponentManufacturersRepository
    {
        ModelContextNew _context;
        public ComponentManufacturersRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
 
       
    }
}
