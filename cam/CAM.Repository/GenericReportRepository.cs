using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models;
using CAM.Entities.Models.Engine;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository
{
    public class GenericReportRepository : RepositoryBase<Dynamicreports>, IGenericReportRepository
    {
        public GenericReportRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

    }
}