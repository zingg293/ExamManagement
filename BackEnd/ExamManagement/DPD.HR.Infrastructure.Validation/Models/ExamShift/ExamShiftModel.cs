using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Infrastructure.Validation.Models.ExamShift
{
    public class ExamShiftModel
    {
        public Guid Id { get; set; }
        public string ExamShiftName { get; set; } = string.Empty;
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public bool IsDeleted { get; set; } = false;         // Mặc định là FALSE
    }

}
