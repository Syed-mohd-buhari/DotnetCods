using CAM.Contracts.RepositoryContracts.Base;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Contracts.RepositoryContracts.Entity
{
    public interface IMajorHardwareBuildAsIsRepository : IRepositoryBase<Majorhardwarebuildasis>
    {
        IQueryable<Majorhardwarebuildasis> GetAllWithRelations();
        void Detach();
    }
}