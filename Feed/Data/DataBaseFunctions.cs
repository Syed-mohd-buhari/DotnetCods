using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using TEMS.Logs;
using static TEMS.Logs.Logger;
using TEMS.Entity;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using String = System.String;
using System.Security.Cryptography;
using System;
using System.Transactions;

namespace TEMS.Data
{
    public class DataBaseFunctions
    {
        public static string returnID = "pkcreater";
        private static string GetConnectionString()
        {
            string conStr = ConfigurationManager.ConnectionStrings["oraclecon"].ConnectionString;
            return conStr;
        }
        public static OracleConnection GetConnection()
        {
            string conStr = GetConnectionString();
            OracleConnection connection = new OracleConnection(conStr);
            connection.Open();
            return connection;
        }

        public static int DBInsertion(string insertQuery)
        {
            
            OracleConnection connection = GetConnection();          
            int primaryKey = 0;
            try
            {
                OracleCommand command = new OracleCommand(insertQuery, connection);
                Logger.WriteLog(TEMLog.Debug, "DBInsertion", insertQuery);
                command.Parameters.Add(new OracleParameter(returnID, OracleDbType.Int64, ParameterDirection.ReturnValue));
                int num = command.ExecuteNonQuery();
                primaryKey = Convert.ToInt32(command.Parameters[returnID].Value.ToString());
                Logger.WriteLog(TEMLog.Debug, "DBInsertion", $"{num} rows inserted");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "DBInsertion", $"{ex.Message} : {insertQuery}");                
                throw;
            }
            finally
            {
                connection.Close();
            }
            return primaryKey;
        }

