using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Infrastructure.Validation.Models.ExaminationType
{
    public class ExaminationTypeModel
    {
        public Guid Id { get; set; }
        public string? ExamTypeName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
