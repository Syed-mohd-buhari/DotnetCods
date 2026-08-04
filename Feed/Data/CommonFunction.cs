using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using TEMS.Data;
using TEMS.DataTransferObject;
using TEMS.Error;
using TEMS.Logs;
using static TEMS.Logs.Logger;

namespace TEMS.Entity
{
    internal class CommonFunction
    {

        /*
       * Module Name : FileName
       * Description : it is used to get the date from Xml files
       * Parameter : xmlFile
       * xmlFile - it is string argumant it is use to store the xml file path

       */
        public static string FileNametoDate(string xmlFile)
        {
            DateTime date = new DateTime();



            try
            {
                //20230317_113936 sample date in file name
                string pattern = @"(\d{4})(\d{2})(\d{2})_(\d{2})(\d{2})(\d{2})";

                Match match = Regex.Match(Path.GetFileName(xmlFile), pattern);

                if (match.Success)
                {
                    int year = int.Parse(match.Groups[1].Value);
                    int month = int.Parse(match.Groups[2].Value);
                    int day = int.Parse(match.Groups[3].Value);
                    int hours = int.Parse(match.Groups[4].Value);
                    int minutes = int.Parse(match.Groups[5].Value);
                    int seconds = int.Parse(match.Groups[6].Value);

                    date = new DateTime(year, month, day, hours, minutes, seconds);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "FileNametoDate parsing issue - " + xmlFile, $"{ex.Message}");
                throw;
            }
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string FileNametoNodetypeName(string xmlFile)
        {
            string nodeTypeName = "";
            try
            {
                //VUK_Output_IP-STP_20230309_143046.xml to IP-STP in file name
                nodeTypeName = Path.GetFileName(xmlFile).Split("_")[2];
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "FileNametoNodetypeName parsing issue - " + xmlFile, $"{ex.Message}");
                throw;
            }
            return nodeTypeName;
        }

