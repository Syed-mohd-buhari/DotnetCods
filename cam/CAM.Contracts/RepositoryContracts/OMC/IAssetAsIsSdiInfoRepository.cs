using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.OMC
{
    public interface IAssetAsIsSdiInfoRepository : IRepositoryBase<Assetasissdiinfo>
    {
        void Detach();
    }
}
