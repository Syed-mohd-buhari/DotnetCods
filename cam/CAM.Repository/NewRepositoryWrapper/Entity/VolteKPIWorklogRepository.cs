//using CAM.Contracts.DBContext;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class VolteKPIWorklogRepository : RepositoryBaseNew<Voltekpiworklog>, IVolteKPIWorklogRepository
    {
        public VolteKPIWorklogRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
