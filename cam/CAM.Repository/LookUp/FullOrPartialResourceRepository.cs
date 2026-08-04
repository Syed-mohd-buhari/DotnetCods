using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class FullOrPartialResourceRepository : RepositoryBase<Fullorpartialresource>, IFullOrPartialResourceRepository
    {
        public FullOrPartialResourceRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

   
        
    }
}