using System.Collections;
using System.Configuration;
using TEMS.Data;
using TEMS.Entity;
using TEMS.Logs;
using static TEMS.Logs.Logger;

namespace TEMS
{
    /*<Summary>
     * This is the main class from which all the different fnctions called and passed the values from it.
     * It has Different class that are used for Xmlparsing, File related classes and Database classes also.
     </Summary>*/
    public class Program
    {
        public static void Main(string[] args)
        {
            int user = CommonFunction.getUserName();
            string[]? fileList;
            Logger.WriteLog(TEMLog.Info, "MainProgram", "TEMS Parsing Program Starting");
            try
            {

                //Getting the SFTP Folder path
                string SFTP_path = CommonFunction.getAppConfigValue("SFTP_path");
                //Instanticiating the FileFunctions class
                FileFunctions files = new FileFunctions();
                //Getting all the opco folderlost from the SFTP folder

                #region// Deleting the Last one yeat data from logs table

                var currentYear = DateTime.Now;
                var previousYear = currentYear.AddYears(-1).ToString("dd-MMM-yy").ToUpper();
                List<int> xmlParseFk = new List<int>();

                //string getFKRelationDataQuery = $"select xmlparserunid from xmlparserun where xmlparserunid in (select xmlparserunid from nodeparsehistory where trunc(creationdate) = to_date('{previousYear}', 'DD-MON-YY'))";
                string getRelationXmlParseidfromNodehistoryQuery = $"select xmlparserunid from nodeparsehistory where trunc(creationdate) = to_date('{previousYear}', 'DD-MON-YY')";
                List<int> getRelationXmlParseidfromNodehistory = DataBaseFunctions.GetFKRelationData(getRelationXmlParseidfromNodehistoryQuery);
                if (getRelationXmlParseidfromNodehistory.Count > 0)
                {
                    string getRelationfeedbackLoopQuery = $"select feedbackloopauditid from xmlparserun where xmlparserunid in ({string.Join(',', getRelationXmlParseidfromNodehistory.Distinct() ?? new List<int>())})";
                    List<int> getRelationFeedbackLoopid = DataBaseFunctions.GetFKRelationData(getRelationfeedbackLoopQuery);
                    if (getRelationFeedbackLoopid.Count > 0)
                    {
                        string deleteNodeHistoryQuery = $"delete from nodeparsehistory where xmlparserunid in ({string.Join(',', getRelationXmlParseidfromNodehistory.Distinct() ?? new List<int>())})";
                        string xmlParseRunDeleteQuery = $"delete from xmlparserun where xmlparserunid in ({string.Join(',', getRelationXmlParseidfromNodehistory.Distinct() ?? new List<int>())})";
                        // string xmlParseRunDeleteQuery = $"delete from xmlparserun where trunc(creationdate) = to_date('{previousYear}', 'DD-MON-YY')";
                        string feedBackLoopAuditDeleteQuery = $"delete from feedbackloopaudits where feedbackloopauditid in ({string.Join(',', getRelationFeedbackLoopid.Distinct() ?? new List<int>())})";


                        // xmlParseFk = DataBaseFunctions.GetFKRelationData(getFKRelationDataQuery);
                        DataBaseFunctions.DBDeletion(deleteNodeHistoryQuery);
                        DataBaseFunctions.DBDeletion(xmlParseRunDeleteQuery);
                        DataBaseFunctions.DBDeletion(feedBackLoopAuditDeleteQuery);
                    }
                }
                #endregion

                string[] opco_folderList = files.GetOPCOFolderList(SFTP_path);
                foreach (var opco_folder in opco_folderList)
                {
                    //Getting all the CFTP and MTAS ...etc folders from the Opco folder 
                    string[] folder_list = files.GetFolderList(SFTP_path, opco_folder);

                    foreach (var folder in folder_list)
                    {
                        DateTime lastCheckedTime = DateTime.UtcNow;
                        DateTime? lastExecutedTime = null;
                        DateTime? lastparsedxmlTime = null;

                        string opCo = "", nodeType = "",oem = "";
                        int xmlparserunid = 0, nodeparsehistoryid = 0, feedBackLoopId = 0;                       
                        Dictionary<string,string> data = new Dictionary<string,string>();
                        try
                        {
                            var feedbackLoopstartDate = DateTime.UtcNow;
                            //Getting all xml files from the respective folders(CFTP,MTAS,..etc)
                            fileList = files.GetFileList(opco_folder, folder);
                            feedBackLoopId = DataBaseFunctions.DBInsertion($"insert into feedbackloopaudits (nodetype,filecount,creationuser,creationdate,processstarttime)" +
                                $"values('{folder}','{fileList.Count()}','{user}',SYS_EXTRACT_UTC(systimestamp),TO_TIMESTAMP('{feedbackLoopstartDate:dd-MMM-yyyy HH:mm:ss}','DD-MM-YYYY HH24:MI:SS')) returning feedbackloopauditid into :pkcreated");
                           
                            foreach (var file in fileList)
                            {
                                var fileStartTime = DateTime.UtcNow;
                                
                                try
                                {

                                    Logger.WriteLog(TEMLog.Info, "MainProgram", "Processing File --> " + file);
                                    string xmlfile = Path.GetFileName(file);                                                                       
                                    xmlparserunid = DataBaseFunctions.DBInsertion($"insert into xmlparserun (filename, creationuser, creationdate,fileprocessstarttime,feedbackloopauditid)" +
                                    $" values ('{xmlfile}','{user}',SYS_EXTRACT_UTC(systimestamp),TO_TIMESTAMP('{fileStartTime:dd-MMM-yy HH:mm:ss}','DD-MM-YYYY HH24:MI:SS'),'{feedBackLoopId}') returning xmlparserunid into :pkcreated");
                                    ArrayList nadList = XmlParseFunctions.XmlParse(file);                         
                                    data = XmlDBFunctions.XmlDBInsertUpdate(xmlparserunid, nadList);
                                    lastparsedxmlTime = DateTime.UtcNow;
                                    DataBaseFunctions.DBUpdation($"update xmlparserun set fileprocessendtime = TO_TIMESTAMP('{lastparsedxmlTime:dd-MMM-yy HH:mm:ss}','DD-MM-YYYY HH24:MI:SS') where xmlparserunid = '{xmlparserunid}'");

                                    files.ArchiveFile(file);                               
                                }
                                catch (Exception ex)
                                {
                                    Logger.WriteLog(TEMLog.Error, "MainProgram", "Error Processing File --> " + file);
                                    Logger.WriteLog(TEMLog.Error, "MainProgram", $"{ex.Message}");
                                    files.FailureFile(file);
                                }
                            }
                            if (data.Count > 0)
                            {
                                opCo = data["opco"].ToString().ToUpper();
                                nodeType = data["nodetype"].ToString().ToUpper();
                                oem = data["oem"].ToString().ToUpper();
                                lastExecutedTime = DateTime.UtcNow;                             
                                DataBaseFunctions.DBUpdation($"update feedbackloopaudits set opco = '{opCo}',oem ='{oem}',processendtime = TO_TIMESTAMP('{lastExecutedTime:dd-MMM-yyyy HH:mm:ss}','DD-MM-YYYY HH24:MI:SS') where feedbackloopauditid = '{feedBackLoopId}'");
                            }
                            else
                            {
                                lastExecutedTime = DateTime.UtcNow;
                                DataBaseFunctions.DBUpdation($"update feedbackloopaudits set processendtime = TO_TIMESTAMP('{lastExecutedTime:dd-MMM-yyyy HH:mm:ss}','DD-MM-YYYY HH24:MI:SS') where feedbackloopauditid = '{feedBackLoopId}'");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog(TEMLog.Error, "MainProgram", $"{ex.Message}");
                        }
                    }                    
                    Logger.WriteLog(TEMLog.Info, "MainProgram", "Cleaning Up Files");
                    files.LogDuplicateRemoval();
                    files.CompressFiles("xmlFiles");
                    files.CompressFiles("LogFiles");
                    files.CompressFiles("HelpFiles");                     
                }
                //XmlDBFunctions.AsIsDataSync();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "MainProgram", $"{ex.Message}");
            }
            Logger.WriteLog(TEMLog.Info, "MainProgram", "TEMS Parsing Program ENDS");
            Logger.WriteLog(TEMLog.Info, "MainProgram", "-----------------------------------------------");
        }
    }
}
