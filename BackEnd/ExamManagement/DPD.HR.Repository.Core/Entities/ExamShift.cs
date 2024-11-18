using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Entities.Entities
{
    public class ExamShift
    {
        public Guid Id { get; set; }

        public string ExamShiftName { get; set; }

        public DateTime? TimeStart { get; set; } 

        public DateTime? TimeEnd { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
