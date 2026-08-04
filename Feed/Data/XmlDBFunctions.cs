using Oracle.ManagedDataAccess.Client;
using System.Collections;
using TEMS.Entity;
using TEMS.Logs;
using static TEMS.Logs.Logger;

namespace TEMS.Data
{
    public class XmlDBFunctions
    {
        public static Dictionary<string,string> XmlDBInsertUpdate(int xmlparserunid, ArrayList nadList)
        {
            bool validate = true;
            string parse_type = "";
            string oem = "", opco = "", elementname = "", nodeType = "";
            Dictionary<string,string> returnValue = new Dictionary<string,string>();
            int rows_affected = 0, user =0, nodeCount = 0;
            DateTime lastCheckDate = DateTime.UtcNow;
            DateTime? lastExecutedDate = null;
            

            try
            {

                foreach (NetworkAssetData networkAssetData in nadList)
                {

                    nodeCount++;
                    validate = true;
                    parse_type = "";
                    oem = networkAssetData.NetworkElement["oem"].ToString().ToUpper();
                    opco = networkAssetData.NetworkElement["opco"].ToString().ToUpper();
                    elementname = networkAssetData.NetworkElement["elementname"].ToString().ToUpper();
                    nodeType = networkAssetData.NetworkElement["nodetypename"].ToString().ToUpper();

                    user = CommonFunction.getUserName();
                    string strQuery = "select count(NETWORKELEMENTID) from NETWORKELEMENT where OPCO = '" + opco + "' and ELEMENTNAME = '" + elementname + "'";
                    if (oem == "")
                    {
                        strQuery += " and OEM is null";
                    }
                    else
                    {
                        strQuery += " and OEM = '" + oem + "'";
                    }
                    int count = DataBaseFunctions.GetRowCount(strQuery);
                    if (count == 0)
                    {
                        parse_type = "INSERT";
                        try
                        {
                            NodeDBInsert(networkAssetData);
                            Logger.WriteLog(TEMLog.Info, "XmlDBInsertUpdate", "Inserted Node Details --> " + elementname);                            
                        }
                        catch (Exception)
                        {
                            validate = false;
                        }
                    }
                    else if (count == 1)
                    {
                        parse_type = "UPDATE";
                        try
                        {
                            rows_affected = NodeDBUpdate(networkAssetData);
                            Logger.WriteLog(TEMLog.Info, "XmlDBInsertUpdate", "Updated Node Details --> " + elementname + " (" + rows_affected + " row(s) updated)");
                        }
                        catch (Exception ex)
                        {
                            validate = false;
                            Logger.WriteLog(TEMLog.Error, "xmlDBInsertUpdate", $"{ex.Message}");
                        }
                    }
                    else
                    {
                        //too many Duplicate values in DB
                        validate = false;
                    }
                    
                        if (nodeCount == nadList.Count)
                            lastExecutedDate = DateTime.UtcNow;

                        if (validate)
                        {
                            string rows_updated = "";
                            if (parse_type == "UPDATE")
                            {
                                rows_updated = rows_affected.ToString();
                            }
                            if (lastExecutedDate != null)
                            {
                                DataBaseFunctions.DBInsertion($"insert into nodeparsehistory (xmlparserunid, opco, oem, elementname, parsetype, updatedrows, status, creationuser, creationdate,nodeprocessstarttime,nodeprocessendtime, nodetype) values" +
                                    $" ('{xmlparserunid.ToString()}','{opco}','{oem}','{elementname}', '{parse_type}', '{rows_updated}', 'PASSED', '{user}', SYS_EXTRACT_UTC(systimestamp), TO_TIMESTAMP('{lastCheckDate:dd-MMM-yy HH:mm:ss}','DD-MM-YYYY HH24:MI:SS')," +
                                    $"TO_TIMESTAMP('{lastExecutedDate:dd-MMM-yy HH:mm:ss}','DD-MM-YYYY HH24:MI:SS'),'{nodeType}') returning nodeparsehistoryid into :pkcreated");
                            }
                            else
                            {
                                DataBaseFunctions.DBInsertion($"insert into nodeparsehistory (xmlparserunid, opco, oem, elementname, parsetype, updatedrows, status, creationuser, creationdate,nodeprocessstarttime, nodetype) values" +
                                    $" ('{xmlparserunid.ToString()}','{opco}','{oem}','{elementname}', '{parse_type}', '{rows_updated}', 'PASSED', '{user}', SYS_EXTRACT_UTC(systimestamp) ,TO_TIMESTAMP('{lastCheckDate:dd-MMM-yy HH:mm:ss}','DD-MM-YYYY HH24:MI:SS')" +
                                    $",'{nodeType}') returning nodeparsehistoryid into :pkcreated");
                            }
                        }
                        else
                            DataBaseFunctions.DBInsertion($"insert into nodeparsehistory (xmlparserunid, opco, oem, elementname, parsetype, updatedrows, status, creationuser, creationdate) values ('{xmlparserunid.ToString()}','{opco}','{oem}','{elementname}', '{parse_type}', '', 'FAILED', {user},SYS_EXTRACT_UTC(systimestamp)) returning nodeparsehistoryid into :pkcreated");
                    }

                    returnValue.Add("opco", opco);
                    returnValue.Add("nodetype", nodeType);
                    returnValue.Add("oem", oem);                   
                
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "xmlDBInsertUpdate", $"{ex.Message}");
                if (parse_type != "")
                {
                    DataBaseFunctions.DBInsertion($"insert into nodeparsehistory (xmlparserunid, opco, oem, elementname, parsetype, status, creationuser, creationdate) values ('{xmlparserunid.ToString()}','{opco}','{oem}','{elementname}', '{parse_type}', 'FAILED', {user},SYS_EXTRACT_UTC(systimestamp)) returning nodeparsehistoryid into :pkcreated");
                }
                throw;
            }
            return returnValue;
        }
        private static bool NodeDBInsert(NetworkAssetData networkAssetData)
        {
            int networkelement_pk, identit_pk, soft_config_pk, softwarecomponentid, hardware_id, function_id, functionareaid, sub_func_id, sub_fun_area_id, componentid;
            string networkelementquery, identityquery, softwareconfiguration, softwarecomponent, hardwareconfiguration, function, function_area, subfunction, subfunction_area, component;
            OracleTransaction? transaction = null;
            OracleConnection? connection = null;
            Dictionary<string, string>? DiffData = null;
            bool IsAsisUpdate = false, isIdentityAsisUpdated = false;
            try
            {
                connection = DataBaseFunctions.GetConnection();
                transaction = connection.BeginTransaction();

                IsAsisUpdate = DBModelFunctions.GenerateInsertAndUpdateQueryfForNetworkAsIs(connection, networkAssetData.NetworkElement["opco"], networkAssetData.NetworkElement["elementname"], networkAssetData.NetworkElement["oem"], networkAssetData, DiffData);
                if (IsAsisUpdate)
                {
                    networkelementquery = DBModelFunctions.GenerateInsertQuery("networkelement", networkAssetData.NetworkElement, "");
                    networkelement_pk = DataBaseFunctions.DBInsertion(connection, networkelementquery);
                    IsAsisUpdate = false;
                }
                else
                {
                    throw new Exception("NetworkElementAsis not Inserted properly please check GenerateInsertAndUpdateQueryfForNetworkAsIs function in NodeDBInsert ");
                }


                if (networkAssetData.Identity != null)
                {
                    DiffData = new Dictionary<string, string>();
                    isIdentityAsisUpdated = DBModelFunctions.GenerateInsertAndUpdateQueryfForIdentityAsis(connection, networkAssetData.Identity["opco"], networkAssetData.Identity["elementname"], networkAssetData.Identity.ContainsKey("oem")?networkAssetData.Identity["oem"]:string.Empty, networkAssetData, DiffData);
                    if (isIdentityAsisUpdated)
                    {
                        identityquery = DBModelFunctions.GenerateInsertQuery("identities", networkAssetData.Identity, networkelement_pk.ToString());
                        identit_pk = DataBaseFunctions.DBInsertion(connection, identityquery);
                    }
                    else
                    {
                        throw new Exception("IdentityAsis not Inserted properly please check GenerateInsertAndUpdateQueryfForNetworkAsIs function in NodeDBInsert ");
                    }
                }

                if (networkAssetData.HardwareConfiguration != null)
                {
                    foreach (Dictionary<string, string> hconfig in networkAssetData.HardwareConfiguration)
                    {
                        string opco = hconfig["opco"].ToString().ToUpper();
                        string elementname = hconfig["elementname"].ToString().ToUpper();
                        string serialnumber = hconfig["serialnumber"].ToString().ToUpper();
                        string unitLocation = hconfig["unitlocation"].ToString().ToUpper();
                        string hardwareconfigurationQuery = "";
                        if (serialnumber == "")
                        {
                            hardwareconfigurationQuery = "Upper(OPCO) = '" + opco + "' and Upper(ELEMENTNAME) = '" + elementname + "'and Upper(serialnumber) is null and networkelementid= '" + networkelement_pk + "' and Upper(unitlocation) = '" + unitLocation + "'";
                        }
                        else
                        {
                            hardwareconfigurationQuery = "Upper(OPCO) = '" + opco + "' and Upper(ELEMENTNAME) = '" + elementname + "'and Upper(serialnumber) = '" + serialnumber + "' and networkelementid= '" + networkelement_pk + "'";
                        }
                        string hardwareconfigurationid = DataBaseFunctions.GetPrimaryKey("hardwareconfiguration", hardwareconfigurationQuery);
                        if (hardwareconfigurationid == "NA")
                        {

                            hardwareconfiguration = DBModelFunctions.GenerateInsertQuery("hardwareconfiguration", hconfig, networkelement_pk.ToString());
                            hardware_id = DataBaseFunctions.DBInsertion(connection, hardwareconfiguration);
                            transaction.Commit();
                            transaction = connection.BeginTransaction();
                        }
                    }
                }

                if (networkAssetData.Softwarecomponent != null)
                {

                    softwarecomponent = DBModelFunctions.GenerateInsertQuery("softwarecomponent", networkAssetData.Softwarecomponent.Data, networkelement_pk.ToString());
                    softwarecomponentid = DataBaseFunctions.DBInsertion(connection, softwarecomponent);

                    if (networkAssetData.Softwarecomponent.ComponentList != null)
                    {
                        foreach (Dictionary<string, string> softcomp_component in networkAssetData.Softwarecomponent.ComponentList)
                        {
                            component = DBModelFunctions.GenerateInsertQuery("component", softcomp_component, softwarecomponentid.ToString());
                            componentid = DataBaseFunctions.DBInsertion(connection, component);
                        }
                    }
                }

                if (networkAssetData.Softconfiguration != null)
                {

                    softwareconfiguration = DBModelFunctions.GenerateInsertQuery("softwareconfiguration", networkAssetData.Softconfiguration.Data, networkelement_pk.ToString());
                    soft_config_pk = DataBaseFunctions.DBInsertion(connection, softwareconfiguration);

                    if (networkAssetData.Softconfiguration.FunctionList != null)
                    {
                        foreach (Function fun in networkAssetData.Softconfiguration.FunctionList)
                        {
                            Dictionary<string, string> data = new Dictionary<string, string>();
                            data.Add("functionname", fun.Function_name);
                            function = DBModelFunctions.GenerateInsertQuery("function", data, soft_config_pk.ToString());
                            function_id = DataBaseFunctions.DBInsertion(connection, function);
                            if (fun.FunctionArea_List != null)
                            {
                                foreach (FunctionArea functionArea in fun.FunctionArea_List)
                                {

                                    function_area = DBModelFunctions.GenerateInsertQueryNclob("swconfigfunctionareas", functionArea.Data, function_id.ToString(), "functionareadescription");
                                    functionareaid = DataBaseFunctions.DBInsertion(connection, function_area);

                                    if (functionArea.Subfunction_List != null && functionArea.Subfunction_List.Count > 0)
                                    {
                                        foreach (SubFunctionArea _subfunction in functionArea.Subfunction_List)
                                        {
                                            subfunction = DBModelFunctions.GenerateInsertQuery("swconfigsubfunction", _subfunction.Data, functionareaid.ToString());
                                            sub_func_id = DataBaseFunctions.DBInsertion(connection, subfunction);
                                            if (_subfunction.SubFunctionAreaList != null)
                                            {
                                                foreach (Dictionary<string, string> subfuncarea in _subfunction.SubFunctionAreaList)
                                                {
                                                    subfunction_area = DBModelFunctions.GenerateInsertQueryNclob("swconfigsubfunctionareas", subfuncarea, sub_func_id.ToString(), "subfunctionareadescription");
                                                    sub_fun_area_id = DataBaseFunctions.DBInsertion(connection, subfunction_area);
                                                }
                                                //foreach (Dictionary<string, string> subfuncarea in _subfunction.SubFunctionAreaList)
                                                //{
                                                //List<string> subfunctionareainsert = DBModelFunctions.GenerateInsertQueries("subfunctionarea", subfuncarea, sub_func_id.ToString());
                                                //{
                                                //    foreach (string query in subfunctionareainsert)
                                                //    {
                                                //sub_fun_area_id = DataBaseFunctions.DBInsertion(connection, query);
                                                //    }
                                                //}
                                                //}
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                transaction.Commit();
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "nodeDBInsert", $"{ex.Message}");
                transaction?.Rollback();
                connection?.Close();
                throw;
            }
            return true;
        }


        private static int NodeDBUpdate(NetworkAssetData networkAssetData)
        {
            string networkelementquery, identityquery, softwareconfiguration, softwarecomponent, hardwareconfigurationQuery, functionQuery, function_areaQuery, subfunctionQuery, subfunctionareaQuery, componentQuery;
            OracleTransaction? transaction = null;
            OracleConnection? connection = null;
            bool isAsis = false;

            Dictionary<string, string>? DBData = null;
            Dictionary<string, string>? DiffData = null;
            var IsAsisUpdate = false;
            var isIdentityAsisUpdated =  false;
            bool isSwConfigurationUpdate = false;

            int rowsUpdated = 0;

            try
            {
                connection = DataBaseFunctions.GetConnection();
                transaction = connection.BeginTransaction();

                string oem = networkAssetData.NetworkElement["oem"].ToString().ToUpper();
                string opco = networkAssetData.NetworkElement["opco"].ToString().ToUpper();
                string elementname = networkAssetData.NetworkElement["elementname"].ToString().ToUpper();
                if (oem == "")
                {
                    networkelementquery = "Upper(OEM) is null and Upper(OPCO) = '" + opco + "' and Upper(ELEMENTNAME) = '" + elementname + "'";
                }
                else
                {
                    networkelementquery = "Upper(OEM) = '" + oem + "' and Upper(OPCO) = '" + opco + "' and Upper(ELEMENTNAME) = '" + elementname + "'";
                }
                string networkelementid = DataBaseFunctions.GetPrimaryKey("networkelement", networkelementquery);
               
                if (networkelementid == "NA")
                {

                    IsAsisUpdate = DBModelFunctions.GenerateInsertAndUpdateQueryfForNetworkAsIs(connection, opco, elementname, oem, networkAssetData, DiffData);
                    if (IsAsisUpdate)
                    {
                        string networkelementInsertquery = DBModelFunctions.GenerateInsertQuery("networkelement", networkAssetData.NetworkElement, "");
                        networkelementid = DataBaseFunctions.DBInsertion(connection, networkelementInsertquery).ToString();
                    }
                    else
                    {
                        throw new Exception("NetworkElementAsis not Inserted properly please check GenerateInsertAndUpdateQueryfForNetworkAsIs");
                    }

                }
                else
                {
                    DBData = DataBaseFunctions.selectFromDB("networkelement", networkelementid);

                    DiffData = CompareData(networkAssetData.NetworkElement, DBData);
                    if (DiffData.Count > 0)
                    {
                        IsAsisUpdate = DBModelFunctions.GenerateInsertAndUpdateQueryfForNetworkAsIs(connection, opco, elementname,oem, networkAssetData, DiffData);
                        if (IsAsisUpdate)
                        {
                            string updateQuery = DBModelFunctions.GenerateUpdateQuery("networkelement", DiffData, networkelementid, isAsis);
                            rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);

                            DBModelFunctions.AuditInsert(connection, "networkelement", DBData, DiffData, networkelementid, opco, oem, elementname);
                        }
                        else
                        {
                            throw new Exception("NetworkElementAsis not updated properly please check GenerateInsertAndUpdateQueryfForNetworkAsIs");   
                        }

                    }
                }
                if (networkAssetData.Identity != null)
                {
                    DiffData = new Dictionary<string, string>();
                    opco = networkAssetData.Identity["opco"].ToString().ToUpper();
                    elementname = networkAssetData.Identity["elementname"].ToString().ToUpper();
                    identityquery = "Upper(OPCO) = '" + opco + "' and Upper(ELEMENTNAME) = '" + elementname + "' and networkelementid= '" + networkelementid + "'";
                    string identityid = DataBaseFunctions.GetPrimaryKey("identities", identityquery);
                    if (identityid == "NA")
                    {
                        isIdentityAsisUpdated = DBModelFunctions.GenerateInsertAndUpdateQueryfForIdentityAsis(connection, opco, elementname, oem, networkAssetData, DiffData);
                        if (isIdentityAsisUpdated)
                        {
                            string identityInsert = DBModelFunctions.GenerateInsertQuery("identities", networkAssetData.Identity, networkelementid.ToString());
                            identityid = DataBaseFunctions.DBInsertion(connection, identityInsert).ToString();
                        }
                        else
                        {
                            throw new Exception("IdentityAsis not Inserted properly please check GenerateInsertAndUpdateQueryfForIdentityAsIs");
                        }

                    }
                    else
                    {
                        DBData = DataBaseFunctions.selectFromDB("identities", identityid);
                        DiffData = CompareData(networkAssetData.Identity, DBData);

                        if (DiffData.Count > 0)
                        {
                            isIdentityAsisUpdated = DBModelFunctions.GenerateInsertAndUpdateQueryfForIdentityAsis(connection, opco, elementname, oem, networkAssetData, DiffData);
                            if (isIdentityAsisUpdated)
                            {

                                string updateQuery = DBModelFunctions.GenerateUpdateQuery("identities", DiffData, identityid, isAsis);
                                rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                                 DBModelFunctions.AuditInsert(connection, "identities", DBData, DiffData, identityid, opco, oem, elementname);
                            }                            
                        }

                        if (DiffData.Count <= 0)
                        {
                            var operationalIds = CommonFunction.GetOperationalIds(opco, elementname, oem);

                            if (operationalIds.AssetId != "NA")
                            {
                                string identityasIsQuery = $"Upper(ASSETID) = '{operationalIds.AssetId}'";
                                string identitasIsId = DataBaseFunctions.GetPrimaryKey("identitiesasis", identityasIsQuery);
                                if (identitasIsId != "NA")
                                {
                                    string getIpAddressQuery = $"Upper(id) = '{identitasIsId}'";
                                    string checkIpaddressInIdentityAsi = DataBaseFunctions.GetAttributesValue("identitiesasis", getIpAddressQuery);

                                    string categoryId = DataBaseFunctions.GetAttributesValue("identitycategory", identityasIsQuery);

                                    if (checkIpaddressInIdentityAsi != networkAssetData.Identity["ipaddress"] && categoryId == "1")
                                    {
                                        var query = DBModelFunctions.GenerateIdentityAsisUpdateQUery(connection, "identitiesasis", networkAssetData.Identity, identityid, opco, elementname, oem);
                                        Logger.WriteLog(TEMLog.Debug, "UpdateQueryForAsIs", $"{query}");
                                        var id = DataBaseFunctions.DBUpdation(connection, query).ToString();
                                    }
                                    if (checkIpaddressInIdentityAsi == networkAssetData.Identity["ipaddress"] && categoryId == "" || categoryId == "NA")
                                    {
                                        var catid = "1";
                                        var query = $"UPDATE identitiesasis set categoryid = '{catid}', modificationuser='{1}', modificationdate=SYS_EXTRACT_UTC(systimestamp) where {DBModelFunctions.GetPrimaryKey("identitiesasis")}='{identitasIsId}'";
                                        Logger.WriteLog(TEMLog.Debug, "UpdateQueryForAsIs", $"{query}");
                                        var id = DataBaseFunctions.DBUpdation(connection, query).ToString();
                                    }
                                }
                                else
                                {
                                    isIdentityAsisUpdated = DBModelFunctions.GenerateInsertAndUpdateQueryfForIdentityAsis(connection, opco, elementname, oem, networkAssetData, DiffData);

                                }
                            }
                        }
                       
                    }
                }
                if (networkAssetData.HardwareConfiguration != null)
                {
                    foreach (Dictionary<string, string> hconfig in networkAssetData.HardwareConfiguration)
                    {
                        opco = hconfig["opco"].ToString().ToUpper();
                        elementname = hconfig["elementname"].ToString().ToUpper();
                        string serialnumber = hconfig["serialnumber"].ToString().ToUpper();
                        string unitLocation = hconfig["unitlocation"].ToString().ToUpper();
                        if (serialnumber == "")
                        {
                            hardwareconfigurationQuery = "Upper(OPCO) = '" + opco + "' and Upper(ELEMENTNAME) = '" + elementname + "'and Upper(serialnumber) is null and networkelementid= '" + networkelementid + "' and Upper(unitlocation) = '"+unitLocation+"'";
                        }
                        else
                        {
                            hardwareconfigurationQuery = "Upper(OPCO) = '" + opco + "' and Upper(ELEMENTNAME) = '" + elementname + "'and Upper(serialnumber) = '" + serialnumber + "' and networkelementid= '" + networkelementid + "'";
                        }
                        string hardwareconfigurationid = DataBaseFunctions.GetPrimaryKey("hardwareconfiguration", hardwareconfigurationQuery);                        
                        if (hardwareconfigurationid == "NA")
                        {
                            IsAsisUpdate = DBModelFunctions.GenerateInsertAndUpdateQueryfForNetworkAsIs(connection, opco, elementname,oem, networkAssetData, DiffData);
                            if (IsAsisUpdate) 
                            {
                                string hardwareconfigurationInsert = DBModelFunctions.GenerateInsertQuery("hardwareconfiguration", hconfig, networkelementid.ToString());
                                hardwareconfigurationid = DataBaseFunctions.DBInsertion(connection, hardwareconfigurationInsert).ToString();
                            }
                            else
                            {
                                throw new Exception("NetworkElementAsis not updated properly please check GenerateInsertAndUpdateQueryfForNetworkAsIs");
                            }
                        }
                        else
                        {
                            DBData = DataBaseFunctions.selectFromDB("hardwareconfiguration", hardwareconfigurationid);
                            DiffData = CompareData(hconfig, DBData);
                            if (DiffData.Count > 0)
                            {
                                IsAsisUpdate = DBModelFunctions.GenerateInsertAndUpdateQueryfForNetworkAsIs(connection, opco, elementname,oem, networkAssetData, DiffData);
                                if (IsAsisUpdate)
                                {
                                    string updateQuery = DBModelFunctions.GenerateUpdateQuery("hardwareconfiguration", DiffData, hardwareconfigurationid, isAsis);
                                    rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                                    DBModelFunctions.AuditInsert(connection, "hardwareconfiguration", DBData, DiffData, hardwareconfigurationid, opco, oem, elementname);
                                }
                                else
                                {
                                    throw new Exception("NetworkElementAsis not updated properly please check GenerateInsertAndUpdateQueryfForNetworkAsIs");
                                }
                                
                            }
                        }
                    }
                }
                if (networkAssetData.Softwarecomponent != null)
                {
                    opco = networkAssetData.Softwarecomponent.Data["opco"].ToString().ToUpper();
                    oem = networkAssetData.Softwarecomponent.Data["oem"].ToString().ToUpper();
                    softwarecomponent = "Upper(OPCO) = '" + opco + "' and Upper(oem) = '" + oem + "' and networkelementid= '" + networkelementid + "'";
                    string softwarecomponentid = DataBaseFunctions.GetPrimaryKey("softwarecomponent", softwarecomponent);
                    if (softwarecomponentid == "NA")
                    {
                        string softwarecomponentInsert = DBModelFunctions.GenerateInsertQuery("softwarecomponent", networkAssetData.Softwarecomponent.Data, networkelementid.ToString());
                        softwarecomponentid = DataBaseFunctions.DBInsertion(connection, softwarecomponentInsert).ToString();

                    }
                    else
                    {
                        DBData = DataBaseFunctions.selectFromDB("softwarecomponent", softwarecomponentid);
                        DiffData = CompareData(networkAssetData.Softwarecomponent.Data, DBData);
                        if (DiffData.Count > 0)
                        {
                            string updateQuery = DBModelFunctions.GenerateUpdateQuery("softwarecomponent", DiffData, softwarecomponentid, isAsis);
                            rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                            //DBModelFunctions.AuditInsert(connection, "softwarecomponent", DBData, DiffData, softwarecomponentid, opco, oem, elementname);
                        }
                    }
                    if (networkAssetData.Softwarecomponent.ComponentList != null)
                    {

                        foreach (Dictionary<string, string> component in networkAssetData.Softwarecomponent.ComponentList)
                        {
                            string comp_Name = component["componentname"].ToString().ToUpper();
                            componentQuery = "Upper(componentname) = '" + comp_Name + "' and softwarecomponentid= '" + softwarecomponentid + "'";
                            string componentid = DataBaseFunctions.GetPrimaryKey("component", componentQuery);
                            if (componentid == "NA")
                            {
                                string componentinsert = DBModelFunctions.GenerateInsertQuery("component", component, softwarecomponentid);
                                componentid = DataBaseFunctions.DBInsertion(connection, componentinsert).ToString();
                            }
                            else
                            {
                                DBData = DataBaseFunctions.selectFromDB("component", componentid);
                                DiffData = CompareData(component, DBData);
                                if (DiffData.Count > 0)
                                {
                                    string updateQuery = DBModelFunctions.GenerateUpdateQuery("component", DiffData, componentid, isAsis);
                                    rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                                    //DBModelFunctions.AuditInsert(connection, "component", DBData, DiffData, componentid, opco, oem, elementname);
                                }
                            }
                        }
                    }
                }
                if (networkAssetData.Softconfiguration != null)
                {
                    opco = networkAssetData.Softconfiguration.Data["opco"].ToString().ToUpper();
                    softwareconfiguration = "Upper(OPCO) = '" + opco + "' and networkelementid= '" + networkelementid + "'";
                    string softconfigurationid = DataBaseFunctions.GetPrimaryKey("softwareconfiguration", softwareconfiguration);
                    if (softconfigurationid == "NA")
                    {
                        string softconfigurationInsert = DBModelFunctions.GenerateInsertQuery("softwareconfiguration", networkAssetData.Softconfiguration.Data, networkelementid.ToString());
                        softconfigurationid = DataBaseFunctions.DBInsertion(connection, softconfigurationInsert).ToString();
                    }
                    else
                    {
                        DBData = DataBaseFunctions.selectFromDB("softwareconfiguration", softconfigurationid);
                        DiffData = CompareData(networkAssetData.Softconfiguration.Data, DBData);
                        if (DiffData.Count > 0)
                        {
                            string updateQuery = DBModelFunctions.GenerateUpdateQuery("softwareconfiguration", DiffData, softconfigurationid, isAsis);
                            rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                            //DBModelFunctions.AuditInsert(connection, "softwareconfiguration", DBData, DiffData, softconfigurationid, opco, oem, elementname);
                        }
                    }
                    if (networkAssetData.Softconfiguration.FunctionList != null)
                    {
                        Dictionary<string, string> data = new Dictionary<string, string>();
                        foreach (Function function_List in networkAssetData.Softconfiguration.FunctionList)
                        {
                            string fun_Name = function_List.Function_name.ToUpper();
                            functionQuery = "Upper(functionname) = '" + fun_Name + "' and softwareconfigurationid= '" + softconfigurationid + "'";
                            string functionid = DataBaseFunctions.GetPrimaryKey("function", functionQuery);
                            if (functionid == "NA")
                            {
                                data = new Dictionary<string, string>();
                                data.Add("functionname", function_List.Function_name);
                                string functionInsert = DBModelFunctions.GenerateInsertQuery("function", data, softconfigurationid.ToString());
                                functionid = DataBaseFunctions.DBInsertion(connection, functionInsert).ToString();
                                isSwConfigurationUpdate = true;
                                
                            }
                            else
                            {
                                DBData = DataBaseFunctions.selectFromDB("function", functionid);
                                DiffData = CompareData(data, DBData);
                                if (DiffData.Count > 0)
                                {
                                    string updateQuery = DBModelFunctions.GenerateUpdateQuery("function", DiffData, functionid, isAsis);
                                    rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                                    isSwConfigurationUpdate = true;

                                    //DBModelFunctions.AuditInsert(connection, "function", DBData, DiffData, functionid, opco, oem, elementname);
                                }
                            }

                            if (function_List.FunctionArea_List != null)
                            {
                                foreach (FunctionArea functionArea in function_List.FunctionArea_List)
                                {

                                    string areaname = functionArea.Data["functionareaname"].ToString().ToUpper();
                                    function_areaQuery = "Upper(functionareaname) = '" + areaname + "' and functionid = '" + functionid + "'";
                                    string functionareaid = DataBaseFunctions.GetPrimaryKey("swconfigfunctionareas", function_areaQuery);
                                    if (functionareaid == "NA")
                                    {
                                        string function_areainsert = DBModelFunctions.GenerateInsertQueryNclob("swconfigfunctionareas", functionArea.Data, functionid.ToString(), "functionareadescription");
                                        functionareaid = DataBaseFunctions.DBInsertion(connection, function_areainsert).ToString();
                                        isSwConfigurationUpdate = true;

                                    }
                                    else
                                    {
                                        //DBData = DataBaseFunctions.selectFromDB("functionarea", functionareaid);
                                        //DiffData = CompareData(functionArea.Data, DBData);
                                        //if (DiffData.Count > 0)
                                        //{
                                        string updateQuery = DBModelFunctions.GenerateUpdateQueryNclob("swconfigfunctionareas", functionArea.Data, functionareaid, "functionareadescription");
                                        rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                                        isSwConfigurationUpdate = true;

                                        //DBModelFunctions.AuditInsert(connection, "functionarea", DBData, DiffData, functionareaid, opco, oem, elementname);
                                        //}
                                    }
                                    if (functionArea.Subfunction_List != null && functionArea.Subfunction_List.Count > 0)
                                    {
                                        foreach (SubFunctionArea _subfunction in functionArea.Subfunction_List)
                                        {
                                            string subfunctionareaname = _subfunction.Data["subfunctionname"].ToUpper();
                                            subfunctionQuery = "Upper(subfunctionname) = '" + subfunctionareaname + "' and swconfigfunctionareaid= '" + functionareaid + "'";
                                            string subfunctionid = DataBaseFunctions.GetPrimaryKey("swconfigsubfunction", subfunctionQuery);
                                            if (subfunctionid == "NA")
                                            {
                                                string subfunctionInsert = DBModelFunctions.GenerateInsertQuery("swconfigsubfunction", _subfunction.Data, functionareaid.ToString());
                                                subfunctionid = DataBaseFunctions.DBInsertion(connection, subfunctionInsert).ToString();
                                                isSwConfigurationUpdate = true;

                                            }
                                            else
                                            {
                                                DBData = DataBaseFunctions.selectFromDB("swconfigsubfunction", subfunctionid);
                                                DiffData = CompareData(_subfunction.Data, DBData);
                                                if (DiffData.Count > 0)
                                                {
                                                    string updateQuery = DBModelFunctions.GenerateUpdateQuery("swconfigsubfunction", DiffData, subfunctionid, isAsis);
                                                    rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                                                    isSwConfigurationUpdate = true;
                                                    //DBModelFunctions.AuditInsert(connection, "subfunction", DBData, DiffData, subfunctionid, opco, oem, elementname);
                                                }
                                            }
                                            if (_subfunction.SubFunctionAreaList != null)
                                            {
                                                foreach (Dictionary<string, string> subfuncarea in _subfunction.SubFunctionAreaList)
                                                {
                                                    subfunctionareaQuery = "swconfigsubfunctionid= '" + subfunctionid + "'";
                                                    string subfunctionareaid = DataBaseFunctions.GetPrimaryKey("swconfigsubfunctionareas", subfunctionareaQuery);
                                                    if (subfunctionareaid == "NA")
                                                    {
                                                        string subfunctionareainsert = DBModelFunctions.GenerateInsertQueryNclob("swconfigsubfunctionareas", subfuncarea, subfunctionid.ToString(), "subfunctionareadescription");
                                                        //foreach (string query in subfunctionareainsert)
                                                        //{
                                                        subfunctionareaid = DataBaseFunctions.DBInsertion(connection, subfunctionareainsert).ToString();
                                                        isSwConfigurationUpdate = true;

                                                        //}

                                                    }

                                                    else
                                                    {
                                                        string updateQuery = DBModelFunctions.GenerateUpdateQueryNclob("swconfigsubfunctionareas", subfuncarea, subfunctionareaid, "subfunctionareadescription");
                                                        rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                                                        isSwConfigurationUpdate = true;

                                                        //List<string> table_list = DBModelFunctions.GetTableName("subfunctionarea");
                                                        //foreach (string table in table_list)
                                                        //{
                                                        //    subfunctionareaQuery = "subfunctionid= '" + subfunctionid + "'";
                                                        //    string _subfunc_area_id = DataBaseFunctions.GetPrimaryKey(table, subfunctionareaQuery);
                                                        //    DBData = DataBaseFunctions.selectFromDB(table, _subfunc_area_id);
                                                        //    DiffData = CompareData(subfuncarea, DBData);
                                                        //    if (DiffData.Count > 0)
                                                        //    {

                                                        //        string updateQuery = DBModelFunctions.GenerateUpdateQuery(table, DiffData, _subfunc_area_id);
                                                        //        rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                                                        //        //DBModelFunctions.AuditInsert(connection, table, DBData, DiffData, _subfunc_area_id, opco, oem, elementname);

                                                        //    }
                                                        //}

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if(isSwConfigurationUpdate == true)
                    {
                        string updateQuery = DBModelFunctions.GenerateUpdateQuery("softwareconfiguration", DiffData, softconfigurationid, isAsis);
                        rowsUpdated += DataBaseFunctions.DBUpdation(connection, updateQuery);
                    }

                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                connection?.Close();
            }

            return rowsUpdated;
        }
        public static Dictionary<string, string> CompareData(Dictionary<string, string> XMLData, Dictionary<string, string> DBData)
        {
            Dictionary<string, string> DataDifference = new Dictionary<string, string>();
            string key = "";
            //Compare Data
            foreach (KeyValuePair<string, string> data in XMLData)
            {
                key = data.Key;

                if (DBData.ContainsKey(key.Trim()))
                {
                    if (DBData[key].Trim() != XMLData[key].Trim())
                    {
                        DataDifference.Add(key, data.Value);
                    }
                }

            }
            return DataDifference;
        }

        public static void AsIsDataSync()
        {
            List<Dictionary<string, string>> SyncList = new List<Dictionary<string, string>>();
            SyncList.Add(new Dictionary<string, string>() { { "oldtable", "networkelement" }, { "oldcolumn", "NODETYPE" }, { "oldCompareColumn", "ELEMENTname" }, { "newtable", "networkelementsasis" }, { "newcolumn", "NODETYPE" }, { "newCompareColumn", "ELEMENTDEPLOYMENTNAME" } });
            SyncList.Add(new Dictionary<string, string>() { { "oldtable", "networkelement" }, { "oldcolumn", "SOFTWAREPRODUCTNUMBER" }, { "oldCompareColumn", "ELEMENTname" }, { "newtable", "networkelementsasis" }, { "newcolumn", "SOFTWAREPRODUCTNUMBER" }, { "newCompareColumn", "ELEMENTDEPLOYMENTNAME" } });
            SyncList.Add(new Dictionary<string, string>() { { "oldtable", "networkelement" }, { "oldcolumn", "DATAACQUISITIONDATE" }, { "oldCompareColumn", "ELEMENTname" }, { "newtable", "networkelementsasis" }, { "newcolumn", "DATAACQUISITIONDATE" }, { "newCompareColumn", "ELEMENTDEPLOYMENTNAME" } });
            SyncList.Add(new Dictionary<string, string>() { { "oldtable", "networkelement" }, { "oldcolumn", "SOFTWAREINSTALLDATE" }, { "oldCompareColumn", "ELEMENTname" }, { "newtable", "networkelementsasis" }, { "newcolumn", "SOFTWAREINSTALLDATE" }, { "newCompareColumn", "ELEMENTDEPLOYMENTNAME" } });
            DataBaseFunctions.ExecuteStoredProcedure("DBMS_OUTPUT.ENABLE", new Dictionary<string, string>());
            foreach (Dictionary<string, string> parameters in SyncList)
            {
                DataBaseFunctions.ExecuteStoredProcedure("Updateasistable", parameters);
            }
        }
    }
}
