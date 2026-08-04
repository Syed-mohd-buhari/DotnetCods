using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.Entity
{
    public interface IPlannedActivityRepository : IRepositoryBase<Plannedactivities>
    {
        void Detach();
    }
}