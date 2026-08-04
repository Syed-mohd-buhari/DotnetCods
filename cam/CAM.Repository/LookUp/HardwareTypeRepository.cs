using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class HardwareTypeRepository : RepositoryBase<Hardwaretypes>, IHardwareTypeRepository
    {
        public HardwareTypeRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Hardwaretypes>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }
    }
}
