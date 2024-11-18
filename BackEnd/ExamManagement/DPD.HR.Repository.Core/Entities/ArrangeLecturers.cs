using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Entities.Entities
{
    public class ArrangeLecturers
    {
        public Guid Id { get; set; }
        public Guid IdLecturer { get; set; }
        public Guid IdStudyGroup { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
