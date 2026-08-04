using CAM.Contracts.RepositoryContracts.LookUp;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Repository.NewRepositoryWrapper.LookUp
{
    public class UsersLoggingLevelRepository : RepositoryBaseNew<Userslogginglevels>,IUsersLoggingLevelRepository
    {
        public UsersLoggingLevelRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Userslogginglevels>> GetAllWithRelations()
        {
            return await FindAll()
                .ToListAsync();
        }
    }
}
