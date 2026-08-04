using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class FullOrPartialResourceRepository : RepositoryBaseNew<Fullorpartialresource>, IFullOrPartialResourceRepository
    {
        public FullOrPartialResourceRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

   
        
    }
}