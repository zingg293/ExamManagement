using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Infrastructure.Validation.Models.RollCall
{
    public class RollCallModel
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public Guid? IdLecturer { get; set; }
        public Guid? IdRoom { get; set; }
        public Guid IdExamShift { get; set; }
        public bool? Present { get; set; }
        public string? Note { get; set; }
        public DateTime? CreateDated { get; set; } // Thêm thuộc tính CreateDated
    }
}
