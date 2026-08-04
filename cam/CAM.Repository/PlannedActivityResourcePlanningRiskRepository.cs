using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository
{
    public class PlannedActivityResourcePlanningRiskRepository : RepositoryBase<Plannedactivityresourceplanningrisk>, IPlannedActivityResourcePlanningRiskRepository
    {
        public PlannedActivityResourcePlanningRiskRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}