using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Entities.Entities
{
    public class StudyGroup
    {
        public Guid Id { get; set; }
        public string StudyGroupName { get; set; }
        public Guid IdExamSubject { get; set; }
        public bool? Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}
