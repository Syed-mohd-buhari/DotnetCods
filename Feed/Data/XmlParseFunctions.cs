using System.Collections;
using System.Data.Common;
using System.Xml;
using TEMS.Entity;
using TEMS.Error;
using TEMS.Logs;
using static TEMS.Logs.Logger;

namespace TEMS.Data
{
    public class XmlParseFunctions 
    {
        static Dictionary<string, ArrayList> dataModel = new Dictionary<string, ArrayList>();
        public static ArrayList nadList = new ArrayList();

        /*
        * Module Name : XmlParse
        * Description : it is use to parse the XMl files
        * Parameter : xmlFile  
        * xmlFile - it is string argumant it is use to store the xml file path
        */
        public static ArrayList XmlParse(string xmlFile)
        {
            NetworkAssetData networkAsset = new NetworkAssetData();
            try
            {
                Logger.WriteLog(TEMLog.Info, "XmlParse", "Parsing xml File --> " + xmlFile);
                nadList = new ArrayList();
                XmlDocument xmldoc = new XmlDocument();
                //Removing all (as_is) in xml file since facing issue in xml loading
                FileFunctions.RemoveTextFromFile(xmlFile, "(as_is)");
                //Populating DataModel from Mapping table
                dataModel = DBModelFunctions.GetDBColumnData();
                xmldoc.Load(xmlFile);
                TraverseNodes(xmldoc);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "XmlParse", $"{ex.Message}");
                throw;
            }
            return nadList;
        }



