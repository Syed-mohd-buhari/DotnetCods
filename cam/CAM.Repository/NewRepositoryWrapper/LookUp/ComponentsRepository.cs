using System;
using System.Collections.Generic;
using System.Text;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models;
using CAM.Repository;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.NewRepositoryWrapper.LookUp
{
    public class ComponentsRepository : RepositoryBaseNew<Component>, IComponentRepository
    {
        public ComponentsRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {

        }
    }
}
