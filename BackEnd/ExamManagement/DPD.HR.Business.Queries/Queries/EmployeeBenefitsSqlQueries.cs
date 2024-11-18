namespace DPD.HR.Application.Queries.Queries;

public static class EmployeeBenefitsSqlQueries
{
    public const string QueryInsertEmployeeBenefits = @"INSERT INTO [dbo].[EmployeeBenefits]
                               ([Id]
                               ,[Status]
                               ,[CreatedDate]
                               ,[IdEmployee]
                               ,[IdCategoryCompensationBenefits]
                               ,[Quantity])
                         VALUES (@Id, @Status, @CreatedDate, @IdEmployee, @IdCategoryCompensationBenefits, @Quantity)";

    public const string QueryUpdateEmployeeBenefits = @"UPDATE [dbo].[EmployeeBenefits] SET 
                                        IdEmployee = @IdEmployee,
                                        IdCategoryCompensationBenefits = @IdCategoryCompensationBenefits,
                                        Quantity = @Quantity
                                        WHERE Id = @Id";
    
    public const string QueryEmployeeBenefitsByIdsEmployee = @"select eb.*
                    from Employee e
                             join EmployeeBenefits eb on eb.IdEmployee = e.Id
                    where e.Id = @IdEmployee";
    
    public const string QueryEmployeeBenefitsByIdIdCategoryCompensationBenefits = "select * from [dbo].[EmployeeBenefits] where IdCategoryCompensationBenefits IN @IdCategoryCompensationBenefits";

    public const string QueryDeleteEmployeeBenefits = "DELETE FROM [dbo].[EmployeeBenefits] WHERE Id IN @Ids";
    public const string QueryGetByIdEmployeeBenefits = "select * from [dbo].[EmployeeBenefits] where Id = @Id";
    public const string QueryEmployeeBenefitsByIds = "select * from [dbo].[EmployeeBenefits] where Id IN @Ids";
    public const string QueryGetAllEmployeeBenefits = "select *from [dbo].[EmployeeBenefits] order by CreatedDate desc";
    public const string QueryEmployeeBenefitsByNotInIds = "select * from [dbo].[EmployeeBenefits] where Id NOT IN @Ids and IdEmployee = @IdEmployee";

    
    public const string QueryGetAllEmployeeBenefitsByIdEmployee =
        "select *from [dbo].[EmployeeBenefits] where IdEmployee = @IdEmployee order by CreatedDate desc";

    public const string QueryInsertEmployeeBenefitsDeleted = @"INSERT INTO [dbo].[Deleted_EmployeeBenefits]
                               ([Id]
                               ,[Status]
                               ,[CreatedDate]
                               ,[IdEmployee]
                               ,[IdCategoryCompensationBenefits]
                               ,[Quantity])
                         VALUES (@Id, @Status, @CreatedDate, @IdEmployee, @IdCategoryCompensationBenefits, @Quantity)";
}