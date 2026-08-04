using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities.Models.Lookup;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.LookUp
{
    public class LcmExportSettingRepository : RepositoryBase<Lcmexportsettings>, ILcmExportSettingRepository
    {
        public LcmExportSettingRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

    }
}
