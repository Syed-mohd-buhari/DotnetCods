using CAM.Contracts.RepositoryContracts.Entity;
 
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class SoftwareBuildCompatibilityRepository : RepositoryBaseNew<Softwarebuildcompatibility>, ISoftwareBuildCompatibilityRepository
    {
        public SoftwareBuildCompatibilityRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
