using System;
using System.Collections.Generic;
using System.Text;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.Entity
{
   public class HardwareConfigurationRepository : RepositoryBase<Hardwareconfiguration>, IHardwareConfigurationRepository
    {
        public HardwareConfigurationRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
