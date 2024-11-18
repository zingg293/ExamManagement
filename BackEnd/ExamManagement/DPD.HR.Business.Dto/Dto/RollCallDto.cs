using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Dtos.Dto
{
    public class RollCallDto
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public Guid? IdLecturer { get; set; }
        public Guid? IdRoom { get; set; }
        public Guid IdExamShift { get; set; }
        public bool? Present { get; set; }
        public string? Note { get; set; }
        public DateTime? CreateDated { get; set; }
        public bool IsDeleted { get; set; } = false; // Thêm thuộc tính IsDeleted với giá trị mặc định là false
    }

}
