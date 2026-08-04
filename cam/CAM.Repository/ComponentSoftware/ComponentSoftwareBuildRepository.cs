using CAM.Contracts.RepositoryContracts.ComponentSoftware;
using CAM.Contracts.RepositoryContracts.Entity;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Repository.ComponentSoftware
{
    public class ComponentSoftwareBuildRepository : RepositoryBase<Componentsoftwarebuilds>, IComponentSoftwareBuildRepository
    {
        ModelContext _context;
        public ComponentSoftwareBuildRepository(ModelContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        } 
        //public IQueryable<Componentsoftwarebuilds> GetAllWithRelations()
        //{
        //    return FindAll();
        //}

        //public void Detach()
        //{

        //    var plannedActivityType = typeof(Componentsoftwarebuilds);
        //    var changedEntriesCopy = _context.ChangeTracker.Entries()
        //        .Where(e => e.Entity.GetType() == plannedActivityType)
        //        .ToList();

        //    foreach (var entry in changedEntriesCopy)
        //        entry.State = EntityState.Detached;


        //}
    }
}
