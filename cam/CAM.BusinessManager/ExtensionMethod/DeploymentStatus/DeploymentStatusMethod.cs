using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.LookUp;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAM.BusinessManager.ExtensionMethod.DeploymentStatus
{
    public static class DeploymentStatusMethod
    {
        public static List<short> toPlannedActivityResourceKeyList(this CAM.Entities.Models.Lookup.DeploymentStatus entity)
        {
            if (entity.PlannedActivityResourceAllowed == null)
            {
                return null;
            }
            if (entity.PlannedActivityResourceAllowed.Contains("|"))
            {
                return entity.PlannedActivityResourceAllowed.Split(" | ", StringSplitOptions.RemoveEmptyEntries).Select(x => short.Parse(x.Trim())).ToList();
            }
            return (new short[] { short.Parse(entity.PlannedActivityResourceAllowed.Trim()) }).ToList();

        }

        public static List<short> toPlannedActivityResourceKeyList( Deploymentstatuses entity)
        {
            if (entity.Plannedactivityresourceallowed == null)
            {
                return null;
            }
            if (entity.Plannedactivityresourceallowed.Contains("|"))
            {
                return entity.Plannedactivityresourceallowed.Split(" | ", StringSplitOptions.RemoveEmptyEntries).Select(x => short.Parse(x.Trim())).ToList();
            }
            return (new short[] { short.Parse(entity.Plannedactivityresourceallowed.Trim()) }).ToList();

        }
        public static string toPlannedActivityResourceAllowedJoinedKeyList(this DeploymentStatusDto entity)
        {
            if (entity.PlannedActivityResourceAllowedId == null || entity.PlannedActivityResourceAllowedId.Count <= 0)
            {
                return null;
            }
            if (entity.PlannedActivityResourceAllowedId.Count == 1)
            {
                return entity.PlannedActivityResourceAllowedId[0].ToString();
            }
           
            return String.Join(" | ", entity.PlannedActivityResourceAllowedId.Select(p=>p.ToString()));
        }
        public static string toPlannedActivityResourceAllowedDescriptions(this CAM.Entities.Models.Lookup.DeploymentStatus entity, IRepositoryWrapper repositoryWrapper)
        {
            if (entity.PlannedActivityResourceAllowed == null || entity.PlannedActivityResourceAllowed == "")
            {
                return "";
            }
            var resources = repositoryWrapper.PlannedActivityResourceRepository.FindAll().ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource);
            var list = entity.toPlannedActivityResourceKeyList();
            if (list.Count == 1)
            {
                if (resources.ContainsKey(list[0]))
                    return resources[list[0]];
                else
                {
                    return "";
                }
            }
            return String.Join(" | ", list.Where(x => resources.ContainsKey(x)).Select(x => resources[x]));
        }
    }
}
