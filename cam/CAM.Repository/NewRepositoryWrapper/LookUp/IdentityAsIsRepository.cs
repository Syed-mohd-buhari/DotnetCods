using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class IdentityAsIsRepository : RepositoryBaseNew<Identitiesasis>, IIdentityAsIsRepository
    {
        public IdentityAsIsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
