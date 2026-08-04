using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class SupportedResourceRepository : RepositoryBaseNew<Supportedresource>, ISupportedResourceRepository
    {
        public SupportedResourceRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

      
    }
}