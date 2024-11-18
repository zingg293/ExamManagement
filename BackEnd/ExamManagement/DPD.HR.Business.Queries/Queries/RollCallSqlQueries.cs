using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class RollCallSqlQueries
    {
        public const string QueryInsertRollCall = @"
        INSERT INTO [dbo].[RollCall]
            ([Id]
            ,[IdUser]
            ,[IdLecturer]
            ,[IdRoom]
            ,[IdExamShift]
            ,[Present]
            ,[Note]
            ,[CreateDated]
            ,[IsDeleted])
        VALUES
            (@Id
            ,@IdUser
            ,@IdLecturer
            ,@IdRoom
            ,@IdExamShift
            ,@Present
            ,@Note
            ,@CreateDated
            ,@IsDeleted)";

        public const string QueryGetAllRollCall = "SELECT * FROM [dbo].[RollCall] ORDER BY CreateDated";

        public const string QueryGetByIdRollCall = "SELECT * FROM [dbo].[RollCall] WHERE Id = @Id";

        public const string QueryUpdateRollCall = @"
        UPDATE [dbo].[RollCall]
        SET IdUser = @IdUser,
            IdLecturer = @IdLecturer,
            IdRoom = @IdRoom,
            IdExamShift = @IdExamShift,
            Present = @Present,
            Note = @Note,
            CreateDated = @CreateDated,
            IsDeleted = @IsDeleted
        WHERE Id = @Id";

        public const string QueryGetRollCallByIds = "SELECT * FROM [dbo].[RollCall] WHERE Id IN @Ids";

        public const string QueryInsertRollCallDeleted = @"
        INSERT INTO [dbo].[Deleted_RollCall]
            ([Id]
            ,[IdUser]
            ,[IdLecturer]
            ,[IdRoom]
            ,[IdExamShift]
            ,[Present]
            ,[Note]
            ,[CreateDated]
            ,[IsDeleted])
        VALUES
            (@Id
            ,@IdUser
            ,@IdLecturer
            ,@IdRoom
            ,@IdExamShift
            ,@Present
            ,@Note
            ,@CreateDated
            ,@IsDeleted)";

        public const string QueryDeleteRollCall = "DELETE FROM [dbo].[RollCall] WHERE Id IN @Ids";
    }
}
