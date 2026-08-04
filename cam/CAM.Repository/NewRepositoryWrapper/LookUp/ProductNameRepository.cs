using CAM.Contracts.RepositoryContracts.LookUp;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class ProductNameRepository : RepositoryBaseNew<Productname>, IProductNameRepository
    {
        public ProductNameRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Productname>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }
}
