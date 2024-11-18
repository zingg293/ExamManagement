using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Entities.Entities
{
    public class ExamForm
    {
        public Guid Id { get; set; }
        public string ExamFormName { get; set; }
        public bool IsDeleted { get; set; }
        public string? TeamCode { get; set; }
    }
}
