using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CaseDiary.Service
{
    public class MockCaseDiaryRepository : ICaseDiaryRepository
    {

        private IConfiguration configuration;
        // List<modelspparam> param { get; set; }
        public string ConnectionString { get; set; }
        List<modelspparam> param { get; set; }
        public MySqlCommand propcommand { get; set; }
        public MockCaseDiaryRepository(IConfiguration iConfig)
        {
            configuration = iConfig;
            ConnectionString = configuration.GetConnectionString("ApiConnection");
            //   ConnectionString = configuration.GetSection("MySettings:DbConnection").Value;
            //ConnectionString = @"server=localhost;port=3306;database=niiast;user=root;password=123456";
        }
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public List<modelspparam> GetSpParam(string dbname, string spname)
        {
            MySqlCommand cmd = new MySqlCommand();
            List<modelspparam> paramlist = new List<modelspparam>();
            MySqlParameter[] pms = new MySqlParameter[3];
            pms[0] = new MySqlParameter("dbname", MySqlDbType.VarChar);
            pms[0].Value = dbname;
            pms[1] = new MySqlParameter("spname", MySqlDbType.VarChar);
            pms[1].Value = spname;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_spparamnamelist";
            cmd.Parameters.Add(pms[0]);
            cmd.Parameters.Add(pms[1]);
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                cmd.Connection = conn;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        paramlist.Add(new modelspparam()
                        {
                            paramname = reader["parametername"].ToString(),
                            datatype = reader["datatype"].ToString()
                            //size = Convert.ToInt32(reader["size"])

                        });
                    }

                }

            }
            return paramlist;
        }
        public Dictionary<int, string> GetDropdownlistValsFromDatabase(string argmaincolumnname, string argtablename, int? argmaincolumnvalue)
        {
            MySqlCommand cmd = new MySqlCommand();
            Dictionary<int, string> listdrpvalues = new Dictionary<int, string>();
            MySqlParameter[] pms = new MySqlParameter[3];
            pms[0] = new MySqlParameter("argmaincolumnname", MySqlDbType.VarChar);
            pms[0].Value = argmaincolumnname;
            pms[1] = new MySqlParameter("argmaincolumnvalue", MySqlDbType.VarChar);
            pms[1].Value = argmaincolumnvalue;
            pms[2] = new MySqlParameter("argtablename", MySqlDbType.VarChar);
            pms[2].Value = argtablename;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getlookupvalues";
            cmd.Parameters.Add(pms[0]);
            cmd.Parameters.Add(pms[1]);
            cmd.Parameters.Add(pms[2]);
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                //if (columnname == "doctorname")
                //{
                //    cmd = new MySqlCommand("select id, doctorname from tblmasterdoctor", conn);
                //}
                cmd.Connection = conn;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (argtablename.Contains("sub"))
                        {
                            listdrpvalues.Add(Convert.ToInt32(reader[0]), reader[2].ToString());
                        }
                        else
                        {
                            listdrpvalues.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
                        }
                    }
                }
            }

            return listdrpvalues;
        }

        public List<T> getpatientdetails<T>(ref T mypatient, string dbname, string spname, params object[] optionalargs)
        {
            param = GetSpParam(dbname, spname);
            Type myType = mypatient.GetType();
            List<T> instancelist = new List<T>();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            //string m = mypatient.;
            int paramcounter = 0;
            modelspparam maani = new modelspparam();
            MySqlParameter[] pms = new MySqlParameter[param.Count];
            propcommand = new MySqlCommand();
            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(mypatient, null);


                // Do something with propValue

                maani = param.Where(x => x.paramname == "arg" + prop.Name).SingleOrDefault();
                if (maani != null)
                {
                    pms[paramcounter] = new MySqlParameter();
                    if (maani.datatype.ToUpper() == "VARCHAR")
                    {
                        pms[paramcounter] = new MySqlParameter(maani.paramname.Trim(), MySqlDbType.VarChar);
                    }
                    else if (maani.datatype.ToUpper() == "INT")
                    {
                        pms[paramcounter] = new MySqlParameter(maani.paramname, MySqlDbType.Int32);
                    }
                    pms[paramcounter].Value = propValue;
                    propcommand.Parameters.Add(pms[paramcounter]);
                    paramcounter = paramcounter + 1;
                }
            }
            propcommand.CommandType = CommandType.StoredProcedure;
            propcommand.CommandText = spname;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                propcommand.Connection = conn;
                using (var reader = propcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //paramlist.Add(new modelspparam()
                        //{
                        //    paramname = reader["parametername"].ToString(),
                        //    datatype = reader["datatype"].ToString(),
                        //    size = Convert.ToInt32(reader["size"])
                        //});
                        instance = Activator.CreateInstance<T>();
                        foreach (PropertyInfo prop in props)
                        {
                            for (int ct = 0; ct <= reader.FieldCount - 1; ct++)
                            {
                                if (reader.GetName(ct) == prop.Name)
                                {
                                    if (prop.PropertyType.Name == "Int32")
                                        myType.GetProperty(prop.Name).SetValue(instance, Convert.ToInt32(reader[prop.Name]));
                                    if (prop.PropertyType.Name == "String")
                                        myType.GetProperty(prop.Name).SetValue(instance, reader[prop.Name].ToString());
                                    if (prop.PropertyType.Name.ToUpper() == "BOOLEAN")
                                    {
                                        if (reader[prop.Name].ToString() == "1")
                                        {
                                            myType.GetProperty(prop.Name).SetValue(instance, true);
                                        }

                                        else
                                        {
                                            myType.GetProperty(prop.Name).SetValue(instance, false);
                                        }
                                    }

                                }
                            }
                        }
                        instancelist.Add(instance);
                    }
                }
            }
            return instancelist;
        }

        public void InsertItems<T>(ref T mypatient, string dbname, string spname)
        {
            T temp = default(T);

            param = GetSpParam(dbname, spname);
            Type myType = mypatient.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            //string m = mypatient.;
            int paramcounter = 0;
            modelspparam maani = new modelspparam();
            MySqlParameter[] pms = new MySqlParameter[param.Count];
            propcommand = new MySqlCommand();
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(mypatient, null);


                // Do something with propValue
                if (prop.Name == "Id")
                {
                    maani = param.Where(x => x.paramname == "arg" + prop.Name).SingleOrDefault();
                }
                else if (prop.Name == "ipno")
                {
                    maani = param.Where(x => x.paramname == "arg" + prop.Name).SingleOrDefault();
                }
                else
                {
                    maani = param.Where(x => x.paramname == "arg" + prop.Name).SingleOrDefault();
                }
                if (maani != null)
                {
                    pms[paramcounter] = new MySqlParameter();
                    if (maani.datatype.ToUpper() == "VARCHAR")
                    {
                        pms[paramcounter] = new MySqlParameter(maani.paramname.Trim(), MySqlDbType.VarChar);
                    }
                    else if (maani.datatype.ToUpper() == "INT")
                    {
                        pms[paramcounter] = new MySqlParameter(maani.paramname, MySqlDbType.Int32);
                    }

                    else if (maani.datatype.ToUpper() == "TINYINT")
                    {
                        pms[paramcounter] = new MySqlParameter(maani.paramname, MySqlDbType.Byte);
                    }

                    else if (maani.datatype.ToUpper() == "FLOAT")
                    {
                        pms[paramcounter] = new MySqlParameter(maani.paramname, MySqlDbType.Float);
                    }
                    else if (maani.datatype.ToUpper() == "DATE")
                    {
                        pms[paramcounter] = new MySqlParameter(maani.paramname, MySqlDbType.Date);
                    }

                    else if (maani.datatype.ToUpper() == "DECIMAL")
                    {
                        pms[paramcounter] = new MySqlParameter(maani.paramname, MySqlDbType.Decimal);
                    }
                    pms[paramcounter].Value = propValue;
                    propcommand.Parameters.Add(pms[paramcounter]);
                    paramcounter = paramcounter + 1;
                }
            }
            propcommand.CommandType = CommandType.StoredProcedure;
            propcommand.CommandText = spname;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                propcommand.Connection = conn;
                propcommand.ExecuteNonQuery();
            }
        }

        public List<V> getmainitemdetails<T, V>(ref T mygenericsearchtype, ref V mygenericreturntype, string dbname, string spname, params object[] optionalargs)
        {
            param = GetSpParam(dbname, spname);
            //Type myType = mygeneric.GetType();
            //Type myType = mygenericreturntype.GetType();
            Type mySearchType = mygenericsearchtype.GetType();
            Type myReturnType = mygenericreturntype.GetType();
            List<V> instancelist = new List<V>();
            IList<PropertyInfo> searchprops = new List<PropertyInfo>(mySearchType.GetProperties());
            IList<PropertyInfo> returnprops = new List<PropertyInfo>(myReturnType.GetProperties());
            //string m = mypatient.;
            int paramcounter = 0;
            modelspparam maani = new modelspparam();
            MySqlParameter[] pms = new MySqlParameter[param.Count];
            propcommand = new MySqlCommand();

            V instance = Activator.CreateInstance<V>();

            foreach (PropertyInfo prop in searchprops)
            {
                object propValue = prop.GetValue(mygenericsearchtype, null);


                // Do something with propValue

                maani = param.Where(x => x.paramname == "arg" + prop.Name).SingleOrDefault();
                if (maani != null)
                {
                    pms[paramcounter] = new MySqlParameter();
                    if (maani.datatype.ToUpper() == "VARCHAR")
                    {
                        pms[paramcounter] = new MySqlParameter(maani.paramname.Trim(), MySqlDbType.VarChar);
                    }
                    else if (maani.datatype.ToUpper() == "INT")
                    {

                        pms[paramcounter] = new MySqlParameter(maani.paramname, MySqlDbType.Int32);
                    }
                    else if (maani.datatype.ToUpper() == "DATE")
                    {

                        pms[paramcounter] = new MySqlParameter(maani.paramname, MySqlDbType.DateTime);
                    }
                    else if (maani.datatype.ToUpper() == "DATETIME")
                    {

                        pms[paramcounter] = new MySqlParameter(maani.paramname, MySqlDbType.DateTime);
                    }

                    else if (maani.datatype.ToUpper() == "DECIMAL")
                    {

                        pms[paramcounter] = new MySqlParameter(maani.paramname, MySqlDbType.Decimal);
                    }
                    if (maani.paramname == "argCaseId")
                    {
                        pms[paramcounter].Value = optionalargs[0];
                    }
                    else
                    {
                        pms[paramcounter].Value = propValue;
                    }

                    propcommand.Parameters.Add(pms[paramcounter]);
                    paramcounter = paramcounter + 1;
                }
            }
            propcommand.CommandType = CommandType.StoredProcedure;
            propcommand.CommandText = spname;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                propcommand.Connection = conn;
                using (var reader = propcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //paramlist.Add(new modelspparam()
                        //{
                        //    paramname = reader["parametername"].ToString(),
                        //    datatype = reader["datatype"].ToString(),
                        //    size = Convert.ToInt32(reader["size"])
                        //});
                        instance = Activator.CreateInstance<V>();
                        foreach (PropertyInfo prop in returnprops)
                        {
                            for (int ct = 0; ct <= reader.FieldCount - 1; ct++)
                            {
                                if (reader.GetName(ct) == prop.Name)
                                {
                                    if (prop.PropertyType.Name == "Int32")
                                        myReturnType.GetProperty(prop.Name).SetValue(instance, Convert.ToInt32(reader[prop.Name]));
                                    if (prop.PropertyType.Name == "String")
                                        myReturnType.GetProperty(prop.Name).SetValue(instance, reader[prop.Name].ToString());
                                    if (prop.PropertyType.Name == "DateTime")
                                        myReturnType.GetProperty(prop.Name).SetValue(instance, Convert.ToDateTime(reader[prop.Name].ToString()));
                                    if (prop.PropertyType.Name == "Decimal")
                                        myReturnType.GetProperty(prop.Name).SetValue(instance, Convert.ToDecimal(reader[prop.Name] is DBNull ? 0 : reader[prop.Name]));
                                    if (prop.PropertyType.Name.ToUpper() == "BOOLEAN")
                                    {
                                        if (reader[prop.Name].ToString() == "1")
                                        {
                                            myReturnType.GetProperty(prop.Name).SetValue(instance, true);
                                        }

                                        else
                                        {
                                            myReturnType.GetProperty(prop.Name).SetValue(instance, false);
                                        }
                                    }

                                }
                            }
                        }
                        instancelist.Add(instance);
                    }
                }
            }
            return instancelist;
        }

        public Dictionary<int, string> GetDropdownlistValsFromDatabase(string relationname, int argmaincolumnvalue)
        {
            MySqlCommand cmd = new MySqlCommand();
            Dictionary<int, string> listdrpvalues = new Dictionary<int, string>();
            MySqlParameter[] pms = new MySqlParameter[3];
            pms[0] = new MySqlParameter("argRelationName", MySqlDbType.VarChar);
            pms[0].Value = relationname;
            pms[1] = new MySqlParameter("argStudentId", MySqlDbType.Int32);
            pms[1].Value = argmaincolumnvalue;
            //pms[2] = new MySqlParameter("argtablename", MySqlDbType.VarChar);
            //pms[2].Value = argtablename;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetAllCoursesForEntities";
            cmd.Parameters.Add(pms[0]);
            cmd.Parameters.Add(pms[1]);
            //cmd.Parameters.Add(pms[2]);
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                //if (columnname == "doctorname")
                //{
                //    cmd = new MySqlCommand("select id, doctorname from tblmasterdoctor", conn);
                //}
                cmd.Connection = conn;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //if (argtablename.Contains("sub"))
                        //{
                        //    listdrpvalues.Add(Convert.ToInt32(reader[0]), reader[2].ToString());
                        //}
                        //else
                        {
                            listdrpvalues.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
                        }
                    }
                }
            }

            return listdrpvalues;
        }
        public Dictionary<int, string> GetCardValsFromDatabase(string relationname, int Id)
        {
            MySqlCommand cmd = new MySqlCommand();
            Dictionary<int, string> listdrpvalues = new Dictionary<int, string>();
            MySqlParameter[] pms = new MySqlParameter[3];
            pms[0] = new MySqlParameter("argRelationName", MySqlDbType.VarChar);
            pms[0].Value = relationname;
            pms[1] = new MySqlParameter("argStudentId", MySqlDbType.Int32);
            pms[1].Value = Id;
            //pms[2] = new MySqlParameter("argtablename", MySqlDbType.VarChar);
            //pms[2].Value = argtablename;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetAllCoursesForEntities";
            cmd.Parameters.Add(pms[0]);
            cmd.Parameters.Add(pms[1]);
            //cmd.Parameters.Add(pms[2]);
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                //if (columnname == "doctorname")
                //{
                //    cmd = new MySqlCommand("select id, doctorname from tblmasterdoctor", conn);
                //}
                cmd.Connection = conn;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //if (argtablename.Contains("sub"))
                        //{
                        //    listdrpvalues.Add(Convert.ToInt32(reader[0]), reader[2].ToString());
                        //}
                        //else
                        {
                            listdrpvalues.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
                        }
                    }
                }
            }

            return listdrpvalues;
        }

        List<T> ICaseDiaryRepository.GetAllStudents<T>(ref T generictype)
        {
            throw new NotImplementedException();
        }
    }
}
