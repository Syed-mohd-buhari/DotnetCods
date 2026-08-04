using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class ClassRepository : RepositoryBaseNew<Classes>, IClassRepository
    {
        public ClassRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public IDictionary<int, string> GetClassesByCategoryId(int categoryId)
        {
            return ModelContext.Classes.Where(x => x.Categoryid == categoryId).ToDictionary(x => x.Id, x => x.Description);
        }
    }
}
