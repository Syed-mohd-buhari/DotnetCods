using CAM.Contracts.RepositoryContracts.RBAC;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.RBAC
{
    public class AspNetUserRolePermissionsRepository : RepositoryBase<Aspnetuserrolepermissions>, IAspNetUserRolePermissionsRepository
    {
        ModelContext _context;
        public AspNetUserRolePermissionsRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
