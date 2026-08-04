//using CAM.Contracts.DBContext;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
    public class VolteKPIWorklogRepository : RepositoryBase<Voltekpiworklog>, IVolteKPIWorklogRepository
    {
        public VolteKPIWorklogRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
