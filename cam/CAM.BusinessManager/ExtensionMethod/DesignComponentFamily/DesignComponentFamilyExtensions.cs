using CAM.Contracts.RepositoryContracts.Base;
using CAM.Enum;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.ExtensionMethod.DesignComponentFamily
{
    public static class DesignComponentFamilyExtensions
    {

        //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries implemented in all the methods()
        public static string toDesignComponentFamilyName(this CAM.Entities.Models.DesignComponentFamily model, IRepositoryWrapper wrapper)
        {
            if (model == null)
                return string.Empty;
            var item = wrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == model.DesignComponentFamilyId,true).Include(p => p.Subnetworkboundary).SingleOrDefault();

            return $" {(item.Systemtypeidentityname)} <b class=\"text-lowercase\"> for </b> {(!string.IsNullOrEmpty(item.Subnetworkboundary.Alias) ? item.Subnetworkboundary.Alias : item.Subnetworkboundary.Description)}";
        }
        public static string toDesignComponentFamilyName(this Designcomponentfamilies model , IRepositoryWrapper wrapper)
        {
            if (model == null)
                return string.Empty;
            var item = wrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == model.Designcomponentfamilyid,true).Include(p => p.Subnetworkboundary).SingleOrDefault();
            return $" {(item.Systemtypeidentityname)} <b class=\"text-lowercase\"> for </b> {(!string.IsNullOrEmpty(item.Subnetworkboundary.Alias) ? item.Subnetworkboundary.Alias : item.Subnetworkboundary.Description)}";
        }

        public static string toDesignComponentFamilyNameForPatNfvi(this Designcomponentfamilies model, IRepositoryWrapper wrapper)
        {
            if (model == null)
                return string.Empty;
            var item = wrapper.DesignComponentFamily.FindByCondition(p =>  p.Designcomponentfamilyid == model.Designcomponentfamilyid,true)
                .Include(x=>x.Productname)
                .Include(p => p.Subnetworkboundary)
                .Include(p => p.Majorsoftwareoem).SingleOrDefault();
            string productName = item.Productname != null ? item.Productname?.Description : "";
            var name = $"{item.Majorsoftwareoem.Originalequipmentmanufacturer} " +
                   $"{productName} ";

            return $" {name} <b class=\"text-lowercase\"> for </b> {(!string.IsNullOrEmpty(item.Subnetworkboundary.Alias) ? item.Subnetworkboundary.Alias : item.Subnetworkboundary.Description)}";
        }

        public static string toDesignComponentFamilyNameForPatNfvi(this CAM.Entities.Models.DesignComponentFamily model, IRepositoryWrapper wrapper)
        {
            if (model == null)
                return string.Empty;
            var item = wrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == model.DesignComponentFamilyId,true)
                .Include(x => x.Productname)
                .Include(p => p.Subnetworkboundary)
                .Include(p => p.Majorsoftwareoem).SingleOrDefault();
            string productName = item.Productname != null ? item.Productname?.Description:"";
            var name = $"{item.Majorsoftwareoem.Originalequipmentmanufacturer} " +
                   $"{productName} ";

            return $" {name} <b class=\"text-lowercase\"> for </b> {(!string.IsNullOrEmpty(item.Subnetworkboundary.Alias) ? item.Subnetworkboundary.Alias : item.Subnetworkboundary.Description)}";
        }


        public static string toDesignAspectName(this Designaspects model, IRepositoryWrapper wrapper)
        {
            if (model == null)
                return string.Empty;
            var name = wrapper.DesignAspectRepository.FindByCondition(x => x.Id == model.Id)
                 .Include(p => p.Designcomponentfamily).Include(p => p.Opco)
                 .Select(p => new { 
                    dcfName = p.Designcomponentfamily.DCFName(wrapper),
                    opcoName = p.Opco.Opco,
                 }).SingleOrDefault();
            return $" {(name.dcfName)}<b class=\"text-lowercase\"> on </b>{name.opcoName}";

        }

        public static string toDesignComponentFamilyNameForResourceKey(this CAM.Entities.Models.DesignComponentFamily model, IRepositoryWrapper wrapper)
        {
            if (model == null)
                return string.Empty;
            var item = wrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == model.DesignComponentFamilyId,true)
                .Include(x => x.Productname)
                .Include(p => p.Subnetworkboundary)
                .Include(p => p.Majorsoftwareoem).SingleOrDefault();
            string productName = item.Productname != null ? item.Productname?.Description : "";
            var name = $"{item.Majorsoftwareoem.Originalequipmentmanufacturer} " +
                   $"{productName} ";

            return $" {name} "+ "<b class=\"text-lowercase\"> on </b> " + $"{item.Majorsoftwareoem.Originalequipmentmanufacturer} "
                +" <b class=\"text-lowercase\"> for </b> "
                +$"{(!string.IsNullOrEmpty(item.Subnetworkboundary.Alias) ? item.Subnetworkboundary.Alias : item.Subnetworkboundary.Description)}";
        }
        public static string toProductName(this Designcomponentfamilies model, IRepositoryWrapper wrapper)
        {
            if (model == null)
                return string.Empty;
            var item = wrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == model.Designcomponentfamilyid,true)
                .Include(x => x.Productname).ThenInclude(x=>x.Vodafonenames).Select(x => x.Productname.Description.Count() >= 31? x.Productname.Vodafonenames.Description : x.Productname.Description) 
                .FirstOrDefault();
            
            return item != null ? item : "";
        }
        public static string toProductName(this CAM.Entities.Models.DesignComponentFamily model, IRepositoryWrapper wrapper)
        {
            if (model == null)
                return string.Empty;
            var item = wrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == model.DesignComponentFamilyId,true)
                .Include(x => x.Productname).ThenInclude(x=>x.Vodafonenames).Select(x => x.Productname.Description.Count() >= 31 ? x.Productname.Vodafonenames.Description : x.Productname.Description)
                .FirstOrDefault();

            return item != null ? item : "";
        }

        public static string DCFName(this Designcomponentfamilies entity,IRepositoryWrapper _repositoryWrapper)
        {
            var name = "";

            var dcEntity =  _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == entity.Designcomponentfamilyid && x.Deleted == false)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname).FirstOrDefault();
            if (dcEntity != null)
            {
                var hardwareEntity = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Ismain && x.Systemtypeid == dcEntity.Systemtypeid)
                    .Include(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                    .Include(x => x.Majorhardware).ThenInclude(x => x.Platform).FirstOrDefault();

                var subnetworkBoundary = _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == entity.Designcomponentfamilyid)
                    .Include(x => x.Subnetworkboundary).FirstOrDefault();

                var oems = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Orgeqpmanufacturerid == hardwareEntity.Majorhardware.Orgeqpmanufacturerid
                || x.Orgeqpmanufacturerid == dcEntity.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid).ToList();


                if (hardwareEntity.Majorhardware.Buildconstruction.Rule == (int)BuildconstructionRuleEnum.ProprietaryHW || hardwareEntity.Majorhardware.Buildconstruction.Rule == (int)BuildconstructionRuleEnum.CotsHW)
                {
                    name = $"{oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == dcEntity.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid)?.Originalequipmentmanufacturer} " +
                                   $"{dcEntity.Systemtype.Majorsoftwarebuilds.Productname.Description} " +
                                   $"<b class=\"text-lowercase\" >on</b> {oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == hardwareEntity.Majorhardware.Orgeqpmanufacturerid)?.Originalequipmentmanufacturer} {hardwareEntity.Majorhardware.Hardwaresolution} {hardwareEntity.Majorhardware.Platform.Platform} " +
                                   $"<b class=\"text-lowercase\"> for </b> {(!string.IsNullOrEmpty(subnetworkBoundary.Subnetworkboundary.Alias) ? subnetworkBoundary.Subnetworkboundary.Alias : subnetworkBoundary.Subnetworkboundary.Description)}";

                }
                else
                {
                    name = $"{oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == dcEntity.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid)?.Originalequipmentmanufacturer} " +
                                  $"{dcEntity.Systemtype.Majorsoftwarebuilds.Productname.Description} " +
                                  $"<b class=\"text-lowercase\" >on</b> {oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == hardwareEntity.Majorhardware.Orgeqpmanufacturerid)?.Originalequipmentmanufacturer} {hardwareEntity.Majorhardware.Platform.Platform} " +
                                  $"<b class=\"text-lowercase\"> for </b> {(!string.IsNullOrEmpty(subnetworkBoundary.Subnetworkboundary.Alias) ? subnetworkBoundary.Subnetworkboundary.Alias : subnetworkBoundary.Subnetworkboundary.Description)}";

                }
            }

            return name;
        }

        public static string DCFName(this CAM.Entities.Models.DesignComponentFamily entity, IRepositoryWrapper _repositoryWrapper)
        {
            var name = "";
            var dcEntity =  _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == entity.DesignComponentFamilyId && x.Deleted == false)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname).FirstOrDefault();
            if (dcEntity != null)
            {

                var hardwareEntity = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Ismain && x.Systemtypeid == dcEntity.Systemtypeid)
                    .Include(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                    .Include(x => x.Majorhardware).ThenInclude(x => x.Platform).FirstOrDefault();

                var subnetworkBoundary = _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == entity.DesignComponentFamilyId)
                    .Include(x => x.Subnetworkboundary).FirstOrDefault();

                var oems = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Orgeqpmanufacturerid == hardwareEntity.Majorhardware.Orgeqpmanufacturerid
                || x.Orgeqpmanufacturerid == dcEntity.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid).ToList();


                
                if(hardwareEntity.Majorhardware.Buildconstruction.Rule == (int)BuildconstructionRuleEnum.ProprietaryHW || hardwareEntity.Majorhardware.Buildconstruction.Rule == (int)BuildconstructionRuleEnum.CotsHW)
                {
                    name = $"{oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == dcEntity.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid)?.Originalequipmentmanufacturer} " +
                                   $"{dcEntity.Systemtype.Majorsoftwarebuilds.Productname.Description} " +
                                   $"<b class=\"text-lowercase\" >on</b> {oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == hardwareEntity.Majorhardware.Orgeqpmanufacturerid)?.Originalequipmentmanufacturer} {hardwareEntity.Majorhardware.Hardwaresolution} {hardwareEntity.Majorhardware.Platform.Platform} " +
                                   $"<b class=\"text-lowercase\"> for </b> {(!string.IsNullOrEmpty(subnetworkBoundary.Subnetworkboundary.Alias) ? subnetworkBoundary.Subnetworkboundary.Alias : subnetworkBoundary.Subnetworkboundary.Description)}";

                }
                else
                {
                    name = $"{oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == dcEntity.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid)?.Originalequipmentmanufacturer} " +
                                  $"{dcEntity.Systemtype.Majorsoftwarebuilds.Productname.Description} " +
                                  $"<b class=\"text-lowercase\" >on</b> {oems.FirstOrDefault(x => x.Orgeqpmanufacturerid == hardwareEntity.Majorhardware.Orgeqpmanufacturerid)?.Originalequipmentmanufacturer} {hardwareEntity.Majorhardware.Platform.Platform} " +
                                  $"<b class=\"text-lowercase\"> for </b> {(!string.IsNullOrEmpty(subnetworkBoundary.Subnetworkboundary.Alias) ? subnetworkBoundary.Subnetworkboundary.Alias : subnetworkBoundary.Subnetworkboundary.Description)}";

                }
            }

            return name;
        }

    }
}
