using System.Linq;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Contracts.RepositoryContracts.Entity
{
    public interface IMajorHardwareBuildRepository : IRepositoryBase<Majorhardwarebuilds>
    {
        IQueryable<Majorhardwarebuilds> GetAllWithRelations();
        void Detach();
    }
}