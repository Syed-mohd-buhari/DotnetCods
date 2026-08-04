using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class VolteKPIRepository : RepositoryBase<Voltekpi>, IVolteKPIRepository
    {
        public VolteKPIRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
