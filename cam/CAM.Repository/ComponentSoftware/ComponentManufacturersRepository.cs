using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.ComponentSoftware
{
    public class ComponentManufacturersRepository : RepositoryBase<Componentmanufacturers>, IComponentManufacturersRepository
    {
        ModelContext _context;
        public ComponentManufacturersRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
 
       
    }
}
