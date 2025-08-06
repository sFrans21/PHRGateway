using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
    //[Keyless]
    public class VwAIMAN_Username
    {
        [Key]
        public string EmpAccount { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string DepartmentDesc { get; set; }
        public string DepartmentID { get; set; }
        public string DirectorateDesc { get; set; }
        public string DirectorateID { get; set; }
        public string DivisionDesc { get; set; }
        public string DivisionID { get; set; }
        public string EmpEmail { get; set; }
        public string EmpGroupID { get; set; }
        public string EmpGroupName { get; set; }
        public string EmpID { get; set; }
        public string EmpName { get; set; }
        public string EmpSubGroupID { get; set; }
        public string EmpSubGroupName { get; set; }
        public string FunctionID { get; set; }
        public string PersAreaID { get; set; }
        public string PersAreaText { get; set; }
        public string PersSubAreaID { get; set; }
        public string PersSubAreaText { get; set; }
        public string PosID { get; set; }
        public string PositionTitle { get; set; }
        public string SectionDesc { get; set; }
        public string SectionID { get; set; }
        public string AssignmentNumber { get; set; }
    }
}
