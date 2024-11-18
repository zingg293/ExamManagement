using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class BankAccountInformationSqlQueries
    {
        public const string QueryInsertBankAccountInformation = @"
INSERT INTO [dbo].[BankAccountInformation]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[ProvincialBank]
           ,[NameOfBank]
           ,[AccountNumber]
           ,[AccountHolder]
           ,[IdEmployee]
           ,[IsDefault])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @ProvincialBank,
           @NameOfBank,
           @AccountNumber,
           @AccountHolder,
           @IdEmployee,
           @IsDefault)";
        public const string QueryUpdateBankAccountInformation = @"UPDATE [dbo].[BankAccountInformation]
          SET
          [CreatedDate]= @CreatedDate,
          [Status]= @Status, 
          [IdEmployee]= @IdEmployee
          WHERE Id = @Id";
        public const string QueryGetByIdBankAccountInformation = @"select * from [dbo].[BankAccountInformation] where Id = @Id";
        public const string QueryGetBankAccountInformationByIds = @"select * from [dbo].[BankAccountInformation] where Id IN @Ids";
        public const string QueryDeleteBankAccountInformation = @"Delete from [dbo].[BankAccountInformation] where Id IN @Ids";
        public const string QueryGetAllBankAccountInformation = @"SELECT * FROM BankAccountInformation order by CreatedDate";
        public const string QueryHideBankAccountInformation = @"";
        public const string QueryInsertBankAccountInformationDeleted = 
          @"
INSERT INTO[dbo].[Deleted_BankAccountInformation]
        ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[ProvincialBank]
           ,[NameOfBank]
           ,[AccountNumber]
           ,[AccountHolder]
           ,[IdEmployee]
           ,[IsDefault])
     VALUES
           (@Id,
           @CreatedDate,
           @Status,
           @ProvincialBank,
           @NameOfBank,
           @AccountNumber,
           @AccountHolder,
           @IdEmployee,
           @IsDefault)";
    }
}
