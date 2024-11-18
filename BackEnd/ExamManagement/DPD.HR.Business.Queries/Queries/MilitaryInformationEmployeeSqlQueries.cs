using System.Collections.Generic;

namespace DPD.HR.Application.Queries.Queries
{
    public static class MilitaryInformationEmployeeSqlQueries
    {
        public const string QueryInsertMilitaryInformationEmployee = @"INSERT INTO [dbo].[MilitaryInformationEmployee]
                                                                                   ([Id]
                                                                                   ,[CreatedDate]
                                                                                   ,[Status]
                                                                                   ,[IdEmployee]
                                                                                   ,[Sort]
                                                                                   ,[TypeOfMilitary]
                                                                                   ,[EnlistmentDate]
                                                                                   ,[DischargeDate]
                                                                                   ,[MilitaryRank]
                                                                                   ,[HighestMilitaryRank])
                                                                             VALUES
                                                                                   (@Id,
                                                                                   @CreatedDate,
                                                                                   @Status,
                                                                                   @IdEmployee,
                                                                                   @Sort,
                                                                                   @TypeOfMilitary,
                                                                                   @EnlistmentDate,
                                                                                   @DischargeDate,
                                                                                   @MilitaryRank,
                                                                                   @HighestMilitaryRank)";
        public const string QueryUpdateMilitaryInformationEmployee = @"UPDATE [dbo].[MilitaryInformationEmployee]
          SET
            [CreatedDate]  = @CreatedDate,
            [Status]  = @Status,
            [IdEmployee]  = @IdEmployee,
            [Sort]  = @Sort,
            [TypeOfMilitary]  = @TypeOfMilitary,
            [EnlistmentDate]  = @EnlistmentDate,
            [DischargeDate]  = @DischargeDate,
            [MilitaryRank]  = @MilitaryRank,
            [HighestMilitaryRank]  = @HighestMilitaryRank
          WHERE Id = @Id";
        public const string QueryGetByIdMilitaryInformationEmployee = @"select * from [dbo].[MilitaryInformationEmployee] where Id = @Id";
        public const string QueryGetMilitaryInformationEmployeeByIds = @"select * from [dbo].[MilitaryInformationEmployee] where Id IN @Ids";
        public const string QueryDeleteMilitaryInformationEmployee = @"Delete from [dbo].[MilitaryInformationEmployee] where Id IN @Ids";
        public const string QueryGetAllMilitaryInformationEmployee = @"SELECT * FROM MilitaryInformationEmployee order by CreatedDate";
        public const string QueryGetAllMilitaryInformationEmployeeAvailable = @"";
        public const string QueryHideMilitaryInformationEmployee = @"";
        public const string QueryInsertMilitaryInformationEmployeeDeleted =
                                       @"
                            INSERT INTO[dbo].[Deleted_MilitaryInformationEmployee]
                                    ([Id]
                                       ,[CreatedDate]
                                       ,[Status]
                                       ,[IdEmployee]
                                       ,[Sort]
                                       ,[TypeOfMilitary]
                                       ,[EnlistmentDate]
                                       ,[DischargeDate]
                                       ,[MilitaryRank]
                                       ,[HighestMilitaryRank])
                                 VALUES
                                       (@Id,
                                       @CreatedDate,
                                       @Status,
                                       @IdEmployee,
                                       @Sort,
                                       @TypeOfMilitary,
                                       @EnlistmentDate,
                                       @DischargeDate,
                                       @MilitaryRank,
                                       @HighestMilitaryRank)";
    }
}
