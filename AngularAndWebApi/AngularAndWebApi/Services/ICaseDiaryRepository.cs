using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Reflection;

using System.Data;
using System.Linq;

using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AngularAndWebApi.Models;

namespace CaseDiary.Service
{
    public interface ICaseDiaryRepository
    {
        Dictionary<int, string> GetCardValsFromDatabase(string relationname, int Id);
        Dictionary<int, string> GetDropdownlistValsFromDatabase(string relationname,  int argmaincolumnvalue);
        Dictionary<int, string> GetDropdownlistValsFromDatabase(string argmaincolumnname, string argtablename, int? argmaincolumnvalue);
        MySqlConnection GetConnection();
        List<modelspparam> GetSpParam(string dbname, string spname);
        void InsertItems <T> (ref T mypatient, string dbname, string spname);
        List<T> getpatientdetails<T>(ref T mypatient, string dbname, string spname, params object[] optionalargs);
        List<V> getmainitemdetails<T,V>(ref T mygenericsearchtype,ref V mygenericreturntype, string dbname, string spname, params object[] optionalargs);
        List<T> GetAllStudents<T>(ref T generictype);
    }
}
