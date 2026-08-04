using CAM.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts
{
    public interface IVNFDesignComponentRepository   : IRepositoryBase<Vfndesigncomponents>
    {
        Task<IEnumerable<Vfndesigncomponents>> GetAllWithRelations();
    }
}
