using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Dtos.Dto
{
    public class TrainingSystemDto
    {
        public Guid Id { get; set; }
        public string TrainingSystemName { get; set; }
        public int? Status { get; set; } // Cho phép giá trị null
        public bool? IsHide { get; set; } // Cho phép giá trị null
        public Guid? IdEduProgram { get; set; }
        public bool IsDeleted { get; set; }
    }
}
