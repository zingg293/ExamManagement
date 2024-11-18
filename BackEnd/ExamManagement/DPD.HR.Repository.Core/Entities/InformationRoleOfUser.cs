namespace DPD.HumanResources.Entities.Entities;

public class InformationRoleOfUser
{
    public Guid IdRole { get; set; }
    public string NameRole { get; set; } = string.Empty;
    public Guid IdUser { get; set; }
    public int NumberRole { get; set; }
    public bool IsAdmin { get; set; }
}