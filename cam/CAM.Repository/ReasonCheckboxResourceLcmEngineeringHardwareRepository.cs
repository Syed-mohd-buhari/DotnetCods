using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository
{
    public class ReasonCheckboxResourceLcmEngineeringHardwareRepository : RepositoryBase<Reasoncheckboxresourcelcmengineeringhardware>, IReasonCheckboxResourceLcmEngineeringHardwareRepository
    {
        public ReasonCheckboxResourceLcmEngineeringHardwareRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }



    }
}