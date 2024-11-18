using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Dtos.Dto
{
    public class ExamShiftDto
    {
        public Guid Id { get; set; }

        public string ExamShiftName { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
