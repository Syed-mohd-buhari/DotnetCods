using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CAM.BusinessManager.Entity.NetworkElementNodeCountManager
{
    public class NetworkElementNodeCountManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        public NetworkElementNodeCountManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        public async Task<IEnumerable<NetworkElementAssociated>> GetNetworkElementassociated(long designComponentId,
            short opcoId)
        {
            #region Ticket 611 - Dev - #609 Power off / removed should not be displayed on LCM Screen - Appropriate count to be updated in LCM - Node count for both PROD & Lab nodes.

            var filteredDeploymentStatus = _repositoryWrapper.DeploymentStatus.FindByCondition(x =>
            !(ConstantValueFilter.excludeDeployementStatus.Contains(x.Deploymentstatus.Replace(" ", "").ToLower())  ))
             .Select(x => x.Deploymentstatusid);
            #endregion
            var networkNodes = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x =>
                x.Designcomponentid == designComponentId && x.Opcoid == opcoId
                && filteredDeploymentStatus.Contains(x.Deploymentstatusid))
                .Include(x => x.Location).Include(x => x.Environment).Include(x => x.Deploymentstatus)
                .Select(p => NetworkElementAsPlannedMapper.Get(p, true)).ToListAsync();
             
            return _mapper.Map<IEnumerable<NetworkElementAssociated>>(networkNodes);
        } 
        public async Task MigrateNetworkElement(PlannedActivityMigrationsDto data,
            PlannedActivity plannedActivitiEntity)
        {

            var start = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x =>
                data.NetworkElementStart.Contains(x.Networkelementasplannedid));
            var end = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x =>
                data.NetworkElementEnd.Contains(x.Networkelementasplannedid));


            foreach (var networkElementAsPlanned in start)
            {
                var lcm = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponentid == data.DesignComponentIdStart && x.Opcoid == networkElementAsPlanned.Opcoid && x.Archived != true).FirstOrDefault();

               
                    networkElementAsPlanned.Designcomponentid = data.DesignComponentIdStart;
                    if (lcm != null) networkElementAsPlanned.Lcmengineeringid = lcm.Lcmengineeringid;
 
                _repositoryWrapper.NetworkElementAsPlanned.Update(networkElementAsPlanned);
            }

            foreach (var networkElementAsPlanned in end)
            {
                if (plannedActivitiEntity.DesignComponentId != null)
                {
                    var lcm = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponentid == plannedActivitiEntity.DesignComponentId.Value && x.Opcoid == networkElementAsPlanned.Opcoid && x.Archived != true).FirstOrDefault();
 
                        networkElementAsPlanned.Designcomponentid = plannedActivitiEntity.DesignComponentId.Value;
                        if (lcm != null) networkElementAsPlanned.Lcmengineeringid = lcm.Lcmengineeringid;
                   
                }
                _repositoryWrapper.NetworkElementAsPlanned.Update(networkElementAsPlanned);
            }

            await _repositoryWrapper.SaveAsync();

        }

    }
}