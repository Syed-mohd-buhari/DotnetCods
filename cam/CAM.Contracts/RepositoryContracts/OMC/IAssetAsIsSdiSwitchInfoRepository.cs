using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.OMC
{
    public interface IAssetAsIsSdiSwitchInfoRepository : IRepositoryBase<Assetasissdiswitchinfo>
    {
        void Detach();
    }
}
