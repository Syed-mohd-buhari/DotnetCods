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
   public class AuditHistoryRepository : RepositoryBase<Audithistory>, IAuditHistoryRepository
    {
        public AuditHistoryRepository(ModelContext repositoryContext) : base(repositoryContext)
        {

        }
    }
}
