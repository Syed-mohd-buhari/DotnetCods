using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class ProductImportanceRepository : RepositoryBase<Productimportances>, IProductImportanceRepository
    {
        public ProductImportanceRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Productimportances>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }


    }
}
