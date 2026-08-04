using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.ExtensionMethod.SystemType
{
    public class EOMType
    {
        public DateTime? EOMDate { get; set; }

        public EOMEnum Status { get; set; }
    }
    public static class SystemTypeExtensionMethod
    {

       
        public static string SystemTypeNameForDC(this Systemtypes systemType, IRepositoryWrapper wrapper)
        {
            var src = wrapper.SystemType
                .FindByCondition(k => k.Systemtypeid == systemType.Systemtypeid,true)
                .Include(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Platform)
                .Include(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction)
                .Select(st => new 
                {
                    OriginalEquipmentManufacturer = st.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                    ProductName = st.Majorsoftwarebuilds.Productname.Description,
                    SoftwareVersion = st.Majorsoftwarebuilds.Softwareversion,
                    MajorHardwareBuild = st.Systemtypesmajorhardwarebuilds
                })
                .FirstOrDefault();

            var name = $"{src.OriginalEquipmentManufacturer} {src.ProductName} {src.SoftwareVersion}";
           
            var mhb = src.MajorHardwareBuild.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware;
            if (mhb != null)
            {
                if (mhb.Buildconstruction != null && (mhb.Buildconstruction.Rule==(int)BuildconstructionRuleEnum.ProprietaryHW || mhb.Buildconstruction.Rule == (int)BuildconstructionRuleEnum.CotsHW))
                {
                    name += $"<b class=\"text-lowercase\"> on </b> {mhb?.Hardwaresolution} {mhb?.Platform.Platform} {mhb?.Hardwaretype}";
                }
                else
                {
                    name += $"<b class=\"text-lowercase\"> on </b> {mhb?.Platform.Platform}";
                }

            }

            if (src.MajorHardwareBuild.Any(x => x.Deleted == false && x.Ismain == false))
            {
                name += "<b class=\"text-lowercase\"> with </b>";
                var mhs = src.MajorHardwareBuild.Where(x => x.Deleted == false && x.Ismain == false)
                    .Select(x => x.Majorhardware);
                foreach (var mh in mhs)
                {
                    if (mh.Platform != null)
                    {
                        name += $"{mhb.Hardwaresolution} {mh.Platform.Platform} {mh.Hardwaretype} ";
                    }
                }
            }

            return name;
        }



        public static string GetStatusName(this Systemtypes src, IRepositoryWrapper wrapper)
        {
       
            var minorDate = src.GetMinorDate(wrapper);
            var toDay = DateTime.Now;
            var toDayX = toDay.AddDays(30);

            if (minorDate >= toDay && minorDate <= toDayX)
            {
                return "On expiration";
            }

            if (minorDate < toDayX)
            {
                return "Expired";
            }

            if (minorDate > toDayX)
            {
                return "On support";
            }

            return "";

        }
        public static string GetStatusColor(this Systemtypes src, IRepositoryWrapper wrapper)
        {
            //var date = new List<DateTime?>();
            //date.Add(src?.MajorSoftwareBuilds?.LastTimeBuyNew);
            //date.Add(src?.MajorSoftwareBuilds?.LastTimeBuyUpgrades);
            //date.Add(src?.MajorSoftwareBuilds?.LastTimeBuyExpansions);
            //date.Add(src?.MajorSoftwareBuilds?.EndOfMaintenance);
            //date.Add(src?.MajorSoftwareBuilds?.EndOfsupport);


            //date.AddRange(src?.SystemTypesMajorHardwareBuilds?.Select(x => x.MajorHardware?.LastTimeBuyNew) ?? Array.Empty<DateTime?>());
            //date.AddRange(src?.SystemTypesMajorHardwareBuilds?.Select(x => x.MajorHardware?.LastTimeBuyUpgrades) ?? Array.Empty<DateTime?>());
            //date.AddRange(src?.SystemTypesMajorHardwareBuilds?.Select(x => x.MajorHardware?.LastTimeBuyExpansions) ?? Array.Empty<DateTime?>());
            //date.AddRange(src?.SystemTypesMajorHardwareBuilds?.Select(x => x.MajorHardware?.EndOfMaintenance) ?? Array.Empty<DateTime?>());
            //date.AddRange(src?.SystemTypesMajorHardwareBuilds?.Select(x => x.MajorHardware?.EndOfsupport) ?? Array.Empty<DateTime?>());

            //var minorDate = date.Where(x => x.HasValue).OrderBy(x => x.Value).FirstOrDefault();
            var minorDate = src.GetMinorDate(wrapper);

            if (!minorDate.HasValue) return "";

            var toDay = DateTime.Now;
            var toDayX = toDay.AddDays(30);

            if (minorDate >= toDay && minorDate <= toDayX)
            {
                return "amber";
            }

            if (minorDate < toDayX)
            {
                return "red";
            }

            if (minorDate > toDayX)
            {
                return "green";
            }

            return "";

        }
        public static string GetMinorDateToString(this Systemtypes src, IRepositoryWrapper wrapper)
        {
            //var date = new List<DateTime?>();
            //date.Add(src?.MajorSoftwareBuilds?.LastTimeBuyNew);
            //date.Add(src?.MajorSoftwareBuilds?.LastTimeBuyUpgrades);
            //date.Add(src?.MajorSoftwareBuilds?.LastTimeBuyExpansions);
            //date.Add(src?.MajorSoftwareBuilds?.EndOfMaintenance);
            //date.Add(src?.MajorSoftwareBuilds?.EndOfsupport);
            //date.AddRange(src?.SystemTypesMajorHardwareBuilds?.Select(x => x.MajorHardware.LastTimeBuyNew) ?? Array.Empty<DateTime?>());
            //date.AddRange(src?.SystemTypesMajorHardwareBuilds?.Select(x => x.MajorHardware.LastTimeBuyUpgrades) ?? Array.Empty<DateTime?>());
            //date.AddRange(src?.SystemTypesMajorHardwareBuilds?.Select(x => x.MajorHardware.LastTimeBuyExpansions) ?? Array.Empty<DateTime?>());
            //date.AddRange(src?.SystemTypesMajorHardwareBuilds?.Select(x => x.MajorHardware.EndOfMaintenance) ?? Array.Empty<DateTime?>());
            //date.AddRange(src?.SystemTypesMajorHardwareBuilds?.Select(x => x.MajorHardware.EndOfsupport) ?? Array.Empty<DateTime?>());
            //return date.Where(x => x.HasValue).OrderBy(x => x.Value).FirstOrDefault()?.ToShortDateString() ?? "";
            return src.GetMinorDate(wrapper)?.ToShortDateString() ?? "";

        }
        public static DateTime? GetMinorDate(this Systemtypes src, IRepositoryWrapper wrapper)
        {
            var date = new List<DateTime?>();
            if (src?.Majorsoftwarebuilds != null)
            {
                date.Add(src?.Majorsoftwarebuilds?.Lasttimebuynew);
                date.Add(src?.Majorsoftwarebuilds?.Lasttimebuyupgrades);
                date.Add(src?.Majorsoftwarebuilds?.Lasttimebuyexpansions);
                date.Add(src?.Majorsoftwarebuilds?.Endofmaintenance);
                date.Add(src?.Majorsoftwarebuilds?.Endofsupport);
            }

            if (src?.Systemtypesmajorhardwarebuilds != null)
            {
                var rec = src?.Systemtypesmajorhardwarebuilds?.FirstOrDefault(m => m.Ismain);
                if (rec != null)
                {
                    var mh = rec.Majorhardware;
                    if (mh == null)
                    {
                        mh = wrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == rec.Majorhardwareid).FirstOrDefault();
                    }
                    date.Add(mh?.Lasttimebuynew);
                    date.Add(mh?.Lasttimebuyupgrades);
                    date.Add(mh?.Lasttimebuyexpansions);
                    date.Add(mh?.Endofmaintenance);
                    date.Add(mh?.Endofsupport);
                }
            }

            return date.Where(x => x.HasValue).OrderBy(x => x.Value).FirstOrDefault();
        }

        public static string GetMinorDateEOM(this Systemtypes src, IRepositoryWrapper wrapper)
        {
            var mSoftwareEOM = new EOMType
            {
                EOMDate = src?.Majorsoftwarebuilds?.Endofmaintenance,
                Status = src?.Majorsoftwarebuilds == null ? (EOMEnum)0 : (EOMEnum)src?.Majorsoftwarebuilds?.Eomstatus
            };

            var mHarwareEOM = new EOMType();
            // date.Add(src?.EndOfMaintenance);           

            try
            {

                if (src?.Systemtypesmajorhardwarebuilds != null)
                {
                    var rec = src?.Systemtypesmajorhardwarebuilds?.FirstOrDefault(m => m.Ismain);
                    if (rec != null)
                    {
                        var mh = rec.Majorhardware;
                        if (mh == null)
                        {
                            mh = wrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == rec.Majorhardwareid).FirstOrDefault();
                        }
                        mHarwareEOM = new EOMType
                        {
                            EOMDate = mh?.Endofmaintenance,
                            Status = (mh?.Eomstatus != null) ? (EOMEnum)mh?.Eomstatus : (EOMEnum)0
                        };
                    }
                }


                if (mSoftwareEOM.Status == EOMEnum.Default && mHarwareEOM.Status == EOMEnum.Default)// && mSoftwareEOM.EOMDate != null && mHarwareEOM.EOMDate !=null)
                {
                    return (mSoftwareEOM.EOMDate <= mHarwareEOM.EOMDate) ? mSoftwareEOM.EOMDate.Value.ToLongDateString() : mHarwareEOM.EOMDate.Value.ToLongDateString();
                }
                else if (mSoftwareEOM.Status == EOMEnum.Default && mHarwareEOM.Status != EOMEnum.Default)// && mSoftwareEOM.EOMDate != null)
                {
                    return mSoftwareEOM.EOMDate.Value.ToLongDateString();
                }
                else if (mSoftwareEOM.Status != EOMEnum.Default && mHarwareEOM.Status == EOMEnum.Default)// && mHarwareEOM.EOMDate != null)
                {
                    return mHarwareEOM.EOMDate.Value.ToLongDateString();
                }
                else if (mSoftwareEOM.Status == EOMEnum.NotAnnounced || mHarwareEOM.Status == EOMEnum.NotAnnounced)
                {
                    return ("Not Announced").ToUpper();
                }
                else if (mSoftwareEOM.Status == EOMEnum.NotSpecified && mHarwareEOM.Status == EOMEnum.NotSpecified)
                {
                    return ("Not Specified").ToUpper();
                }

            }
            catch(Exception e )
            {
                string vv = e.Message;
            }
            return null;
        }

        public static string GetDateFormatEOM(this Systemtypes src, IRepositoryWrapper wrapper)
        {
            var mSoftwareEOM = new EOMType
            {
                EOMDate = src?.Majorsoftwarebuilds?.Endofmaintenance,
                Status = (EOMEnum)src?.Majorsoftwarebuilds?.Eomstatus
            };

            var mHarwareEOM = new EOMType();
            // date.Add(src?.EndOfMaintenance);           

            if (src?.Systemtypesmajorhardwarebuilds != null)
            {
                var rec = src?.Systemtypesmajorhardwarebuilds?.FirstOrDefault(m => m.Ismain);
                if (rec != null)
                {
                    var mh = rec.Majorhardware;
                    if (mh == null)
                    {
                        mh = wrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == rec.Majorhardwareid).FirstOrDefault();
                    }
                    mHarwareEOM = new EOMType
                    {
                        EOMDate = mh?.Endofmaintenance,
                        Status = (mh?.Eomstatus != null) ? (EOMEnum)mh?.Eomstatus : (EOMEnum)0
                    };
                }
            }


            if (mSoftwareEOM.Status == EOMEnum.Default && mHarwareEOM.Status == EOMEnum.Default  ) //&& mHarwareEOM.EOMDate != null && mSoftwareEOM.EOMDate != null)
            {
                return (mSoftwareEOM.EOMDate <= mHarwareEOM.EOMDate) ? mSoftwareEOM.EOMDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : mHarwareEOM.EOMDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else if (mSoftwareEOM.Status == EOMEnum.Default && mHarwareEOM.Status != EOMEnum.Default)// && mSoftwareEOM.EOMDate != null)
            {
                return mSoftwareEOM.EOMDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else if (mSoftwareEOM.Status != EOMEnum.Default && mHarwareEOM.Status == EOMEnum.Default ) //&& mHarwareEOM.EOMDate != null)
            {
                return  
                    mHarwareEOM.EOMDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)  ;
            }
            else if (mSoftwareEOM.Status == EOMEnum.NotAnnounced || mHarwareEOM.Status == EOMEnum.NotAnnounced)
            {
                return ("Not Announced").ToUpper();
            }
            else if (mSoftwareEOM.Status == EOMEnum.NotSpecified && mHarwareEOM.Status == EOMEnum.NotSpecified)
            {
                return ("Not Specified").ToUpper();
            }
            return null;
        }



        public static string toSystemTypeName(this Systemtypes data, IRepositoryWrapper wrapper)
        {
            var name = "";
            if (
                data.Systemtypesmajorhardwarebuilds == null ||
                data.Systemtypesmajorhardwarebuilds.Any(x => x.Majorhardware == null) ||
                data.Systemtypesmajorhardwarebuilds.Any(x => x.Majorhardware.Platform == null) ||
                data.Majorsoftwarebuilds == null ||
                data.Majorsoftwarebuilds.Orgeqpmanufacturer == null
               )
            {
                var src = wrapper.SystemType
                    .FindByCondition(x => x.Systemtypeid == data.Systemtypeid, true)
                    .Include(x => x.Majorsoftwarebuilds)
                    .Include(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Majorsoftwarebuilds.Orgeqpmanufacturer)
                    .SingleOrDefault();

                name = src.SystemTypeName(wrapper);
            }
            else
            {
                name = data.SystemTypeName(wrapper);
            }
            return name;
        }

        public static string SystemTypeName(this Systemtypes systemType, IRepositoryWrapper wrapper)
        {
            var src = wrapper.SystemType
                .FindByCondition(k => k.Systemtypeid == systemType.Systemtypeid,true)
                .Include(p => p.Majorsoftwarebuilds).ThenInclude(p=>p.Orgeqpmanufacturer)
                .Include(x=>x.Majorsoftwarebuilds).ThenInclude(x=>x.Productname)
                .Include(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Platform)
                .Include(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction)
                .FirstOrDefault();

            var mhb = src.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware;
            var name =
                $"{src.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer} {(src.Majorsoftwarebuilds.Productname != null ? src.Majorsoftwarebuilds.Productname.Description:"")} {src.Majorsoftwarebuilds.Softwareversion}";

            if (mhb != null)
            {
                
                if( mhb.Buildconstruction!=null && ( mhb.Buildconstruction.Rule == (int)BuildconstructionRuleEnum.ProprietaryHW || mhb.Buildconstruction.Rule == (int)BuildconstructionRuleEnum.CotsHW ))
                {
                    name += $"<b class=\"text-lowercase\"> on </b> {mhb?.Hardwaresolution} {mhb?.Platform.Platform} {mhb?.Hardwaretype}";
                }
                else
                {
                    name += $"<b class=\"text-lowercase\"> on </b> {mhb?.Platform.Platform}";
                }

            }

            if (src.Systemtypesmajorhardwarebuilds.Any(x => x.Deleted == false && x.Ismain == false))
            {
                name += "<b class=\"text-lowercase\"> with </b>";
                var mhs = src.Systemtypesmajorhardwarebuilds.Where(x => x.Deleted == false && x.Ismain == false)
                    .Select(x => x.Majorhardware);
                foreach (var mh in mhs)
                {
                    if (mh.Platform != null)
                    {
                        name += $"{mhb.Hardwaresolution} {mh.Platform.Platform} {mh.Hardwaretype} ";
                    }
                }
            }

            return name;
        }


        public static string toSystemTypeName(this Entities.Models.SystemType data, IRepositoryWrapper wrapper)
        {
            var name = "";
            if (
                data.SystemTypesMajorHardwareBuilds == null ||
                data.SystemTypesMajorHardwareBuilds.Any(x => x.MajorHardware == null) ||
                data.SystemTypesMajorHardwareBuilds.Any(x => x.MajorHardware.Platform == null) ||
                data.MajorSoftwareBuilds == null ||
                data.MajorSoftwareBuilds.ProductName == null ||
                data.MajorSoftwareBuilds.OriginalEquipmentManufacturer == null
               )
            {
                var src = wrapper.SystemType
                    .FindByCondition(x => x.Systemtypeid == data.SystemTypeId, true)
                    .Include(x => x.Majorsoftwarebuilds)
                    .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                    .Include(x => x.Systemtypesmajorhardwarebuilds)

                    .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Majorsoftwarebuilds.Orgeqpmanufacturer).SingleOrDefault();

                name = src.SystemTypeName(wrapper);
            }
            else
            {
                name = SystemTypeMapper.SetSystemTypeMapper(data).SystemTypeName(wrapper);
            }
            return name;
        }


        public static string toHardwareName(this Entities.Models.SystemType src)
        {
            var mhb = src.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain && x.Deleted == false)?.MajorHardware;
            var name = "";
            var countAdditional = 0;
            if (mhb != null)
            {
                name += $"{mhb?.Platform.PlatformDescription} {mhb?.HardwareType}";
            }
            if (src.SystemTypesMajorHardwareBuilds.Any(x => x.Deleted == false && x.IsMain == false))
            {
                var mhs = src.SystemTypesMajorHardwareBuilds.Where(x => x.Deleted == false && x.IsMain == false)
                    .Select(x => x.MajorHardware);
                name += "<b class=\"text-lowercase\"> with </b>";
                foreach (var mh in mhs)
                {
                    name += $"{mh.Platform.PlatformDescription} {mh.HardwareType} ";
                    countAdditional += 1;
                    if (mhs.Count() > countAdditional)
                    {
                        name += "<b class=\"text-lowercase\"> and </b>";
                    }
                }
            }
            return name;
        }
        public static string toHardwareName(this Systemtypes src)
        {
            var mhb = src.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware;
            var name = "";
            var countAdditional = 0;
            if (mhb != null)
            {
                name += $"{mhb?.Platform.Platform} {mhb?.Hardwaretype}";
            }
            if (src.Systemtypesmajorhardwarebuilds.Any(x => x.Deleted == false && x.Ismain == false))
            {
                var mhs = src.Systemtypesmajorhardwarebuilds.Where(x => x.Deleted == false && x.Ismain == false)
                    .Select(x => x.Majorhardware);
                name += "<b class=\"text-lowercase\"> with </b>";
                foreach (var mh in mhs)
                {
                    name += $"{mh.Platform.Platform} {mh.Hardwaretype} ";
                    countAdditional += 1;
                    if (mhs.Count() > countAdditional)
                    {
                        name += "<b class=\"text-lowercase\"> and </b>";
                    }
                }
            }
            return name;
        }

        public static string toLcmDbExportHardwareName(this Systemtypes src)
        {
            var mhb = src.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware;
            var name = "";
            var countAdditional = 0;
            if (mhb != null)
            {
                name += $"{mhb?.Platform.Platform} {mhb?.Hardwaretype}";
            }
            if (src.Systemtypesmajorhardwarebuilds.Any(x => x.Deleted == false && x.Ismain == false))
            {
                var mhs = src.Systemtypesmajorhardwarebuilds.Where(x => x.Deleted == false && x.Ismain == false)
                    .Select(x => x.Majorhardware);
                name += " with ";
                foreach (var mh in mhs)
                {
                    name += $"{mh.Platform.Platform} {mh.Hardwaretype} ";
                    countAdditional += 1;
                    if (mhs.Count() > countAdditional)
                    {
                        name += " and ";
                    }
                }
            }
            return name;
        }

        public static string toAssetClassDescription(this Entities.Models.SystemType src)
        {
            string assetClass = src.AssetClassIdNavigation?.AssetClassDescription?.Trim() ?? "";
            string buildConstruction = $"{src.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain).SingleOrDefault()?.MajorHardware?.BuildConstruction?.BuildConstructionDescription}";
            if (assetClass != "" && buildConstruction != "")
            {
                return $"{assetClass} <b class=\"text-lowercase\"> on </b> {buildConstruction}";
            }
            else
            {
                return $"{assetClass}{buildConstruction}";
            }
        }
        public static string toAssetClassDescription(this Entities.Models.SystemType src, IRepositoryWrapper wrapper)
        {
            if (
                (src.AssetClassIdNavigation == null && src.AssetClassId != null) ||
                src.SystemTypesMajorHardwareBuilds == null ||
                src.SystemTypesMajorHardwareBuilds.Any(x => x.MajorHardware == null) ||
                src.SystemTypesMajorHardwareBuilds.Any(x => (x.MajorHardware.BuildConstruction == null && x.MajorHardware.BuildConstructionId != null))
               )
            {
                var systemType = wrapper.SystemType
                .FindByCondition(x => x.Systemtypeid == src.SystemTypeId)
                .Include(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Buildconstruction)
                .Include(x => x.Assetclass)
                .SingleOrDefault();
                return SystemTypeMapper.GetSystemTypeMapper(systemType).toAssetClassDescription();
            }
            else
            {
                return src.toAssetClassDescription();
            }
        }

        public static string toAssetClassDescription(this Systemtypes src) {
            string assetClass = src.Assetclass?.Assetclass?.Trim() ?? "";
            string buildConstruction = $"{src.Systemtypesmajorhardwarebuilds.Where(x => x.Ismain).SingleOrDefault()?.Majorhardware?.Buildconstruction?.Buildconstruction}";
            if (assetClass != "" && buildConstruction != "")
            {
                return $"{assetClass} <b class=\"text-lowercase\"> on </b> {buildConstruction}";
            }
            else {
                return $"{assetClass}{buildConstruction}";
            }
        }
        public static string toAssetClassDescription(this Systemtypes src, IRepositoryWrapper wrapper )
        {
            if (
                (src.Assetclass == null && src.Assetclassid != null) ||
                src.Systemtypesmajorhardwarebuilds == null ||
                src.Systemtypesmajorhardwarebuilds.Any(x => x.Majorhardware == null) ||
                src.Systemtypesmajorhardwarebuilds.Any(x => (x.Majorhardware.Buildconstruction == null && x.Majorhardware.Buildconstructionid != null))
               )
            {
                var systemType = wrapper.SystemType
                .FindByCondition(x => x.Systemtypeid == src.Systemtypeid)
                .Include(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Buildconstruction)
                .Include(x => x.Assetclass)
                .SingleOrDefault();
                return systemType.toAssetClassDescription();
            }
            else {
                return src.toAssetClassDescription();
            }
        }

        public static List<string> SystemTypeNameList(this Systemtypes src)
        {
            List<string> list = new List<string>();
            var mhb = src.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware;
            list.Add(src.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer);
            list.Add(src.Majorsoftwarebuilds.Productname.Description);
            list.Add(src.Majorsoftwarebuilds.Softwareversion);
            if (mhb != null)
            {
                list.Add("<b class=\"text-lowercase\"> on </b>");
                list.Add(mhb?.Platform.Platform);
                list.Add(mhb?.Hardwaretype);
            }

            //if (src.SystemTypesMajorHardwareBuilds.Any(x => x.Deleted == false && x.IsMain == false))
            //{
            //    list.Add("<b class=\"text-lowercase\"> with </b>");

            //    var mhs = src.SystemTypesMajorHardwareBuilds.Where(x => x.Deleted == false && x.IsMain == false)
            //        .Select(x => x.MajorHardware);
            //    foreach (var mh in mhs)
            //    {
            //        name += $" {mh.Platform.PlatformDescription} {mh.HardwareType} ";
            //    }
            //}

            return list;
        }
        public static List<string> toSystemTypeNameList(this Systemtypes data, IRepositoryWrapper wrapper)
        {
            var name = new List<string>();
            if (
                data.Systemtypesmajorhardwarebuilds == null ||
                data.Systemtypesmajorhardwarebuilds.Any(x => x.Majorhardware == null) ||
                data.Systemtypesmajorhardwarebuilds.Any(x => x.Majorhardware.Platform == null) ||
                data.Majorsoftwarebuilds == null ||
                data.Majorsoftwarebuilds.Orgeqpmanufacturer == null
               )
            {
                var src = wrapper.SystemType
                    .FindByCondition(x => x.Systemtypeid == data.Systemtypeid, true)
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Majorsoftwarebuilds.Orgeqpmanufacturer).SingleOrDefault();

                name = src.SystemTypeNameList();
            }
            else
            {
                name = data.SystemTypeNameList();
            }
            return name;
        }


        //public static string toSystemTypeName(this Entities.Models.SystemType src, MajorHardwareBuild mhb, List<MajorHardwareBuild> mhs,MajorSoftwareBuild msb)
        //{

        //    var name =
        //        $"{msb.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription} {src.MajorSoftwareBuilds.SoftwareApplicationType} {src.MajorSoftwareBuilds.SoftwareApplication}";

        //    if (mhb != null)
        //    {
        //        name += $" on {mhb?.Platform} {mhb?.HardwareType}";
        //    }
        //    if (mhs != null && mhs.Any())
        //    {
        //        name += " with ";

        //        foreach (var mh in mhs)
        //        {
        //            name += $" {mh.Platform} {mh.HardwareType} ";
        //        }

        //    }

        //    return name;
        //    //if (major != null)
        //    //{
        //    //    var mhb = major;
        //    //    return $"{src.SystemTypeNameOem} - {mhb?.Platform} - {mhb?.HardwareType}";
        //    //}
        //    //else
        //    //{
        //    //    return src.SystemTypeNameOem;
        //    //}
        //}
    }
}
