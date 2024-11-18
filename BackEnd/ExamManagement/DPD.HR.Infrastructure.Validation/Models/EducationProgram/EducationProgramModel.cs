using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Infrastructure.Validation.Models.EducationProgram
{
    public class EducationProgramModel
    {
        public Guid Id { get; set; }
        public string EducationProgramName { get; set; }
        public int? Status { get; set; } // Cho phép giá trị null
        public bool? IsHide { get; set; } // Cho phép giá trị null
        public bool IsDeleted { get; set; }

    }
}
