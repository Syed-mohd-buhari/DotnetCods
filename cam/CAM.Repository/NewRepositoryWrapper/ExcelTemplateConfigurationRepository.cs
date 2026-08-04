using CAM.Contracts.RepositoryContracts;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper
{
    public class ExcelTemplateConfigurationRepository : RepositoryBaseNew<Exceltemplateconfiguration>, IExcelTemplateConfigurationRepository
    {
        public ExcelTemplateConfigurationRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

    }
}