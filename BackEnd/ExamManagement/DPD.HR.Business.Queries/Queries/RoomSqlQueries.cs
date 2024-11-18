using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.EXAMM.Application.Queries.Queries
{
    public class RoomSqlQueries
    {
        public const string QueryInsertRoom = @"INSERT INTO [dbo].[Room]
           ([Id]
           ,[RoomName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IsOrder]
           ,[FromDate]
           ,[ToDate]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @RoomName,
           @IsActive,
           @CreatedDate,
           @IsOrder,
           @FromDate,
           @ToDate,
           @IsDeleted)";
        public const string QueryGetAllRoom = @"select *from [dbo].[Room] ";
        public const string QueryGetByIdRoom = @"select * from [dbo].[Room] where Id = @Id";
        public const string QueryUpdateRoom = @"UPDATE [dbo].[Room] 
                                                SET
                                                RoomName = @RoomName,
                                                IsActive = @IsActive,
                                                IsOrder = @IsOrder,
                                                FromDate = @FromDate,
                                                ToDate = @ToDate
                                                WHERE Id = @Id";
        public const string QueryGetRoomByIds = "select * from [dbo].[Room] where Id IN @Ids";
        public const string QueryInsertRoomDeleted = @"INSERT INTO [dbo].[Deleted_Room]
           ([Id]
           ,[RoomName]
           ,[IsActive]
           ,[CreatedDate]
           ,[IsOrder]
           ,[FromDate]
           ,[ToDate]
           ,[IsDeleted])
     VALUES
           (
           @Id,
           @RoomName,
           @IsActive,
           @CreatedDate,
           @IsOrder,
           @FromDate,
           @ToDate,
           @IsDeleted)";
        public const string QueryDeleteRoom = "DELETE FROM [dbo].[Room] WHERE Id IN @Ids";

    }
}
