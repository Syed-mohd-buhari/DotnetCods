using CAM.Contracts.RepositoryContracts.ForeignIndex;
using CAM.Entities;
using CAM.Entities.Models.ForeignIndex;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.Repository.ForeignIndex
{
    public class FI_DesignComponentRepository : RepositoryBase<FiDesigncomponents>, IFI_DesignComponentsRepository
    {
        public FI_DesignComponentRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<FiDesigncomponents>> GetAllWithRelations()
        {
            return await FindAll().ToListAsync();
        }


    }
}
