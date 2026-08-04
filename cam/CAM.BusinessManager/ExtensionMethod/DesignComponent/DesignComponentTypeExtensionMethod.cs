using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.Enum;
using IdentityServer4.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NPOI.OpenXmlFormats.Spreadsheet;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAM.BusinessManager.ExtensionMethod.DesignComponent
{
    public static class DesignComponentTypeExtensionMethod
    {
        public static string ToDesignComponentName(this Designcomponents item, IRepositoryWrapper _repositoryWrapper)
        {
            if (item == null)
                return string.Empty;
            var x = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == item.Designcomponentid, true)
                .Include(dc => dc.Systemtype)
                .Select(dc => new
                {
                    SubnetworkBoundryAlias = dc.Designcomponentfamily.Subnetworkboundary.Alias,
                    SubnetworkBoundryDescription = dc.Designcomponentfamily.Subnetworkboundary.Description,
                    SystemType = dc.Systemtype
                })
                .FirstOrDefault();
            if (x == null)
                return string.Empty;
            var name = !string.IsNullOrEmpty(x?.SubnetworkBoundryAlias) ? x.SubnetworkBoundryAlias : x?.SubnetworkBoundryDescription;
            if (name != null)
                name = "<b class=\"text-lowercase\"> for </b>" + name;

            return $"{x.SystemType.SystemTypeNameForDC(_repositoryWrapper)}  {name}";
        }
        public static string ToDesignComponentNameHomePage(this Designcomponents item, IRepositoryWrapper _repositoryWrapper)
        {
            if (item == null)
                return string.Empty;
            var x = item;
            var name = !string.IsNullOrEmpty(x?.Designcomponentfamily.Subnetworkboundary.Alias) ? x?.Designcomponentfamily.Subnetworkboundary.Alias : x?.Designcomponentfamily.Subnetworkboundary.Description;
            if (name != null)
                name = "<b class=\"text-lowercase\"> for </b>" + name;

            return $"{x.Systemtype.SystemTypeNameForDC(_repositoryWrapper)}  {name}";
        }

        public static string toDesignComponentNameLcm(this Designcomponents item, IRepositoryWrapper _repositoryWrapper)
        {
            if (item == null)
                return string.Empty;
            var x = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == item.Designcomponentid, true)
                .Include(p => p.Systemtype)
                .Include(p => p.Designcomponentfamily)
                .ThenInclude(p => p.Subnetworkboundary)
                .FirstOrDefault();
            if (x == null)
                return string.Empty;
            var name = !string.IsNullOrEmpty(x?.Designcomponentfamily?.Subnetworkboundary?.Alias) ? x?.Designcomponentfamily?.Subnetworkboundary?.Alias : x?.Designcomponentfamily?.Subnetworkboundary?.Description;
            if (name != null)
                name = "<b class=\"text-lowercase\"> for </b>" + name;
            return $"{x.Systemtype.toSystemTypeName(_repositoryWrapper)}  {name}";
        }
        public static string ToDesignComponentNameBasedOnDcId(long dcId, IRepositoryWrapper _repositoryWrapper)
        {
            if (dcId == 0)
                return string.Empty;
            var x = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == dcId, true)
                .Include(p => p.Systemtype)
                .Include(p => p.Designcomponentfamily)
                .ThenInclude(p => p.Subnetworkboundary)
                .FirstOrDefault();
            if (x == null)
                return string.Empty;
            var name = !string.IsNullOrEmpty(x?.Designcomponentfamily?.Subnetworkboundary?.Alias) ? x?.Designcomponentfamily?.Subnetworkboundary?.Alias : x?.Designcomponentfamily?.Subnetworkboundary?.Description;
            if (name != null)
                name = "<b class=\"text-lowercase\"> for </b>" + name;
            return $"{x.Systemtype.toSystemTypeName(_repositoryWrapper)}  {name}";
        }
        public static string toDesignComponentNameLcmFromDCModel(this CAM.Entities.Models.DesignComponent item, IRepositoryWrapper _repositoryWrapper)
        {
            if (item == null)
                return string.Empty;
            var x = (_repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == item.DesignComponentId, true)
                .Include(p => p.Systemtype)
                .Include(p => p.Designcomponentfamily)
                .ThenInclude(p => p.Subnetworkboundary)).FirstOrDefault();
            if (x == null)
                return string.Empty;
            var name = !string.IsNullOrEmpty(x?.Designcomponentfamily?.Subnetworkboundary?.Alias) ? x?.Designcomponentfamily?.Subnetworkboundary?.Alias : x?.Designcomponentfamily?.Subnetworkboundary?.Description;
            if (name != null)
                name = "<b class=\"text-lowercase\"> for </b>" + name;
            return $"{x.Systemtype.toSystemTypeName(_repositoryWrapper)}  {name}";
        }
        public static string toDesignComponentNameLcm(this Entities.Models.DesignComponent item, IRepositoryWrapper _repositoryWrapper)
        {
            if (item == null)
                return string.Empty;
            var x = (_repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == item.DesignComponentId, true)
                .Include(p => p.Systemtype)
                .Include(p => p.Designcomponentfamily)
                .ThenInclude(p => p.Subnetworkboundary)).FirstOrDefault();
            if (x == null)
                return string.Empty;
            var name = !string.IsNullOrEmpty(x?.Designcomponentfamily?.Subnetworkboundary?.Alias) ? x?.Designcomponentfamily?.Subnetworkboundary?.Alias : x?.Designcomponentfamily?.Subnetworkboundary?.Description;
            if (name != null)
                name = "<b class=\"text-lowercase\"> for </b>" + name;
            return $"{x.Systemtype.toSystemTypeName(_repositoryWrapper)}  {name}";
        }


        public static (List<string>, string) toDesignComponentNameLcmToList(this Designcomponents x, IRepositoryWrapper _repositoryWrapper)
        {
            var subnetworkName = !string.IsNullOrEmpty(x.Designcomponentfamily.Subnetworkboundary.Alias) ? x.Designcomponentfamily.Subnetworkboundary.Alias : x.Designcomponentfamily.Subnetworkboundary.Description;
            List<string> list = new List<string>();
            list = x.Systemtype.toSystemTypeNameList(_repositoryWrapper);
            list.Add("<b class=\"text-lowercase\" >for</b>");
            list.Add(subnetworkName);
            if (subnetworkName != null)
                subnetworkName = "< b class=\"text-lowercase\" >for</b>" + subnetworkName;
            var name =
                $"{x.Systemtype.toSystemTypeName(_repositoryWrapper)} {subnetworkName} ";
            return (list, name);
        }

        public static IDictionary<long, string> toDesignComponentResource(this IEnumerable<Designcomponents> dc, IRepositoryWrapper _repositoryWrapper)
        {
            return dc.ToDictionary(
                 x => x.Designcomponentid,
                 x => $"{x.Systemtype.SystemTypeName(_repositoryWrapper)} " +
                 $"{(!string.IsNullOrEmpty(x?.Designcomponentfamily?.Subnetworkboundary?.Alias) ? ("<b class=\"text-lowercase\" > for </b> " + x?.Designcomponentfamily?.Subnetworkboundary?.Alias) : ("<b class=\"text-lowercase\" > for </b> " + x?.Designcomponentfamily?.Subnetworkboundary?.Description))} "
             );

        }    
        public static IDictionary<long?, string> toDesignComponentFamilyResource(this IEnumerable<Designcomponents> dcf, IRepositoryWrapper _repositoryWrapper)
        {
            return dcf.Take(1).ToDictionary(
                 x => x.Designcomponentfamilyid,
                 x => $"{x.Systemtype.SystemTypeName(_repositoryWrapper)} " +
                 $"{(!string.IsNullOrEmpty(x?.Designcomponentfamily?.Subnetworkboundary?.Alias) ? ("<b class=\"text-lowercase\" > for </b> " + x?.Designcomponentfamily?.Subnetworkboundary?.Alias) : ("<b class=\"text-lowercase\" > for </b> " + x?.Designcomponentfamily?.Subnetworkboundary?.Description))} "
             );

        }



        public static string toDesignComponentFamily(this Designcomponents item, IRepositoryWrapper _repositoryWrapper)
        {
            var result = string.Empty;
            if (item != null)
            {
                var x = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == item.Designcomponentid, true)
                           .Include(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Orgeqpmanufacturer)
                           .Include(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname)
                           .Include(p => p.Systemtype).ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Platform)
                           .Include(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary)
                           .Include(p => p.Systemtype).ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Orgeqpmanufacturer)
                           .Include(p => p.Systemtype).ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction).FirstOrDefault();
                string productNameString = (x != null && x.Systemtype?.Majorsoftwarebuilds?.Productname != null) ? x.Systemtype?.Majorsoftwarebuilds?.Productname.Description : "";
                string originalequipmentmanufacturer = (x != null && x.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer != null) ? x.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer.Originalequipmentmanufacturer
                    : "";
                string platform = (x != null && x.Systemtype?.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Platform != null)
                     ? x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Platform.Platform
                     : "";
                string Subnetworkboundary = (x != null && !string.IsNullOrEmpty(x.Designcomponentfamily?.Subnetworkboundary.Alias)
                    ? x.Designcomponentfamily?.Subnetworkboundary.Alias : item.Designcomponentfamily.Subnetworkboundary.Description);

                string hardOem = (x != null && x.Systemtype?.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Orgeqpmanufacturer != null) ?
                    x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer :
                    "";

                string hardSolution = (x != null && x.Systemtype?.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware != null) ?
                    x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Hardwaresolution :
                    "";

                int? rule = (x != null && x.Systemtype?.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Buildconstruction != null) ?
                     x.Systemtype?.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Buildconstruction?.Rule : 0 ;
                if (rule == (int)BuildconstructionRuleEnum.ProprietaryHW || rule == (int)BuildconstructionRuleEnum.CotsHW)
                {
                    result = $"{originalequipmentmanufacturer} " +
                    $"{productNameString} " +
                    $"<b class=\"text-lowercase\" >on</b> " +
                    $"{hardOem} {hardSolution} {platform} " +
                    $"<b class=\"text-lowercase\"> for </b> " +
                    Subnetworkboundary;
                }
                else
                {
                    result = $"{originalequipmentmanufacturer} " +
                    $"{productNameString} " +
                    $"<b class=\"text-lowercase\" >on</b> " +
                    $"{hardOem} {platform} " +
                    $"<b class=\"text-lowercase\"> for </b> " +
                    Subnetworkboundary;
                }
                
            }

            return result;
        }

        public static string toDesignComponentFamily(this Entities.Models.DesignComponent x)
        {
            if (x == null) return string.Empty;
            try
            {
                string productName = x?.SystemType?.MajorSoftwareBuilds?.ProductName != null ? x?.SystemType?.MajorSoftwareBuilds?.ProductName?.Description : "";
                return $"{x?.SystemType?.MajorSoftwareBuilds?.OriginalEquipmentManufacturer?.OriginalEquipmentManufacturerDescription} " +
                    $"{productName} " +
                    $"<b class=\"text-lowercase\" >on</b> " +
                    $"{x?.SystemType?.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain && x.Deleted == false)?.MajorHardware?.Platform?.PlatformDescription} " +
                    $"<b class=\"text-lowercase\"> for </b> " +
                   (string.IsNullOrEmpty(x.DesignComponentFamily?.SubNetworkBoundary?.Alias) ? x.DesignComponentFamily?.SubNetworkBoundary?.Description : x.DesignComponentFamily?.SubNetworkBoundary?.Alias);
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

        public static string toDesignComponentName(this Designcomponents x)
        {
            try
            {
                var majorHardware = x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware;
                string name =
                 $"{x.Systemtype?.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer} {(x.Systemtype?.Majorsoftwarebuilds.Productname != null ? x.Systemtype?.Majorsoftwarebuilds.Productname.Description : "")} {x.Systemtype?.Majorsoftwarebuilds.Softwareversion}";

                if (majorHardware != null)
                {
                    if(majorHardware.Buildconstruction != null && (majorHardware.Buildconstruction.Rule == (int)BuildconstructionRuleEnum.ProprietaryHW || majorHardware.Buildconstruction.Rule == (int)BuildconstructionRuleEnum.CotsHW))
                    {
                        name += $"<b class=\"text-lowercase\"> on </b> {majorHardware?.Hardwaresolution} {majorHardware?.Platform.Platform} {majorHardware?.Hardwaretype}";
                    }
                    else
                    {
                        name += $"<b class=\"text-lowercase\"> on </b> {majorHardware?.Platform.Platform}";
                    }

                }

                if (x.Systemtype.Systemtypesmajorhardwarebuilds.Any(x => x.Deleted == false && x.Ismain == false))
                {
                    name += "<b class=\"text-lowercase\"> with </b>";
                    var mhs = x.Systemtype.Systemtypesmajorhardwarebuilds.Where(x => x.Deleted == false && x.Ismain == false)
                        .Select(x => x.Majorhardware);
                    foreach (var mh in mhs)
                    {
                        if (mh.Platform != null)
                        {
                            name += $"{majorHardware.Hardwaresolution} {mh.Platform.Platform} {mh.Hardwaretype} ";
                        }
                    }
                }

                return name +
                    $"<b class=\"text-lowercase\"> for </b> " +
                   (string.IsNullOrEmpty(x.Designcomponentfamily.Subnetworkboundary.Alias) ? x.Designcomponentfamily.Subnetworkboundary.Description : x.Designcomponentfamily.Subnetworkboundary.Alias);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string toDesignComponentFamily(this Designcomponents x)
        {
            var result = string.Empty;
            try
            {
                if (x == null) return string.Empty;
                string hardOem = x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(f => f.Ismain && f.Deleted == false)?.Majorhardware?.Orgeqpmanufacturer.Originalequipmentmanufacturer;
                string Productname = x?.Systemtype?.Majorsoftwarebuilds?.Productname != null ? x?.Systemtype?.Majorsoftwarebuilds?.Productname?.Description : "";
                int? rule = x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(f => f.Ismain && f.Deleted == false)?.Majorhardware?.Buildconstruction.Rule;
                string hardSolution = x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(f => f.Ismain && f.Deleted == false)?.Majorhardware?.Hardwaresolution;

                if (rule == (int)BuildconstructionRuleEnum.ProprietaryHW || rule == (int)BuildconstructionRuleEnum.CotsHW)
                {
                    result = $"{x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer} " +
                        $"{Productname} " +
                        $"<b class=\"text-lowercase\" >on</b> " +
                        $"{hardOem} {hardSolution} " +
                        $"{x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Platform.Platform} " +
                        $"<b class=\"text-lowercase\"> for </b> " +
                       (string.IsNullOrEmpty(x.Designcomponentfamily.Subnetworkboundary.Alias) ? x.Designcomponentfamily.Subnetworkboundary.Description : x.Designcomponentfamily.Subnetworkboundary.Alias);

                }
                else
                {
                    result = $"{x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer} " +
                                             $"{Productname} " +
                                             $"<b class=\"text-lowercase\" >on</b> " +
                                             $"{hardOem} " +
                                             $"{x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware.Platform.Platform} " +
                                             $"<b class=\"text-lowercase\"> for </b> " +
                                            (string.IsNullOrEmpty(x.Designcomponentfamily.Subnetworkboundary.Alias) ? x.Designcomponentfamily.Subnetworkboundary.Description : x.Designcomponentfamily.Subnetworkboundary.Alias);

                }
                return result;
            }
            catch
            {
                return result;
            }
        }

        public static string ToDesignComponentFamily(this Systemtypes x, string SubnetworkboundaryDescription, List<string> platformDescriptionName)
        {
            string productName = x?.Majorsoftwarebuilds != null ? x.Majorsoftwarebuilds.Productname.Description : "";

            var name = $"{x.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer} " +
                   $"{productName} " +
                   $"<b class=\"text-lowercase\" >on</b>";
            name = platformDescriptionName.Aggregate(name, (current, paltform) => current + $" {paltform} ");
            name += $"<b class=\"text-lowercase\"> for </b> " +
                    SubnetworkboundaryDescription;
            return name;
        }

        public static string ToDesignComponentFamily(this Designcomponentfamilies x, List<string> platformDescriptionName)
        {
            string productName = x.Productname != null ? x.Productname.Description : "";
            var name = $"{x.Majorsoftwareoem.Originalequipmentmanufacturer} " +
                       $"{productName} " +
                       $"<b class=\"text-lowercase\" >on</b> ";
            name = platformDescriptionName.Aggregate(name, (current, platform) => current + $" {platform} ");
            name += $"<b class=\"text-lowercase\"> for </b> " +
                    (!string.IsNullOrEmpty(x.Subnetworkboundary.Alias) ? x.Subnetworkboundary.Alias : x.Subnetworkboundary.Description);
            return name;
        }
        public static string toDesignComponentNameLcm(this Designcomponents x, Majorhardwarebuilds mh)
        {
            string productName = x.Systemtype?.Majorsoftwarebuilds?.Productname != null ? x.Systemtype?.Majorsoftwarebuilds?.Productname.Description : "";
            return $"{x.Systemtype.Systemtypenameoem} " +
                $"{productName} " +
                $"{x.Systemtype?.Majorsoftwarebuilds?.Softwareversion} - " +
                $"{mh.Platform.Platform} - {x.Systemtype.Systemtypenameoem} - {mh.Hardwaretype} " +
                $"<b class=\"text-lowercase\"> for </b> " +
                 (!string.IsNullOrEmpty(x.Designcomponentfamily.Subnetworkboundary.Alias) ? x.Designcomponentfamily.Subnetworkboundary.Alias : x.Designcomponentfamily.Subnetworkboundary.Description);
        }

        public static (List<string>, string) toDestructuredDesignComponentNameLcmToList(this Designcomponents x, IRepositoryWrapper _repositoryWrapper, long majorHardwareId, long majorSoftwareId, long designComponentFamilyId)
        {

            var mhb = _repositoryWrapper.MajorHardwareBuild
                .FindByCondition(x => x.Majorhardwareid == majorHardwareId)
                .Include(x => x.Platform)
                .FirstOrDefault(x => x.Deleted == false);
            var msb = _repositoryWrapper.MajorSoftwareBuild
                .FindByCondition(x => x.Majorsoftwarebuildsid == majorSoftwareId)
                .Include(x => x.Orgeqpmanufacturer)
                .SingleOrDefault(x => !x.Deleted.Value);

            var designComponentFamily = _repositoryWrapper.DesignComponentFamily
                .FindByCondition(x => x.Designcomponentfamilyid == designComponentFamilyId)
                .Include(x => x.Subnetworkboundary)
                .SingleOrDefault();

            List<string> list = new List<string>();

            list.Add(msb.Orgeqpmanufacturer.Originalequipmentmanufacturer);
            list.Add(msb.Productname != null ? msb.Productname?.Description : "");
            list.Add(msb.Softwareversion);
            list.Add("<b class=\"text-lowercase\"> on </b>");
            list.Add(mhb?.Platform.Platform);
            list.Add(mhb?.Hardwaretype);
            list.Add("<b class=\"text-lowercase\" >for</b>");
            list.Add(!string.IsNullOrEmpty(designComponentFamily?.Subnetworkboundary.Alias) ? designComponentFamily?.Subnetworkboundary.Alias : designComponentFamily?.Subnetworkboundary.Description);

            string productName = msb.Productname != null ? msb.Productname?.Description : "";
            var name = $"{msb.Orgeqpmanufacturer.Originalequipmentmanufacturer} {productName} {msb.Softwareversion}";

            if (mhb != null)
            {
                name += $"<b class=\"text-lowercase\"> on </b> {mhb?.Platform.Platform} {mhb?.Hardwaretype} ";
                name += $"<b class=\"text-lowercase\" >for</b> {(!string.IsNullOrEmpty(designComponentFamily?.Subnetworkboundary.Alias) ? designComponentFamily?.Subnetworkboundary.Alias : designComponentFamily?.Subnetworkboundary.Description)}";
            }




            return (list, name);

        }
        public static string DesignComponentNameForResourceKey(this CAM.Entities.Models.DesignComponent x, IRepositoryWrapper _repositoryWrapper)
        {
            string productName = x.SystemType?.MajorSoftwareBuilds?.ProductName != null ? x.SystemType?.MajorSoftwareBuilds?.ProductName.Description : "";
            string name = $"{x.SystemType.MajorSoftwareBuilds.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription} " +
                $"{productName} " +
                $"{x.SystemType?.MajorSoftwareBuilds?.SoftwareVersion} " + $"<b class=\"text-lowercase\"> on </b> " +
                $"{x.SystemType.MajorSoftwareBuilds.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription} " + $"{x.SystemType.SystemTypesMajorHardwareBuilds} " + $"{x.SystemType.EndOfMaintenance} " +
                $"<b class=\"text-lowercase\"> for </b> " +
                 (!string.IsNullOrEmpty(x.DesignComponentFamily.SubNetworkBoundary.Alias) ? x.DesignComponentFamily.SubNetworkBoundary.Alias : x.DesignComponentFamily.SubNetworkBoundary.Description);

            return name; //$"{x.SystemType.SystemTypeNameOem} " +
        }

        public static IQueryable<Designcomponents> verticalBasedDesignComponentRecord(List<int> _verticalList, IRepositoryWrapper _repositoryWrapper, string? transientDcvisible = "")
        {
            //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries  - June 13 2024
            var dcRecords = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Deleted == false)
             .Include(x => x.Systemtype);

            //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
            return (!string.IsNullOrEmpty(transientDcvisible) && transientDcvisible.ToLower() == "all") ? dcRecords : dcRecords.Where(x => x.Visibleflag == true);
        }

        public static IOrderedQueryable<Designcomponents> GetDesignComponentResource(List<int> _verticalList, IRepositoryWrapper _repositoryWrapper, string? transientDcvisible = "")
        {
            var dcResoucre = verticalBasedDesignComponentRecord(_verticalList, _repositoryWrapper, transientDcvisible)
       .Include(p => p.Systemtype)
       .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
       .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction)
       .ThenInclude(x => x.Systemfunction).OrderBy(x => x.Designcomponentfamilyid).ThenBy(x => x.Designcomponentid);


            return dcResoucre;
        }

        public static List<KeyValuePair<long, string>> GetDCDropdownRecord(IOrderedQueryable<Designcomponents> dcResource, long dcId, IRepositoryWrapper _repositoryWrapper)
        {
            List<KeyValuePair<long, string>> designComponentResourcePairs = new List<KeyValuePair<long, string>>();

            var dcfidFromDcId = dcResource.Where(x => x.Designcomponentid == dcId).
                Select(x => x.Designcomponentfamilyid)?.FirstOrDefault();

            designComponentResourcePairs = dcResource.Where(x => x.Designcomponentfamilyid == dcfidFromDcId).ToList()
                .toDesignComponentResource(_repositoryWrapper)
                       .Where(
                           x => !x.Value.ToLower().Contains("unknown")).
                           Select(x => new KeyValuePair<long, string>(x.Key, x.Value)).ToList()?.ToList();

            designComponentResourcePairs.AddRange(
                dcResource.Where(x => x.Designcomponentfamilyid != dcfidFromDcId).ToList().toDesignComponentResource(_repositoryWrapper)
                       .Where(x => !x.Value.ToLower().Contains("unknown")).Select(x => new KeyValuePair<long, string>(x.Key, x.Value))

       );

            return designComponentResourcePairs;

        }

        public static List<KeyValuePair<long, string>> GetDCFDropdownRecord(IRepositoryWrapper _repositoryWrapper)
        {
            List<KeyValuePair<long, string>> designComponentResourcePairs = new List<KeyValuePair<long, string>>();

            designComponentResourcePairs = _repositoryWrapper.DesignComponentFamily.FindAll().Include(x => x.Subnetworkboundary)
                .ToDictionary(x => x.Designcomponentfamilyid, x => x.DCFName(_repositoryWrapper)).Where(f => !f.Value.IsNullOrEmpty())
                .ToDictionary(k => k.Key, v => v.Value).ToList();


            return designComponentResourcePairs;

        }
        public static string GetCriticalityOrCriticalityType(this Subnetworkboundaries subnetworkboundaries, bool isCriticalityType)
        {
            string criticalityType = string.Empty;
            string criticality = ConstantValueFilter.No;
            string criticalityOrCriticalityType = string.Empty;
            if (subnetworkboundaries.Gdprclassification != null)
            {
                criticality = ConstantValueFilter.Yes;
                criticalityType += "GDPR Classfifcation: ";
            }
            if (subnetworkboundaries.C3C4 != null)
            {
                criticality = ConstantValueFilter.Yes;
                criticalityType += "C3/C4: ";
            }
            if (subnetworkboundaries.PciSox != null)
            {
                criticality = ConstantValueFilter.Yes;
                criticalityType += "PCI/SOX: ";
            }
            if (subnetworkboundaries.Internetfacing != null)
            {
                criticality = ConstantValueFilter.Yes;
                criticalityType += "Internet Facing: ";
            }
            if (subnetworkboundaries.Securityelement != null)
            {
                criticality = ConstantValueFilter.Yes;
                criticalityType += "Security Element: ";
            }
            if (subnetworkboundaries.Missioncritical != null)
            {
                criticality = ConstantValueFilter.Yes;
                criticalityType += "Mission Critical: ";
            }

            return criticalityOrCriticalityType = isCriticalityType == true ? criticalityType : criticality;
        }

    }
}