        /*
        * Module Name : TraverseNodes
        * Description : Reading the xml nodes
        * Parameter : XmlNode nodes
        * XmlNode nodes - this is use to  pass all the xml nodes
        */
        public static ArrayList TraverseNodes(XmlNode nodes)
        {
            ArrayList objArray = new ArrayList();
            NetworkAssetData networkassetdata = new NetworkAssetData();
            try
            {
                foreach (XmlNode node in nodes)
                {
                    if ((node.Name == "xml") || (node.Name == "#comment"))
                    {
                        //Do nothing
                    }
                    else if (node.Name == "nodes")
                    {
                        networkassetdata = TraverseChildnodes(node.ChildNodes, node.Name, "", node.BaseURI);
                        objArray.Add(networkassetdata);
                    }
                    else
                    {
                        Logger.WriteLog(TEMLog.Error, "TraverseNodes", $"Unknown tag : {node.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "TraverseNodes", $"{ex.Message}");
                throw;
            }
            return objArray;
        }


        /*
        * Module Name : TraverseChildnodes
        * Description : this function is use to validate all the xml childNodes
        * Parameter :  nodes, node_name, element_name
        * nodes : this is pass list of xml nodes
        * element_name : this is pass the particular node atrribute name 
        * node_name : Just to print in the log 
        */
        public static NetworkAssetData TraverseChildnodes(XmlNodeList nodes, string node_name, string element_name, string xmlFile)
        {
            Logger.WriteLog(TEMLog.Debug, "TraverseChildnodes", $"{node_name}");
            NetworkAssetData networkassetdata = new NetworkAssetData();
            try
            {
                if (node_name == "networkassetdata")
                {

                    networkassetdata = new NetworkAssetData();
                    networkassetdata.name = CommonFunction.GetProperElementName(element_name);
                    networkassetdata.XmlDate = CommonFunction.FileNametoDate(xmlFile);
                    foreach (XmlNode node in nodes)
                    {
                        if (node.Name == "networkelement")
                        {
                            networkassetdata.NetworkElement = Traverse_nodes_networkelement(node.ChildNodes);
                            networkassetdata.NetworkElement.Add("XMLLASTPARSEFILEDATE".ToLower(), networkassetdata.XmlDate);
                            networkassetdata.NetworkElement.Add("NODETYPENAME".ToLower(), CommonFunction.FileNametoNodetypeName(xmlFile).ToUpper());
                        }
                        else if (node.Name == "softwarecomponent")
                        {
                            networkassetdata.Softwarecomponent = Traverse_nodes_softwarecomponent(node.ChildNodes);
                        }
                        else if (node.Name == "softconfiguration")
                        {
                            networkassetdata.Softconfiguration = Traverse_nodes_softconfiguration(node.ChildNodes);
                        }
                        else if (node.Name == "identity")
                        {
                            networkassetdata.Identity = Traverse_nodes_identity(node.ChildNodes);
                        }
                        else if (node.Name == "hardwareconfiguration")
                        {
                            networkassetdata.HardwareConfiguration = Traverse_nodes_hardwareconfiguration(node.ChildNodes);
                        }
                        else
                        {
                            Logger.WriteLog(TEMLog.Error, "TraverseChildnodes", $"{node_name} not handled");
                        }
                    }

                    //Patch to align oem, opco, element name in all tables
                    string oem = "", opco = "", elementname = "";
                    oem = networkassetdata.NetworkElement["oem"].ToUpper();
                    if(oem == "")
                    {
                        oem = "ERICSSON";
                    }
                    opco = networkassetdata.NetworkElement["opco"].ToUpper();
                    if(opco.ToLower().Trim() == "vodafone_uk")
                    {
                        opco = "UK";
                    }
                    elementname = CommonFunction.GetProperElementName(networkassetdata.NetworkElement["elementname"].ToUpper()).ToUpper();

                    if(networkassetdata.NetworkElement != null)
                    {
                        networkassetdata.NetworkElement["oem"] = oem;
                        networkassetdata.NetworkElement["opco"] = opco;
                        networkassetdata.NetworkElement["elementname"] = elementname;
                    }

                    if (networkassetdata.Identity != null)
                    {
                        networkassetdata.Identity["oem"] = oem;
                        networkassetdata.Identity["opco"] = opco;
                        networkassetdata.Identity["elementname"] = elementname;
                    }

                    if (networkassetdata.Softwarecomponent != null)
                    {
                        networkassetdata.Softwarecomponent.Data["oem"] = oem;
                        networkassetdata.Softwarecomponent.Data["opco"] = opco;
                        networkassetdata.Softwarecomponent.Data["elementname"] = elementname;
                    }

                    if (networkassetdata.Softconfiguration != null)
                    {
                        networkassetdata.Softconfiguration.Data["oem"] = oem;
                        networkassetdata.Softconfiguration.Data["opco"] = opco;
                        networkassetdata.Softconfiguration.Data["elementname"] = elementname;
                    }

                    if (networkassetdata.HardwareConfiguration != null)
                    {
                        foreach (Dictionary<string, string> hconfig in networkassetdata.HardwareConfiguration)
                        {
                            hconfig["oem"] = oem;
                            hconfig["opco"] = opco;
                            hconfig["elementname"] = elementname;
                        }
                    }

                    nadList.Add(networkassetdata);
                }
                else
                {
                    foreach (XmlNode node in nodes)
                    {
                        if (node.Name == "node" && node.Attributes != null)
                        {
                            element_name = CommonFunction.GetProperElementName(node.Attributes["name"].Value);
                        }
                        TraverseChildnodes(node.ChildNodes, node.Name, element_name, node.BaseURI);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "TraverseChildnodes", $"{ex.Message}");
                throw;
            }
            return networkassetdata;
        }

        /*
       * Module Name : Traverse_nodes_networkelement
       * Description : this function is use to validate inside networkelement node 
       * Parameter :  nodes
       * nodes : this is pass list networkelement of xml nodes
       */
        public static Dictionary<string, string> Traverse_nodes_networkelement(XmlNodeList nodes)
        {
            Dictionary<string, string> networkElement = new Dictionary<string, string>();
            Logger.WriteLog(TEMLog.Debug, "Traverse_nodes_networkelement", "Inside Network Element");
            try
            {
                foreach (XmlNode node in nodes)
                {
                    if ( node.Name.ToLower().Equals("softwareproductdate".ToLower()) || node.Name.ToLower().Equals("softwareinstalldate".ToLower())
                        || node.Name.ToLower().Equals("softwareproductdateAp".ToLower()) || node.Name.ToLower().Equals("softwareinstalldateAp".ToLower())
                        || node.Name.ToLower().Equals("softwareproductdateCp".ToLower()) || node.Name.ToLower().Equals("softwareinstalldateCp".ToLower())
                        || node.Name.ToLower().Equals("dataacquisitiondate".ToLower()))
                    {
                        networkElement.Add(node.Name.ToLower(), CommonFunction.RawDateConvertion(node.InnerText.ToUpper()));                       
                    }
                    else
                    {
                        networkElement.Add(node.Name.ToLower(), node.InnerText.ToUpper());
                    }
                }

                networkElement = Nodes_Validation(networkElement, "networkelement");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "Traverse_nodes_networkelement", $"{ex.Message}");
                throw;
            }
            Logger.WriteLog(TEMLog.Debug, "Traverse_nodes_networkelement", "Exiting Network Element");
            return networkElement;

        }


        /*
      * Module Name : subTag
      * Description : this function is use to validate if any node contain subnode 
      * Parameter :  nodes,node_name,element_name
      * nodes : this is pass list of xml nodes
      * node_name :  Just to print in the log 
      * element_name : this is pass the particular node atrribute name 
      */
        public static Dictionary<string, string> SubTag(XmlNodeList nodes, string node_name, string element_name)
        {
            Dictionary<string, string> tagList = new Dictionary<string, string>();
            try
            {
                foreach (XmlNode node in nodes)
                {
                    tagList.Add(node.Name.ToLower(), node.InnerText.Replace("'", ""));
                }
                tagList.Add("subfunctionareaname", element_name);
                //Node Validation
                //tagList = Nodes_Validation(tagList, "subfunctionarea");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "subTag", $"{node_name},{ex.Message}");
                throw;
            }
            return tagList;

        }

        /*
        * Module Name : Component
        * Description : this function is use to validate if any node contain subnode 
        * Parameter :  nodes,node_name,element_name
        * nodes : this is pass list of xml nodes
        * node_name :  Just to print in the log 
        * element_name : this is pass the particular node atrribute name 
        */
        public static Dictionary<string, string> Component(XmlNodeList nodes, string node_name, string element_name)
        {
            Dictionary<string, string> tag_list = new Dictionary<string, string>();
            try
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.Name.ToLower().Equals("productiondate".ToLower()))
                    {
                        tag_list.Add(node.Name.ToLower(), CommonFunction.RawDateConvertion(node.InnerText.ToUpper()));
                    }
                    else
                    {
                        tag_list.Add(node.Name.ToLower(), node.InnerText.ToUpper());
                    }
                }
                tag_list.Add("componentname", element_name);
                tag_list = Nodes_Validation(tag_list, "component");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "component", $"{ex.Message}");
                throw;
            }
            return tag_list;

        }


        /*
       * Module Name : Traverse_nodes_softwarecomponent
       * Description : this function is use to validate inside sofwarecomponent node 
       * Parameter :  nodes
       * nodes : this is pass list of sofwarecompoonent xml nodes
       */
        public static SoftwareComponent Traverse_nodes_softwarecomponent(XmlNodeList nodes)
        {
            SoftwareComponent softComp = new SoftwareComponent();
            Dictionary<string, string> data = new Dictionary<string, string>();
            Logger.WriteLog(TEMLog.Debug, "Traverse_nodes_softwarecomponent", "Inside Software Component");
            try
            {
                Dictionary<string, string> subtag = new Dictionary<string, string>();
                ArrayList arr = new ArrayList();
                foreach (XmlNode node in nodes)
                {
                    try
                    {
                        if (node.Name.ToLower() == "component")
                        {
                            subtag = new Dictionary<string, string>();
                            subtag = Component(node.ChildNodes, node.Name, node.Attributes["name"].Value);
                            arr.Add(subtag);
                        }
                        else
                        {

                            data.Add(node.Name.ToLower(), node.InnerText);
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(TEMLog.Error, "Traverse_nodes_softwarecomponent", $"{ex.Message}");
                        throw;
                    }

                }
                data = Nodes_Validation(data, "softwarecomponent");
                softComp.Data = data;
                softComp.ComponentList = arr;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "Traverse_nodes_softwarecomponent", $"{ex.Message}");
                throw;
            }
            Logger.WriteLog(TEMLog.Debug, "Traverse_nodes_softwarecomponent", "Exiting software Component");
            return softComp;
        }

        /*
      * Module Name : Traverse_nodes_softconfiguration
      * Description : this function is use to validate inside softwareconfiguration node 
      * Parameter :  nodes
      * nodes : this is pass list of softwareconfiguration xml nodes
      */
        public static SoftwareConfiguration Traverse_nodes_softconfiguration(XmlNodeList nodes)
        {
            SoftwareConfiguration softConf = new SoftwareConfiguration();
            Dictionary<string, string> data = new Dictionary<string, string>();
            Logger.WriteLog(TEMLog.Debug, "Traverse_nodes_softconfiguration", "Inside softconfiguration");
            try
            {
                Function _function = new Function();
                ArrayList arr = new ArrayList();
                foreach (XmlNode node in nodes)
                {
                    if (node.Name.ToLower() == "function")
                    {
                        _function = new Function();
                        _function = Function_SubTag(node.ChildNodes, node.Name, node.Attributes["name"].Value);
                        arr.Add(_function);
                    }
                    else
                    {
                        data.Add(node.Name.ToLower(), node.InnerText);
                    }
                }
                data = Nodes_Validation(data, "softconfiguration");
                softConf.Data = data;
                softConf.FunctionList = arr;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "Traverse_nodes_softconfiguration", $"{ex.Message}");
                throw;
            }
            Logger.WriteLog(TEMLog.Debug, "Traverse_nodes_softconfiguration", "Exiting softwareconfiguration");
            return softConf;
        }


        /*
           * Module Name : Function_SubTag
           * Description : this method is use to validate inside softwareconfiguration node if contains any function node 
           * Parameter :  nodes,element_name,node_name
           * nodes : this is pass list of softwareconfiguration xml nodes
           * node_name :  Just to print in the log 
           * element_name : this is pass the particular node atrribute name 
           */
        public static Function Function_SubTag(XmlNodeList nodes, string node_name, string element_name)
        {

            Function _function = new Function();
            Dictionary<string, string> data = new Dictionary<string, string>();
            FunctionArea _functionarea = new FunctionArea();
            ArrayList functionarea_array = new ArrayList();
            try
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.Name.ToLower() == "area")
                    {
                        _functionarea = Functionarea_subTag(node.ChildNodes, node.Name, node.Attributes["name"].Value);
                        functionarea_array.Add(_functionarea);
                    }
                    else
                    {
                        Logger.WriteLog(TEMLog.Error, "function_subTag", "Unknown tag in function: " + node.Name);
                        //data.Add(node.Name.ToLower(), node.InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "function_subTag", $"{ex.Message}");
                throw;
            }
            _function.Function_name = element_name;
            _function.FunctionArea_List = functionarea_array;
            return _function;
        }


        /*
           * Module Name : subfunction_subTag
           * Description : this method is use to validate inside function node  if contains any area node 
           * Parameter :  nodes,element_name,node_name
           * nodes : this is pass list of softwareconfiguration xml nodes
           * node_name :  Just to print in the log 
           * element_name : this is pass the particular node atrribute name 
           */

        public static FunctionArea Functionarea_subTag(XmlNodeList nodes, string node_name, string element_name)
        {
            FunctionArea _functionarea = new FunctionArea();
            SubFunctionArea _subfunctArea = new SubFunctionArea();
            Dictionary<string, string> data = new Dictionary<string, string>();

            ArrayList arr = new ArrayList();
            try
            {
                data.Add("functionareaname", element_name);
                foreach (XmlNode node in nodes)
                {
                    if (node.Name.ToLower() == "area")
                    {
                        //_subfunctArea = new SubFunctionArea();
                        //_subfunctArea = subfuncArea_subTag(node.ChildNodes, node.Name, node.Attributes["name"].Value);
                        //arr.Add(_subfunctArea);
                    }
                    else if (node.Name.ToLower() == "subfunction")
                    {
                        _subfunctArea = new SubFunctionArea();
                        _subfunctArea = SubfuncArea_subTag(node.ChildNodes, node.Name, node.Attributes["name"].Value);
                        arr.Add(_subfunctArea);
                    }
                    else
                    {
                        data.Add(node.Name.ToLower(), node.InnerText);
                    }
                }
                 //data = Nodes_Validation(data, "functionarea");
                //data.Add("name", element_name);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "functionarea_subTag", $"{ex.Message}");
                throw;
            }
            _functionarea.Data = data;
            _functionarea.Subfunction_List = arr;
            return _functionarea;
        }



        /*
      * Module Name : subfuncArea_subTag
      * Description : this method is use to validate inside area node  if contains any subfunction nodes 
      * Parameter :  nodes,element_name,node_name
      * nodes : this is pass list of softwareconfiguration xml nodes
      * node_name :  Just to print in the log 
      * element_name : this is pass the particular node atrribute name 
      */
        public static SubFunctionArea SubfuncArea_subTag(XmlNodeList nodes, string node_name, string element_name)
        {
            SubFunctionArea _subfunctArea = new SubFunctionArea();
            Dictionary<string, string> data = new Dictionary<string, string>();
            Dictionary<string, string> area_data = new Dictionary<string, string>();
            ArrayList arr = new ArrayList();
            try
            {
                foreach (XmlNode node in nodes)
                {
                    if (node.Name.ToLower() == "area")
                    {
                        area_data = new Dictionary<string, string>();
                        area_data = SubTag(node.ChildNodes, node.Name, node.Attributes["name"].Value);
                        arr.Add(area_data);
                    }
                    else
                    {
                        data.Add(node.Name.ToLower(), node.InnerText);
                    }
                }
                data.Add("subfunctionname", element_name);
                data = Nodes_Validation(data, "subfunction");
                
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "subfuncArea_subTag", $"{ex.Message}");
                throw;
            }
            _subfunctArea.Data = data;
            _subfunctArea.SubFunctionAreaList = arr;
            return _subfunctArea;
        }


        /*
      * Module Name : Traverse_nodes_identity
      * Description : this function is use to validate inside identity node 
      * Parameter :  nodes
      * nodes : this is pass list of identity xml nodes
      */
        public static Dictionary<string, string> Traverse_nodes_identity(XmlNodeList nodes)
        {
            Dictionary<string, string> identity = new Dictionary<string, string>();
            Logger.WriteLog(TEMLog.Debug, "Traverse_nodes_identity", "Inside identity");
            try
            {
                foreach (XmlNode node in nodes)
                {
                    identity.Add(node.Name.ToLower(), node.InnerText);
                }
                identity = Nodes_Validation(identity, "identity");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "Traverse_nodes_identity", $"{ex.Message}");
                throw;
            }
            Logger.WriteLog(TEMLog.Debug, "Traverse_nodes_identity", "Exiting identity");
            return identity;
        }

        /*
     * Module Name : Traverse_nodes_hardwareconfiguration
     * Description : this function is use to validate inside hardwareconfiguration node 
     * Parameter :  nodes
     * nodes : this is pass list of hardwareconfiguration xml nodes
     */
        public static ArrayList Traverse_nodes_hardwareconfiguration(XmlNodeList nodes)
        {

            ArrayList configuration = new ArrayList();
            Dictionary<string, string> config = new Dictionary<string, string>();
            Logger.WriteLog(TEMLog.Debug, "Traverse_nodes_hardwareconfiguration", "Inside Traverse_nodes_hardwareconfiguration");
            try
            {
                foreach (XmlNode node in nodes)
                {
                    config = new Dictionary<string, string>();
                    if (node.Name == "configuration")
                    {
                        config = Traverse_nodes_configuration(node.ChildNodes);
                        configuration.Add(config);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "Traverse_nodes_hardwareconfiguration", $"{ex.Message}");
                throw;
            }
            return configuration;
        }

        /*
      * Module Name : Traverse_nodes_configuration
      * Description : this function is use to validate inside hardwareconfiguration node that contains list of configuration nodes
      * Parameter :  nodes
      * nodes : this is pass list of configuration xml nodes
      */
        public static Dictionary<string, string> Traverse_nodes_configuration(XmlNodeList nodes)
        {
            Logger.WriteLog(TEMLog.Debug, "Traverse_nodes_configuration", "Inside hardwareconfiguration nodes");
            Dictionary<string, string> configuration = new Dictionary<string, string>();
            try
            {
                foreach (XmlNode node in nodes)
                {
                    configuration.Add(node.Name.ToLower(), node.InnerText);
                }
                configuration = Nodes_Validation(configuration, "hardwareconfiguration");

            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "Traverse_nodes_configuration", $"{ex.Message}");
                throw;
            }
            return configuration;
        }


        /*
       * Module Name : Nodes_Validation
       * Description : To validate the oracle table and column names and Column size 
       * Parameter : nodeTags, node_name
       * nodeTags - dictionary datatype with contains key value pair of tags and cloumn data, values
       * node_name - Just to print in the log 
       */


        public static Dictionary<string, string> Nodes_Validation(Dictionary<string, string> nodeTags, string node_name)
        {
            ArrayList nodes_Tag_Defined = new ArrayList();
            Dictionary<string, string> updated_Data = new Dictionary<string, string>();
            bool validate = true;
            try
            {
                foreach (KeyValuePair<string, string> node_element in nodeTags)
                {
                    bool found = false;

                    List<string> tables = DBModelFunctions.GetTableName(node_name);
                    foreach (string table in tables)
                    {
                        // _table = table;
                        nodes_Tag_Defined = dataModel[table];

                        //nodes_Tag_Defined = dataModel[node_name];
                        if (nodes_Tag_Defined.Count == 0)
                        {
                            validate = false;
                            throw new CustomError("Module_name : Nodes_Validation", "No Tags to validate");
                        }

                        if (found == true)
                            continue;

                        foreach (Dictionary<string, string> node in nodes_Tag_Defined)
                        {

                            if (node["childtag"] == node_element.Key)
                            {
                                found = true;
                                updated_Data.Add(node["columnname"], node_element.Value);
                                if (node_element.Value.Length > Convert.ToInt32(node["columnsize"]))
                                {
                                    Logger.WriteLog(TEMLog.Warn, "Nodes_verified", table + " " + node_element.Key + " Db column size mismatched : Expected : " + node["columnsize"] + " actual  " + node_element.Value.Length);
                                    validate = false;
                                    //throw new CustomError("Module_name : Nodes_Validation", "Dbsize mismatched");
                                }
                                else if (((node_element.Value.Length) + 20) > Convert.ToInt32(node["columnsize"]))
                                {
                                    Logger.WriteLog(TEMLog.Warn, "Nodes_verified", table + " " + node_element.Key + " Db column size mismatched : Expected : " + node["columnsize"] + " actual  " + node_element.Value.Length);
                                }
                                break;
                            }

                        }//foreach node check ends
                    }//foreach tables ends
                    if (found == false)
                    {
                       //int count = dataModel[table].Count 
                        Logger.WriteLog(TEMLog.Warn, "Nodes_verified", node_name + "  tag a new tag " + node_element.Key);
                        validate = true;
                        Logger.WriteLog(TEMLog.Error, "Nodes_verified", $"New tag {node_element.Key.ToLower()} found in table {node_name}");
                        if (node_element.Key.Length > 30)
                        {
                            Logger.WriteLog(TEMLog.DBMappingHelp, "Nodes_verified", $"{node_name};{node_element.Key.ToLower()};false;{node_name};{node_element.Key.ToLower()};50");
                        }
                        else
                        {
                            Logger.WriteLog(TEMLog.DBMappingHelp, "Nodes_verified", $"{node_name};{node_element.Key.ToLower()};false;{node_name};{node_element.Key.ToLower()};50");
                        }
                        Logger.WriteLog(TEMLog.DBTableHelp, "Nodes_verified", $"ALTER TABLE {node_name} ADD {node_element.Key.ToLower()} nvarchar2(50);");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "Nodes_Validation", $"for node {node_name} error = {ex.Message}");
                throw;
            }
            return updated_Data;
        }

    }
}

