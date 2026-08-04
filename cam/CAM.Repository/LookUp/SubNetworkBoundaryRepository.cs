using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.Repository.LookUp
{
    public partial class SubNetworkBoundaryRepository : RepositoryBase<Subnetworkboundaries>, ISubNetworkBoundaryRepository
    {
        public SubNetworkBoundaryRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Subnetworkboundaries>> GetAllWithRelations()
        {
            return await FindAll()
                .Include(x => x.Designcomponentfamilies)
                .ToListAsync();
        }
    }
}
