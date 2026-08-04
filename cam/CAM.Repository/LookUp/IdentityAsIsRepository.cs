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
    public class IdentityAsIsRepository : RepositoryBase<Identitiesasis>, IIdentityAsIsRepository
    {
        public IdentityAsIsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
