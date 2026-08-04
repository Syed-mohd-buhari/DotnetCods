using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class ClaimsRepository : RepositoryBaseNew<Aspnetuserclaims>, IClaimsRepository
    {
        public ClaimsRepository(ModelContextNew modelContext) : base(modelContext)
        {
        }
    }
}
