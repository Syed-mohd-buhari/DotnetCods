using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Models
{
    public class RepositoryContextNew : ModelContextNew
    {
        public RepositoryContextNew(DbContextOptions<ModelContextNew> options)
            : base(options)
        {

        }


    }
}
