using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.RBAC
{
    public class AspNetUserRolePermissionsRepository : RepositoryBaseNew<Aspnetuserrolepermissions>, IAspNetUserRolePermissionsRepository
    {
        ModelContextNew _context;
        public AspNetUserRolePermissionsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}