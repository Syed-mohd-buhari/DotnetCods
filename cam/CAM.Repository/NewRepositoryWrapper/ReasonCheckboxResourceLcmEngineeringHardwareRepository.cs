using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper
{
    public class ReasonCheckboxResourceLcmEngineeringHardwareRepository : RepositoryBaseNew<Reasoncheckboxresourcelcmengineeringhardware>, IReasonCheckboxResourceLcmEngineeringHardwareRepository
    {
        public ReasonCheckboxResourceLcmEngineeringHardwareRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }



    }
}