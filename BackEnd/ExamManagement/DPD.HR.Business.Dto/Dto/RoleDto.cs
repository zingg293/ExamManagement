namespace DPD.HumanResources.Dtos.Dto
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleCode { get; set; } = string.Empty;
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
        public int NumberRole { get; set; }
        public bool IsAdmin { get; set; }
    }
}
