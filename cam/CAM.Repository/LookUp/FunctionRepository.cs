using System;
using System.Collections.Generic;
using System.Text;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Contracts.RepositoryContracts.LookUp;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.LookUp
{
    public class FunctionRepository : RepositoryBase<Function>, IFunctionRepository
    {
        public FunctionRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
