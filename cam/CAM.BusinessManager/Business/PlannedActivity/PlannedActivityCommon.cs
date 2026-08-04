using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.BusinessManager.Entity;
using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CAM.BusinessManager.Business.PlannedActivity
{
    public class PlannedActivityCommon : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly PlannedActivityManager _plannedActivityManager;
        public PlannedActivityCommon(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, PlannedActivityManager pmanager, IHttpContextAccessor contextAccessor) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            this._plannedActivityManager = pmanager;
        }

        public async Task ReloadPlannedActivity(List<long> sistemTypeIds,bool detach = false)
        {
            if (detach)
            {
                _repositoryWrapper.PlannedActivity.Detach();
            }
            

            var designComponent = await _repositoryWrapper.DesignComponent
                .FindByCondition(x => sistemTypeIds.Contains(x.Systemtypeid)).Include(x => x.Lcmengineering)
                .ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource).ToListAsync();
            foreach (var t in designComponent)
            {
                foreach (var s in t.Lcmengineering)
                {
                    
                    foreach (var plannedActivity in s.PlannedactivitiesLcmengineering)
                    {
                        if (plannedActivity?.Plannedactivityresource?.Ruleacticvitydetails != null)
                        {
                            var planned = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedActivity.Plannedactivityid, true).Single();
                            var result = await _plannedActivityManager.GetActivityDetailsFromLcmRule(t.Designcomponentid, plannedActivity.Plannedactivityresource.Ruleacticvitydetails);
                            planned.Activitydetails = result;                           
                            _repositoryWrapper.PlannedActivity.Update(planned);
                           
                        }
                    }
                }
            }
            await _repositoryWrapper.SaveAsync();
        }
    }
}
