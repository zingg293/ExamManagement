using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Infrastructure.Validation.Models.Lecturers
{
    public class LecturersModel
    {
        public Guid ID { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Avatar { get; set; }
        public Guid IdFaculty { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsDeleted { get; set; }
        public string InstructorCode { get; set; }
    }
}
