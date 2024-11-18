using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class LecturersSqlQueries
    {
        public const string QueryInsertLecturers = @"INSERT INTO [dbo].[Lecturers]
                                                   ([ID]
                                                   ,[FullName]
                                                   ,[Gender]
                                                   ,[Phone]
                                                   ,[Status]
                                                   ,[CreatedDate]
                                                   ,[Avatar]
                                                   ,[IdFaculty]
                                                   ,[Birthday]
                                                   ,[IsDeleted])
                                             VALUES
                                                   (
                                                   @ID,
                                                   @FullName,
                                                   @Gender,
                                                   @Phone,
                                                   @Status,
                                                   @CreatedDate,
                                                   @Avatar,
                                                   @IdFaculty,
                                                   @Birthday,
                                                   @IsDeleted)";
        public const string QueryGetAllLecturers = @"select *from [dbo].[Lecturers]";
        public const string QueryGetByIdLecturers = @"select * from [dbo].[Lecturers] where ID = @Id";
        public const string QueryUpdateLecturers = @"UPDATE [dbo].[Lecturers] SET Phone = @Phone, FullName = @FullName, Status = @Status, Gender =@Gender, IdFaculty = @IdFaculty WHERE ID = @Id";
        public const string QueryGetLecturersByIds = "select * from [dbo].[Lecturers] where ID IN @Ids";
        public const string QueryInsertLecturersDeleted = @"INSERT INTO [dbo].[Deleted_Lecturers]
                                                                                ([ID]
                                                                               ,[FullName]
                                                                               ,[Gender]
                                                                               ,[Phone]
                                                                               ,[Status]
                                                                               ,[CreatedDate]
                                                                               ,[Avatar]
                                                                               ,[IdFaculty]
                                                                               ,[Birthday]
                                                                               ,[IsDeleted])
                                                                         VALUES
                                                                               (
                                                                               @ID,
                                                                               @FullName,
                                                                               @Gender,
                                                                               @Phone,
                                                                               @Status,
                                                                               @CreatedDate,
                                                                               @Avatar,
                                                                               @IdFaculty,
                                                                               @Birthday,
                                                                               @IsDeleted)";
        public const string QueryDeleteLecturers = "DELETE FROM [dbo].[Lecturers] WHERE ID IN @Ids";
    }
}
