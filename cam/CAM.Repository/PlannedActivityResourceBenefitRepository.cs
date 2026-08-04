using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository
{
    public class PlannedActivityResourceBenefitRepository : RepositoryBase<Plannedactivityresourcebenefit>, IPlannedActivityResourceBenefitRepository
    {
        public PlannedActivityResourceBenefitRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}