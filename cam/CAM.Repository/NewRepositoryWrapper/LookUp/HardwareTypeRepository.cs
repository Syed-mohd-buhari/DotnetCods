using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class HardwareTypeRepository : RepositoryBaseNew<Hardwaretypes>, IHardwareTypeRepository
    {
        public HardwareTypeRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Hardwaretypes>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
