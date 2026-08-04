using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.LookUp
{
    public interface IOriginalEquipmentManufacturerRepository : IRepositoryBase<Originalequipmentmanufacturers>
    {
        void Detach();
    }
}