using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Dtos.Dto
{
    public class ArrangeLecturersDto
    {
        public Guid Id { get; set; }
        public Guid IdLecturer { get; set; }
        public Guid IdStudyGroup { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
