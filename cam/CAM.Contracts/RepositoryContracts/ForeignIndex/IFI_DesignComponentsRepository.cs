using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models.ForeignIndex;
using System.Collections.Generic;
using System.Threading.Tasks;
using OracleModels.DBModels;
namespace CAM.Contracts.RepositoryContracts.ForeignIndex
{
    public interface IFI_DesignComponentsRepository : IRepositoryBase<FiDesigncomponents>
    {
        Task<IEnumerable<FiDesigncomponents>> GetAllWithRelations();
    }
}