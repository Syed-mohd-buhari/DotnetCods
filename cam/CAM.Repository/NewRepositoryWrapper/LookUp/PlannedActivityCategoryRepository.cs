using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class PlannedActivityCategoryRepository : RepositoryBaseNew<Plannedactivitycategory>, IPlannedActivityCategoryRepository
    {
        public PlannedActivityCategoryRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
