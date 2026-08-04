using CAM.Contracts.RepositoryContracts.LookUp;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.LookUp
{
    public class ProductNameRepository : RepositoryBase<Productname>, IProductNameRepository
    {
        public ProductNameRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Productname>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }
}
