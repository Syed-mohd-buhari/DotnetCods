using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class RepositoryLcmDBExportUpdateHistory : RepositoryBaseNew<Lcmdbexportupdatehistory>, IRepositoryLcmDBExportUpdateHistory
    {
        public RepositoryLcmDBExportUpdateHistory(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Lcmdbexportupdatehistory>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
