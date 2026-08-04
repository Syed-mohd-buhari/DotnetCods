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
    public class AssetTypeRepository : RepositoryBase<Assettypes>, IAssetTypeRepository
    {
        public AssetTypeRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Assettypes>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }


    }
}
