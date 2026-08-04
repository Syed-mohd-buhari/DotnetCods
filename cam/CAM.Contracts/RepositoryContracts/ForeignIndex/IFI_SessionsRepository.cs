using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models.ForeignIndex;
using System.Collections.Generic;
using System.Threading.Tasks;
using OracleModels.DBModels;
namespace CAM.Contracts.RepositoryContracts.ForeignIndex
{
    public interface IFI_SessionsRepository : IRepositoryBase<FiSessions>
    {
        Task<IEnumerable<FiSessions>> GetAllWithRelations();
    }
}