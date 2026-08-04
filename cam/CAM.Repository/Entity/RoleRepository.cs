using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.Entity
{
    public class RoleRepository : RepositoryBase<Aspnetroles>, IRoleRepository
    {
        public RoleRepository(ModelContext modelContext) : base(modelContext)
        {
        }
    }
}
