using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class VolteKPIRepository : RepositoryBaseNew<Voltekpi>, IVolteKPIRepository
    {
        public VolteKPIRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
