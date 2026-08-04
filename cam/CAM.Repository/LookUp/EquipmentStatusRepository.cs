using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class EquipmentStatusRepository : RepositoryBase<Equipmentstatuses>, IEquipmentStatusRepository
    {
        public EquipmentStatusRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Equipmentstatuses>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }
}