        public static int DBInsertion(OracleConnection connection, string insertQuery)
        {
           
            int primaryKey=0;
            try
            {
                OracleCommand command = new OracleCommand(insertQuery, connection);
                Logger.WriteLog(TEMLog.Debug, "DBInsertion", insertQuery);
                command.Parameters.Add(new OracleParameter(returnID, OracleDbType.Int64, ParameterDirection.ReturnValue));
                int num = command.ExecuteNonQuery();
                primaryKey =Convert.ToInt32(command.Parameters[returnID].Value.ToString());
                Logger.WriteLog(TEMLog.Debug, "DBInsertion", $"{num} rows inserted");     
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "DBInsertion", $"{ex.Message} : {insertQuery}");
                if( ex.Message.ToUpper().Contains ("ORA-00904"))
                {
                    int pFrom = ex.Message.IndexOf("\"");
                    int pTo = ex.Message.LastIndexOf("\"");
                    String columnname = ex.Message.Substring(pFrom + 1, pTo - pFrom -1);
                    pFrom = insertQuery.ToUpper().IndexOf("INSERT INTO ");
                    pTo = insertQuery.IndexOf(" (");
                    String tablename = insertQuery.Substring(pFrom+12, pTo-12);
                    Logger.WriteLog(TEMLog.DBTableHelp, "DBInsertion", $"ALTER TABLE {tablename} ADD {columnname.ToLower()} nvarchar2(50);");
                    Logger.WriteLog(TEMLog.DBMappingHelp, "getColumnNamesFromConfig", $"{tablename};{columnname.ToLower()};false;{tablename};{columnname.ToLower()};50");
                }
                throw;
            }            
            return primaryKey;
        }

        public static int DBUpdation(string updateQuery)
        {
            int rowsaffected = 0;
            OracleConnection connection = GetConnection();           
            try
            {
                rowsaffected = DBUpdation(connection, updateQuery);
            }
            catch
            {
                throw;
            }
            return rowsaffected;
        }

        public static int DBUpdation(OracleConnection connection, string updateQuery)
        {
            int rows_affected = 0;
            try
            {
                OracleCommand command = new OracleCommand(updateQuery, connection);
                Logger.WriteLog(TEMLog.Debug, "DBUpdation", updateQuery);
                rows_affected = command.ExecuteNonQuery();
                Logger.WriteLog(TEMLog.Debug, "DBUpdation", $"{rows_affected} rows affected");
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "DBUpdation", $"{ex.Message} : {updateQuery}");
                throw;
            }
            return rows_affected;
        }

        public static List<int> GetFKRelationData(string query)
        {
            List<int> xmlparserunIds = new List<int>();
            OracleConnection connection = GetConnection();
            var transaction = connection.BeginTransaction();
            try
            {
                OracleCommand command = new OracleCommand(query, connection);
                Logger.WriteLog(TEMLog.Debug, "GetFKRelationData", query);
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        int xmlparserunId = reader.GetInt32(0); // Assuming XMLPARSERUNID is in the first column
                        xmlparserunIds.Add(xmlparserunId);
                    }
                }
                Logger.WriteLog(TEMLog.Debug, "GetFKRelationData", $"Retrieved {xmlparserunIds.Count} XMLPARSERUNID values.");
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "GetFKRelationData", $"{ex.Message} : {query}");
                throw;
            }
            finally
            {
                connection.Close();
            }
            return xmlparserunIds;
        }

        public static int DBDeletion(string deleteQuery)
        {

            OracleConnection connection = GetConnection();
            var transaction = connection.BeginTransaction();
            int primaryKey = 0;
            try
            {
                OracleCommand command = new OracleCommand(deleteQuery, connection);
                Logger.WriteLog(TEMLog.Debug, "DBDeletion", deleteQuery);
                command.Parameters.Add(new OracleParameter(returnID, OracleDbType.Int64, ParameterDirection.ReturnValue));
                int num = command.ExecuteNonQuery();
                //primaryKey = Convert.ToInt32(command.Parameters[returnID].Value.ToString());
                Logger.WriteLog(TEMLog.Debug, "DBDeletion", $" '{num}' records deleted successfully ");
                transaction.Commit();               
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, "DBDeletion", $"{ex.Message} : {deleteQuery}");
                throw;
            }
            finally
            {
                connection.Close();
            }
            return primaryKey;
        }

        public static void DBDeletion(List<int> xmlParseIds)
        {
            int count = 0;
            foreach (var id in xmlParseIds)
            {
                string deleteQuery = $"delete from xmlparserun where xmlparserunid = '{id}'";
               
                OracleConnection connection = GetConnection();
                var transaction = connection.BeginTransaction();
                int primaryKey = 0;
                try
                {
                    OracleCommand command = new OracleCommand(deleteQuery, connection);
                    Logger.WriteLog(TEMLog.Debug, "DBDeletion", deleteQuery);
                    command.Parameters.Add(new OracleParameter(returnID, OracleDbType.Int64, ParameterDirection.ReturnValue));
                    int num = command.ExecuteNonQuery();
                    //primaryKey = Convert.ToInt32(command.Parameters[returnID].Value.ToString());
                    count++;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(TEMLog.Error, "DBDeletion", $"{ex.Message} : {deleteQuery}");
                    throw;
                }
                finally
                {
                    connection.Close();
                }
                
            }
            Logger.WriteLog(TEMLog.Debug, "DBDeletion", $"{count} rows Deleted");
        }

        public static int GetRowCount(string selectQuery)

        {
            OracleConnection connection = GetConnection();

            var rowCount = 0;
            try
            {
                OracleCommand command = new OracleCommand(selectQuery, connection);
                Logger.WriteLog(TEMLog.Debug, "GetRowCount", selectQuery);
                rowCount = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, $"{ex.Message}", "GetRowCount");
                throw;
            }
            return rowCount;
        }

        public static Dictionary<string, string> selectFromDB(string tablename, string rowPKvalue)
        {
            string selectQuery = $"select * from {tablename} where {DBModelFunctions.GetPrimaryKey(tablename)} = '{rowPKvalue}'";
            Dictionary<string, string> row = new Dictionary<string, string>();
            OracleConnection connection = GetConnection();
            var transaction = connection.BeginTransaction();
            try
            {
                OracleCommand command = new OracleCommand(selectQuery, connection);
                OracleDataReader reader = command.ExecuteReader();
                string key = "";
                int rowCount = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        row = new Dictionary<string, string>();
                        for (int counter = 0; counter < reader.FieldCount; counter++)
                        {
                            key = reader.GetName(counter).ToLower();
                            if ((tablename == "networkelement" && key.ToLower() == "xmllastparsefiledate") || (tablename == "networkelement" && key.ToLower() == "dataacquisitiondate")
                            || (tablename == "networkelement" && key.ToLower() == "softwareinstalldate") || (tablename == "networkelement" && key.ToLower() == "softwareinstalldateap")
                            || (tablename == "networkelement" && key.ToLower() == "softwareinstalldatecp") || (tablename == "networkelement" && key.ToLower() == "softwareproductdate")
                            || (tablename == "networkelement" && key.ToLower() == "softwareproductdateap") || (tablename == "networkelement" && key.ToLower() == "softwareproductdatecp")
                            || (tablename == "component" && key.ToLower() == "productiondate"))
                            {

                                row.Add(key, reader.IsDBNull(counter) ? "" : CommonFunction.ConvertDBDate(reader.GetString(counter).ToString()));
                            }
                            else if (key != DBModelFunctions.GetPrimaryKey(tablename) &&
                               key != DBModelFunctions.GetForeignyKey(tablename) &&
                               key != "creationuser" && key != "creationdate" &&
                               key != "modificationuser" && key != "modificationdate"&&
                               key != "deleted" && key != "deletiondate")
                            {
                                row.Add(key, reader.IsDBNull(counter) ? "" : reader.GetString(counter).ToString());
                            }                            
                        }
                        rowCount++;
                    }
                }
                if(rowCount == 0)
                {
                    //No rows returned
                }
                else if(rowCount > 1)
                {
                    //more rows returned
                }

                reader.Close();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, $"{ex.Message}", "DbReader");
                throw;
            }
            finally
            {
                if(connection != null)
                    connection.Close();
            }
           // row = DBModelFunctions.updateColumnName(tablename, row);
            return row;
        }

        public static List<Dictionary<string, string>> selectFromDBByQuery(string selectQuery)
        {
            List<Dictionary<string, string>> rowList = new List<Dictionary<String, String>>();
            Dictionary<string, string> row = new Dictionary<string, string>();
            OracleConnection connection = GetConnection();
            try
            {
                OracleCommand command = new OracleCommand(selectQuery, connection);
                OracleDataReader reader = command.ExecuteReader();
                string key = "";
                int rowCount = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        row = new Dictionary<string, string>();
                        for (int counter = 0; counter < reader.FieldCount; counter++)
                        {
                            key = reader.GetName(counter).ToLower();
                            row.Add(key, reader.IsDBNull(counter) ? "" : reader.GetString(counter).ToString());
                        }
                        rowList.Add(row);
                        rowCount++;
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, $"{ex.Message}", "DbReader");
                throw;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            // row = DBModelFunctions.updateColumnName(tablename, row);
            return rowList;
        }

        public static string GetPrimaryKey(string tablename, string whereClause)
        {

            string selectQuery = $"select {DBModelFunctions.GetPrimaryKey(tablename)} from {tablename} where {whereClause}";
            string pk_id = "";
            OracleConnection connection = GetConnection();
            try
            {
                OracleCommand command = new OracleCommand(selectQuery, connection);
                OracleDataReader reader = command.ExecuteReader();
                int rowCount = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        pk_id = reader.GetString(0);
                    }
                    rowCount++;
                }
                if (rowCount == 0)
                {
                   pk_id="NA";
                }
                else if (rowCount > 1)
                {
                    Logger.WriteLog(TEMLog.Error, "GetPrimaryKey "," Found more records");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, $"{ex.Message}", "GetPrimaryKey");
                throw;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return pk_id;
        }

        public static string GetAttributesValue(string tablename, string whereClause)
        {
            string selectFields = DBModelFunctions.GetAttributesValue(tablename);

            if(tablename == "identitycategory")
            {
                tablename = "identitiesasis";
            }

            string selectQuery = $"select {selectFields} from {tablename} where {whereClause}";
            string Rkey = "";
            OracleConnection connection = GetConnection();
            try
            {
                OracleCommand command = new OracleCommand(selectQuery, connection);
                OracleDataReader reader = command.ExecuteReader();
                int rowCount = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if(!reader.IsDBNull(0))
                        {
                            Rkey = reader.GetString(0);
                        }
                        
                    }
                    rowCount++;
                }
                if (rowCount == 0)
                {
                    Rkey = "NA";
                }
                else if (rowCount > 1)
                {
                    Logger.WriteLog(TEMLog.Error, "GetResourceKey ", " Found more records");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, $"{ex.Message}", "GetResourceKey");
                throw;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return Rkey;
        }

        public static List<Dictionary<string, string>> getUpdateTableValues()
        {
            string selectQuery = $"select * from RAWDATAVALUEMAPPING";
            Dictionary<string, string> row = new Dictionary<string, string>();
            List<Dictionary<string, string>> rowList = new List<Dictionary<String, String>>();
            OracleConnection connection = GetConnection();
            try
            {
                OracleCommand command = new OracleCommand(selectQuery, connection);
                OracleDataReader reader = command.ExecuteReader();
                string key = "";
                int rowCount = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        row = new Dictionary<string, string>();
                        for (int counter = 0; counter < reader.FieldCount; counter++)
                        {
                            key = reader.GetName(counter).ToLower();
                            row.Add(key, reader.IsDBNull(counter) ? "" : reader.GetString(counter).ToString());                            
                        }
                        rowCount++;
                        rowList.Add(row);
                    }
                   
                }
                if (rowCount == 0)
                {
                    //No rows returned
                }
                else if (rowCount > 1)
                {
                    //more rows returned
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, $"{ex.Message}", "DbReader");
                throw;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            // row = DBModelFunctions.updateColumnName(tablename, row);
            return rowList;
        }

        public static List<string> ExecuteStoredProcedure(string procedureName, Dictionary<string,string> parameters)
        {
            List<string> result = new List<string>();
            int outpitLines = 0;
            OracleConnection connection = GetConnection();
            try
            {
                OracleCommand command = new OracleCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;
                Logger.WriteLog(TEMLog.Info, "ExecuteStoredProcedure", $"{procedureName}");
                if (!procedureName.Equals("DBMS_OUTPUT.ENABLE"))
                    command.Parameters.Add("userID", OracleDbType.Int16).Value = 1;
                foreach (var param in parameters)
                    command.Parameters.Add(param.Key, OracleDbType.Varchar2).Value = param.Value;
                command.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                Logger.WriteLog(TEMLog.Error, $"{ex.Message}", "DbReader");
                throw;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            // row = DBModelFunctions.updateColumnName(tablename, row);
            return result;
        }
    }
}
