using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Infrastructure.Validation.Models.StudyGroup
{
    public class StudyGroupModel
    {
        public Guid Id { get; set; }
        public Guid IdExamSubject { get; set; }
        public bool? Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}
