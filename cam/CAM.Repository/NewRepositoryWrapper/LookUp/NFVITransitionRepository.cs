using System.Linq;
using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
   public class NFVITransitionRepository : RepositoryBaseNew<Nfvitransitions>, INFVITransitionRepository
    {
        public NFVITransitionRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public IQueryable<Nfvitransitions> GetAllWithRelations()
        {
            return FindAll();
        }

        
    }
}
