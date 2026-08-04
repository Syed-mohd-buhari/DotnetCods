using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;
namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class GlossaryItemsRepository : RepositoryBaseNew<Glossaryitems>, IGlossaryItemsRepository
    {
        ModelContextNew _context;

        public GlossaryItemsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
