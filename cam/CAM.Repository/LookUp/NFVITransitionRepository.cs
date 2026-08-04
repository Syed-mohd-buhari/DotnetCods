using System.Linq;
using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
   public class NFVITransitionRepository : RepositoryBase<Nfvitransitions>, INFVITransitionRepository
    {
        public NFVITransitionRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public IQueryable<Nfvitransitions> GetAllWithRelations()
        {
            return FindAll();
        }

        
    }
}
