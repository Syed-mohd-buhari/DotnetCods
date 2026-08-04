using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class PlannedActivityCategoryRepository : RepositoryBase<Plannedactivitycategory>, IPlannedActivityCategoryRepository
    {
        public PlannedActivityCategoryRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

    }
}
