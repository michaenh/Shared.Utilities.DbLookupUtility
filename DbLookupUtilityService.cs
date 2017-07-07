using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client; 

namespace Shared.Utilities.DbLookupUtility
{
    public class DbLookupUtilityService
    {
        string _connectionString;

        public XPathNavigator LookupByQuery(string query, string connectionStringKey)
        {
            try
            {
                query = Regex.Replace(query, "&gt;", ">");
                query = Regex.Replace(query, "&lt;", "<");

                System.Diagnostics.Trace.Write(string.Format("Entering DbLookupUtilityService with query {0} and connection key {1}", query, connectionStringKey));

                _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;

                XElement resultElement = new XElement("Result");

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader;

                        connection.Open();

                        using (reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    resultElement.Add(new XElement(reader.GetName(i), reader.GetValue(i)));                                    
                                }
                            }
                        }

                        connection.Close();
                    }
                }

                System.Diagnostics.Trace.Write(string.Format("Exiting DbLookupUtilityService with element {0}", resultElement.Value));
                return resultElement.CreateNavigator();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(string.Format("Exception in DbLookupUtilityService {0}", ex.Message));
                throw;
            }
        }
        
        
        public XPathNavigator OraLookupByQuery(string query, string connectionStringKey)
                {
                    try
                    {

                        query = Regex.Replace(query, "&gt;", ">");
                        query = Regex.Replace(query, "&lt;", "<");

                        System.Diagnostics.Trace.Write(string.Format("Entering DbLookupUtilityService with query {0} and connection key {1}", query, connectionStringKey));
                        //_connectionString = "User Id=System;Password=2387Visa;Data Source=(DESCRIPTION =(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST = BTS2013R2DEV)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XE)))";
                        _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;

                
                        XElement resultElement = new XElement("Result");

                        using (OracleConnection connection = new OracleConnection(_connectionString))
                        {

                            using (OracleCommand command = new OracleCommand(query, connection))
                            {
                                OracleDataReader reader;

                                connection.Open();

                                using (reader = command.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        reader.Read();

                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            resultElement.Add(new XElement(reader.GetName(i), reader.GetValue(i)));                                            
                                        }
                                    }
                                }

                                connection.Close();
                                //connection.Dispose();
                            }
                        }        

                        //resultElement.Add(new XElement("test", _connectionString));
                        System.Diagnostics.Trace.Write(string.Format("Exiting DbLookupUtilityService with element {0}", resultElement.Value));
                        return resultElement.CreateNavigator();
                    }
                

                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.Write(string.Format("Exception in DbLookupUtilityService {0}", ex.Message));
                        throw;
                    }
                }           

    } 
}