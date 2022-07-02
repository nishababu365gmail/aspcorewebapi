using AngularAndWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaseDiary.Service;
using System.Text;

namespace AngularAndWebApi
{
    public class BL
    {

        private ICaseDiaryRepository CaseDiaryRepository { get; set; }
        public BL(ICaseDiaryRepository repository)
        {
            CaseDiaryRepository = repository;

        }
        public List<Student> getmainitemdetails(StudentSearch obj, Student objreturntype, string dbname, string spname, params object[] optionalargs)
        {
            List<Student> listparty = CaseDiaryRepository.getmainitemdetails<StudentSearch, Student>(ref obj, ref objreturntype, dbname, spname, optionalargs);
            return listparty;
        }

        public List<FeesPayment> getmainitemdetails(FeesPaymentMasterSearch obj, FeesPayment objreturntype, string dbname, string spname, params object[] optionalargs)
        {
            List<FeesPayment> listparty = CaseDiaryRepository.getmainitemdetails<FeesPaymentMasterSearch, FeesPayment>(ref obj, ref objreturntype, dbname, spname, optionalargs);
            return listparty;
        }
        public void InsertItems(Student mystudent, string dbname, string spname)
        {
            CaseDiaryRepository.InsertItems<Student>(ref mystudent, dbname, spname);
        }
        public void InsertItems(FeesPayment fp, string dbname, string spname)
        {
            CaseDiaryRepository.InsertItems<FeesPayment>(ref fp, dbname, spname);
        }
        public Dictionary<int,string> GetDropdownlistValsFromDatabase(string argmaincolumnname, string argtablename, int? argmaincolumnvalue)
        {
            Dictionary<int, string> valsfordropdowns = CaseDiaryRepository.GetDropdownlistValsFromDatabase(argmaincolumnname, argtablename,0);
            return valsfordropdowns;
        }
        public List<SelectVals> DictToList(Dictionary<int, string> dictvals)
        {
            SelectVals s = new SelectVals();          
             List<SelectVals> ls = new List<SelectVals>();            
            foreach (var item in dictvals)
            {
                s = new SelectVals();
                s.id = item.Key;
                s.itemName = item.Value;
                ls.Add(s);
            }
            return ls;
        }

        public string StudentCourse(SelectVals[] lst)
        {
            string course = "";
            StringBuilder sb = new StringBuilder();
            foreach (SelectVals item in lst)
            {
                sb.Append(item.id);
                sb.Append(",");
            }
            course = sb.ToString();
            course=course.Substring(0, course.LastIndexOf(','));
            return course;
        }

        public List<SelectVals> ConvertStudentCourseObjectToString(string stuCourse)
        {
            Dictionary<int, string> valsfordropdowns = CaseDiaryRepository.GetDropdownlistValsFromDatabase("", "m_course", 0);
            string[] sepcourse = stuCourse.Split(new char[',']);
            List<SelectVals> ls = new List<SelectVals>();
            Dictionary<int, string> selvalsfordropdowns = new Dictionary<int, string>();
            foreach (string item in sepcourse)
            {
                // if(valsfordropdowns.FirstOrDefault(x=>x.Key.Equals(item))
                if (valsfordropdowns.ContainsKey(Convert.ToInt32(item))){
                    
                        KeyValuePair<int,string> pair = valsfordropdowns.SingleOrDefault(p => p.Key == Convert.ToInt32(item));
                    selvalsfordropdowns.Add(pair.Key,pair.Value);
                  
                  
                }
            }
             ls = DictToList(selvalsfordropdowns);
            return ls;
        }
        public Dictionary<int, string> GetCardValsFromDatabase(string relationname, int Id)
        {
            Dictionary<int, string> valsfordropdowns = CaseDiaryRepository.GetCardValsFromDatabase(relationname, Id);
            return valsfordropdowns;
        }
    }
}
