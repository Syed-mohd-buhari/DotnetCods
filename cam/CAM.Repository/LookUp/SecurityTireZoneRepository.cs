using CAM.Contracts.RepositoryContracts.LookUp;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.Repository.LookUp
{
    public partial class SecurityTireZoneRepository : RepositoryBase<Securitytirezone>, ISecurityTireZoneRepository
    {
        public SecurityTireZoneRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Securitytirezone>> GetAllWithRelations()
        {
            return null;
                
                //await FindAll().Include(x => x.Designcomponentfamilies)
                //.ToListAsync();
        }
    }
}
