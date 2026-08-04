using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class OperatingSystemRepository : RepositoryBaseNew<Operatingsystems>, IOperatingSystemRepository
    {

        public OperatingSystemRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {

        }

        async Task<IEnumerable<Operatingsystems>> IOperatingSystemRepository.GetAllWithRelations()
        {
            return await FindAll().OrderBy(o => o.Operatingsystemname).Include(m => m.Majorsoftwarebuilds).ToListAsync();
        }
    }
}
