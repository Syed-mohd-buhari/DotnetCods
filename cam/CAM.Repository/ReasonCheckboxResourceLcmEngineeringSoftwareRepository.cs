using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository
{
    public class ReasonCheckboxResourceLcmEngineeringSoftwareRepository : RepositoryBase<Reasoncheckboxresourcelcmengineeringsoftware>, IReasonCheckboxResourceLcmEngineeringSoftwareRepository
    {
        public ReasonCheckboxResourceLcmEngineeringSoftwareRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }



    }
}