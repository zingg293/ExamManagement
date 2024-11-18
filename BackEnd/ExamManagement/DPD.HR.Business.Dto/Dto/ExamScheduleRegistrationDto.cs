using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Dtos.Dto
{
    public class ExamScheduleRegistrationDto
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = true;           // Mặc định là TRUE
        public DateTime? CreatedDate { get; set; } = DateTime.Now;  // Mặc định là ngày hiện tại
        public Guid? IdEMS { get; set; }
        public Guid? IdStudyGroup { get; set; }
        public Guid? IdDOTS { get; set; }
        public int? NumberOfRegistrations { get; set; }
        public bool IsDeleted { get; set; } = false;         // Mặc định là FALSE
    }
}
