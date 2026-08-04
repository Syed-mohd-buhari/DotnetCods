using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.LookUp
{
    public class VodafoneNameRepository : RepositoryBase<Vodafonenames>, IVodafoneNameRepository
    {
        public VodafoneNameRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Vodafonenames>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }

}
