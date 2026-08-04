using System.Linq;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.Entity
{
    public interface IMajorSoftwareBuildRepository : IRepositoryBase<Majorsoftwarebuilds>
    {
        IQueryable<Majorsoftwarebuilds> GetAllWithRelations();
        void Detach();
    }
}