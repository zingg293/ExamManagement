using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Dtos.Dto
{
    public class EMSDto
    {
        public Guid Id { get; set; }
        public string? EMSName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid IdExam { get; set; }
        public Guid IdExamSubject { get; set; }
        public Guid IdTestSchedule { get; set; }
        public int? NumberOfStudents { get; set; }
        public int? NumberOfLecturers { get; set; }
        public Guid? IdStudyGroup { get; set; }
        public bool IsDeleted { get; set; }
    }
}
