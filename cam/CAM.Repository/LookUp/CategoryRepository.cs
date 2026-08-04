using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.LookUp
{
    public class CategoryRepository : RepositoryBase<Categories>, ICategoryRepository
    {
        public CategoryRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }

    }
}
