using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.LookUp
{
    public class RepositoryLcmDBExportUpdateHistory : RepositoryBase<Lcmdbexportupdatehistory>, IRepositoryLcmDBExportUpdateHistory
    {
        public RepositoryLcmDBExportUpdateHistory(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Lcmdbexportupdatehistory>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
