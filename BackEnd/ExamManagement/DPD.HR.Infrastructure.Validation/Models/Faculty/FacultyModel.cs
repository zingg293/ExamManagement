﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Infrastructure.Validation.Models.Faculty
{
    public class FacultyModel
    {
        public Guid Id { get; set; }
        public string FacultyName { get; set; }
        public int? Status { get; set; } // Cho phép giá trị null
        public bool? IsHide { get; set; } // Cho phép giá trị null
        public bool IsDeleted { get; set; }

    }
}