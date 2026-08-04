using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.Entity
{
    public class ClaimsRepository : RepositoryBase<Aspnetuserclaims>, IClaimsRepository
    {
        public ClaimsRepository(ModelContext modelContext) : base(modelContext)
        {
        }
    }
}
