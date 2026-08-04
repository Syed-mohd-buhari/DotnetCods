using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper
{
    public class PlannedActivityResourceBenefitRepository : RepositoryBaseNew<Plannedactivityresourcebenefit>, IPlannedActivityResourceBenefitRepository
    {
        public PlannedActivityResourceBenefitRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}