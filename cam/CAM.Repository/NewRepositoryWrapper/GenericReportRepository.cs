using CAM.Contracts.RepositoryContracts;
using CAM.Entities;
using CAM.Entities.Models;
using CAM.Entities.Models.Engine;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper
{
    public class GenericReportRepository : RepositoryBaseNew<Dynamicreports>, IGenericReportRepository
    {
        public GenericReportRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        


    }
}