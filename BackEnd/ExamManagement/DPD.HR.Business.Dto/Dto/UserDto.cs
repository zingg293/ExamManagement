﻿namespace DPD.HumanResources.Dtos.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Fullname { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public Guid UserTypeId { get; set; }
        public string? Address { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? UserCode { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid UnitId { get; set; }
        public string ActiveCode { get; set; } = string.Empty;
        public string? Avatar { get; set; }
    }
}
