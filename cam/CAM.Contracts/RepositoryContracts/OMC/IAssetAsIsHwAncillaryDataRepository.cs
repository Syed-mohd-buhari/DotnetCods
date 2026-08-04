using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.OMC
{
    public interface IAssetAsIsHwAncillaryDataRepository : IRepositoryBase<Assetasishwancillarydata>
    {
        void Detach();
    }
}
