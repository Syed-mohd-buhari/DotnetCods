using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Repository.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class LcmExportSettingRepository : RepositoryBaseNew<Lcmexportsettings>, ILcmExportSettingRepository
    {
        public LcmExportSettingRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
