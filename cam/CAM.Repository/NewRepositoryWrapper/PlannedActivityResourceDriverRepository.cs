using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper
{
    public class PlannedActivityResourceDriverRepository : RepositoryBaseNew<Plannedactivityresourcedriver>, IPlannedActivityResourceDriverRepository
    {
        public PlannedActivityResourceDriverRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}