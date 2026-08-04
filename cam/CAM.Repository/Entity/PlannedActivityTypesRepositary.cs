using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;


namespace CAM.Repository.Entity
{
    public class PlannedActivityTypesRepositary : RepositoryBase<Plannedactivitytypes>, IPlannedActivityTypesRepository
    {
        public PlannedActivityTypesRepositary(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
