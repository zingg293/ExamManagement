namespace DPD.HR.Application.Queries.Queries;

public static class UserSqlQueries
{
    public const string AllUserAvailable =
        "select *from [dbo].[User] where IsDeleted = 0 and IsLocked = 0 order by CreatedDate desc";

    public const string UserById = "select * from [dbo].[User] where Id = @Id";
    public const string UserByIds = "select * from [dbo].[User] where Id IN @Ids";
    public const string UserByIdsUserType = "select * from [dbo].[User] where UserTypeId IN @UserTypeId";

    public const string AllUser =
        "select *from [dbo].[User] where IsDeleted = 0 order by CreatedDate desc";

    public const string InsertUser = @"INSERT INTO [dbo].[User]
                   ([Id]
                   ,[Fullname]
                   ,[Description]
                   ,[Password]
                   ,[Email]
                   ,[Phone]
                   ,[UserTypeId]
                   ,[Address]
                   ,[Status]
                   ,[CreatedDate]
                   ,[UserCode]
                   ,[IsLocked]
                   ,[IsDeleted]
                   ,[UnitId]
                   ,[IsActive]
                   ,[CreatedBy]
                   ,[ActiveCode]
                   ,[Avatar]
                   ,[RefreshToken]
                   ,[Salt])
                     VALUES
                           (@Id
                           ,@Fullname
                           ,@Description
                           ,@Password
                           ,@Email
                           ,@Phone
                           ,@UserTypeId
                           ,@Address
                           ,@Status
                           ,@CreatedDate
                           ,@UserCode
                           ,@IsLocked
                           ,@IsDeleted
                           ,@UnitId
                           ,@IsActive
                           ,@CreatedBy
                           ,@ActiveCode
                           ,@Avatar,
                            @RefreshToken,
                            @Salt)";

    public const string UpdateUser = @"UPDATE [dbo].[User] SET Fullname = @Fullname,
                                                Description = @Description,
                                                Phone = @Phone,
                                                UserTypeId = @UserTypeId,
                                                Address = @Address,
                                                UnitId = @UnitId,
                                                Avatar = @Avatar
                                                WHERE Id = @Id";

    public const string DeleteUser =
        "update [dbo].[User] set IsDeleted = 1, Email = (select CONCAT(Email,'/' ,Id) from [User] where Id = @Id) where Id = @Id";

    public const string HideUser = "update [dbo].[User] set IsLocked = @IsLocked where Id In @Ids";

    public const string UserByEmail = "select * from [dbo].[User] where Email = @Email";

    public const string ActiveUser = "update [dbo].[User] set IsActive = 1 where Id = @Id";
    public const string UpdateCodeUser = "update [dbo].[User] set ActiveCode = @ActiveCode where Id = @Id";
    public const string UpdatePasswordUser = "update [dbo].[User] set Password = @Password where Id = @Id";
    public const string UpdateSaltUser = "update [dbo].[User] set Salt = @Salt where Id = @Id";

    public const string InsertUserDelete = @"INSERT INTO [dbo].[Deleted_User]
                   ([Id]
                   ,[Fullname]
                   ,[Description]
                   ,[Password]
                   ,[Email]
                   ,[Phone]
                   ,[UserTypeId]
                   ,[Address]
                   ,[Status]
                   ,[CreatedDate]
                   ,[UserCode]
                   ,[IsLocked]
                   ,[IsDeleted]
                   ,[UnitId]
                   ,[IsActive]
                   ,[CreatedBy]
                   ,[ActiveCode]
                   ,[Avatar]
                   ,[RefreshToken]
                   ,[Salt])
                     VALUES
                           (@Id
                           ,@Fullname
                           ,@Description
                           ,@Password
                           ,@Email
                           ,@Phone
                           ,@UserTypeId
                           ,@Address
                           ,@Status
                           ,@CreatedDate
                           ,@UserCode
                           ,@IsLocked
                           ,@IsDeleted
                           ,@UnitId
                           ,@IsActive
                           ,@CreatedBy
                           ,@ActiveCode
                           ,@Avatar,
                            @RefreshToken,
                            @Salt)";
}