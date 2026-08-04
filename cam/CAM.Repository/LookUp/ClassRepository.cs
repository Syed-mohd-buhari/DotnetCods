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
    public class ClassRepository : RepositoryBase<Classes>, IClassRepository
    {
        public ClassRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
        public IDictionary<int, string> GetClassesByCategoryId(int categoryId)
        {
            return ModelContext.Classes.Where(x => x.Categoryid == categoryId).ToDictionary(x => x.Id, x => x.Description);
        }
    }
}
