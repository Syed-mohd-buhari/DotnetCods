using CAM.Contracts.RepositoryContracts.Base;
using CAM.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.DesigComponent
{
    public static class Utils
    {
        public static async Task<string> STIM(IRepositoryWrapper _repositoryWrapper, int? hwOemId, int? swOemId, string softwareAppType, string platformDescriptionName, string hwSolution, int? rule)
        {
            var oems = await _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Orgeqpmanufacturerid == hwOemId || x.Orgeqpmanufacturerid == swOemId).ToListAsync();
            var name = "";
            if (rule == (int)BuildconstructionRuleEnum.ProprietaryHW || rule == (int)BuildconstructionRuleEnum.CotsHW)
            {
                name = $"{oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == swOemId)?.Originalequipmentmanufacturer} " +
                               $"{softwareAppType} " +
                               $"<b class=\"text-lowercase\" >on</b> {oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == hwOemId)?.Originalequipmentmanufacturer} {hwSolution} {platformDescriptionName} ";
            }
            else
            {
                name = $"{oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == swOemId)?.Originalequipmentmanufacturer} " +
                              $"{softwareAppType} " +
                              $"<b class=\"text-lowercase\" >on</b> {oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == hwOemId)?.Originalequipmentmanufacturer} {platformDescriptionName} ";

            }
           
            return name;
        }
        //public static async Task<string> STIM(IRepositoryWrapper _repositoryWrapper,int? hwOemId, int? swOemId,string softwareAppType,   string platformDescriptionName)
        //{
        //    var oems = await _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Orgeqpmanufacturerid == hwOemId || x.Orgeqpmanufacturerid == swOemId).ToListAsync();
        //    var name = $"{oems.FirstOrDefault(x=>x.Orgeqpmanufacturerid == swOemId)?.Originalequipmentmanufacturer} " +
        //           $"{softwareAppType} " +
        //           $"<b class=\"text-lowercase\" > on </b> {oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == hwOemId)?.Originalequipmentmanufacturer} {platformDescriptionName}";

        //    return name;
        //}
    }
}
