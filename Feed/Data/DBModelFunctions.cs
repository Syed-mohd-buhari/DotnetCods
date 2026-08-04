using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Xml.Linq;
using TEMS.DataTransferObject;
using TEMS.Entity;
using TEMS.Error;
using TEMS.Logs;
using static TEMS.Logs.Logger;

namespace TEMS.Data
{
    public static class DBModelFunctions
    {
        /*
     * Module Name : GetDBColumnData
     * Description : To get column data that present in the Model-->editconfig file
     * Parameter : 
     */
        public static Dictionary<string, ArrayList> GetDBColumnData()
        {

            Dictionary<string, ArrayList> columnData = new Dictionary<string, ArrayList>();
            Dictionary<string, string> row = new Dictionary<string, string>();
            columnData["networkelement"] = new ArrayList();
            columnData["softwarecomponent"] = new ArrayList();
            columnData["component"] = new ArrayList();//component is the subtag of software component
            columnData["identities"] = new ArrayList();
            columnData["softwareconfiguration"] = new ArrayList();
            columnData["hardwareconfiguration"] = new ArrayList();
            columnData["configuration"] = new ArrayList();//configuration is the subtag of hardware configuartion
            columnData["function"] = new ArrayList();//function area is a subtag for software configuration
            columnData["functionarea"] = new ArrayList();//function area is a subtag for software configuration
            columnData["subfunction"] = new ArrayList();//function area is a subtag for software configuration
            columnData["subfunctionarea"] = new ArrayList();//subfunctionare is a subtag of functiona area under software configuration
            columnData["subfunctionarea2"] = new ArrayList();
            columnData["swconfigfunctionareas"] = new ArrayList();
            columnData["swconfigsubfunction"] = new ArrayList();
            columnData["swconfigsubfunctionareas"] = new ArrayList();
            columnData["networkelementsasis"] = new ArrayList();
            columnData["identitiesasis"] = new ArrayList();

            string? line = "";
            try
            {
                string DBModelFile = CommonFunction.getAppConfigValue("DBModel");
                StreamReader configReader = new StreamReader(DBModelFile);
                while ((line = configReader.ReadLine()) != null)
                {
                    //Console.WriteLine(line);
                    if (line.StartsWith("#") || line.StartsWith("//"))
                    {
                        continue;
                    }
                    else if (line.Trim() == "")
                    {
                        //do nothing
                    }
                    else if (line.Contains(";"))
                    {
                        string[] values = line.Split(';');
                        row = new Dictionary<string, string>();
                        if (values.Length == 6)
                        {
                            row.Add("childtag", values[1]);
                            row.Add("skiptag", values[2]);
                            row.Add("tablename", values[3]);
                            row.Add("columnname", values[4]);
                            row.Add("columnsize", values[5]);

                            columnData[values[3]].Add(row);
                        }

                        else
                        {
                            throw new CustomError("Improper Congiguration. Doesnt have 5 semicolon", " Module name: GetDBColumnData");
                        }
                    }
                    else
                    {
                        throw new CustomError("Invalid Line", " Module name: GetDBColumnData");
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GetDBColumnData", $"{ex.Message}");
                throw;
            }
            return columnData;
        }


        public static string GetPrimaryKey(string tablename)
        {
            Dictionary<string, ArrayList> tableData = new Dictionary<string, ArrayList>();
            Dictionary<string, string> row = new Dictionary<string, string>();
            string PK_ID = "";
            string? line = "";
            try
            {
                string DBModelFile = CommonFunction.getAppConfigValue("DBTableModel");
                StreamReader configReader = new StreamReader(DBModelFile);
                while ((line = configReader.ReadLine()) != null)
                {
                    if (line.StartsWith("#") || line.StartsWith("//"))
                    {
                        continue;
                    }
                    else if (line.Trim() == "")
                    {
                        //do nothing
                    }
                    else if (line.Contains(";"))
                    {
                        string[] values = line.Split(';');
                        if (values[1] == tablename)
                        {
                            row = new Dictionary<string, string>();
                            if (values.Length == 4)
                            {
                                PK_ID = values[2];
                                break;
                            }
                            else if(values.Length == 5)
                            {
                                PK_ID = values[2];
                                break;
                            }
                            else
                            {
                                throw new CustomError("Improper Congiguration. Doesnt have 3 semicolon", " Module name: getPrimaryKey");
                            }
                        }
                    }
                    else
                    {
                        throw new CustomError("Invalid Line", " Module name: getPrimaryKey" + tablename);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GetDBColumnData", $"{ex.Message}");
                throw;
            }
            return PK_ID.ToLower();
        }

        public static string GetAttributesValue(string tablename)
        {
            Dictionary<string, ArrayList> tableData = new Dictionary<string, ArrayList>();
            Dictionary<string, string> row = new Dictionary<string, string>();
            string Rkey = "";
            string? line = "";
            try
            {
                string DBModelFile = CommonFunction.getAppConfigValue("DBTableModel");
                StreamReader configReader = new StreamReader(DBModelFile);
                while ((line = configReader.ReadLine()) != null)
                {
                    if (line.StartsWith("#") || line.StartsWith("//"))
                    {
                        continue;
                    }
                    else if (line.Trim() == "")
                    {
                        //do nothing
                    }
                    else if (line.Contains(";"))
                    {
                        string[] values = line.Split(';');
                        if (values[1] == tablename)
                        {
                            row = new Dictionary<string, string>();
                            if (values.Length == 5)
                            {
                                Rkey = values[4];
                                break;
                            }                           
                            else
                            {
                                throw new CustomError("Improper Congiguration. Doesnt have 3 semicolon", " Module name: GetResourceKey");
                            }
                        }
                    }
                    else
                    {
                        throw new CustomError("Invalid Line", " Module name: GetResourceKey" + tablename);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GetResourceKey", $"{ex.Message}");
                throw;
            }
            return Rkey.ToLower();
        }

        public static string GetForeignyKey(string tablename)
        {

            Dictionary<string, ArrayList> tableData = new Dictionary<string, ArrayList>();
            Dictionary<string, string> row = new Dictionary<string, string>();
            string FK_ID = "";
            try
            {
                string DBModelFile = CommonFunction.getAppConfigValue("DBTableModel");
                StreamReader configReader = new StreamReader(DBModelFile);
                string? line;
                while ((line = configReader.ReadLine()) != null)
                {
                    if (line.StartsWith("#") || line.StartsWith("//"))
                    {
                        continue;
                    }
                    else if (line.Trim() == "")
                    {
                        //do nothing
                    }
                    else if (line.Contains(";"))
                    {
                        string[] values = line.Split(';');
                        if (values[1] == tablename)
                        {
                            row = new Dictionary<string, string>();
                            if (values.Length == 4)
                            {
                                FK_ID = values[3];
                                break;
                            }
                            else if (values.Length == 5)
                            {
                                FK_ID = values[2];
                                break;
                            }
                            else
                            {
                                throw new CustomError("Improper Congiguration. Doesnt have 3 semicolon", " Module name: getPrimaryKey");
                            }
                        }
                    }
                    else
                    {
                        throw new CustomError("Invalid Line", " Module name: getPrimaryKey" + tablename);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GetDBColumnData", $"{tablename + ex.Message}");
                throw;
            }
            return FK_ID.ToLower();
        }

        public static Dictionary<string, string> getColumnNamesFromConfig(string tableName, Dictionary<string, string> data, string FK_value)
        {
            Dictionary<string, ArrayList> DBDetails = GetDBColumnData();
            Dictionary<string, string> updated_data = new Dictionary<string, string>();
            ArrayList columnDetails = DBDetails[tableName];
            bool validate = true;
            bool foundKey = false;
            string errorDescription = "";
            try
            {
                if (tableName != "networkelement")
                {
                    updated_data.Add(GetForeignyKey(tableName), FK_value);
                }
                data = data ?? new Dictionary<string, string> { };
                foreach (KeyValuePair<string, string> column in data)
                {
                    foundKey = false;
                    foreach (Dictionary<string, string> node in columnDetails)
                    {
                        if (node["columnname"] == column.Key.ToLower())
                        {
                            foundKey = true;
                            if (node["skiptag"] == "true")
                            {
                                break;
                            }

                            //Validating Column Size greater than 30, Since DB Column Name should not be more than 30
                            if (node["columnname"].Length > 30)
                            {
                                validate = false;
                                errorDescription += "\n" + node["columnname"].ToLower() + "Column Name Size > 30;";
                                Logger.WriteLog(TEMLog.DBMappingHelp, "getColumnNamesFromConfig", $"{tableName};{column.Key.ToLower()};false;{tableName};{column.Key.ToLower()};50  //Modify in DBDesign.mapping. Column size greater than 30. Update less than 30 ");

                            }
                            if (Convert.ToInt32(node["columnsize"]) < column.Value.Length)
                            {
                                validate = false;
                                errorDescription += "\n" + column.Value + "Column Value Size > " + Convert.ToInt32(node["columnsize"]) + " Actual : " + column.Value.Length + ";";
                                updated_data.Add(node["columnname"].ToLower(), column.Value.ToString().Remove(Convert.ToInt32(node["columnsize"])));
                                int suggested_column_size = column.Value.Length + 50;
                                Logger.WriteLog(TEMLog.DBTableHelp, "getColumnNamesFromConfig", $"ALTER TABLE {tableName} MODIFY {column.Key.ToLower()} nvarchar2({suggested_column_size}); //Column Size is Suggested based on Column value + 50 ");
                            }
                            else
                            {
                                updated_data.Add(node["columnname"].ToLower(), column.Value);
                                break;
                            }

                        }
                    }

                }

                if (errorDescription != "" || !validate)
                {
                    if (errorDescription != "")
                        Logger.WriteLog(TEMLog.Warn, "getColumnNamesFromConfig ", errorDescription);
                    //throw new CustomError("Validation Failure", " Module name: getColumnNamesFromConfig");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "getColumnNamesFromConfig", $"{ex.Message}");
                throw;
            }
            return updated_data;
        }

        public static string GenerateInsertQuery(string tableName, Dictionary<string, string> datas, string ForeignKey_Value)
        {
            string query = "";
            int user = CommonFunction.getUserName();
            try
            {
                string updateKeys = "";
                string updateValues = "";

                Dictionary<string, string> modified_datas = getColumnNamesFromConfig(tableName, datas, ForeignKey_Value);
                string columns = string.Join(", ", modified_datas.Keys);
                string values = string.Join(", ", modified_datas.Values.Select(v => $"'{v}'"));
                foreach (KeyValuePair<string, string> data in modified_datas)
                {
                    if (updateKeys != "")
                    {
                        updateKeys += $", ";
                        updateValues += $", ";

                    }
                    if (tableName == "networkelement" && data.Key.ToLower() == "xmllastparsefiledate" || tableName == "networkelement" && data.Key.ToLower() == "dataacquisitiondate"
                        || tableName == "networkelement" && data.Key.ToLower() == "softwareinstalldate" || tableName == "networkelement" && data.Key.ToLower() == "softwareinstalldateap"
                        || tableName == "networkelement" && data.Key.ToLower() == "softwareinstalldatecp" || tableName == "networkelement" && data.Key.ToLower() == "softwareproductdate"
                        || tableName == "networkelement" && data.Key.ToLower() == "softwareproductdateap" || tableName == "networkelement" && data.Key.ToLower() == "softwareproductdatecp"
                        || tableName == "component" && data.Key.ToLower() == "productiondate")
                    {
                        updateKeys += $"{data.Key}";
                        updateValues += $" TO_DATE('{data.Value}', 'YYYY-MM-DD HH24:MI:SS')";
                    }
                    else
                    {
                        updateKeys += $"{data.Key}";
                        updateValues += $"'{data.Value}'";
                    }

                }
                if (datas.Count != 0)
                {
                    query = $"INSERT INTO {tableName} ({updateKeys}, creationuser,creationdate) VALUES ({updateValues},{user},SYS_EXTRACT_UTC(systimestamp)) returning " + GetPrimaryKey(tableName) + " into :pkcreated";

                }
                else
                {
                    query = $"INSERT INTO {tableName} ({updateKeys}, creationuser,creationdate) VALUES ({updateValues},{user},SYS_EXTRACT_UTC(systimestamp)) returning " + GetPrimaryKey(tableName) + " into :pkcreated";
                }
                Logger.WriteLog(TEMLog.Debug, "GenerateInsertQuery", $"{query}");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateInsertQuery", $"{ex.Message}");
                throw;
            }
            return query;
        }

        public static List<string> GenerateInsertQueries(string tableName, Dictionary<string, string> datas, string ForeignKey_Value)
        {
            List<string> querys = new List<string>();
            try
            {
                List<string> table_list = GetTableName(tableName);
                foreach (string table in table_list)
                {
                    string _query = GenerateInsertQuery(table, datas, ForeignKey_Value);
                    querys.Add(_query);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateInsertQueries", $"{ex.Message}");
                throw;
            }
            return querys;
        }

        public static List<string> GenerateInsertQueryNclobs(string tableName, Dictionary<string, string> datas, string ForeignKey_Value)
        {
            List<string> querys = new List<string>();
            try
            {
                List<string> table_list = GetTableName(tableName);
                foreach (string table in table_list)
                {
                    string _query = GenerateInsertQueryNclob(table, datas, ForeignKey_Value, "");
                    querys.Add(_query);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateInsertQueryNclobs", $"{ex.Message}");
                throw;
            }
            return querys;
        }

        public static string GenerateInsertQueryNclob(string tableName, Dictionary<string, string> datas, string ForeignKey_Value, string jsonColumn)
        {
            string query = "";
            int user = CommonFunction.getUserName();

            try
            {
                string columns = "", values = "";
                string updateKeys = "";
                string updateValues = "";
                Dictionary<string, string> updated_data = new Dictionary<string, string>();

                // datas.Add(GetForeignyKey(tableName), ForeignKey_Value);              

                var temp1 = new Dictionary<string, string>();
                var keysToExclude = new HashSet<string> { "functionareaname", "functionid", "swconfigsubfunctionid", "subfunctionname", "subfunctionareaname" };
                foreach (var keys in datas)
                {
                    if (keys.Key.ToLower().Trim() == "functionareaname" || keys.Key.ToLower().Trim() == "subfunctionareaname")
                    {
                        temp1.Add(keys.Key, keys.Value);
                    }
                }
                temp1.Add(GetForeignyKey(tableName), ForeignKey_Value);
                var filteredData = datas.Where(kvp => !keysToExclude.Contains(kvp.Key))
                                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                string jsonString = JsonConvert.SerializeObject(filteredData);

                columns = string.Join(", ", temp1.Keys.Concat(new[] { jsonColumn }));
                values = string.Join(", ", temp1.Values.Select(v => $"'{v}'").Concat(new[] { $"'{jsonString}'" }));


                if (datas.Count != 0)
                {
                    query = $"INSERT INTO {tableName} ({columns}, creationuser, creationdate) " +
                            $"VALUES ({values}, {user}, SYS_EXTRACT_UTC(systimestamp)) " +
                            $"RETURNING {GetPrimaryKey(tableName)} INTO :pkcreated";
                }
                else
                {
                    query = $"INSERT INTO {tableName} (creationuser, creationdate) " +
                            $"VALUES ({user}, SYS_EXTRACT_UTC(systimestamp)) " +
                            $"RETURNING {GetPrimaryKey(tableName)} INTO :pkcreated";
                }

                Logger.WriteLog(TEMLog.Debug, "GenerateInsertQuery", $"{query}");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateInsertQuery", $"{ex.Message}");
                throw;
            }

            return query;
        }

        public static string GenerateUpdateQueryNclob(string tableName, Dictionary<string, string> datas, string PrimaryKey_Value, string jsonColumn)
        {
            string query = "";
            int user = CommonFunction.getUserName();
            bool found = false;
            try
            {
                // Dictionary<string, string> modified_datas = getColumnNamesFromConfig(tableName, datas, PrimaryKey_Value);                
                string updateValues = "";
                var keysToExclude = new HashSet<string> { "functionareaname", "functionid", "swconfigsubfunctionid", "subfunctionname", "subfunctionareaname" };

                var filteredData = datas.Where(kvp => !keysToExclude.Contains(kvp.Key))
                                      .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                string jsonString = JsonConvert.SerializeObject(filteredData);
                //string jsonColumn = "functionareadescription";

                updateValues += $" {jsonColumn}='{jsonString}'";




                if (updateValues != "")
                {
                    query = $"UPDATE {tableName} set {updateValues}, modificationuser={user}, modificationdate=SYS_EXTRACT_UTC(systimestamp) where {GetPrimaryKey(tableName)}='{PrimaryKey_Value}'";
                    Logger.WriteLog(TEMLog.Debug, "GenerateUpdateQuery", $"{query}");
                }
                else
                {
                    query = $"UPDATE {tableName} set modificationuser={user}, modificationdate=SYS_EXTRACT_UTC(systimestamp) where {GetPrimaryKey(tableName)}='{PrimaryKey_Value}'";
                    Logger.WriteLog(TEMLog.Debug, "GenerateUpdateQuery", $"{query}");
                }



            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateUpdateQuery", $"{ex.Message}");
                throw;
            }
            return query;
        }



        public static string GenerateUpdateQuery(string tableName, Dictionary<string, string> datas, string PrimaryKey_Value, bool isAsis)
        {
            string query = "";
            int user = CommonFunction.getUserName();
            bool found = false;
            try
            {
                Dictionary<string, string> modified_datas = getColumnNamesFromConfig(tableName, datas, PrimaryKey_Value);
                string updateValues = "";
                Dictionary<string, string> temp = new Dictionary<string, string>();

                if (modified_datas.ContainsKey("softwareproductdate") && isAsis)
                {
                    foreach (var data in modified_datas)
                    {
                        if (data.Key == "softwareproductdate")
                        {
                            temp.Add("softwareproductiondate", data.Value);
                        }
                    }
                }
                if (modified_datas.ContainsKey("spare1ossorenm") && isAsis)
                {
                    foreach (var data in modified_datas)
                    {
                        if (data.Key == "spare1ossorenm")
                        {
                            temp.Add("elementmanager", data.Value);
                        }
                    }
                }
                if(modified_datas.ContainsKey("ipaddress") && isAsis)
                {
                    foreach (var data in modified_datas)
                    {
                        if(data.Key == "ipaddress")
                        {
                            temp.Add("value", data.Value);
                        }    
                    }
                }
                if (temp.Count > 0)
                {
                    foreach (var result in temp)
                    {
                        modified_datas.Add(result.Key, result.Value);
                    }
                    modified_datas.Remove("softwareproductdate");
                    modified_datas.Remove("spare1ossorenm");
                    modified_datas.Remove("ipaddress");
                }

                foreach (KeyValuePair<string, string> data in modified_datas)
                {
                    if (data.Key != GetForeignyKey(tableName))
                    {

                        if (updateValues != "")
                        {
                            updateValues += $", ";
                        }
                        if ((tableName == "networkelement" && data.Key.ToLower() == "xmllastparsefiledate") || (tableName == "networkelement" && data.Key.ToLower() == "dataacquisitiondate")
                            || (tableName == "networkelement" && data.Key.ToLower() == "softwareinstalldate") || (tableName == "networkelement" && data.Key.ToLower() == "softwareinstalldateap")
                            || (tableName == "networkelement" && data.Key.ToLower() == "softwareinstalldatecp") || (tableName == "networkelement" && data.Key.ToLower() == "softwareproductdate")
                            || (tableName == "networkelement" && data.Key.ToLower() == "softwareproductdateap") || (tableName == "networkelement" && data.Key.ToLower() == "softwareproductdatecp")
                            || (tableName == "component" && data.Key.ToLower() == "productiondate") || (tableName == "networkelementsasis" && data.Key.ToLower() == "softwareinstalldate")
                            || (tableName == "networkelementsasis" && data.Key.ToLower() == "dataacquisitiondate") || (tableName == "networkelementsasis" && data.Key.ToLower() == "softwareproductiondate"))
                        {
                            updateValues += $"{data.Key}=TO_DATE('{data.Value}','YYYY-MM-DD HH24:MI:SS')";
                        }
                        else
                        {
                            updateValues += $" {data.Key}='{data.Value}'";
                        }
                    }
                }

                if (updateValues != "")
                {
                    query = $"UPDATE {tableName} set {updateValues}, modificationuser={user}, modificationdate=SYS_EXTRACT_UTC(systimestamp) where {GetPrimaryKey(tableName)}='{PrimaryKey_Value}'";
                    Logger.WriteLog(TEMLog.Debug, "GenerateUpdateQuery", $"{query}");
                }
                else
                {
                    query = $"UPDATE {tableName} set modificationuser={user}, modificationdate=SYS_EXTRACT_UTC(systimestamp) where {GetPrimaryKey(tableName)}='{PrimaryKey_Value}'";
                    Logger.WriteLog(TEMLog.Debug, "GenerateUpdateQuery", $"{query}");
                }



            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateUpdateQuery", $"{ex.Message}");
                throw;
            }
            return query;
        }

        public static List<string> GenerateUpdateQueries(string tableName, Dictionary<string, string> datas, string ForeignKey_Value)
        {
            List<string> querys = new List<string>();
            bool isAsis = false;
            try
            {
                List<string> table_list = GetTableName(tableName);
                foreach (string table in table_list)
                {
                    string _query = GenerateUpdateQuery(table, datas, ForeignKey_Value, isAsis);
                    querys.Add(_query);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateUpdateQueries", $"{ex.Message}");
                throw;
            }
            return querys;
        }
        public static string GenerateUpdateQueries(string tableName, string PrimaryKey_Value)
        {
            string query = "";
            int user = CommonFunction.getUserName();
            bool found = false;
            try
            {
                query = $"UPDATE {tableName} set modificationuser={user}, modificationdate=SYS_EXTRACT_UTC(systimestamp) where {GetPrimaryKey(tableName)}='{PrimaryKey_Value}'";
                Logger.WriteLog(TEMLog.Debug, "GenerateUpdateQuery", $"{query}");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateUpdateQueries", $"{ex.Message}");
                throw;
            }
            return query;
        }
        public static bool AuditInsert(OracleConnection connection, string tableName, Dictionary<string, string> DBData, Dictionary<string, string> DiffData, string PrimaryKey_Value, string opco, string oem, string elementname)
        {
            string query = "";
            int user = CommonFunction.getUserName();
            string updateValues = "";
            string ipaddress = "Ipaddress";

            try
            {

                foreach (KeyValuePair<string, string> data in DiffData)
                {
                    if (CommonFunction.AsIsAttributes.Contains(data.Key))
                    {
                        if (data.Key.ToLower() == "value")
                        {
                            query = $"INSERT INTO AUDITHISTORY (TABLENAME, COLUMNNAME, PRIMARYKEY, OLDVALUE, NEWVALUE, STATUS, CREATIONUSER, CREATIONDATE, OPCO, OEM, ELEMENTNAME) VALUES" +
                            $"('{tableName}','{ipaddress}','{PrimaryKey_Value}','{DBData[data.Key]}','{data.Value}','AUTO_APPROVED',{user},SYS_EXTRACT_UTC(systimestamp), '{opco}', '{oem}', '{elementname}')";
                        }
                        else
                        {
                            query = $"INSERT INTO AUDITHISTORY (TABLENAME, COLUMNNAME, PRIMARYKEY, OLDVALUE, NEWVALUE, STATUS, CREATIONUSER, CREATIONDATE, OPCO, OEM, ELEMENTNAME) VALUES" +
                            $"('{tableName}','{data.Key}','{PrimaryKey_Value}','{DBData[data.Key]}','{data.Value}','AUTO_APPROVED',{user},SYS_EXTRACT_UTC(systimestamp), '{opco}', '{oem}', '{elementname}')";
                        }

                        Logger.WriteLog(TEMLog.Debug, "GenerateAuditInsertQuery", $"{query}");
                        DataBaseFunctions.DBUpdation(connection, query);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateAuditInsertQuery", $"{ex.Message}");
                throw;
            }
            return true;
        }

        public static List<string> GetTableName(string tableName)
        {
            string line = "";
            string[] table = { };
            List<string> list = new List<string>();
            try
            {

                string DBModelFile = CommonFunction.getAppConfigValue("DBModel");
                StreamReader configReader = new StreamReader(DBModelFile);
                while ((line = configReader.ReadLine()) != null)
                {
                    if (line.StartsWith("#") || line.StartsWith("//"))
                    {
                        continue;
                    }
                    else if (line.Trim() == "")
                    {
                        //do nothing
                    }
                    else if (line.Contains(";"))
                    {
                        string[] values = line.Split(';');

                        if (values[0] == tableName)
                        {
                            if (!list.Contains(values[3]))
                            {
                                list.Add(values[3]);
                            }
                        }
                    }
                    else
                    {
                        throw new CustomError("Invalid Line", " Module name: GetDBColumnData");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return list;
        }

        public static bool GenerateInsertAndUpdateQueryfForNetworkAsIs(OracleConnection connection, string opCo, string elementName, string oem, NetworkAssetData networkAssetData, Dictionary<string, string>? DiffData)
        {
            bool isSuccess = false;
            int user = CommonFunction.getUserName();
            var AsIsinsertquery = "";
            var AsIsupdatequery = "";
            string asIsEntityId = "";
            try
            {
                var operationalIds = CommonFunction.GetOperationalIds(opCo, elementName, oem);

                isSuccess = operationalIds.AssetId == "NA" ? true : false;

                if (operationalIds.AssetId != "NA")
                {
                    // need to compare the versions
                    var isUpdated = false;
                    string asIsQuery = $"Upper(OPCOID) = '{operationalIds.OpCoId}' AND Upper(ELEMENTDEPLOYMENTNAME) = '{elementName.ToUpper()}'";
                    string asIsId = DataBaseFunctions.GetPrimaryKey("networkelementsasis", asIsQuery);
                    if (asIsId == "NA")
                    {

                        if (networkAssetData.NetworkElement.ContainsKey("softwarereleaseinformation"))
                        {
                            isUpdated = CommonFunction.UpdateAssetTable(connection, operationalIds.AssetId,
                                networkAssetData.NetworkElement["softwarereleaseinformation"], "networkelementsasplanned");
                        }
                        if (isUpdated)
                        {
                            string getAssetQuery = CommonFunction.GetAssetQuery(operationalIds.OpCoId, elementName.ToUpper(), operationalIds.OemId);

                            List<Dictionary<string, string>> assetAttributes = DataBaseFunctions.selectFromDBByQuery(getAssetQuery);
                            foreach (var assetField in assetAttributes)
                            {
                                AsIsinsertquery = InsertQueryForAsIs(connection, operationalIds.OpCoId, elementName, assetField, networkAssetData, "networkelementsasis");
                                Logger.WriteLog(TEMLog.Debug, "InsertQueryForAsIs", $"{AsIsinsertquery}");
                                asIsEntityId = DataBaseFunctions.DBInsertion(connection, AsIsinsertquery).ToString();
                                isSuccess = asIsEntityId != null ? true : false;
                            }
                        }
                        else
                        {
                            throw new Exception("While Inserting ASIS");
                        }

                    }
                    else if (asIsId != "NA" && (DiffData == null || DiffData.Count <= 0))
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        if (DiffData.ContainsKey("softwarereleaseinformation"))
                        {
                            isUpdated = CommonFunction.UpdateAssetTable(connection, operationalIds.AssetId,
                            DiffData["softwarereleaseinformation"], "networkelementsasplanned");
                        }
                        else
                        {
                            // check the existing entry version in asis table
                            isUpdated = CommonFunction.CheckExistingEntryInAsIsTable(connection, asIsId, operationalIds.AssetId, "networkelementsasis");
                            //isUpdated = true;
                        }
                        if (isUpdated)
                        {
                            AsIsupdatequery = UpdateQueryForAsIs(connection, networkAssetData, DiffData, opCo, elementName, asIsId, "networkelementsasis", oem);
                            Logger.WriteLog(TEMLog.Debug, "UpdateQueryForAsIs", $"{AsIsupdatequery}");
                            asIsEntityId = DataBaseFunctions.DBUpdation(connection, AsIsupdatequery).ToString();
                            isSuccess = asIsEntityId != null ? true : false;
                        }
                        else
                        {
                            throw new Exception("While Updating ASIS");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateInsertAndUpdateQueryfForNetworkAsIs", $"{ex.Message}");
                throw;
            }
            return isSuccess;
        }
        public static string InsertQueryForAsIs(OracleConnection connection, string opCoId, string elementName, Dictionary<string, string>? assetField, NetworkAssetData networkAssetData, string tableName)
        {
            string query = "";
            int user = CommonFunction.getUserName();
            Dictionary<string, string>? datas = null;

            try
            {
                string assetId = assetField["networkelementasplannedid"];
                string locationId = assetField["locationid"];
                string systemId = assetField["systemtypeid"];
                string oemID = assetField["orgeqpmanufacturerid"];
                string hardwareType = "";
                string IpAddress = networkAssetData.Identity != null && networkAssetData.Identity.Count > 0 && networkAssetData.Identity.ContainsKey("ipaddress") ? networkAssetData.Identity["ipaddress"] : string.Empty;

                if (tableName == "networkelementsasis")
                {
                    if (networkAssetData.HardwareConfiguration != null && networkAssetData.HardwareConfiguration.Count > 0)
                    {
                        foreach (Dictionary<string, string> hconfig in networkAssetData.HardwareConfiguration)
                        {
                            hardwareType = hconfig.ContainsKey("hardwaretype") ?hconfig["hardwaretype"]:string.Empty;
                        }
                    }
                    var NetworkasisInsertFields = new NetworkAsIsInsertionDto()
                    {
                        OpCoId = opCoId,
                        SystemId = systemId,
                        OemId = oemID,
                        AssetId = assetId,
                        LocationId = locationId,
                        SoftwareProductNumber = networkAssetData.NetworkElement.ContainsKey("softwareproductnumber")?networkAssetData.NetworkElement["softwareproductnumber"]:string.Empty,
                        SoftwareProductDate = networkAssetData.NetworkElement.ContainsKey("softwareproductdate") ? networkAssetData.NetworkElement["softwareproductdate"] : string.Empty,
                        SoftwareInstallDate = networkAssetData.NetworkElement.ContainsKey("softwareinstalldate") ? networkAssetData.NetworkElement["softwareinstalldate"] : string.Empty,
                        DataAcquisitionDate = networkAssetData.NetworkElement.ContainsKey("dataacquisitiondate") ? networkAssetData.NetworkElement["dataacquisitiondate"] : string.Empty,
                        ElementName = elementName,
                        NodeType = networkAssetData.NetworkElement.ContainsKey("nodetype") ? networkAssetData.NetworkElement["nodetype"] : string.Empty,
                        PlatformType = networkAssetData.NetworkElement.ContainsKey("platformtype") ? networkAssetData.NetworkElement["platformtype"] : string.Empty,
                        HardwareType = hardwareType,
                        SoftwareReleaseInformation = networkAssetData.NetworkElement.ContainsKey("softwarereleaseinformation") ? networkAssetData.NetworkElement["softwarereleaseinformation"] : string.Empty,
                        User = user,
                        Spare1ossorenm = networkAssetData.NetworkElement.ContainsKey("spare1ossorenm") ? networkAssetData.NetworkElement["spare1ossorenm"] : string.Empty,
                    };

                    query = CommonFunction.GetInsertionQueryForNetworkAsis(NetworkasisInsertFields, tableName);

                }
                else if (tableName == "identitiesasis")
                {
                    var categoryQuery = $"lower(replace(trim(description), ' ', '')) = 'ipaddress'";
                    var cateid = DataBaseFunctions.GetPrimaryKey("categories", categoryQuery);

                    var IdentityAsisInsertEntry = new IdentityAsisCreateDto()
                    {
                        AssetEntity = assetField,
                        OpCoId = opCoId,
                        ElementName = elementName,
                        OemId = oemID,
                        IPAddress = networkAssetData.Identity != null && networkAssetData.Identity.Count > 0 && networkAssetData.Identity.ContainsKey("ipaddress") ? networkAssetData.Identity["ipaddress"] : string.Empty,
                        CategoryId = IpAddress.IsNullOrEmpty()? null: cateid,
                        User = user,
                    };

                    var resourceKey = ResourceKeyGeneration.CreateorupdateIdentityResourceKey(connection, IdentityAsisInsertEntry);

                    if (resourceKey.IsNullOrEmpty())
                    {
                        Exception ex = new Exception();
                        return ex.Message;
                    }
                    else
                    {
                        IdentityAsisInsertEntry.ResourceKey = resourceKey;
                    }


                    query = CommonFunction.GetInsertionQueryForIdentityAsis(tableName, IdentityAsisInsertEntry, assetId);
                }


                Logger.WriteLog(TEMLog.Debug, "InsertQueryForAsIs", $"{query}");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "InsertQueryForAsIs", $"{ex.Message}");
                throw;
            }
            return query;
        }

        public static string UpdateQueryForAsIs(OracleConnection connection, NetworkAssetData networkAssetData, Dictionary<string, string>? DiffData, string opCo, string elementName, string asIsId, string tableName, string oem)
        {
            string query = "";
            bool isAsis = true;
            try
            {               
                if (tableName == "networkelementsasis")
                {

                    query = GenerateUpdateQuery(tableName, DiffData, asIsId, isAsis);
                    Logger.WriteLog(TEMLog.Debug, "UpdateQueryForAsIs", $"{query}");
                }
                else if (tableName == "identitiesasis" && DiffData.Count == 0)
                {
                    query = GenerateIdentityAsisUpdateQUery(connection, tableName, networkAssetData.Identity, asIsId, opCo,elementName,oem);
                    Logger.WriteLog(TEMLog.Debug, "UpdateQueryForAsIs", $"{query}");
                }
                else
                {
                    query = GenerateUpdateQuery(tableName, DiffData, asIsId, isAsis);
                    Logger.WriteLog(TEMLog.Debug, "UpdateQueryForAsIs", $"{query}");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "UpdateQueryForAsIs", $"{ex.Message}");
                throw;
            }
            return query;
        }

        public static bool GenerateInsertAndUpdateQueryfForIdentityAsis(OracleConnection connection, string opCo, string elementName, string oem, NetworkAssetData networkAssetData, Dictionary<string, string>? DiffData)
        {
            bool isSuccess = false;
            int user = CommonFunction.getUserName();
            var AsIsinsertquery = "";
            var AsIsupdatequery = "";
            string asIsEntityId = "";
            try
            {
                var operationalIds = CommonFunction.GetOperationalIds(opCo, elementName, oem);

                isSuccess = operationalIds.AssetId == "NA" ? true : false;

                if (operationalIds.AssetId != "NA")
                {
                    string identityasIsQuery = $"Upper(ASSETID) = '{operationalIds.AssetId}'";
                    string identitasIsId = DataBaseFunctions.GetPrimaryKey("identitiesasis", identityasIsQuery);

                    if (identitasIsId == "NA")
                    {
                        string getAssetQuery = CommonFunction.GetAssetQuery(operationalIds.OpCoId, elementName, operationalIds.OemId);

                        List<Dictionary<string, string>> assetAttributes = DataBaseFunctions.selectFromDBByQuery(getAssetQuery);
                        foreach (var assetField in assetAttributes)
                        {
                            AsIsinsertquery = InsertQueryForAsIs(connection, operationalIds.OpCoId, elementName, assetField, networkAssetData, "identitiesasis");
                            Logger.WriteLog(TEMLog.Debug, "InsertQueryForAsIs", $"{AsIsinsertquery}");
                            asIsEntityId = DataBaseFunctions.DBInsertion(connection, AsIsinsertquery).ToString();
                            isSuccess = asIsEntityId != null ? true : false;
                        }

                    }
                    else
                    {
                        AsIsupdatequery = UpdateQueryForAsIs(connection, networkAssetData, DiffData, opCo, elementName, identitasIsId, "identitiesasis", oem);
                        if (AsIsupdatequery == "false")
                        {
                            // don't do anything
                            isSuccess = true;

                        }
                        else
                        {
                            Logger.WriteLog(TEMLog.Debug, "UpdateQueryForAsIs", $"{AsIsupdatequery}");
                            asIsEntityId = DataBaseFunctions.DBUpdation(connection, AsIsupdatequery).ToString();
                            isSuccess = asIsEntityId != null ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateInsertAndUpdateQueryfForIdentityAsis", $"{ex.Message}");
                throw;
            }
            return isSuccess;
        }

        public static string GenerateIdentityAsisUpdateQUery(OracleConnection connection, string tableName, Dictionary<string,string>? identityData, string pkId,string opCo, string elementName, string oem)
        {
            try
            {
                string query = "";
                Dictionary<string, string>? DBData = new Dictionary<string, string>();
                Dictionary<string, string>? CurrentIPAddress = new Dictionary<string, string>();

                var operationalIds = CommonFunction.GetOperationalIds(opCo, elementName, oem);

                if (operationalIds.AssetId != "NA")
                {
                    string identityasIsQuery = $"Upper(ASSETID) = '{operationalIds.AssetId}'";
                    string identitasIsId = DataBaseFunctions.GetPrimaryKey("identitiesasis", identityasIsQuery);

                    string getIpAddressQuery = $"Upper(ID) = '{identitasIsId}'";
                    string checkIpaddressInIdentityAsi = DataBaseFunctions.GetAttributesValue("identitiesasis", getIpAddressQuery);

                    string categoryId = DataBaseFunctions.GetAttributesValue("identitycategory", identityasIsQuery);


                    if (checkIpaddressInIdentityAsi != identityData["ipaddress"] && categoryId == "1")
                    {
                        query = $"UPDATE {tableName} set value = '{identityData["ipaddress"]}', modificationuser='{1}', modificationdate=SYS_EXTRACT_UTC(systimestamp) where {GetPrimaryKey(tableName)}='{identitasIsId}'";

                        DBData = new Dictionary<string, string>();
                        CurrentIPAddress = new Dictionary<string, string>();

                        DBData.Add("ipaddress", checkIpaddressInIdentityAsi);
                        CurrentIPAddress.Add("ipaddress", identityData["ipaddress"]);

                    }
                   
                }

                if (query != "")
                {
                    var isupdate = DBModelFunctions.AuditInsert(connection, "identities", DBData, CurrentIPAddress, pkId, opCo, oem, elementName);
                    if (!isupdate)
                    {
                        Exception ex = new Exception();
                        throw ex;
                    }

                }
                else
                {
                    query = "false";
                }

                return query;
            }
            catch(Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GenerateIdentityAsisUpdateQUery", $"{ex.Message}");
                throw;
            }
        }



    }
}
