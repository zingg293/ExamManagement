using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Entities.Entities
{
    public class DOTS
    {
        public Guid Id { get; set; }
        public Guid? IdExam { get; set; } 
        public Guid? IdExamSubject { get; set; } 
        public Guid? IdTestSchedule { get; set; } 
        public Guid? IdExamForm { get; set; } 
        public int? ExamTime { get; set; }
        public bool IsDeleted { get; set; }

    }
}
