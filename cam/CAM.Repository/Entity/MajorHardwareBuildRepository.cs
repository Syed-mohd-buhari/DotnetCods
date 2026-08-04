using System.Linq;
using CAM.Contracts.RepositoryContracts.Entity;
using CAM.Entities;
using CAM.Entities.Models;
using OracleModels.DBContext;
using OracleModels.DBModels;
using Microsoft.EntityFrameworkCore;
namespace CAM.Repository.Entity
{
    public class MajorHardwareBuildRepository : RepositoryBase<Majorhardwarebuilds>, IMajorHardwareBuildRepository
    {
        ModelContext _context;

        public MajorHardwareBuildRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

       

        //public new void Update(MajorHardwareBuild entity)
        //{

        //}


        public IQueryable<Majorhardwarebuilds> GetAllWithRelations()
        {
            return FindAll();
        }
        public void Detach()
        {

            var majorhardwarebuilds = typeof(Majorhardwarebuilds);
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.Entity.GetType() == majorhardwarebuilds)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;


        }

    }
}
