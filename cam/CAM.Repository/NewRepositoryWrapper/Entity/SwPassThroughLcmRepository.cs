using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;

namespace CAM.Repository.NewRepositoryWrapper.Entity
{
    public class SwPassThroughLcmRepository : RepositoryBaseNew<Swpassthroughlcm>, ISwPassThroughRepositoryLcm
    {
        ModelContextNew _repositoryContext;
        public SwPassThroughLcmRepository(ModelContextNew repositoryContext) : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public void Detach()
        {

            var passThrough = typeof(Swpassthroughlcm);
            var changedEntriesCopy = _repositoryContext.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == passThrough)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }
    }
}
