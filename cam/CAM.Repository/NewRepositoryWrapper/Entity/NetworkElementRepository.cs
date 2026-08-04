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
   public class NetworkElementRepository : RepositoryBaseNew<Networkelement>, INetworkElementRepository
    {
        public NetworkElementRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
        }
    }
}
