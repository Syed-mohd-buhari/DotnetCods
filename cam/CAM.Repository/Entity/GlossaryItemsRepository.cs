using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;
namespace CAM.Repository.Entity
{
    public class GlossaryItemsRepository : RepositoryBase<Glossaryitems>, IGlossaryItemsRepository
    {
        ModelContext _context;

        public GlossaryItemsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
