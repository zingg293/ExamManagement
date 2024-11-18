using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Dtos.Dto
{
    public class ExamDutyRegistrationDetailsDto
    {
        public Guid Id { get; set; }
        public Guid IdExamScheduleRegistration { get; set; }
        public Guid? IdLecturer { get; set; }
        public Guid? IdExamShift { get; set; }
        public bool IsDeleted { get; set; } = false;         // Mặc định là FALSE
    }

}
