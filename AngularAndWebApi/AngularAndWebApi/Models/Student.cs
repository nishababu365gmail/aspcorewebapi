using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularAndWebApi.Models
{
    public class Student
    {        
        public string StuFirstName { get; set; }        
        public string StuLastName { get; set; }        
        public string StuCourse { get; set; }
        public string StuDegree { get; set; }
        public int StudentId { get; set; }
        public string StuAdharNo { get; set; }
        [Phone]
        public string StuPhoneNo { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string StuEmailId { get; set; }
        public string StuNotes { get; set; }
        public string StuPhotoPath { get; set; }
        public DateTime StuRegistrationDate { get; set; }
        public Boolean IsPlacementRequired { get; set; }
        public Boolean IsWorking { get; set; }
        public SelectVals [] newselectedItems { get; set; }
        public string maani { get; set; }
    }
}

