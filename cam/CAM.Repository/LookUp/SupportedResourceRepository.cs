using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class SupportedResourceRepository : RepositoryBase<Supportedresource>, ISupportedResourceRepository
    {
        public SupportedResourceRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

      
    }
}