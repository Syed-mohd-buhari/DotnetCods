using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.NewRepositoryWrapper
{
    public class RoleRepository : RepositoryBaseNew<Aspnetroles>, IRoleRepository
    {
        public RoleRepository(ModelContextNew modelContext) : base(modelContext)
        {
        }
    }
}
