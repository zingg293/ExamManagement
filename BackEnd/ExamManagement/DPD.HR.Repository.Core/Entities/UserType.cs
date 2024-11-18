namespace DPD.HumanResources.Entities.Entities
{
    public class UserType
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public int? Status { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string TypeCode { get; set; } = string.Empty;
    }
}
