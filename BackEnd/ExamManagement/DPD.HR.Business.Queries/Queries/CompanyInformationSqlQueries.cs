namespace DPD.HR.Application.Queries.Queries;

public static class CompanyInformationSqlQueries
{
    public const string QueryInsertCompanyInformation = @"INSERT INTO [dbo].[CompanyInformation]
           ([Id]
           ,[CompanyName]
           ,[TaxNumber]
           ,[AccountNumber]
           ,[Address]
           ,[PhoneNumber]
           ,[Email]
           ,[OpeningHours]
           ,[CreatedDate]
           ,[Status]
           ,[Copyright]
           ,[Logo]
           ,[Fax])
            VALUES (@Id, @CompanyName, @TaxNumber, @AccountNumber, @Address, @PhoneNumber, @Email, @OpeningHours,
                                @CreatedDate, @Status, @Copyright, @Logo, @Fax)";

    public const string QueryUpdateCompanyInformation = @"UPDATE [dbo].[CompanyInformation] SET 
                            CompanyName = @CompanyName,
                            TaxNumber = @TaxNumber,
                            AccountNumber = @AccountNumber,
                            Address = @Address,
                            PhoneNumber = @PhoneNumber,
                            Email = @Email,
                            OpeningHours = @OpeningHours,
                            Copyright = @Copyright,
                            Logo = @Logo,
                            Fax = @Fax
                            WHERE Id = @Id";

    public const string QueryDeleteCompanyInformation = "DELETE FROM [dbo].[CompanyInformation] WHERE Id IN @Ids";
    public const string QueryGetByIdCompanyInformation = "select * from [dbo].[CompanyInformation] where Id = @Id";
    public const string QueryGetCompanyInformationByIds = "select * from [dbo].[CompanyInformation] where Id IN @Ids";
    public const string QueryGetAllCompanyInformation = "select *from [dbo].[CompanyInformation] order by CreatedDate desc";

    public const string QueryInsertCompanyInformationDeleted = @"INSERT INTO [dbo].[Deleted_CompanyInformation]
           ([Id]
           ,[CompanyName]
           ,[TaxNumber]
           ,[AccountNumber]
           ,[Address]
           ,[PhoneNumber]
           ,[Email]
           ,[OpeningHours]
           ,[CreatedDate]
           ,[Status]
           ,[Copyright]
           ,[Logo]
           ,[Fax])
            VALUES (@Id, @CompanyName, @TaxNumber, @AccountNumber, @Address, @PhoneNumber, @Email, @OpeningHours,
                                @CreatedDate, @Status, @Copyright, @Logo, @Fax)";
}