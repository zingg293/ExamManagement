using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Infrastructure.Validation.Models.TestSchedule
{
    public class TestScheduleModel
    {
        public Guid Id { get; set; }
        public string? TestScheduleName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? IdExam { get; set; }
        public Guid? IdExamSubject { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool OrganizeFinalExams { get; set; }
        public string? Note { get; set; }
    }
}
