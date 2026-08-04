using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.LookUp;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class ProductImportanceRepository : RepositoryBaseNew<Productimportances>, IProductImportanceRepository
    {
        public ProductImportanceRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Productimportances>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }


    }
}
