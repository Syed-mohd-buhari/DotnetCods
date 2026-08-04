using CAM.Contracts.RepositoryContracts.Entity;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class TokenRepository : RepositoryBaseNew<Refreshtoken>, ITokenRepository
    {
        public TokenRepository(ModelContextNew modelContext) : base(modelContext)
        {
        }
    }
}
