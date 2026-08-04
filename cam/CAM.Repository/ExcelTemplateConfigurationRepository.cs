using CAM.Contracts.RepositoryContracts;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository
{
    public class ExcelTemplateConfigurationRepository : RepositoryBase<Exceltemplateconfiguration>, IExcelTemplateConfigurationRepository
    {
        public ExcelTemplateConfigurationRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

    }
}
 