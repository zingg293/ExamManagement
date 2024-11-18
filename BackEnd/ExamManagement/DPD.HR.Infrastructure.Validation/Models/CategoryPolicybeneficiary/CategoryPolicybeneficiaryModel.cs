using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPD.HR.Infrastructure.Validation.Models.CategoryPolicybeneficiary
{
    public class CategoryPolicybeneficiaryModel
    {
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public string? NamePolicybeneficiary { get; set; }
    }
}
