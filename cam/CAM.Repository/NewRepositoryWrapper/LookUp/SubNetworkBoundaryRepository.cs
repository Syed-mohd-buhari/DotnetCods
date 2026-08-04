using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public partial class SubNetworkBoundaryRepository : RepositoryBaseNew<Subnetworkboundaries>, ISubNetworkBoundaryRepository
    {
        public SubNetworkBoundaryRepository(ModelContextNew repositoryContext) : base(repositoryContext)
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
