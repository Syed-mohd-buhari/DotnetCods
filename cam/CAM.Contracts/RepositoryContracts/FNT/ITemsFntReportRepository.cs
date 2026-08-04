using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.Entity
{
    public interface ITemsFntReportRepository : IRepositoryBase<Temsfntreport>
    {
        void Detach();
    }
}
