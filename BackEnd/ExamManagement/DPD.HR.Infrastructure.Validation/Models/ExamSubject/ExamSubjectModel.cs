using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Infrastructure.Validation.Models.ExamSubject
{
    public class ExamSubjectModel
    {
        public Guid Id { get; set; }
        public string? ExamSubjectName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? IdExam { get; set; }
        public Guid? IdExamType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
