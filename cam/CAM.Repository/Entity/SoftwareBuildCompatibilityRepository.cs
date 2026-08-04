using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class SoftwareBuildCompatibilityRepository : RepositoryBase<Softwarebuildcompatibility>, ISoftwareBuildCompatibilityRepository
    {

        public SoftwareBuildCompatibilityRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
    

   
}
