using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Dtos.Dto
{
    public class ExaminationTypeDto
    {
        public Guid Id { get; set; }
        public string? ExamTypeName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
