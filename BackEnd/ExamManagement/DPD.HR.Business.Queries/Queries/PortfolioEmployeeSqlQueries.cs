using System.Collections.Generic;
using System.Xml.Linq;

namespace DPD.HR.Application.Queries.Queries
{
    public static class PortfolioEmployeeSqlQueries
    {
        public const string QueryInsertPortfolioEmployee = @"
INSERT INTO [dbo].[PortfolioEmployee]
           ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[IsSubmit]
           ,[Name]
           ,[Type]
           ,[DateSubmit])
     VALUES
           (@Id
           ,@CreatedDate
           ,@Status
           ,@IdEmployee
           ,@Sort
           ,@IsSubmit
           ,@Name
           ,@Type
           ,@DateSubmit)";
        public const string QueryUpdatePortfolioEmployee = @"
                    UPDATE [dbo].[PortfolioEmployee]
                       SET[CreatedDate] = @CreatedDate,
                         [Status] = @Status,
                         [IdEmployee] = @IdEmployee,
                         [Sort] = @Sort,
                          [IsSubmit] = @IsSubmit,
                          [Name] = @Name,
                          [Type] = @Type,
                          [DateSubmit] = @DateSubmit
                              WHERE Id = @Id";
        public const string QueryGetByIdPortfolioEmployee = @"select * from [dbo].[PortfolioEmployee] where Id = @Id";
        public const string QueryGetPortfolioEmployeeByIds = @"select * from [dbo].[PortfolioEmployee] where Id IN @Ids";
        public const string QueryDeletePortfolioEmployee = @"Delete from [dbo].[PortfolioEmployee] where Id IN @Ids";
        public const string QueryGetAllPortfolioEmployeeAvailable = @"";
        public const string QueryHidePortfolioEmployee = @"";
        public const string QueryGetAllPortfolioEmployee = @"SELECT * FROM PortfolioEmployee order by CreatedDate";
        public const string QueryInsertPortfolioEmployeeDeleted =
          @"
INSERT INTO[dbo].[Deleted_PortfolioEmployee]
        ([Id]
           ,[CreatedDate]
           ,[Status]
           ,[IdEmployee]
           ,[Sort]
           ,[IsSubmit]
           ,[Name]
           ,[Type]
           ,[DateSubmit])
     VALUES
           (@Id
           , @CreatedDate
           , @Status
           , @IdEmployee
           , @Sort
           , @IsSubmit
           , @Name
           , @Type
           , @DateSubmit)";
    }
}
