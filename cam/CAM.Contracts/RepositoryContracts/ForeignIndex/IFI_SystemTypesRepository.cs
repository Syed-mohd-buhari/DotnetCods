using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models.ForeignIndex;
using System.Collections.Generic;
using System.Threading.Tasks;
using OracleModels.DBModels;
namespace CAM.Contracts.RepositoryContracts.ForeignIndex
{
    public interface IFI_SystemTypesRepository : IRepositoryBase<FiSystemtypes>
    {
        Task<IEnumerable<FiSystemtypes>> GetAllWithRelations();
    }
}