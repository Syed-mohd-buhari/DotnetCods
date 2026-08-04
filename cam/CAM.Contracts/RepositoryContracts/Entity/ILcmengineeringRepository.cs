using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.Entity
{
    public interface ILcmEngineeringRepository : IRepositoryBase<Lcmengineering>
    {
        Task<IEnumerable<Lcmengineering>> GetAllWithRelations();
        void Detach();
    }
}