        public static string ConvertDBDate(string dbDate)
        {
            DateTime parsedDate;
            string formattedDate = "";
            if (dbDate is null || dbDate == "")
            {
                return "";
            }
            try
            {
                string dbformat = "dd-MMM-yy hh.mm.ss.000000000 tt";
                string processformat = "yyyy-MM-dd HH:mm:ss";

                if (DateTime.TryParseExact(dbDate, dbformat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    formattedDate = parsedDate.ToString(processformat);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "FileName", $"{ex.Message}");
                throw;
            }
            return formattedDate;
        }

        public static string RawDateConvertion(string rawDate)
        {
            DateTime date = new DateTime();
            string pattern = "";
            Match match = Regex.Match(rawDate, pattern);
            int year = 0, month = 0, day = 0, hours = 0, minutes = 0, seconds = 0;
            bool dateConverted = false;
            try
            {
                //To handle for date format 2021-31-12 and 12-31-2021
                if (rawDate.Length == 10)
                {
                    pattern = @"^[0-9-]+$";
                    match = Regex.Match(rawDate, pattern);
                    if (match.Success)
                    {
                        if (rawDate.IndexOf("-") == 4 && rawDate.LastIndexOf("-") == 7)
                        {
                            pattern = @"(\d{4})-(\d{2})-(\d{2})";
                            match = Regex.Match(rawDate, pattern);
                            if (match.Success)
                            {
                                try
                                {
                                    year = int.Parse(match.Groups[1].Value);
                                    month = int.Parse(match.Groups[2].Value);
                                    day = int.Parse(match.Groups[3].Value);
                                    date = new DateTime(year, month, day, hours, minutes, seconds);
                                    dateConverted = true;
                                }
                                catch (Exception)
                                {
                                    year = 0; month = 0; day = 0; hours = 0; minutes = 0; seconds = 0;
                                }
                            }
                        }
                        else if (rawDate.IndexOf("-") == 2 && rawDate.LastIndexOf("-") == 5)
                        {
                            pattern = @"(\d{2})-(\d{2})-(\d{4})";
                            match = Regex.Match(rawDate, pattern);
                            if (match.Success)
                            {
                                try
                                {
                                    year = int.Parse(match.Groups[3].Value);
                                    month = int.Parse(match.Groups[2].Value);
                                    day = int.Parse(match.Groups[1].Value);
                                    date = new DateTime(year, month, day, hours, minutes, seconds);
                                    dateConverted = true;
                                }
                                catch (Exception)
                                {
                                    year = 0; month = 0; day = 0; hours = 0; minutes = 0; seconds = 0;
                                }
                            }
                        }


                    }
                }
                //Mon Jan 16 00:00:00 GMT 2023
                if (rawDate.Contains("GMT") && !dateConverted)
                {
                    pattern = @"^([A-Za-z]{3})\s([A-Za-z]{3})\s(\d{2})\s(\d{2}):(\d{2}):(\d{2})\sGMT\s(\d{4})$";
                    match = Regex.Match(rawDate, pattern);
                    if (match.Success)
                    {
                        try
                        {
                            year = int.Parse(match.Groups[7].Value);
                            month = DateTime.ParseExact(match.Groups[2].Value, "MMM", CultureInfo.CurrentCulture).Month;
                            day = int.Parse(match.Groups[3].Value);
                            hours = int.Parse(match.Groups[4].Value);
                            minutes = int.Parse(match.Groups[5].Value);
                            seconds = int.Parse(match.Groups[6].Value);
                            date = new DateTime(year, month, day, hours, minutes, seconds);
                            dateConverted = true;
                        }
                        catch (Exception)
                        {
                            year = 0; month = 0; day = 0; hours = 0; minutes = 0; seconds = 0;
                        }
                    }
                }
                /*
                2021-11-12T08:10:49
                2022-08-18T11:25:54.704+01:00
                2023-03-22T11:53:38.283Z
                */
                if (rawDate.Length >= 19 && !dateConverted)
                {
                    string? dateTrim = rawDate.Substring(0, 19);
                    pattern = @"^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})$";
                    match = Regex.Match(dateTrim, pattern);
                    if (match.Success)
                    {
                        if (dateTrim.IndexOf("-") == 4 && dateTrim.LastIndexOf("-") == 7)
                        {
                            pattern = @"^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})$";
                            match = Regex.Match(dateTrim, pattern);
                            if (match.Success)
                            {
                                try
                                {
                                    year = int.Parse(match.Groups[1].Value);
                                    month = int.Parse(match.Groups[2].Value);
                                    day = int.Parse(match.Groups[3].Value);
                                    hours = int.Parse(match.Groups[4].Value);
                                    minutes = int.Parse(match.Groups[5].Value);
                                    seconds = int.Parse(match.Groups[6].Value);
                                    date = new DateTime(year, month, day, hours, minutes, seconds);
                                    dateConverted = true;
                                }
                                catch (Exception)
                                {
                                    year = 0; month = 0; day = 0; hours = 0; minutes = 0; seconds = 0;
                                }
                            }

                        }
                    }
                }
                if (rawDate.Length >= 10)
                {

                    pattern = @"^[0-9-]+$";
                    match = Regex.Match(rawDate, pattern);
                    if (match.Success)
                    {

                        if (rawDate.IndexOf("-") == 4 && rawDate.LastIndexOf("-") == 7)
                        {

                            pattern = @"(\d{4})-(\d{2})-(\d{2})";
                            match = Regex.Match(rawDate, pattern);
                            if (match.Success)
                            {
                                try
                                {
                                    year = int.Parse(match.Groups[1].Value);
                                    month = int.Parse(match.Groups[2].Value);
                                    day = int.Parse(match.Groups[3].Value);
                                    date = new DateTime(year, month, day, hours, minutes, seconds);
                                    dateConverted = true;
                                }
                                catch (Exception)
                                {
                                    year = 0; month = 0; day = 0; hours = 0; minutes = 0; seconds = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "RawDateConvertion", $"{ex.Message}");
                throw;
            }
            if (dateConverted)
                return date.ToString("yyyy-MM-dd HH:mm:ss");
            else return "";
        }

        public static string getAppConfigValue(string appConfigName)
        {
            string? appConfigValue = "";
            if (string.IsNullOrEmpty(appConfigName))
            {
                appConfigValue = "";
                throw new CustomError("Accessing AppConfig which is not available", " Module name: getAppConfigValue");
            }
            else
            {
                appConfigValue = ConfigurationManager.AppSettings[appConfigName];
                appConfigValue = appConfigValue != null ? appConfigValue : "";
            }
            return appConfigValue;
        }

        public static int getUserName()
        {
            int userName = 1;//admincam           
            return userName;
        }

        public static GetOpcoAndOemIdsGto GetOperationalIds(string opCo, string elementName, string oem)
        {


            string opCoQuery = $"Upper(OPCO) = '{opCo.Replace("VODAFONE_UK", "UK")}'";
            string opCoid = DataBaseFunctions.GetPrimaryKey("opcos", opCoQuery);

            string oemQuery = $"Upper(ORIGINALEQUIPMENTMANUFACTURER) = '{oem}'";
            string oemid = DataBaseFunctions.GetPrimaryKey("originalequipmentmanufacturers", oemQuery);
            if(oemid == "NA")
            {
                oemid = "null";
            }
            else
            {
                oemid = oemid;
            }

            string assetQuery = $"Upper(OPCOID) = '{opCoid}' AND Upper(ELEMENTNAME) = '{elementName.ToUpper()}' AND Upper(orgeqpmanufacturerid) = {oemid}";
            string getExistingassetId = DataBaseFunctions.GetPrimaryKey("networkelementsasplanned", assetQuery);
            var operationalIds = new GetOpcoAndOemIdsGto()
            {
                OpCoId = opCoid,
                AssetId = getExistingassetId,
                OemId = oemid
            };
            return operationalIds;

        }

        public static string GetAssetQuery(string OpcoId, string ElementName, string OemId)
        {
            var query = $"select p.networkelementasplannedid,p.locationid,p.orgeqpmanufacturerid,dc.systemtypeid,dc.designcomponentfamilyid, dc.designcomponentid, p.buildbagid " +
                                                $"from networkelementsasplanned p join designcomponents dc on dc.designcomponentid = p.designcomponentid " +
                                                $"where Upper(OPCOID) = '{OpcoId}' AND Upper(ELEMENTNAME) = '{ElementName}' AND Upper(ORGEQPMANUFACTURERID) = '{OemId}' ";

            return query;
        }

        public static string GetInsertionQueryForNetworkAsis(NetworkAsIsInsertionDto dto, string tableName)
        {
            return $"INSERT INTO {tableName} " +
                     $"(OPCOID,SYSTEMTYPEID,ORGEQPMANUFACTURERID,NETWORKELEMENTASPLANNEDID,LOCATIONID" +
                     $",SOFTWAREPRODUCTNUMBER,SOFTWAREPRODUCTIONDATE,SOFTWAREINSTALLDATE,DATAACQUISITIONDATE,DATAACQUISITIONMETHOD,MANUALOVERRIDE," +
                     $"ELEMENTDEPLOYMENTNAME,NODETYPE,PLATFORMTYPE,HARDWARETYPE,SOFTWARERELEASEINFORMATION,CREATIONUSER,MODIFICATIONUSER,ELEMENTMANAGER) VALUES" +
                     $"('{dto.OpCoId}','{dto.SystemId}','{dto.OemId}','{dto.AssetId}','{dto.LocationId}','{dto.SoftwareProductNumber}',TO_TIMESTAMP('{dto.SoftwareProductDate}','YYYY-MM-DD HH24:MI:SS')" +
                     $",TO_TIMESTAMP('{dto.SoftwareInstallDate}','YYYY-MM-DD HH24:MI:SS'),TO_TIMESTAMP('{dto.DataAcquisitionDate}','YYYY-MM-DD HH24:MI:SS'),'Discovered','{0}'," +
                     $"'{dto.ElementName}','{dto.NodeType}','{dto.PlatformType}'," +
                     $"'{dto.HardwareType}','{dto.SoftwareReleaseInformation}','{dto.User}','{dto.User}','{dto.Spare1ossorenm}') returning  {DBModelFunctions.GetPrimaryKey(tableName)}  into :pkcreated";
        }

        public static string GetInsertionQueryForIdentityAsis(string tableName, IdentityAsisCreateDto dto, string assetId)
        {
            return $"INSERT INTO {tableName} (VALUE,RESOURCEKEY,ASSETID,CREATIONUSER,MODIFICATIONUSER,CATEGORYID) VALUES" +
                $"('{dto.IPAddress}', '{dto.ResourceKey}', '{assetId}', '{dto.User}', '{dto.User}', '{dto.CategoryId}')  returning  {DBModelFunctions.GetPrimaryKey(tableName)}  into :pkcreated";
        }

        public static string GetProperElementName(string Elementname)
        {
            if (Elementname.Contains("BSP-"))
            {
                var splitValue = Elementname.Split("-");
                return splitValue[1];
            }
            else
            {
                return Elementname;
            }
        }

        public static string SoftwareVersionQuery(string Assetid)
        {
            var query = $"select msb.softwareversion,p.isassured from networkelementsasplanned p " +
                        $"join designcomponents dc on dc.designcomponentid = p.designcomponentid " +
                        $"join systemtypes sy on sy.systemtypeid = dc.systemtypeid " +
                        $"join majorsoftwarebuilds msb on msb.majorsoftwarebuildsid = sy.majorsoftwarebuildsid " +
                        $"where networkelementasplannedid = {Assetid} ";

            return query;
        }

        public static string SoftwareVersionQueryForAsIs(string Asisid)
        {
            var query = $"select asis.softwarereleaseinformation from networkelementsasis asis " +                        
                        $"where networkelementasisid = {Asisid} ";

            return query;
        }

        public static bool UpdateAssetTable(OracleConnection connection, string assetId, string currentVerion, string tableName)
        {
            var isUpdated = false;
            try
            {
                int user = getUserName();
                var assetSoftwareVersionQuery = SoftwareVersionQuery(assetId);
                List<Dictionary<string, string>> assetSoftwareVersion =  DataBaseFunctions.selectFromDBByQuery(assetSoftwareVersionQuery);
                foreach(var version in assetSoftwareVersion)
                {
                    if (version["softwareversion"] == currentVerion)
                    {
                        var updateQuery = $"UPDATE {tableName} set isassured = 1, modificationuser={user}, modificationdate=SYS_EXTRACT_UTC(systimestamp) " +
                            $"where {DBModelFunctions.GetPrimaryKey(tableName)}='{assetId}'";

                        var updateId = DataBaseFunctions.DBUpdation(connection, updateQuery).ToString();
                        isUpdated = updateId != null ? true : false;
                        break;
                    }
                    else if(version["softwareversion"] != currentVerion)
                    {
                        var updateQuery = $"UPDATE {tableName} set isassured = 0, modificationuser={user}, modificationdate=SYS_EXTRACT_UTC(systimestamp) " +
                            $"where {DBModelFunctions.GetPrimaryKey(tableName)}='{assetId}'";

                        var updateId = DataBaseFunctions.DBUpdation(connection, updateQuery).ToString();
                        isUpdated = updateId != null ? true : false;
                        break;
                    }
                    isUpdated = true;
                }
                
                return isUpdated;
            }
            catch
            {
                return isUpdated;
            }

        }

        public static bool CheckExistingEntryInAsIsTable(OracleConnection connection, string asIsId, string assetId,string tableName)
        {
            var isUpdated = false;            
            try
            {
                int user = getUserName();
                var asisVersion = "";
                var assetVersion = "";
                var isAssurevalue = "";

                // get asis version
                var asIsSoftwareVersionQuery = SoftwareVersionQueryForAsIs(asIsId);
                List<Dictionary<string, string>> asIsSoftwareVersion = DataBaseFunctions.selectFromDBByQuery(asIsSoftwareVersionQuery);

                //get asset version
                var assetSoftwareVersionQuery = SoftwareVersionQuery(assetId);
                List<Dictionary<string, string>> assetSoftwareVersion = DataBaseFunctions.selectFromDBByQuery(assetSoftwareVersionQuery);

                foreach(var asisversion in asIsSoftwareVersion)
                {
                    asisVersion = asisversion["softwarereleaseinformation"];
                }
                foreach(var assetversion in assetSoftwareVersion)
                {
                    assetVersion = assetversion["softwareversion"];
                    isAssurevalue = assetversion["isassured"];
                }

                if(isAssurevalue != "1")
                {
                    if(assetVersion == asisVersion)
                    {
                        var updateQuery = $"UPDATE networkelementsasplanned set isassured = 1, modificationuser={user}, modificationdate=SYS_EXTRACT_UTC(systimestamp) " +
                            $"where {DBModelFunctions.GetPrimaryKey("networkelementsasplanned")}='{assetId}'";

                        var updateId = DataBaseFunctions.DBUpdation(connection, updateQuery).ToString();
                        isUpdated = updateId != null ? true : false;
                    }
                    isUpdated = true;
                }
                else
                {
                    isUpdated = true;
                }

                return isUpdated;
            }
            catch
            {
                return isUpdated;
            }

        }


        public static string GetDCAndDCFNameRecords(string assetId)
        {
            var query = $"select sub.alias,sub.description as subnetworkname,oem.originalequipmentmanufacturer," +
                $"prd.description as productname,msb.softwareversion,mhb.hardwaresolution,pl.platform,pl.platformid," +
                $"mhb.hardwaretype,bc.rule,symb.ismain,symb.deleted,opco.opco,dc.designcomponentid,bag.bagdescription from networkelementsasplanned p " +
                $"join designcomponents dc on dc.designcomponentid = p.designcomponentid " +
                $"join designcomponentfamilies dcf on dcf.designcomponentfamilyid = dc.designcomponentfamilyid " +
                $"join systemtypes sy on sy.systemtypeid = dc.systemtypeid " +
                $"join majorsoftwarebuilds msb on msb.majorsoftwarebuildsid = sy.majorsoftwarebuildsid " +
                $"join originalequipmentmanufacturers oem on oem.orgeqpmanufacturerid = msb.orgeqpmanufacturerid " +
                $"join productname prd on prd.productnameid = msb.productnameid " +
                $"join systemtypesmajorhardwarebuilds symb on symb.systemtypeid = sy.systemtypeid " +
                $"join majorhardwarebuilds mhb on  mhb.majorhardwareid = symb.majorhardwareid " +
                $"join platforms pl on pl.platformid = mhb.platformid " +
                $"join subnetworkboundaries sub on sub.id = dcf.subnetworkboundaryid " +
                $"join buildconstructions bc on bc.buildconstructionid = mhb.buildconstructionid " +
                $"join opcos opco on opco.opcoid = p.opcoid " +
                $"join buildbags bag on bag.buildbagid = p.buildbagid "+
                $"where networkelementasplannedid = {assetId}";
            return query;
        }

        public static string GetInsertionQueryForDcfLifeCycle(string tableName, DcfLifeCycle dto)
        {
            var eventId = @"""EventID""";
            var eventName = @"""EventName""";
            return $@"INSERT INTO {tableName} ({eventId},{eventName},currentdetails,opcoid,dcfid,resourcekey,categorytype,dcfdescription,opcodescription,"+
                 $"dcdescription,creationuser,modificationuser,dcid,bagname) VALUES" +
                $"('{dto.EventId}', '{dto.EventName}', '{dto.Currentdetails}', '{dto.Opcoid}', '{dto.Dcfid}', '{dto.Resourcekey}', '{dto.Categorytype}', '{dto.Dcfdescription}'," +
                $"'{dto.Opcodescription}','{dto.Dcfdescription}', '{1}', '{1}','{dto.DcId}','{dto.BagName}')  returning  dcflifecycleid  into :pkcreated";
        }

        public static List<string> AsIsAttributes { get; set; } = new List<string> { "softwareproductnumber",
            "softwareproductdate",
            "softwareinstalldate",
            "dataacquisitiondate",
            "nodetype",
            "platformtype",
            "hardwaretype",
            "softwarereleaseinformation",
            "value"
        };
    }
}
