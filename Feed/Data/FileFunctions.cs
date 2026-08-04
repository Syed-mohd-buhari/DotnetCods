using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using TEMS.Entity;
using TEMS.Logs;
using static TEMS.Logs.Logger;
using System.Linq;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace TEMS.Data
{

    public class FileFunctions 
    {
        public string separator;

        public FileFunctions()
        {
            if (OperatingSystem.IsLinux())
            {
                separator = "/";
            }
            else
            {
                separator = "\\";
            }
        }

        public string[] GetOPCOFolderList(string folder)
        {
            try
            {



                string[] country_folderList = { };
                if (CommonFunction.getAppConfigValue("Countries_folder") == "*")
                {
                    country_folderList = Directory.GetDirectories(folder).Select(Path.GetFileName).ToArray();
                    country_folderList = Array.FindAll(country_folderList, x => x != "Archive" && x != "FailedXMLs"&& x!= "TEMS_Log");
                }
                else
                {
                    country_folderList = CommonFunction.getAppConfigValue("Countries_folder").Split(';');
                }



                return country_folderList;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GetOPCOFolderList", $"{ex.Message}");
                throw;
            }
        }
        public string[] GetFolderList(string path, string folder)
        {
            try
            {
                //Getting the CFTP and MTAS folder path
                string[] folderList = { };
                if (CommonFunction.getAppConfigValue("SFTP_folders") == "*")
                {
                    folderList = Directory.GetDirectories(path + separator + folder).Select(Path.GetFileName).ToArray();
                    foreach (string country_folder in folderList)
                    {
                        Logger.WriteLog(TEMLog.Info, "GetFolderList", country_folder);
                    }
                }
                else
                {
                    folderList = CommonFunction.getAppConfigValue("SFTP_folders").Split(';');
                }
                return folderList;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GetFolderList", $"{ex.Message}");
                throw;
            }



        }

        //This function is used to get all the Files from the respective opco folders
        public string[] GetFileList(string opco_folder, string folder)
        {
            //Getting the SFTP path from app.config
            string SFTP_path = CommonFunction.getAppConfigValue("SFTP_path");
            string? pattern = "*_" + folder + "_*.xml";
            Logger.WriteLog(TEMLog.Info, "GetFileList", "Inputs --> " + opco_folder + ":" + folder);
            try
            {
                if (Directory.Exists(SFTP_path + opco_folder))
                {
                    //Getting the all xmlm files with the required pattern and stored into the string array
                    string[] arrFiles = Directory.GetFiles(SFTP_path + opco_folder + separator + folder, pattern);
                    //Calling the Validate method
                    ValidateFolders(folder, opco_folder);
                    //checking the files is empty or not
                    if (arrFiles.Length == 0)
                    {
                        Logger.WriteLog(TEMLog.Info, "GetFileList", $"No xml files to be processed in folder --> {folder}");
                    }
                    else
                    {
                        Logger.WriteLog(TEMLog.Info, "GetFileList", String.Join(";", arrFiles));
                    }
                    return arrFiles;
                }
                else
                {
                    return new string[] { };
                }
            }
            catch (FileNotFoundException fe)
            {
                Logger.WriteLog(TEMLog.Error, "GetFileList", $"{fe.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GetFileList", $"{ex.Message}");
                throw;
            }
        }

        //This Function is used to check Whether archieve and failure folder is created or not
        public void ValidateFolders(string folder, string opco_folder)
        {
            try
            {
                //Gettihg the SFTP Path fro app.config
                string SFTP_path = CommonFunction.getAppConfigValue("SFTP_path");
                //Getting the archiveFolderName from the app.config
                string archiveFolderName = CommonFunction.getAppConfigValue("archiveFolderName");
                //Getting the failureFolderName from the app.config
                string failureFolderName = CommonFunction.getAppConfigValue("failureFolderName");
                string? archiveFolder = SFTP_path + separator + archiveFolderName;
                string? failureFolder = SFTP_path + separator + failureFolderName;

                if (!Directory.Exists(archiveFolder))
                {
                    Directory.CreateDirectory(archiveFolder);
                }
                if (!Directory.Exists(failureFolder))
                {
                    Directory.CreateDirectory(failureFolder);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "ValidateFolders", $"{ex.Message}");
                throw;
            }
        }

        //This function is for the Xml files That are Successed in parsing is automatically stored into Archive folder
        public void ArchiveFile(string fileName)
        {
            try
            {
                //Getting the each Xml filename
                string xmlname = Path.GetFileName(fileName);
                string SFTP_path = CommonFunction.getAppConfigValue("SFTP_path");
                string archiveFolderName = CommonFunction.getAppConfigValue("archiveFolderName");
                string archFilePath = SFTP_path + separator + archiveFolderName;
                string archiveFileFolder = Path.Combine(archFilePath, xmlname);
                // The Success Xml file moved to the Archive folder
                try
                {
                    File.Move(fileName, archiveFileFolder);
                    Logger.WriteLog(TEMLog.Info, "ArchiveFile", $"{fileName}Processed and Archived");
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Cannot create a file when that file already exists"))
                    {
                        string _xmlName = Path.GetFileNameWithoutExtension(fileName);
                        string filedate = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string pattern = @"(\d{4})(\d{2})(\d{2})_(\d{2})(\d{2})(\d{2})";
                        string _filename = Regex.Replace(_xmlName, pattern, filedate) + ".xml";
                        string _archiveFileFolder = Path.Combine(archFilePath, _filename);

                        File.Move(fileName, _archiveFileFolder);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "ArchiveFile", $"{ex.Message}");
                throw;
            }
        }

        //This function is for the Xml files That are failed in parsing is automatically stored into Failure folder
        public void FailureFile(string fileName)
        {
            try
            {
                //Getting the each failed xmlfile name
                string? xmlName = Path.GetFileName(fileName);
                //string? folderName = Path.GetDirectoryName(fileName);
                string SFTP_path = CommonFunction.getAppConfigValue("SFTP_path");
                string failureFolderName = CommonFunction.getAppConfigValue("failureFolderName");
                string failFilePath = Path.Combine(SFTP_path, failureFolderName);
                string failureFolder = Path.Combine(failFilePath, xmlName);
                // Move the failed XML file to the failure folder
                File.Move(fileName, failureFolder);
                Logger.WriteLog(TEMLog.Error, "FailureFile", $"Error in Parsing Function{fileName}");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "FailureFile", $"{ex.Message}");
                throw;
            }
        }

        /*
      * Module Name : RemoveTextFromFile
      * Description : it is used to remove (as_is) name
      * Parameter : xmlFile, keyword
      * xmlFile - it is string argumant it is use to store the xml file path
      * keyword - this is pass a (as_is) name
      */
        public static void RemoveTextFromFile(string xmlFile, string keyword)
        {

            string xml_reader = File.ReadAllText(xmlFile);
            try
            {
                if (xml_reader.Contains(keyword))
                {
                    string change = xml_reader.Replace(keyword, "");
                    File.WriteAllText(xmlFile, change);
                    Logger.WriteLog(TEMLog.Info, "RemoveTextFromFile", "Remove keyword " + keyword + " from file " + xmlFile);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "RemoveTextFromFile", $"{ex.Message}");
                throw;
            }

        }

        public void RemoveDuplicateLinesFromFile(string xmlFile)
        {
            try
            {
                if (File.Exists(xmlFile))
                {
                    string[] lines = File.ReadAllLines(xmlFile);
                    File.WriteAllLines(xmlFile, lines.Distinct().ToArray());
                    var contents = File.ReadAllLines(xmlFile);
                    Array.Sort(contents);
                    File.WriteAllLines(xmlFile, contents);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "RemoveDuplicateLinesFromFile", $"{ex.Message}");
                throw;
            }

        }

        public void LogDuplicateRemoval()
        {
            try
            {
                string SFTP_path = CommonFunction.getAppConfigValue("SFTP_path");
                string logFolder = CommonFunction.getAppConfigValue("logFolder");
                string dbMapFileName = SFTP_path + logFolder + separator + DateTime.Now.ToString("yyyyMMdd") + ".dbmaphelp";
                string dbTableFileName = SFTP_path + logFolder + separator + DateTime.Now.ToString("yyyyMMdd") + ".dbtablehelp";
                string duplicate = SFTP_path + logFolder + separator + "duplicate.txt";
                RemoveDuplicateLinesFromFile(dbMapFileName);
                RemoveDuplicateLinesFromFile(dbTableFileName);
                RemoveDuplicateLinesFromFile(duplicate);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "RemoveDuplicateLinesFromFile", $"{ex.Message}");
                throw;
            }

        }

        public void CompressFiles(string fileType)
        {
            string SFTP_path = CommonFunction.getAppConfigValue("SFTP_path");
            int ArchiveXMLDays = Convert.ToInt32(CommonFunction.getAppConfigValue("ArchiveXMLDays"));
            try
            {
                string[] archiveFiles = "".Split("'");
                string archiveFolderName ="";
                string zipFilename = "";
                if (fileType.Equals("xmlFiles"))
                {
                    archiveFolderName = SFTP_path + CommonFunction.getAppConfigValue("archiveFolderName");
                    archiveFiles = Directory.GetFiles(archiveFolderName, "*.xml");
                    zipFilename = SFTP_path + DateTime.Now.ToString("yyyyMMdd") + "_XML";
                }
                else if (fileType.Equals("LogFiles"))
                {
                    archiveFolderName = SFTP_path + CommonFunction.getAppConfigValue("logFolder");
                    List<string> temp = Directory.GetFiles(archiveFolderName, "*.log").ToList();
                    temp.AddRange(Directory.GetFiles(archiveFolderName, "*.error").ToList());
                    archiveFiles = temp.ToArray();
                    zipFilename = SFTP_path + DateTime.Now.ToString("yyyyMMdd") + "_log";
                }
                else if (fileType.Equals("HelpFiles"))
                {
                    archiveFolderName = SFTP_path + CommonFunction.getAppConfigValue("logFolder");
                    List<string> temp = Directory.GetFiles(archiveFolderName, "*.dbmaphelp").ToList();
                    temp.AddRange(Directory.GetFiles(archiveFolderName, "*.dbtablehelp").ToList());
                    archiveFiles = temp.ToArray();                    
                    zipFilename = SFTP_path + DateTime.Now.ToString("yyyyMMdd") + "_help";
                }
                else
                {
                    Logger.WriteLog(TEMLog.Error, "ArchiveFiles", "Unknown Archive Type");
                    return;
                }
                
                string tempFolderPath = Path.Combine(SFTP_path, "TempFolder");
                string zipFilePath = "";
                Directory.CreateDirectory(tempFolderPath);
                foreach (string xmlFile in archiveFiles)
                {
                    DateTime lastWriteTime = File.GetLastWriteTime(xmlFile);
                    if (DateTime.Now - lastWriteTime > TimeSpan.FromDays(ArchiveXMLDays))
                    {
                        string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(xmlFile));
                        File.Move(xmlFile, tempFilePath);
                    }
                }                
                if (Directory.GetFiles(tempFolderPath).Length > 0)
                {
                    if (OperatingSystem.IsLinux())
                    {
                        zipFilePath = Path.Combine(SFTP_path, zipFilename + ".rar");
                    }
                    else
                    {
                        zipFilePath = Path.Combine(SFTP_path, zipFilename + ".zip");
                    }
                    if (File.Exists(zipFilePath))
                    {
                        using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Update))
                        {
                            foreach (var file in Directory.GetFiles(tempFolderPath))
                            {
                                var fileInfo = new FileInfo(file);
                                zipArchive.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name);
                            }
                        }
                    }
                    else
                    {
                        ZipFile.CreateFromDirectory(tempFolderPath, zipFilePath);
                    }
                }
                else
                {
                    //no files to archive
                }

                Directory.Delete(tempFolderPath, true);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "ArchiveZip", ex.Message);
            }
        }
    }
}
