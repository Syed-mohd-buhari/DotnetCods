using System;
using System.Collections.Generic;
using System.Text;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
   public class NetworkElementAsIsRepository : RepositoryBaseNew<Networkelementsasis>, INetworkElementAsIsRepository
    {
        public NetworkElementAsIsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
