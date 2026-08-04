using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.Entity
{
    public interface INFVIBundleIDRepository : IRepositoryBase<Nfvibundleids>
    {
        Task<IEnumerable<Nfvibundleids>> GetAllWithRelations();
    }
}
