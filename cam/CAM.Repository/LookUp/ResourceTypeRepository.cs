using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class ResourceTypeRepository : RepositoryBase<Resourcetypes>, IResourceTypeRepository
    {
        public ResourceTypeRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

       
        
    }
}