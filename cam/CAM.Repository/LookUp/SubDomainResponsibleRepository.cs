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
    public class SubDomainResponsibleRepository : RepositoryBase<Subdomainresponsibles>, ISubDomainResponsibleRepository
    {
        public SubDomainResponsibleRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Subdomainresponsibles>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }


    }
}
