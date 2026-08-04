using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class TypeRepository : RepositoryBaseNew<Types>, ITypeRepository
    {
        public TypeRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public IDictionary<int, string> GetTypesByClassId(int classId)
        {
            return ModelContext.Types.Where(x => x.Classid == classId).ToDictionary(x => x.Id, x => x.Description);
        }
    }
}
