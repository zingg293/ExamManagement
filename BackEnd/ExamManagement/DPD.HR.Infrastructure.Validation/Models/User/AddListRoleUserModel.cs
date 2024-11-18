namespace DPD.HR.Infrastructure.Validation.Models.User;

public class AddListRoleUserModel
{
    public List<Guid>? IdsRole { get; set; }
    public Guid IdUser { get; set; }
}