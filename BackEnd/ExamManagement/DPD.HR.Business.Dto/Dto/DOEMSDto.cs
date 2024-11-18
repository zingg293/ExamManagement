using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HumanResources.Dtos.Dto
{
    public class DOEMSDto
    {
        public Guid Id { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? IdEMS { get; set; }
        public Guid? IdLecturer { get; set; }
        public Guid? IdRoom { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? IdStudyGroup { get; set; }
        public Guid? IdDOTS { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? DoUseLabRoom { get; set; }
        public string? Note { get; set; }
        public string? IdTeamcode { get; set; }
    }
}
