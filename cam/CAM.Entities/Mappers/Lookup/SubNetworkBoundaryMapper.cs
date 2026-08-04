using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Lookup
{
    public static class SubNetworkBoundaryMapper
    {
        public static Models.Lookup.SubNetworkBoundary GetSubNetworkBoundaryMapper(Subnetworkboundaries Subnetworkboundaries , bool include =false)
        {

            if (Subnetworkboundaries == null)
                return null;
            var result = new Models.Lookup.SubNetworkBoundary()
            {
                Id = Subnetworkboundaries.Id,
                Description = Subnetworkboundaries.Description,
                CreationDate = Subnetworkboundaries.Creationdate,
                CreationUser = Subnetworkboundaries.Creationuser,
                ModificationDate = Subnetworkboundaries.Modificationdate,
                ModificationUser = Subnetworkboundaries.Modificationuser,
                Deleted = Subnetworkboundaries.Deleted.Value,
                DeletionDate = Subnetworkboundaries.Deletiondate,
                Alias = Subnetworkboundaries.Alias,
                Default = Subnetworkboundaries.Default.Value,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Subnetworkboundaries.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Subnetworkboundaries.ModificationuserNavigation),
                Order = Subnetworkboundaries.Order,
                GdprRelevant = Subnetworkboundaries.Gdprrelevant,
                InternetFacing = Subnetworkboundaries.Internetfacing,
                LcmPolicy = Subnetworkboundaries.Lcmpolicy,
                Criticality = Subnetworkboundaries.Criticality,
                GDPRClassification = Subnetworkboundaries.Gdprclassification,
                Pcisox = Subnetworkboundaries.PciSox,
                C3C4 = Subnetworkboundaries.C3C4,
                MissionCritical = Subnetworkboundaries.Missioncritical,
                SecurityElement = Subnetworkboundaries.Securityelement,
                VodafoneNameId = Subnetworkboundaries.Vodafonenameid,
                VodafoneName = VodafoneNameMapper.GetVodafoneNamesMapper(Subnetworkboundaries.Vodafonename),
            };
            result.DesignComponentFamilies = Subnetworkboundaries.Designcomponentfamilies.Select(p => DesignComponentFamilyMapper.Get(p,false)).ToList();
            result.SystemFunctions = Subnetworkboundaries.Subnetwrokboundarysystemfunction.Select
                (p => SubnetworkBoundarySystemFunctionMapper.Get(p)).ToList();
            result.CustomerWheels = Subnetworkboundaries.Subnetworkboundarycustomerwheel.Select
                (p => SubnetworkBoundaryCustomerWheelMapper.Get(p)).ToList();


            result.SupportedServices = Subnetworkboundaries.Subnetworksupportedsvr.Select(p=> new SubNetworkBoundary_SupportedService() { 
                Id = p.Id,
                SubNetworkId = p.Subnetworkid,
                ServiceId = p.Serviceid,
                SupportedService = SupportedServiceMapper.Get(p.Service),

            } ).ToList();
            return result;
        }
        public static Subnetworkboundaries SetSubNetworkBoundaryMapper(Models.Lookup.SubNetworkBoundary subNetworkBoundary)
        {
            if (subNetworkBoundary == null)
                return null;
            var result = new Subnetworkboundaries()
            {

                Id = subNetworkBoundary.Id,
                Description = subNetworkBoundary.Description,
                Creationdate = subNetworkBoundary.CreationDate,
                Creationuser = subNetworkBoundary.CreationUser,
                Modificationdate = subNetworkBoundary.ModificationDate,
                Modificationuser = subNetworkBoundary.ModificationUser,
                Deleted = subNetworkBoundary.Deleted,
                Deletiondate = subNetworkBoundary.DeletionDate,
                Default = subNetworkBoundary.Default,
                Alias = subNetworkBoundary.Alias,
                Order = subNetworkBoundary.Order,
                //Swapplicationname = subNetworkBoundary.SWApplicationName,
                Gdprrelevant = subNetworkBoundary.GdprRelevant,
                Internetfacing = subNetworkBoundary.InternetFacing,
                Lcmpolicy = subNetworkBoundary.LcmPolicy,
                Criticality = subNetworkBoundary.Criticality,
                Securityelement = subNetworkBoundary.SecurityElement,
                Gdprclassification = subNetworkBoundary.GDPRClassification,
                PciSox = subNetworkBoundary.Pcisox,
                C3C4 = subNetworkBoundary.C3C4,
                Missioncritical = subNetworkBoundary.MissionCritical,
                Vodafonenameid = subNetworkBoundary.VodafoneNameId,
                Vodafonename = VodafoneNameMapper.SetVodafoneNameMapper(subNetworkBoundary.VodafoneName),
            };
            result.Subnetworksupportedsvr = subNetworkBoundary.SupportedServices.Select(p => new Subnetworksupportedsvr()
            {
                Subnetworkid = p.SubNetworkId,
                Serviceid = p.ServiceId,
                Id =p.Id,
            }).ToList();
            result.Subnetwrokboundarysystemfunction = subNetworkBoundary.SystemFunctions.Select(p => new Subnetwrokboundarysystemfunction()
            {
                Id = p.Id,  
                Subnetwrokboundaryid = p.SubnetworkBoundaryId,
                Systemfunctionid = (short)p.SystemFunctionId,

            }).ToList();
            result.Subnetworkboundarycustomerwheel = subNetworkBoundary.CustomerWheels.Select(p => new Subnetworkboundarycustomerwheel()
            {
                Id = p.Id,
                Subnetworkboundaryid = p.SubnetworkBoundaryId,
                Customerwheelid = p.CustomerWheelId,

            }).ToList();
            return result;
        }
    }
}
