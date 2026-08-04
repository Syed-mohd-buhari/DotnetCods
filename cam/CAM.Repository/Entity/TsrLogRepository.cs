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
   public class TsrLogRepository : RepositoryBase<Tsrlogs>, ITsrLogRepository
    {
        public TsrLogRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
