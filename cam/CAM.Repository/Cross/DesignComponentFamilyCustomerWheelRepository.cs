using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Cross;
using CAM.Entities;
using CAM.Entities.Models.Cross;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Cross
{
    public class DesignComponentFamilyCustomerWheelRepository : RepositoryBase<Designcomponentfamilycustomerwheel>, IDesignComponentFamilyCustomerWheelRepository
    {
        public DesignComponentFamilyCustomerWheelRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}