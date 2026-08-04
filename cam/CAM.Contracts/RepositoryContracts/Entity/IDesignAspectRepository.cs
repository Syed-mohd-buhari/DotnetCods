using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Contracts.RepositoryContracts.Entity
{
    public interface IDesignAspectRepository : IRepositoryBase<Designaspects>
    {
        Task<IEnumerable<Designaspects>> GetAllWithRelations();
        void Detach();

    }
}
