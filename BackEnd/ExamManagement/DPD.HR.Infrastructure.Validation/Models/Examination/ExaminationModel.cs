﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Infrastructure.Validation.Models.Examination
{
    public class ExaminationModel
    {
        public Guid Id { get; set; }
        public string? ExamName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? IdExamType { get; set; }
        public DateTime? SchoolYear { get; set; }
        public bool IsDeleted { get; set; }

    }
}